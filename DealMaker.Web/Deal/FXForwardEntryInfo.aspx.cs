using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.UIProcessComponent.Deal;
using KK.DealMaker.UIProcessComponent.Admin;
using KK.DealMaker.Core.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KK.DealMaker.Web.Deal
{
    public partial class FXForwardEntryInfo : BasePage
    {     
        protected void Page_Load(object sender, EventArgs e)
        {
            ProductId = new JavaScriptSerializer().Serialize(Request.QueryString["id"]);
        }
        [WebMethod(EnableSession = true)]
        public static object GetInstrument(Guid id)
        {
            try
            {
                MA_INSTRUMENT ins = InstrumentUIP.GetByID(SessionInfo, id);
                return new { Result = "OK", Message = "", CCY = new { CURRENCY1 = ins.MA_CURRENCY.LABEL, CURRENCY2 = ins.MA_CURRENCY2.LABEL, CURRENCYID1 = ins.MA_CURRENCY.ID, CURRENCYID2 = ins.MA_CURRENCY2.ID, FLAG_MULTIPLY = ins.FLAG_MULTIPLY } };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        [WebMethod(EnableSession = true)]
        public static object GenerateTrnObject(string strTradeDate, string strSpotDate, string strSetDate, string strCtpy, string strPortfolio
                                                , string strCurrencyPair, string strBS, string strContractCcy, string strCounterCcy
                                                , string strSpotRate, string strSwapPoint, string strContractAmt, string strCounterAmt, string strRemark, string strProductId, bool settleFlag, bool blnIsSubmit)
        {
            try
            {
                DA_TRN TrnInfo =  DealUIP.GenerateFXForwardTransactionObject(SessionInfo, strTradeDate, strSpotDate, strSetDate, strCtpy, strPortfolio
                                                               , strCurrencyPair, strBS, strContractCcy, strCounterCcy
                                                               , strSpotRate, strSwapPoint, strContractAmt, strCounterAmt, strRemark, settleFlag, strProductId);

                object  PCESCEObject;
                CheckFXSpotLimits(blnIsSubmit, TrnInfo, out PCESCEObject);
                return new { Result = "OK", 
                    record = JsonConvert.SerializeObject(TrnInfo, new IsoDateTimeConverter()),
                    pcesce = PCESCEObject,
                    Message = "" };
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
                MA_INSTRUMENT ins = InstrumentUIP.GetByID(SessionInfo, trn.MA_INSRUMENT.ID);
                var query = new
                {
                    ID = trn.ID,
                    TradeDate = trn.TRADE_DATE.HasValue ? trn.TRADE_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    SpotDate = trn.SPOT_DATE.HasValue ? trn.SPOT_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    MaturityDate = trn.MATURITY_DATE.HasValue ? trn.MATURITY_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    Counterparty = trn.CTPY_ID.ToString(),
                    Portfolio = trn.PORTFOLIO_ID.ToString(),
                    Instrument = trn.INSTRUMENT_ID.ToString(),
                    BuySell = trn.FLAG_BUYSELL,
                    CCY1 = trn.FIRST.CCY_ID.ToString(),
                    SpotRate = trn.FIRST.RATE - (trn.FIRST.SWAP_POINT.HasValue ?  trn.FIRST.SWAP_POINT : 0),
                    SwapPoint = trn.FIRST.SWAP_POINT,
                    Remark = trn.REMARK,
                    flag_settle = trn.FLAG_SETTLE,
                    Notional1 = Math.Abs(trn.FIRST.NOTIONAL.Value),
                    Notional2 = Math.Abs(trn.SECOND.NOTIONAL.Value)
                };
                return new { Result = "OK", record = query, CCY = new { CURRENCY1 = ins.MA_CURRENCY.LABEL, CURRENCY2 = ins.MA_CURRENCY2.LABEL, CURRENCYID1 = ins.MA_CURRENCY.ID, CURRENCYID2 = ins.MA_CURRENCY2.ID, FLAG_MULTIPLY = ins.FLAG_MULTIPLY } };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        [WebMethod(EnableSession = true)]
        private static void CheckFXSpotLimits(Boolean blnIsSubmit, DA_TRN record, out object PCESCEObject)
        {
            try
            {
                LimitDisplayModel PCECheckRecords = DealUIP.CheckPCE(SessionInfo, record, ProductId);
                LimitDisplayModel SCECheckRecords = record.FLAG_SETTLE.Value ? DealUIP.CheckSCE(SessionInfo, record, ProductId) : null;
                LimitDisplayModel CountryCheckRecords = DealUIP.CheckCountryLimit(SessionInfo, record, ProductId);

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
        public static object GetSCERecords(string record, int jtStartIndex, int jtPageSize)
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
            return DealUIP.SubmitFXForwardDeal(SessionInfo
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

        [WebMethod(EnableSession = true)]
        public static object GetInstrumentByName(string name_startsWith)
        {
            return InstrumentUIP.GetInstrumentByName(SessionInfo, ProductCode.FXFORWARD, name_startsWith.ToUpper());


        }
    }
}