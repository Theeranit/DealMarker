using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Business.Deal;
using KK.DealMaker.Business.Master;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.Core.Constraint;
using System.IO;
namespace KK.DealMaker.UIProcessComponent.Admin
{
    public class DealUIP : BaseUIP
    {
        #region FI Product
            public static object SubmitFIDeal(SessionInfo sessioninfo
                                                , DA_TRN trn
                                                , string strOverApprover
                                                , string strOverComment
                                                , string strProductId)
            {
                try
                {
                    DealBusiness _dealBusiness = new DealBusiness();

                    string strDealNO = _dealBusiness.SubmitFIDeal(sessioninfo
                                                                    , trn
                                                                    , strOverApprover
                                                                    , strOverComment
                                                                    , strProductId);

                    return new { Result = "OK", Message = strDealNO };
                }
                catch (Exception ex)
                {
                    return new { Result = "ERROR", Message = ex.Message };
                }
            }

            public static DA_TRN GenerateFITransactionObject(SessionInfo sessionInfo
                                                            , string strTradeDate
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
                                                            , string strProductId)

            {
                DealBusiness _dealBusiness = new DealBusiness();

                return _dealBusiness.GenerateFITransactionObject(sessionInfo
                                                                , strTradeDate
                                                                , strBuySell
                                                                , strInstrument
                                                                , strCtpy
                                                                , strPortfolio
                                                                , strSettlementDate
                                                                , strYield
                                                                , strUnit
                                                                , strCleanPrice
                                                                , strGrossPrice
                                                                , strNotional
                                                                , strCCY
                                                                , strPceFlag
                                                                , strSettleFlag
                                                                , strYeildType
                                                                , strReportBy
                                                                , strPurpose
                                                                , strTerm
                                                                , strRate
                                                                , strTBMARemark
                                                                , strRemark
                                                                , strProductId);

            }

            public static object SendFIReport(SessionInfo sessioninfo, string IDs)
            {
                try
                {

                    string[] TrnsID = IDs.Split(',');
                    DealBusiness _dealBusiness = new DealBusiness();
                    LookupBusiness _lookupBusiness = new LookupBusiness();
                    MA_TBMA_CONFIG config = _lookupBusiness.GetTBMAConfig(sessioninfo);
                    Guid productID = _lookupBusiness.GetProductAll().FirstOrDefault(p => p.LABEL == ProductCode.BOND.ToString()).ID;
                    List<DA_TRN> trns = _dealBusiness.GetDealInternalByProcessDate(sessioninfo.Process.CurrentDate)
                                       .Where(a => a.PRODUCT_ID.Value == productID && TrnsID.Contains(a.ID.ToString()) && a.DA_TMBA_EXTENSION.SEND_DATE == null).ToList();
                    var validateTBMA_NAME = (from o1 in trns
                                            where string.IsNullOrEmpty(o1.MA_COUTERPARTY.TBMA_NAME)
                                            select o1.MA_COUTERPARTY.SNAME).Distinct().ToArray();

                    //verify required info
                    if (validateTBMA_NAME.Length > 0) {
                        throw CreateException(new Exception(), "TBMA_NAME can't be empty. Please input TBMA_NAME on counterparty " + string.Join(",", validateTBMA_NAME));
                    }
                    if (config == null)
                        throw CreateException(new Exception(), "Cannot get configurations. Please contact administrator");

                    foreach (DA_TRN trn in trns)
                    {
                        _dealBusiness.CreatingTBMAReportFile(sessioninfo, trn, config);
                    }

                    Guid[] TrnsIDSend = trns.Select(t => t.ID).ToArray();
                    _dealBusiness.UpdateFISendReport(sessioninfo, TrnsIDSend);   
                    return new
                    {
                        Result = "OK",
                        Message = String.Format("Sending report completed")
                    };
                }
                catch (Exception ex)
                {
                    return new { Result = "ERROR", Message = ex.Message };
                }
            }
        #endregion

        #region Swap Product
            public static object SubmitSwapDeal(SessionInfo sessioninfo
                                                , DA_TRN trn
                                                , string strOverApprover
                                                , string strOverComment
                                                , string strProductId)
            {
                try
                {
                    DealBusiness _dealBusiness = new DealBusiness();


                    string strDealNO = _dealBusiness.SubmitSwapDeal(sessioninfo
                                                                    , trn
                                                                    , strOverApprover
                                                                    , strOverComment
                                                                    , strProductId);

                    return new { Result = "OK", Message = strDealNO };
                }
                catch (Exception ex)
                {
                    return new { Result = "ERROR", Message = ex.Message };
                }
            }
        
