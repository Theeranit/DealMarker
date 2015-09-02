using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using KK.DealMaker.Core.Data;
using KK.DealMaker.UIProcessComponent.View;
using KK.DealMaker.Core.Helper;
using System.Web.Script.Serialization;
using KK.DealMaker.Core.Common;
using KK.DealMaker.UIProcessComponent.Admin;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.UIProcessComponent.Deal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KK.DealMaker.Web.Deal
{
    public partial class SwapEntryInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ProductId = new JavaScriptSerializer().Serialize(Request.QueryString["id"]);
        }

        [WebMethod(EnableSession = true)]
        public static object GenerateTrnObject(string strTradeDate, string strInstrument, string strCtpy, string strPortfolio
                                                , string strEffDate,string strMatDate
                                                , string strNotional1, string strCCY1, string strFFL1, string strFFix1, string strRate1, string strFreq1
                                                , string strNotional2, string strCCY2, string strFFL2, string strFFix2, string strRate2, string strFreq2
                                                , string strOverApprover, string strOverPCE, string strOverSCE, string strComment, string strRemark, string strProductId, bool blnIsSubmit)
        {
            try
            {
                DA_TRN TrnInfo = DealUIP.GenerateSwapTransactionObject(SessionInfo, strTradeDate, strInstrument, strCtpy, strPortfolio
                                                                , strEffDate, strMatDate, strNotional1
                                                                , strCCY1, strFFL1, strFFix1, strRate1, strFreq1
                                                                , strNotional2, strCCY2, strFFL2, strFFix2, strRate2, strFreq2
                                                                , Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[AppSettingName.CASHFLOW_SPAN])
                                                                , strRemark, strProductId);

                object PCESCEObject;
                CheckSwapLimits(blnIsSubmit, TrnInfo, out PCESCEObject);
                return new
                {
                    Result = "OK",
                    record = JsonConvert.SerializeObject(TrnInfo, new IsoDateTimeConverter()),
                    pcesce = PCESCEObject,
                    Message = ""
                };
            }
            catch ( Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetEditByID(Guid id)
        {
            try
            {

                DA_TRN trn = DealUIP.GetByID(id);
                var query = new
                {
                    ID = trn.ID,
                    TradeDate = trn.TRADE_DATE.HasValue ? trn.TRADE_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    EffectDate = trn.START_DATE.HasValue ? trn.START_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    MaturityDate = trn.MATURITY_DATE.HasValue ? trn.MATURITY_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    Counterparty = trn.CTPY_ID.ToString(),
                    Portfolio = trn.PORTFOLIO_ID.HasValue ? trn.PORTFOLIO_ID.Value.ToString() : "-1",
                    Instrument = trn.PORTFOLIO_ID.HasValue ? trn.INSTRUMENT_ID.Value.ToString() : "-1",
                    Notional1 = Math.Abs(trn.FIRST.NOTIONAL.Value),
                    Notional2 = Math.Abs(trn.SECOND.NOTIONAL.Value),
                    FlagFixed1 = trn.FIRST.FLAG_FIXED.HasValue ? (trn.FIRST.FLAG_FIXED.Value ? "1" : "0") : "0",
                    FixAmt1 = trn.FIRST.FIRSTFIXINGAMT,
                    Rate1 = trn.FIRST.RATE,
                    Feq1 = trn.FIRST.FREQTYPE_ID.HasValue ? trn.FIRST.FREQTYPE_ID.Value.ToString() : string.Empty,
                    FlagFixed2 = trn.SECOND.FLAG_FIXED.HasValue ? (trn.SECOND.FLAG_FIXED.Value ? "1" : "0") : "0",
                    FixAmt2 = trn.SECOND.FIRSTFIXINGAMT,
                    Rate2 = trn.SECOND.RATE,
                    Feq2 = trn.SECOND.FREQTYPE_ID.HasValue ? trn.SECOND.FREQTYPE_ID.Value.ToString() : string.Empty,
                    CCY1 = trn.FIRST.CCY_ID,
                    CCY2 = trn.SECOND.CCY_ID
                };
                return new { Result = "OK", record = query };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        private static void CheckSwapLimits(Boolean blnIsSubmit, DA_TRN record, out object PCESCEObject)
        {
            try
            {
                LimitDisplayModel PCECheckRecords = DealUIP.CheckPCE(SessionInfo, record, ProductId);
                LimitDisplayModel SCECheckRecords = DealUIP.CheckSwapSCE(SessionInfo, record, ProductId);
                LimitDisplayModel CountryCheckRecords = DealUIP.CheckSwapCountryLimit(SessionInfo, record, ProductId);

                PCESCEObject = ConsoLimitDisplay(blnIsSubmit, PCECheckRecords, SCECheckRecords, CountryCheckRecords);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        [WebMethod(EnableSession = true)]
        public static object GetPCERecords(string record)
        {
            List<LimitCheckModel> temp = JsonConvert.DeserializeObject<List<LimitCheckModel>>(record);
            return new
            {
                Result = "OK",
                Records = temp,
                TotalRecordCount = temp.Count
            };

        }

        [WebMethod(EnableSession = true)]
        public static object GetSCERecords(string record,int jtStartIndex, int jtPageSize)
        {
            List<LimitCheckModel> temp = JsonConvert.DeserializeObject<List<LimitCheckModel>>(record);
            return new
            {
                Result = "OK",
                Records = jtPageSize > 0 ? temp.Skip(jtStartIndex).Take(jtPageSize).ToList() : temp,
                TotalRecordCount = temp.Count
            };
        }

        [WebMethod(EnableSession = true)]
        public static object GetCountryRecords(string record, int jtStartIndex, int jtPageSize)
        {
            List<LimitCheckModel> temp = JsonConvert.DeserializeObject<List<LimitCheckModel>>(record);
            return new
            {
                Result = "OK",
                Records = jtPageSize > 0 ? temp.Skip(jtStartIndex).Take(jtPageSize).ToList() : temp,
                TotalRecordCount = temp.Count
            };
        }

        [WebMethod(EnableSession = true)]
        public static object SubmitDeal(string strOverApprover, string strOverComment, string record, string strProductId)
        {
            DA_TRN TrnInfo = JsonConvert.DeserializeObject<DA_TRN>(record);
            return DealUIP.SubmitSwapDeal(SessionInfo
                                            , TrnInfo
                                            , strOverApprover
                                            , strOverComment
                                            , strProductId);
        }

        [WebMethod(EnableSession = true)]
        public static object GetCounterpartyByName(string name_startsWith)
        {
            return CounterpartyUIP.GetCounterpartyByName(name_startsWith.ToUpper());

        }
    }
}