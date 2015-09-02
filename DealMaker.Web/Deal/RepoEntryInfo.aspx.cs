using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;
using KK.DealMaker.Core.Data;
using KK.DealMaker.UIProcessComponent.View;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.UIProcessComponent.Admin;
using KK.DealMaker.UIProcessComponent.Deal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KK.DealMaker.Web.Deal
{
    public partial class RepoEntryInfo : BasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            ProductId = new JavaScriptSerializer().Serialize(Request.QueryString["id"]);
        }

        [WebMethod(EnableSession = true)]
        public static object GenerateTrnObject(string strTradeDate
                                                , string strBuySell
                                                , string strInstrument
                                                , string strCtpy
                                                , string strPortfolio
                                                , string strEffectiveDate
                                                , string strMaturityDate
                                                , string strNotional
                                                , string strProductId
                                                , string strRemark
                                                , bool blnIsSubmit)
        {
            try
            {
                DA_TRN TrnInfo = DealUIP.GenerateRepoTransactionObject(SessionInfo
                                                            ,   strTradeDate
                                                            ,  strBuySell
                                                            ,  strInstrument
                                                            ,  strCtpy
                                                            ,  strPortfolio
                                                            ,  strEffectiveDate
                                                            ,  strMaturityDate
                                                            ,  strNotional
                                                            ,  strProductId
                                                            ,  strRemark);

                object PCESCEObject;
                CheckRepoLimit(blnIsSubmit, TrnInfo, out PCESCEObject);
                return new
                {
                    Result = "OK",
                    record = JsonConvert.SerializeObject(TrnInfo, new IsoDateTimeConverter()),
                    pcesce = PCESCEObject,
                    Message = ""
                };
            }
            catch (Exception ex)
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
                    BuySell = trn.FLAG_BUYSELL,
                    MaturityDate = trn.MATURITY_DATE.HasValue ? trn.MATURITY_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    EffectiveDate = trn.START_DATE.HasValue ? trn.START_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    Counterparty = trn.CTPY_ID.ToString(),
                    Portfolio = trn.PORTFOLIO_ID.ToString(),
                    Instrument = trn.INSTRUMENT_ID.ToString(),
                    Notional = Math.Abs(trn.FIRST.NOTIONAL.Value),
                    Remark = trn.REMARK
                };
                return new { Result = "OK", record = query };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        private static void CheckRepoLimit(Boolean blnIsSubmit, DA_TRN record, out object PCESCEObject)
        {
            try
            {
                LimitDisplayModel PCECheckRecords = DealUIP.CheckPCE(SessionInfo, record, ProductId);
                LimitDisplayModel CountryCheckRecords = DealUIP.CheckCountryLimit(SessionInfo, record, ProductId);

                PCESCEObject = ConsoLimitDisplay(blnIsSubmit, PCECheckRecords, null, CountryCheckRecords);
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
            return DealUIP.SubmitRepoDeal(SessionInfo
                                        , TrnInfo
                                        , strOverApprover
                                        , strOverComment
                                        , strProductId);
        }
    }
}