            public static DA_TRN GenerateSwapTransactionObject(SessionInfo sessionInfo, string strTradeDate, string strInstrument, string strCtpy, string strPortfolio
                                                                , string strEffDate, string strMatDate, string strNotional1
                                                                , string strCCY1, string strFFL1, string strFFix1, string strRate1, string strFreq1
                                                                , string strNotional2, string strCCY2, string strFFL2, string strFFix2, string strRate2, string strFreq2
                                                                , int intDaySpan,string strRemark, string strProductId)
            {
                DealBusiness _dealBusiness = new DealBusiness();
                return _dealBusiness.GenerateSwapTransactionObject(sessionInfo, strTradeDate, strInstrument, strCtpy, strPortfolio
                                                                    , strEffDate, strMatDate, strNotional1
                                                                    , strCCY1, strFFL1, strFFix1, strRate1, strFreq1
                                                                    , strNotional2, strCCY2, strFFL2, strFFix2, strRate2, strFreq2
                                                                    , intDaySpan, strRemark, strProductId);
                
                //return trn;
            }

            //public static List<DA_TRN_CASHFLOW> GenerateCashflows(DA_TRN trn, int intDaySpan)
            //{
            //    DealBusiness _dealBusiness = new DealBusiness();

            //    return _dealBusiness.GenerateCashFlows(trn, intDaySpan);
            //}

        #endregion      
  
        #region FX Spot Product
         public static DA_TRN GenerateFXSpotTransactionObject(SessionInfo sessionInfo, string strTradeDate, string strSpotDate, string strCtpy, string strPortfolio
                                                                , string strCurrencyPair, string strBS, string strContractCcy, string strCounterCcy
                                                                , string strSpotRate, string strContractAmt, string strCounterAmt, string strRemark, bool settleFlag, string strProductId)
        {
            DealBusiness _dealBusiness = new DealBusiness();
            return _dealBusiness.GenerateFXSpotTransactionObject(sessionInfo, strTradeDate, strSpotDate, strCtpy, strPortfolio
                                                                , strCurrencyPair, strBS, strContractCcy, strCounterCcy
                                                                , strSpotRate, strContractAmt, strCounterAmt, strRemark, settleFlag, strProductId);       
        }

         public static object SubmitFXSpotDeal(SessionInfo sessioninfo
                                                , DA_TRN trn
                                                , string strOverApprover
                                                , string strOverComment
                                                , string strProductId)
         {
             try
             {
                 DealBusiness _dealBusiness = new DealBusiness();


                 string strDealNO = _dealBusiness.SubmitFXDeal(sessioninfo
                                                                 , trn
                                                                 , strOverApprover
                                                                 , strOverComment
                                                                 , strProductId);

                 return new { Result = "OK", Message = strDealNO };
             }
             catch (Exception ex)
             {
                 return new { Result = "ERROR", Message = ex.Message };
             }
         }
        #endregion      

        #region FX Forwawrd Product
         public static DA_TRN GenerateFXForwardTransactionObject(SessionInfo sessionInfo, string strTradeDate, string strSpotDate, string strSetDate, string strCtpy, string strPortfolio
                                                                , string strCurrencyPair, string strBS, string strContractCcy, string strCounterCcy
                                                                , string strSpotRate, string strSwapPoint, string strContractAmt, string strCounterAmt, string strRemark, bool settleFlag, string strProductId)
         {
             DealBusiness _dealBusiness = new DealBusiness();
             return _dealBusiness.GenerateFXForwardTransactionObject(sessionInfo, strTradeDate, strSpotDate, strSetDate, strCtpy, strPortfolio
                                                                 , strCurrencyPair, strBS, strContractCcy, strCounterCcy
                                                                 , strSpotRate, strSwapPoint, strContractAmt, strCounterAmt, strRemark, settleFlag, strProductId);
         }
         public static object SubmitFXForwardDeal(SessionInfo sessioninfo
                                                , DA_TRN trn
                                                , string strOverApprover
                                                , string strOverComment
                                                , string strProductId)
         {
             try
             {
                 DealBusiness _dealBusiness = new DealBusiness();


                 string strDealNO = _dealBusiness.SubmitFXDeal(sessioninfo
                                                                 , trn
                                                                 , strOverApprover
                                                                 , strOverComment
                                                                 , strProductId);

                 return new { Result = "OK", Message = strDealNO };
             }
             catch (Exception ex)
             {
                 return new { Result = "ERROR", Message = ex.Message };
             }
         }
         #endregion

