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
using System.Configuration;
using ThaiBMA.WebService.Key;
using System.Reflection;

namespace KK.DealMaker.Web.Deal
{
    public partial class FIEntryInfo : BasePage
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
                                                , string strSettlementDate
                                                , string strYield
                                                , string strUnit
                                                , string strCleanPrice
                                                , string strGrossPrice
                                                , string strNotional
                                                , string strCCY
                                                , string strPceFlag
                                                , string strSettleFlag
                                                , string strYeildType
                                                , string strReportBy
                                                , string strPurpose
                                                , string strTerm
                                                , string strRate
                                                , string strTBMARemark
                                                , string strRemark
                                                , string strProductId
                                                , bool blnIsSubmit)
        {
            try
            {
                DA_TRN TrnInfo = DealUIP.GenerateFITransactionObject(SessionInfo
                                                                ,  strTradeDate
                                                                ,  strBuySell
                                                                ,  strInstrument
                                                                ,  strCtpy
                                                                ,  strPortfolio
                                                                ,  strSettlementDate
                                                                ,  strYield
                                                                ,  strUnit
                                                                ,  strCleanPrice
                                                                ,  strGrossPrice
                                                                ,  strNotional
                                                                ,  strCCY
                                                                ,  strPceFlag
                                                                ,  strSettleFlag
                                                                ,  strYeildType
                                                                ,  strReportBy
                                                                ,  strPurpose
                                                                ,  strTerm
                                                                ,  strRate
                                                                ,  strTBMARemark
                                                                ,  strRemark
                                                                ,  strProductId);

      
              object  PCESCEObject;
              CheckFILimit(blnIsSubmit, TrnInfo, out PCESCEObject);
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
                var query = new
                {
                    ID = trn.ID,
                    TradeDate = trn.TRADE_DATE.HasValue ? trn.TRADE_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    BuySell = trn.FLAG_BUYSELL,
                    MaturityDate = trn.MATURITY_DATE.HasValue ? trn.MATURITY_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL) : string.Empty,
                    Counterparty = trn.CTPY_ID.ToString(),
                    Portfolio = trn.PORTFOLIO_ID.ToString(),
                    Instrument = trn.INSTRUMENT_ID.ToString(),
                    Notional1 = Math.Abs(trn.FIRST.NOTIONAL.Value),
                    SettleFlag = trn.FLAG_SETTLE.HasValue ? (trn.FLAG_SETTLE.Value ? "1" : "0") : "0",
                    Remark = trn.REMARK,
                    Yield = trn.FIRST.RATE,
                    Unit = trn.DA_TMBA_EXTENSION.UNIT,
                    CPrice = trn.DA_TMBA_EXTENSION.CLEAN_PRICE,
                    GPrice = trn.DA_TMBA_EXTENSION.GROSS_PRICE,
                    PMarket = trn.FLAG_PCE.HasValue ? (trn.FLAG_PCE.Value ? "1" : "0") : "0",
                    YType = trn.DA_TMBA_EXTENSION.YIELD_TYPE,
                    Purpose = trn.DA_TMBA_EXTENSION.PURPOSE,
                    ReportBy = trn.DA_TMBA_EXTENSION.IS_REPORT_CLEAN ? "0" : "1",
                    Term = trn.DA_TMBA_EXTENSION.TERM.HasValue ? trn.DA_TMBA_EXTENSION.TERM.Value.ToString() : "" ,
                    Rate = trn.DA_TMBA_EXTENSION.RATE.HasValue ? trn.DA_TMBA_EXTENSION.RATE.Value.ToString() : "",
                    TBMARemark = trn.DA_TMBA_EXTENSION.REMARK,
                    LotSize = trn.MA_INSRUMENT.LOT_SIZE
                };
                return new { Result = "OK", record = query };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetCCYByInstrumentID(Guid id)
        {
            try
            {
                MA_INSTRUMENT ins = InstrumentUIP.GetByID(SessionInfo, id);
                return new { Result = "OK", record = new {id = ins.MA_CURRENCY.ID.ToString(), label = ins.MA_CURRENCY.LABEL } };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object GetDealInternalByProcessDate(int jtStartIndex, int jtPageSize)
        {
            try
            {
                List<FIDealModel> trns = DealUIP.GetDealInternalByProcessDate(SessionInfo);
                return new
                {
                    Result = "OK",
                    Records = jtPageSize > 0 ? trns.Skip(jtStartIndex).Take(jtPageSize).ToList() : trns,
                    TotalRecordCount = trns.Count
                };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        
        private static void CheckFILimit(Boolean blnIsSubmit, DA_TRN record, out object PCESCEObject)
        {
            try
            {
                LimitDisplayModel PCECheckRecords = DealUIP.CheckPCE(SessionInfo, record, ProductId);
                LimitDisplayModel SCECheckRecords = null;
                LimitDisplayModel CountryCheckRecords = DealUIP.CheckCountryLimit(SessionInfo, record, ProductId);

                var flagSettle = record.FLAG_SETTLE.Value && record.NOTIONAL_THB.Value > 0;

                if (flagSettle) { 
                    SCECheckRecords = DealUIP.CheckSCE(SessionInfo, record, ProductId);
                }

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
        public static object SubmitDeal(string strOverApprover, string strOverComment, string record, string strProductId)
        {
            DA_TRN TrnInfo = JsonConvert.DeserializeObject<DA_TRN>(record);
            return DealUIP.SubmitFIDeal(SessionInfo
                                        , TrnInfo
                                        , strOverApprover
                                        , strOverComment
                                        , strProductId);
        }

        [WebMethod(EnableSession = true)]
        public static object GetSCERecords(string record,int jtStartIndex, int jtPageSize)
        {
            List<LimitCheckModel> temp = JsonConvert.DeserializeObject<List<LimitCheckModel>>(record);
            return new
            {
                Result = "OK",
                Records = jtPageSize > 0 ? temp.Skip(jtStartIndex).Take(jtPageSize).ToList() :  temp,
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
        public static object SendFIReport(string TransID)
        {
            return DealUIP.SendFIReport(SessionInfo, TransID);
        }
        
        [WebMethod(EnableSession = true)]
        public static object GetCounterpartyByName(string name_startsWith)
        {
            return CounterpartyUIP.GetCounterpartyByName(name_startsWith.ToUpper());            

        }
        [WebMethod(EnableSession = true)]
        public static object GetInstrumentByName(string name_startsWith)
        {
            return InstrumentUIP.GetInstrumentByName(SessionInfo,ProductCode.BOND, name_startsWith.ToUpper());
            

        }
        [WebMethod(EnableSession = true)]
        public static object GetLotSizeByInstrumentID(string ID)
        {
            try
            {
               MA_INSTRUMENT ins = InstrumentUIP.GetByID(SessionInfo, new Guid(ID));
               if (!ins.LOT_SIZE.HasValue)
                   return new { Result = "ERROR", Message = "Instrument's lot size is empty." };
               return new { Result = "OK", lotsize = ins.LOT_SIZE };
            
             }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object CancelDeal(DA_TRN record)
        {
            return DealViewUIP.CancelDeal(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object CallTBMA(string instrumentid, string setdate, double yield, double cprice, string ytype, bool y2p)
        {
            try
            {
                MA_TBMA_CONFIG config = LookupUIP.GetTBMAConfig(SessionInfo);

                string username, userpassword, token, Key, ErrorMessage;
                username = config.TBMA_CAL_USERNAME;
                userpassword = config.TBMA_CAL_PASSWORD;
                MA_INSTRUMENT ins = InstrumentUIP.GetByID(SessionInfo, new Guid(instrumentid));
                if (ins == null)
                    throw new Exception("Instrument is not found");

                LoggingHelper.Info("TBMA Calculation Service has been called by " + SessionInfo.UserLogon);

                //Step 1 Create new instant object
                ThaiBMACalc.ThaiBMA_Claculation_Service calc = new ThaiBMACalc.ThaiBMA_Claculation_Service();  //Service Object
                ThaiBMACalc.BondFactor BF = new ThaiBMACalc.BondFactor(); //input object
                ThaiBMACalc.AuthenHeader header = new ThaiBMACalc.AuthenHeader();  //authen object
                //Step 2 Get token
                Authen.ThaiBMA_Calculation_Auth authen = new Authen.ThaiBMA_Calculation_Auth();
                token = authen.GetToken(username);

                //Step 3 Get Key for access
                Key = GetKey.getKeyLogin(token, username, userpassword);
                header.key = Key;
                header.username = username;

                //Step 4 Set auhen value
                calc.AuthenHeaderValue = header;

                //Step 5 Set input value to object
                BF.Symbol = ins.LABEL;
                BF.SettlementDate = DateTime.ParseExact(setdate, "dd/MM/yyyy", null);
                BF.TradeDateAndTime = System.DateTime.Now;
                BF.Yield = yield;
                BF.Percent_Price = cprice;
                BF.isYield2Price = y2p;
                BF.isCallPutOption = false;
                BF.Unit = 1;
                BF.PriceType = ThaiBMACalc.PriceType.Clean;

                if (ins.LABEL.StartsWith("ILB"))
                    BF.isILB = true;
                
                if (ytype == "DM")
                    BF.YieldType = ThaiBMACalc.YieldType.DM;
                else
                    BF.YieldType = ThaiBMACalc.YieldType.YTM;

                //Step 6 Call calc method
                ThaiBMACalc.CalculationOutput result = calc.BondCalculation(BF);
                ThaiBMACalc.ServiceError sresult = (ThaiBMACalc.ServiceError)result.ServiceResult;

                //Error while calling service
                if (sresult != null && !sresult.Result)
                {
                    ErrorMessage = sresult.ErrorMessage;
                    string ErrorNo = sresult.ErrorNo;
                    bool rtn = sresult.Result;
                    string attime = sresult.TimeStamp.ToString();
                    LoggingHelper.Error("ThaiBMA service is fail. " + ErrorMessage);
                    return new { Result = "ERROR", Message = "ThaiBMA service is fail. " + ErrorMessage };
                }

                if ((result.CalcError == null) && (result.CalcResult != null))
                {
                    ThaiBMACalc.CalcResult myResult = (ThaiBMACalc.CalcResult)result.CalcResult;

                    //Calculation Result
                    double RGrossPrice = 0;
                    double RCleanPrice = 0;
                    double RYield = 0;
                    
                    if (myResult.Symbol.StartsWith("ILB"))
                    {
                        RCleanPrice = (double)myResult.Percent_Unadjusted_CleanPrice;
                        RYield = (double)myResult.Percent_RealYield;
                        RGrossPrice = (double)myResult.Percent_Unadjusted_GrossPrice;
                    }
                    else
                    {
                        RYield = ytype == "DM" ? (double)myResult.Percent_DM : (double)myResult.Percent_Yield;
                        RCleanPrice = (double)myResult.Percent_CleanPrice;
                        RGrossPrice = (double)myResult.Percent_GrossPrice;
                    }
                    return new { Result = "OK", gprice = RGrossPrice, cprice = RCleanPrice, yield = RYield };
                    //.... and more
                }
                else
                {
                    Type error = result.CalcError.GetType();
                    IList<PropertyInfo> props = new List<PropertyInfo>(error.GetProperties());

                    string errmsg = string.Join(",", props.Where(p => p.GetValue(result.CalcError, null).ToString() != "").Select(p => p.GetValue(result.CalcError, null)).ToList());

                    LoggingHelper.Error("ThaiBMA Caculation is fail. " + errmsg);
                    return new { Result = "ERROR", Message = "ThaiBMA Caculation is fail. " + errmsg};
                }

            }
            catch (Exception ex)
            {
                LoggingHelper.Error("ThaiBMA service is fail. " + ex.Message);
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        
    }
}