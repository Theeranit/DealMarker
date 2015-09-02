using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;
using KK.DealMaker.UIProcessComponent.Deal;
using KK.DealMaker.UIProcessComponent.Admin;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KK.DealMaker.Web.Deal
{
    public partial class FXSwapEntryInfo : BasePage
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
        public static object GenerateTrnObject(string strTradeDate, string strCtpy, string strPortfolio, string strCurrencyPair
                                                , string strContractCcy, string strCounterCcy, string strSpotRate
                                                , string strBSNear, string strSetDateNear, string strSwapPointNear
                                                , string strContractAmtNear, string strCounterAmtNear
                                                , string strBSFar, string strSetDateFar, string strSwapPointFar
                                                , string strContractAmtFar, string strCounterAmtFar, string strSpotDate, string strRemark, string strProductId1, bool settleFlag, bool blnIsSubmit)
        {
            try
            {
                DA_TRN TrnInfo1 = DealUIP.GenerateFXSwapTransactionObject1(SessionInfo, strTradeDate, strCtpy, strPortfolio, strCurrencyPair
                                                               , strContractCcy, strCounterCcy, strSpotRate
                                                               , strBSNear, strSetDateNear, strSwapPointNear, strContractAmtNear, strCounterAmtNear, strSpotDate, strRemark, settleFlag, strProductId1);
                DA_TRN TrnInfo2 = DealUIP.GenerateFXSwapTransactionObject2(SessionInfo, strTradeDate, strCtpy, strPortfolio, strCurrencyPair
                                                               , strContractCcy, strCounterCcy, strSpotRate
                                                               , strBSFar, strSetDateFar, strSwapPointFar
                                                               , strContractAmtFar, strCounterAmtFar, strSpotDate, strRemark, settleFlag, TrnInfo1.VERSION);

                object PCESCEObject;
                CheckFXSpotLimits(blnIsSubmit, TrnInfo1, TrnInfo2, out PCESCEObject);
                return new
                {
                    Result = "OK",
                    record = JsonConvert.SerializeObject(TrnInfo1, new IsoDateTimeConverter()),
                    record2 = JsonConvert.SerializeObject(TrnInfo2, new IsoDateTimeConverter()),
                    pcesce = PCESCEObject,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        private static void CheckFXSpotLimits(Boolean blnIsSubmit, DA_TRN record, DA_TRN record2, out object PCESCEObject)
        {
            try
            {
                LimitDisplayModel PCECheckRecords = DealUIP.CheckFXSwapPCE(SessionInfo, record, record2, ProductId);
                LimitDisplayModel SCECheckRecords = record.FLAG_SETTLE.Value ?  DealUIP.CheckFXSwapSCE(SessionInfo, record, record2, ProductId) : null;
                LimitDisplayModel CountryCheckRecords = DealUIP.CheckFXSwapCountryLimit(SessionInfo, record, record2, ProductId);

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
        public static object SubmitDeal(string strOverApprover, string strOverComment, string record1, string record2, string strProductId1, string strProductId2)
        {
            DA_TRN TrnInfo1 = JsonConvert.DeserializeObject<DA_TRN>(record1);
            DA_TRN TrnInfo2 = JsonConvert.DeserializeObject<DA_TRN>(record2);
            return DealUIP.SubmitFXSwapDeal(SessionInfo
                                            , TrnInfo1
                                            , TrnInfo2
                                            , strOverApprover
                                            , strOverComment
                                            , strProductId1
                                            , strProductId2);
        }

        [WebMethod(EnableSession = true)]
        public static object GetCounterpartyByName(string name_startsWith)
        {
            return CounterpartyUIP.GetCounterpartyByName(name_startsWith.ToUpper());

        }

        [WebMethod(EnableSession = true)]
        public static object GetInstrumentByName(string name_startsWith)
        {
            return InstrumentUIP.GetInstrumentByName(SessionInfo, ProductCode.FXSWAP, name_startsWith.ToUpper());

        }

        [WebMethod(EnableSession = true)]
        public static object GetEditByID(Guid id)
        {
            try
            {

                DA_TRN lastTrn1 = DealUIP.GetByID(id);
                DA_TRN lastTrn2 = DealUIP.GetFXSwapPair(SessionInfo, lastTrn1.INT_DEAL_NO, lastTrn1.VERSION, lastTrn1.ID);

                MA_INSTRUMENT ins = InstrumentUIP.GetByID(SessionInfo, lastTrn1.MA_INSRUMENT.ID);
                var NearLeg = lastTrn1.FLAG_NEARFAR == "N" ? lastTrn1 : lastTrn2;
                var FarLeg = lastTrn2.FLAG_NEARFAR == "F" ? lastTrn2 : lastTrn1;
                var query = new
                {

                    TradeDate = lastTrn1.TRADE_DATE.HasValue ? lastTrn1.TRADE_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    Counterparty = lastTrn1.CTPY_ID.ToString(),
                    Portfolio = lastTrn1.PORTFOLIO_ID.ToString(),
                    Instrument = lastTrn1.INSTRUMENT_ID.ToString(),
                    ContractCcy  = lastTrn1.FIRST.CCY_ID.ToString(),
                    SpotDate = lastTrn1.SPOT_DATE.HasValue ? lastTrn1.SPOT_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    SpotRate = lastTrn1.FIRST.RATE.Value - lastTrn1.FIRST.SWAP_POINT.Value,
                    BSN = NearLeg.FLAG_BUYSELL,
                    BSF = FarLeg.FLAG_BUYSELL,
                    SetDateN = NearLeg.MATURITY_DATE.HasValue ? NearLeg.MATURITY_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    SetDateF = FarLeg.MATURITY_DATE.HasValue ? FarLeg.MATURITY_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    SwapPoitN = NearLeg.FIRST.SWAP_POINT,
                    SwapPoitF = FarLeg.FIRST.SWAP_POINT,
                    ContAmtN = Math.Abs(NearLeg.FIRST.NOTIONAL.Value),
                    CountAmtN = Math.Abs(NearLeg.SECOND.NOTIONAL.Value),
                    ContAmtF = Math.Abs(FarLeg.FIRST.NOTIONAL.Value),
                    CountAmtF = Math.Abs(FarLeg.SECOND.NOTIONAL.Value),
                    flag_settle = lastTrn1.FLAG_SETTLE,
                };
                return new { Result = "OK", record = query, productid2 = lastTrn2.ID.ToString(), CCY = new { CURRENCY1 = ins.MA_CURRENCY.LABEL, CURRENCY2 = ins.MA_CURRENCY2.LABEL, CURRENCYID1 = ins.MA_CURRENCY.ID, CURRENCYID2 = ins.MA_CURRENCY2.ID, FLAG_MULTIPLY = ins.FLAG_MULTIPLY } };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
    }
}