        #region FX Swap Product
         public static DA_TRN GenerateFXSwapTransactionObject1(SessionInfo sessionInfo, string strTradeDate, string strCtpy, string strPortfolio, string strCurrencyPair
                                                                , string strContractCcy, string strCounterCcy, string strSpotRate
                                                                , string strBSNear, string strSetDateNear, string strSwapPointNear
                                                                , string strContractAmtNear, string strCounterAmtNear, string strSpotDate, string strRemark,bool settleFlag,string strProductId1)
         {
             DealBusiness _dealBusiness = new DealBusiness();
             return _dealBusiness.GenerateFXSwapTransactionObject1(sessionInfo, strTradeDate, strCtpy, strPortfolio, strCurrencyPair
                                                               , strContractCcy, strCounterCcy, strSpotRate
                                                               , strBSNear, strSetDateNear, strSwapPointNear, strContractAmtNear, strCounterAmtNear, strSpotDate, strRemark, settleFlag, strProductId1);
         }
         public static DA_TRN GenerateFXSwapTransactionObject2(SessionInfo sessionInfo, string strTradeDate, string strCtpy, string strPortfolio, string strCurrencyPair
                                                                , string strContractCcy, string strCounterCcy, string strSpotRate
                                                                , string strBSFar, string strSetDateFar, string strSwapPointFar
                                                                , string strContractAmtFar, string strCounterAmtFar, string strSpotDate, string strRemark, bool settleFlag, int Version)
         {
             DealBusiness _dealBusiness = new DealBusiness();
             return _dealBusiness.GenerateFXSwapTransactionObject2(sessionInfo, strTradeDate, strCtpy, strPortfolio, strCurrencyPair
                                                               , strContractCcy, strCounterCcy, strSpotRate
                                                               , strBSFar, strSetDateFar, strSwapPointFar
                                                               , strContractAmtFar, strCounterAmtFar, strSpotDate, strRemark, settleFlag, Version);
         }

         public static object SubmitFXSwapDeal(SessionInfo sessioninfo
                                                , DA_TRN trn1
                                                , DA_TRN trn2
                                                , string strOverApprover
                                                , string strOverComment
                                                , string strProductId1
                                                , string strProductId2)
         {
             try
             {
                 DealBusiness _dealBusiness = new DealBusiness();

                 string strDealNO1 = _dealBusiness.SubmitFXDeal(sessioninfo
                                                                 , trn1
                                                                 , strOverApprover
                                                                 , strOverComment
                                                                 , strProductId1);
                 trn2.INT_DEAL_NO = strDealNO1;
                 string strDealNO2 = _dealBusiness.SubmitFXDeal(sessioninfo
                                                                 , trn2
                                                                 , strOverApprover
                                                                 , strOverComment
                                                                 , strProductId2
                                                                 );
                 return new { Result = "OK", Message = strDealNO1 };
             }
             catch (Exception ex)
             {
                 return new { Result = "ERROR", Message = ex.Message };
             }
         }

#endregion

         #region Repo Product
          public static DA_TRN GenerateRepoTransactionObject(SessionInfo sessioninfo
                                                            , string  strTradeDate
                                                            , string strBuySell
                                                            , string strInstrument
                                                            , string strCtpy
                                                            , string strPortfolio
                                                            , string strEffectiveDate
                                                            , string strMaturityDate
                                                            , string strNotional
                                                            , string strProductId
                                                            , string strRemark)
          {
             DealBusiness _dealBusiness = new DealBusiness();
             return _dealBusiness.GenerateRepoTransactionObject(sessioninfo, strTradeDate, strBuySell, strInstrument
                                                            , strCtpy, strPortfolio, strEffectiveDate, strMaturityDate
                                                            , strNotional, strProductId, strRemark);   
          }
          public static object SubmitRepoDeal(SessionInfo sessioninfo
                                                  , DA_TRN trn
                                                  , string strOverApprover
                                                  , string strOverComment
                                                  , string strProductId)
          {
              try
              {
                  DealBusiness _dealBusiness = new DealBusiness();


                  string strDealNO = _dealBusiness.SubmitRepoDeal(sessioninfo
                                                                  , trn
                                                                  , strOverApprover
                                                                  , strOverComment
                                                                  , strProductId);

                  return new { Result = "OK", Message = strDealNO };
              }
              catch (Exception ex)
              {
                  return new { Result = "ERROR", Message = ex.Message };
              }
          }

