using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Business.Deal;
using KK.DealMaker.Business.Master;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.UIProcessComponent.Common;

namespace KK.DealMaker.UIProcessComponent.View
{
    public class DealViewUIP : BaseUIP
    {
        public static object GetDealInquiryByFilter(SessionInfo sessioninfo
                                                    , string strDMKNo
                                                    , string strOPICNo
                                                    , string strProduct
                                                    , string strCtpy
                                                    , string strPortfolio
                                                    , string strTradeDate
                                                    , string strEffDate
                                                    , string strMatDate
                                                    , string strInstrument
                                                    , string strUser
                                                    , string strStatus
                                                    , string strOverStatus
                                                    , string strProcDate
                                                    , string strSettleStatus
                                                    , int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {                
                //Sorting
                string[] sortsp = jtSorting.Split(' ');
                
                var query = GetDealInquiryData(sessioninfo
                                                , strDMKNo
                                                , strOPICNo
                                                , strProduct
                                                , strCtpy
                                                , strPortfolio
                                                , strTradeDate
                                                , strEffDate
                                                , strMatDate
                                                , strInstrument
                                                , strUser
                                                , strStatus
                                                , strOverStatus
                                                , strProcDate
                                                , strSettleStatus).AsQueryable().OrderBy(sortsp[0], sortsp[1]).ToList();
                
                //Return result to jTable
                return new { Result = "OK",
                             Records = jtPageSize > 0 ? query.Skip(jtStartIndex).Take(jtPageSize).ToList() : query,
                             TotalRecordCount = query.Count
                };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static List<DealViewModel> GetDealInquiryData(SessionInfo sessioninfo
                                                            , string strDMKNo
                                                            , string strOPICNo
                                                            , string strProduct
                                                            , string strCtpy
                                                            , string strPortfolio
                                                            , string strTradeDate
                                                            , string strEffDate
                                                            , string strMatDate
                                                            , string strInstrument
                                                            , string strUser
                                                            , string strStatus
                                                            , string strOverStatus
                                                            , string strProcDate
                                                            , string strSettleStatus)
        {
            DealBusiness _dealBusiness = new DealBusiness();
            UserBusiness _userBusiness = new UserBusiness();
            LookupBusiness _lookupBusiness = new LookupBusiness();

            //Get data from database
            List<DA_TRN> trns = _dealBusiness.GetDealInquiryByFilter(sessioninfo
                                                                        , strDMKNo
                                                                        , strOPICNo
                                                                        , strProduct
                                                                        , strCtpy
                                                                        , strPortfolio
                                                                        , strTradeDate
                                                                        , strEffDate
                                                                        , strMatDate
                                                                        , strInstrument
                                                                        , strUser
                                                                        , strStatus
                                                                        , strProcDate);
            //Get user data
            List<MA_USER> users = _userBusiness.GetAll();
            List<MA_CURRENCY> ccys = _lookupBusiness.GetCurrencyAll();
            List<MA_FREQ_TYPE> freq = _lookupBusiness.GetFreqTypeAll();
            
            return (from t in trns
                         join user in users on t.LOG.INSERTBYUSERID equals user.ID into ljuser
                         from inputuser in ljuser.DefaultIfEmpty()
                         join tempccy1 in ccys on t.FIRST.CCY_ID equals tempccy1.ID into ljccy1
                         from ccy1 in ljccy1.DefaultIfEmpty()
                         join tempccy2 in ccys on t.SECOND.CCY_ID equals tempccy2.ID into ljccy2
                         from ccy2 in ljccy2.DefaultIfEmpty()
                         join f1 in freq on t.FIRST.FREQTYPE_ID equals f1.ID into fg1
                         from subfg1 in fg1.DefaultIfEmpty()
                         join f2 in freq on t.SECOND.FREQTYPE_ID equals f2.ID into fg2
                         from subfg2 in fg2.DefaultIfEmpty()
                         select new DealViewModel
                         {
                             ID = t.ID,
                             EntryDate = t.LOG.INSERTDATE,
                             DMK_NO = t.INT_DEAL_NO,
                             OPICS_NO = t.EXT_DEAL_NO,
                             TradeDate = t.TRADE_DATE.Value,
                             EffectiveDate = t.START_DATE,
                             Instrument = t.MA_INSRUMENT.LABEL,
                             MaturityDate = t.MATURITY_DATE,
                             BuySell = t.FLAG_BUYSELL,
                             Product = t.MA_PRODUCT.LABEL,
                             Portfolio = t.MA_PORTFOLIO.LABEL,
                             Counterparty = t.MA_COUTERPARTY.SNAME,
                             Notional1 = t.FIRST.NOTIONAL,
                             PayRec1 = t.FIRST.FLAG_PAYREC,
                             FixedFloat1 = !t.FIRST.FLAG_FIXED.HasValue ? null : t.FIRST.FLAG_FIXED.Value ? "FIXED" : "FLOAT",
                             Rate1 = t.FIRST.RATE,
                             Fixing1 = t.FIRST.FIRSTFIXINGAMT,
                             SwapPoint1 = t.FIRST.SWAP_POINT,
                             Notional2 = t.SECOND.NOTIONAL,
                             PayRec2 = t.SECOND.FLAG_PAYREC,
                             FixedFloat2 = !t.SECOND.FLAG_FIXED.HasValue ? null : t.SECOND.FLAG_FIXED == true ? "FIXED" : "FLOAT",
                             Rate2 = t.SECOND.RATE,
                             Fixing2 = t.SECOND.FIRSTFIXINGAMT,
                             SwapPoint2 = t.SECOND.SWAP_POINT,
                             Status = t.MA_STATUS.LABEL,
                             KKContribute = t.KK_CONTRIBUTE,
                             BotContribute = t.BOT_CONTRIBUTE                             ,
                             LimitOverwrite = string.IsNullOrEmpty(t.OVER_APPROVER) ? "No" : t.OVER_AMOUNT > 0 && t.OVER_SETTL_AMOUNT > 0 ? "Yes" : t.OVER_AMOUNT > 0 ? "PCE" : "SET",
                             LimitApprover = t.OVER_APPROVER,
                             Trader = inputuser != null ? inputuser.USERCODE : null,
                             Remark = t.REMARK,
                             CCY1 = ccy1 != null ? ccy1.LABEL : null,
                             CCY2 = ccy2 != null ? ccy2.LABEL : null,
                             SettlementLimit = !t.FLAG_SETTLE.HasValue || !t.FLAG_SETTLE.Value ? "No" : "Yes",
                             Freq1 = subfg1 != null ? subfg1.USERCODE : null,
                             Freq2 = subfg2 != null ? subfg2.USERCODE : null,
                             OpicsTrader = t.INSERT_BY_EXT,
                             TBMA_SENT = t.DA_TMBA_EXTENSION == null ? "N/A" : t.DA_TMBA_EXTENSION.SEND_DATE != null ? "Yes" : "No"
                         }).Where(p => p.LimitOverwrite == strOverStatus || String.IsNullOrEmpty(strOverStatus)) 
                         .Where(t => t.SettlementLimit == strSettleStatus || string.IsNullOrEmpty(strSettleStatus)).ToList();
        }

        public static object CancelDeal(SessionInfo sessioninfo, DA_TRN trn)
        {
            try
            {
                LookupBusiness _lookupBusiness = new LookupBusiness();
                UserBusiness _userBusiness = new UserBusiness();
                DealBusiness _dealbusiness = new DealBusiness();
                DA_TRN t = _dealbusiness.CancelDeal(sessioninfo, trn);
                DA_TRN t2 = _dealbusiness.GetDealByDealNo(t.INT_DEAL_NO,t.VERSION).FirstOrDefault(p => p.ID != t.ID);
                if (t2 != null) _dealbusiness.CancelDeal(sessioninfo, t2);
                var inputuser = _userBusiness.GetAll().FirstOrDefault(u => u.ID == t.LOG.INSERTBYUSERID);

                var query = new DealViewModel
                            {
                                ID = t.ID,
                                EntryDate = t.LOG.INSERTDATE,
                                DMK_NO = t.INT_DEAL_NO
                                ,
                                OPICS_NO = t.EXT_DEAL_NO,
                                TradeDate = t.TRADE_DATE.Value
                                ,
                                EffectiveDate = t.START_DATE,
                                Instrument = t.MA_INSRUMENT.LABEL
                                ,
                                MaturityDate = t.MATURITY_DATE,
                                BuySell = t.FLAG_BUYSELL
                                ,
                                Product = t.MA_PRODUCT.LABEL,
                                Portfolio = t.MA_PORTFOLIO.LABEL
                                ,
                                Counterparty = t.MA_COUTERPARTY.SNAME,
                                Notional1 = t.FIRST.NOTIONAL
                                ,
                                PayRec1 = t.FIRST.FLAG_PAYREC,
                                FixedFloat1 = !t.FIRST.FLAG_FIXED.HasValue ? null : t.FIRST.FLAG_FIXED.Value ? "FIXED" : "FLOAT"
                                ,
                                Rate1 = t.FIRST.RATE,
                                Fixing1 = t.FIRST.FIRSTFIXINGAMT,
                                SwapPoint1 = t.FIRST.SWAP_POINT,
                                Notional2 = t.SECOND.NOTIONAL
                                ,
                                PayRec2 = t.SECOND.FLAG_PAYREC,
                                FixedFloat2 = !t.SECOND.FLAG_FIXED.HasValue ? null : t.SECOND.FLAG_FIXED == true ? "FIXED" : "FLOAT"
                                ,
                                Rate2 = t.SECOND.RATE,
                                Fixing2 = t.SECOND.FIRSTFIXINGAMT,
                                SwapPoint2 = t.SECOND.SWAP_POINT,
                                Status =  _lookupBusiness.GetStatusAll().FirstOrDefault(p => p.ID == t.STATUS_ID).LABEL
                                ,
                                KKContribute = t.KK_CONTRIBUTE,
                                BotContribute = t.BOT_CONTRIBUTE
                                ,
                                LimitOverwrite = string.IsNullOrEmpty(t.OVER_APPROVER) ? "No" : t.OVER_AMOUNT > 0 && t.OVER_SETTL_AMOUNT > 0 ? "Yes" : t.OVER_AMOUNT > 0 ? "PCE" : "SET"
                                ,
                                LimitApprover = t.OVER_APPROVER
                                ,
                                Trader = inputuser != null ? inputuser.USERCODE : ""
                                ,
                                Remark = t.REMARK
                            };
                return new { Result = "OK", Record = query, DealPairID = t2 != null? t2.ID.ToString() : string.Empty };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        //public static object GetSwapLegDetailByID(SessionInfo sessioninfo, Guid ID)
        //{
        //    try
        //    {
        //        DealBusiness _dealbusiness = new DealBusiness();

        //        DA_TRN trn = _dealbusiness.GetByID(ID);

        //        DealViewModel view = new DealViewModel();
        //        view.ID = trn.ID;
        //        view.PayRec1 = trn.FIRST.FLAG_PAYREC;
        //        view.FixedFloat1 = trn.FIRST.FLAG_FIXED.Value ? CouponType.FIXED.ToString() : CouponType.FLOAT.ToString();
        //        view.Fixing1 = trn.FIRST.FIRSTFIXINGAMT.Value;
        //        view.Rate1 = trn.FIRST.RATE;
        //        view.PayRec2 = trn.SECOND.FLAG_PAYREC;
        //        view.FixedFloat2 = trn.SECOND.FLAG_FIXED.Value ? CouponType.FIXED.ToString() : CouponType.FLOAT.ToString();
        //        view.Fixing2 = trn.SECOND.FIRSTFIXINGAMT.Value;
        //        view.Rate2 = trn.SECOND.RATE;

        //        //Return result to jTable
        //        return new { Result = "OK", Records = view, TotalRecordCount = 1 };
        //    }
        //    catch (BusinessWorkflowsException bex)
        //    {
        //        return new { Result = "ERROR", Message = bex.Message };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new { Result = "ERROR", Message = ex.Message };
        //    }
        //}
    }
}