          #endregion

        #region Limit Check
        public static LimitDisplayModel CheckPCE(SessionInfo sessioninfo, DA_TRN trn, string strExcludeID)
        {
            DealBusiness _dealBusiness = new DealBusiness();

            return _dealBusiness.CheckPCE(sessioninfo, trn, strExcludeID);
        }

        public static LimitDisplayModel CheckSCE(SessionInfo sessioninfo, DA_TRN trn, string strExcludeID)
        {
            DealBusiness _dealBusiness = new DealBusiness();

            return _dealBusiness.CheckSCE(sessioninfo, trn, strExcludeID);
        }

        public static LimitDisplayModel CheckFXSwapPCE(SessionInfo sessioninfo, DA_TRN trn1, DA_TRN trn2, string strExcludeID)
        {
            DealBusiness _dealBusiness = new DealBusiness();

            return _dealBusiness.CheckFXSwapPCE(sessioninfo, trn1, trn2, strExcludeID);
        }

        public static LimitDisplayModel CheckFXSwapSCE(SessionInfo sessioninfo, DA_TRN trn1, DA_TRN trn2, string strExcludeID)
        {
            DealBusiness _dealBusiness = new DealBusiness();

            return _dealBusiness.CheckFXSwapSCE(sessioninfo, trn1, trn2, strExcludeID);
        }

        public static LimitDisplayModel CheckSwapSCE(SessionInfo sessioninfo, DA_TRN trn, string strExcludeID)
        {
            DealBusiness _dealBusiness = new DealBusiness();

            return _dealBusiness.CheckSwapSCE(sessioninfo, trn, strExcludeID);
        }

        public static LimitDisplayModel CheckCountryLimit(SessionInfo sessioninfo, DA_TRN trn, string strExcludeID)
        {
            DealBusiness _dealBusiness = new DealBusiness();

            return _dealBusiness.CheckCountryLimit(sessioninfo, trn, strExcludeID);
        }

        public static LimitDisplayModel CheckFXSwapCountryLimit(SessionInfo sessioninfo, DA_TRN trn1, DA_TRN trn2, string strExcludeID)
        {
            DealBusiness _dealBusiness = new DealBusiness();

            return _dealBusiness.CheckFXSwapCountryLimit(sessioninfo, trn1, trn2, strExcludeID);
        }

        public static LimitDisplayModel CheckSwapCountryLimit(SessionInfo sessioninfo, DA_TRN trn, string strExcludeID)
        {
            DealBusiness _dealBusiness = new DealBusiness();

            return _dealBusiness.CheckSwapCountryLimit(sessioninfo, trn, strExcludeID);
        }
        #endregion

        public static DA_TRN GetByID(Guid id) 
        {
                DealBusiness _dealBusiness = new DealBusiness();
                return _dealBusiness.GetByID(id);
        }

        public static DA_TRN GetFXSwapPair(SessionInfo sessioninfo, string strIntDealNo, int intVersion, Guid id)
        {
             DealBusiness _dealBusiness = new DealBusiness();

             return _dealBusiness.GetFXSwapPair(strIntDealNo, intVersion, id);
         }
        public static List<FIDealModel> GetDealInternalByProcessDate(SessionInfo sessioninfo)
        {
            DealBusiness _dealBusiness = new DealBusiness();
            UserBusiness _userBusiness = new UserBusiness();
            LookupBusiness _lookupBusiness = new LookupBusiness();
            List<MA_CURRENCY> ccys = _lookupBusiness.GetCurrencyAll();
            List<MA_USER> users = _userBusiness.GetAll();
            Guid productID = _lookupBusiness.GetProductAll().FirstOrDefault(p => p.LABEL == ProductCode.BOND.ToString()).ID;
            List<DA_TRN> trns = _dealBusiness.GetDealInternalByProcessDate(sessioninfo.Process.CurrentDate);

            return (from t in trns.Where(a => a.PRODUCT_ID.Value == productID && a.DA_TMBA_EXTENSION != null)
                      join user in users on t.LOG.INSERTBYUSERID equals user.ID into ljuser
                      from inputuser in ljuser.DefaultIfEmpty()
                      join tempccy1 in ccys on t.FIRST.CCY_ID equals tempccy1.ID  into ljccy1
                      from ccy1 in ljccy1.DefaultIfEmpty()
                      join tbmauser in users on  t.DA_TMBA_EXTENSION.SENDER_ID equals tbmauser.ID  into ljtbmauser
                      from tbmainputuser in ljtbmauser.DefaultIfEmpty()
                      orderby t.LOG.INSERTDATE descending
                      select new FIDealModel
                      {
                          ID = t.ID,
                          DMK_NO = t.INT_DEAL_NO,
                          OPICS_NO = t.EXT_DEAL_NO,
                          InsertState = t.MA_STATUS.LABEL == StatusCode.CANCELLED.ToString()
                                            ? "Cancelled"
                                            : t.DA_TMBA_EXTENSION.SEND_DATE != null 
                                                ? "Sent"
                                                : 1800 - DateTime.Now.Subtract(t.LOG.INSERTDATE).TotalSeconds <=0 
                                                    ? "Late"
                                                    : (1800 - Math.Round( DateTime.Now.Subtract(t.LOG.INSERTDATE).TotalSeconds)).ToString() ,
                          TradeDate = t.TRADE_DATE.Value,
                          MaturityDate = t.MATURITY_DATE,
                          Instrument = t.MA_INSRUMENT.LABEL,
                          BuySell = t.FLAG_BUYSELL,
                          Portfolio = t.MA_PORTFOLIO.LABEL,
                          Counterparty = t.MA_COUTERPARTY.SNAME,
                          Yield = t.FIRST.RATE.Value,
                          GrossValue = t.FIRST.NOTIONAL.Value,
                          CCY = ccy1 != null ? ccy1.LABEL : null,
                          Trader = inputuser != null ? inputuser.USERCODE : null,
                          Status = t.MA_STATUS.LABEL,
                          PCE = t.KK_CONTRIBUTE.HasValue ? t.KK_CONTRIBUTE.Value : 0,
                          Sender = tbmainputuser != null ? tbmainputuser.USERCODE : null,
                          Unit = t.DA_TMBA_EXTENSION != null ? t.DA_TMBA_EXTENSION.UNIT : 0,
                          CleanPrice = t.DA_TMBA_EXTENSION != null ? t.DA_TMBA_EXTENSION.CLEAN_PRICE : 0,
                          GrossPrice = t.DA_TMBA_EXTENSION != null ? t.DA_TMBA_EXTENSION.GROSS_PRICE : 0,
                          YieldType = t.DA_TMBA_EXTENSION != null ? t.DA_TMBA_EXTENSION.YIELD_TYPE : string.Empty,
                          ReporyBy = t.DA_TMBA_EXTENSION != null ? t.DA_TMBA_EXTENSION.IS_REPORT_CLEAN ? "Clean Price" : "Gross Price" : string.Empty,
                          Purpose = t.DA_TMBA_EXTENSION != null ? t.DA_TMBA_EXTENSION.PURPOSE : string.Empty,
                          Term = t.DA_TMBA_EXTENSION != null && t.DA_TMBA_EXTENSION.TERM.HasValue ? t.DA_TMBA_EXTENSION.TERM.Value : 0,
                          Rate = t.DA_TMBA_EXTENSION != null && t.DA_TMBA_EXTENSION.RATE.HasValue ? t.DA_TMBA_EXTENSION.RATE.Value : 0,
                          TBMA_Remark = t.DA_TMBA_EXTENSION != null ? t.DA_TMBA_EXTENSION.REMARK : string.Empty,
                          SendTime = t.DA_TMBA_EXTENSION != null ? t.DA_TMBA_EXTENSION.SEND_DATE.HasValue ? t.DA_TMBA_EXTENSION.SEND_DATE.Value.ToString("dd MMM yyy HH:mm") : string.Empty : string.Empty,
                          PrimaryMarket = t.FLAG_PCE.Value ? "Yes" : "No",
                          NonDVP = t.FLAG_SETTLE.Value ? "Yes" : "No"
                      }).ToList();
         }
    }
}
