using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.OpicsData;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.Business.Deal;
using KK.DealMaker.Business.External;
using KK.DealMaker.UIProcessComponent.Common;
using KK.DealMaker.Business.Reconcile;
using KK.DealMaker.Business.Master;

namespace KK.DealMaker.UIProcessComponent.Deal
{
    public class ReconcileUIP : BaseUIP
    {
        private static ILookupValuesRepository _lookupvaluesRepository { get { return RepositorySesssion.GetRepository(); } }
        
        public static object GetDealByProcessDateStatusCode(SessionInfo sessioninfo, DateTime processdate, StatusCode statuscode, int startIndex, int count, string sorting)
        {
            DealBusiness _dealBusiness = new DealBusiness();
            OpicsBusiness _opicsBusiness = new OpicsBusiness();
            LookupBusiness _lookupBusiness = new LookupBusiness();
            List<DA_TRN> deals = null;
            List<DA_TRN> dealExts = null;
            List<DEALModel> opicdeals = null;

            try
            {
                //Get data from database
                if (statuscode == StatusCode.MATCHED)
                {
                    LoggingHelper.Debug("Get DMK Deal is matched on " + processdate.ToString());
                    
                    deals = _dealBusiness.GetDealByProcessDate(processdate, StatusCode.MATCHED);
                    dealExts = _dealBusiness.GetImportedDealsByProcessDate(sessioninfo.Process.NextDate);

                }
                else
                {
                    LoggingHelper.Debug("Get DMK Deal is not matched on " + processdate.ToString());
                    //Get data from database
                    opicdeals = _opicsBusiness.GetOPICSDealExternal(processdate);
                    
                    //auto match with OPICS deal
                    AutoMatchAndUpdateDeals(sessioninfo, processdate, opicdeals, out deals);
                }

                if (dealExts == null)
                    dealExts = new List<DA_TRN>();

                List<MA_FREQ_TYPE> freq = _lookupBusiness.GetFreqTypeAll();
                List<MA_CURRENCY> ccys = _lookupBusiness.GetCurrencyAll();

                //Sorting
                string[] sortsp = sorting.Split(' ');
                List<DealViewModel> dealviews = (from t in deals
                                                 join p in dealExts on new { t.INT_DEAL_NO, t.FLAG_NEARFAR } equals new { p.INT_DEAL_NO, p.FLAG_NEARFAR } into ps
                                                 join f1 in freq on t.FIRST.FREQTYPE_ID equals f1.ID into fg1
                                                 from subfg1 in fg1.DefaultIfEmpty()
                                                 join f2 in freq on t.SECOND.FREQTYPE_ID equals f2.ID into fg2
                                                 from subfg2 in fg2.DefaultIfEmpty()
                                                 join tempccy1 in ccys on t.FIRST.CCY_ID equals tempccy1.ID into ljccy1
                                                 from ccy1 in ljccy1.DefaultIfEmpty()
                                                 join tempccy2 in ccys on t.SECOND.CCY_ID equals tempccy2.ID into ljccy2
                                                 from ccy2 in ljccy2.DefaultIfEmpty()
                                                 where t.MA_STATUS.LABEL.Contains(statuscode.ToString()) && t.MATURITY_DATE.HasValue ? t.MATURITY_DATE.Value > sessioninfo.Process.CurrentDate : true
                                                 select new DealViewModel
                                                 {
                                                     ID = t.ID,
                                                     EntryDate = t.LOG.INSERTDATE,
                                                     DMK_NO = t.INT_DEAL_NO,
                                                     OPICS_NO = ps == null ? t.EXT_DEAL_NO : string.Join(",", ps.Select(i => i.EXT_DEAL_NO)),
                                                     TradeDate = t.TRADE_DATE.Value,
                                                     EffectiveDate = t.START_DATE,
                                                     Instrument = t.MA_INSRUMENT.LABEL,
                                                     MaturityDate = t.MATURITY_DATE.HasValue ? t.MATURITY_DATE : null,
                                                     BuySell = t.FLAG_BUYSELL,
                                                     Product = t.MA_PRODUCT.LABEL,
                                                     Portfolio = t.MA_PORTFOLIO.LABEL,
                                                     Counterparty = t.MA_COUTERPARTY.SNAME,
                                                     Notional1 = t.FIRST.NOTIONAL,
                                                     PayRec1 = t.FIRST.FLAG_PAYREC,
                                                     FixedFloat1 = !t.FIRST.FLAG_FIXED.HasValue ? null : t.FIRST.FLAG_FIXED == true ? "FIXED" : "FLOAT",
                                                     Fixing1 = t.FIRST.FIRSTFIXINGAMT,
                                                     Rate1 = t.FIRST.RATE,
                                                     SwapPoint1 = t.FIRST.SWAP_POINT,
                                                     Notional2 = t.SECOND.NOTIONAL,
                                                     PayRec2 = t.SECOND.FLAG_PAYREC,
                                                     FixedFloat2 = !t.SECOND.FLAG_FIXED.HasValue ? null : t.SECOND.FLAG_FIXED == true ? "FIXED" : "FLOAT",
                                                     Fixing2 = t.SECOND.FIRSTFIXINGAMT,
                                                     Rate2 = t.SECOND.RATE,
                                                     SwapPoint2 = t.SECOND.SWAP_POINT,
                                                     Status = t.MA_STATUS.LABEL,
                                                     KKContribute = t.KK_CONTRIBUTE,
                                                     BotContribute = t.BOT_CONTRIBUTE,
                                                     Remark = t.REMARK,
                                                     Freq1 = subfg1 != null ? subfg1.USERCODE : null,
                                                     Freq2 = subfg2 != null ? subfg2.USERCODE : null,
                                                     CCY1 = ccy1 != null ? ccy1.LABEL : null,
                                                     CCY2 = ccy2 != null ? ccy2.LABEL : null
                                                 }).AsQueryable().OrderBy(sortsp[0], sortsp[1]).ToList();
                
                //Return result to jTable
                return new
                {
                    Result = "OK",
                    Records = dealviews.Count > 0 ? dealviews.Skip(startIndex).Take(count).ToList() : dealviews,
                    TotalRecordCount = dealviews.Count
                };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetDealExternalByProcessDate(SessionInfo sessioninfo, DateTime processdate, int startIndex, int count, string sorting)
        {
            OpicsBusiness _opicsBusiness = new OpicsBusiness();
            DealBusiness _dealBusiness = new DealBusiness();
            ReconcileBusiness _reconcileBusiness = new ReconcileBusiness();
            List<DealTranModel> DealTrans = new List<DealTranModel>();
            try
            {               
                FlagReconcile(sessioninfo, true);
                LoggingHelper.Debug("Locked Reconcile table " + processdate.ToString());
                LoggingHelper.Debug("Get OPICS Deal on " + processdate.ToString());
                //Get data from database
                List<DEALModel> dealopics = _opicsBusiness.GetOPICSDealExternal(processdate);
                List<DA_TRN> importeddeals = _dealBusiness.GetImportedDealsByProcessDate(sessioninfo.Process.NextDate);

                //Ignore imported DMK Deals
                var query = from o in dealopics
                            where !importeddeals.Any(d => d.EXT_DEAL_NO == o.EXT_DEAL_NO)
                            select o;

                //Import unimported OPIC deals, and engine date < process date
                var passedopicdeals = query.Where(d => d.INSERT_DATE.Date < processdate.Date);

                //todaydeals for display on screen
                var todaydeals = from o in query
                                 where !passedopicdeals.Any(p => p.EXT_DEAL_NO == o.EXT_DEAL_NO)
                                 select o;

                //20140129
                //OPICS deals which are cancelled after reconcile have been run once, must be deleted from DMK
                var cancelleddeals = from o in importeddeals
                                     where !dealopics.Any(p => p.EXT_DEAL_NO == o.EXT_DEAL_NO)
                                     select o;

                if (passedopicdeals != null)
                {
                    LoggingHelper.Debug("Import Passed OPICS Deal");

                    foreach (DEALModel passedopicdeal in passedopicdeals.ToList())
                    {
                        if (ValidateOPICS(passedopicdeal))
                            ImportPassedOPICSDeal(sessioninfo, passedopicdeal, sessioninfo.Process.NextDate, ref DealTrans);
                    }

                    if (cancelleddeals != null)
                    {
                        foreach (DA_TRN trn in cancelleddeals.ToList())
                        {
                            DealTrans.Add(new DealTranModel() { ProductTransaction = (ProductCode)Enum.Parse(typeof(ProductCode), trn.MA_PRODUCT.LABEL.Replace(" ", string.Empty)), Transaction = trn, UpdateStates = UpdateStates.Deleting });
                        }
                    }
                    LoggingHelper.Debug("End Import Passed OPICS Deal as " + passedopicdeals.Count().ToString());
                    
                    if (DealTrans.Count > 0)
                    {
                        LoggingHelper.Debug("UpdateDealReconcile for passed deal on " + processdate.ToString());

                        _reconcileBusiness.UpdateDealReconcile(sessioninfo, DealTrans);
                    }
                }
                
                //Sort order by sorting
                string[] sortsp = sorting.Split(' ');
                IQueryable<DEALModel> orderedRecords = todaydeals.AsQueryable().OrderBy(sortsp[0], sortsp[1]);
                IEnumerable<DEALModel> sortedRecords = orderedRecords.ToList();
                //if (sortsp[1].ToLower() == "desc") sortedRecords = sortedRecords.Reverse();           
                FlagReconcile(sessioninfo, false);
                LoggingHelper.Debug("Un-locked Reconcile table " + processdate.ToString());
                //Return result to jTable
                return new { Result = "OK",
                             Records = count > 0 ? sortedRecords.Skip(startIndex).Take(count).ToList() : sortedRecords,
                             TotalRecordCount = sortedRecords.ToList().Count
                };
            }
            catch (Exception ex)
            {
                FlagReconcile(sessioninfo,false);
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        /// <summary>
        /// Match deal between DMK deal no and OPICS deal no on processing date
        /// </summary>
        /// <param name="sessioninfo"></param>
        /// <param name="processdate"></param>
        /// <param name="DMKDealID"></param>
        /// <param name="OPICSNo"></param>
        /// <returns></returns>
        public static object MatchingDeal(SessionInfo sessioninfo, DateTime processdate, Guid DMKDealID, string OPICSNo)
        {
            DealBusiness _dealBusiness = new DealBusiness();
            OpicsBusiness _opicsBusiness = new OpicsBusiness();
            DA_TRN deal = null;
            string[] OPICSNoArr = OPICSNo.Split(',');
            List<DA_TRN> UpdateDeals = new List<DA_TRN>();
            List<DEALModel> opicdeals = null;
            try
            {
                LoggingHelper.Debug("Match deal process on " + processdate.ToString());

                //find deal before
                deal = _dealBusiness.GetByID(DMKDealID);
                if (deal==null)
                    return new { Result = "ERROR", Message = String.Format("Deal {0} is not found.", deal.INT_DEAL_NO) };
                
                //Find Deal in OPICS deal
                opicdeals = _opicsBusiness.GetOPICSDealExternal(processdate).Where(p => OPICSNoArr.Contains(p.EXT_DEAL_NO)).ToList();
                foreach(string product in opicdeals.Select(o => o.PRODUCT).ToArray() )
                {
                    if (!deal.MA_PRODUCT.LABEL.Equals(product))
                    {
                        return new { Result = "ERROR", Message = "Product does not match." };
                    }  
                }
                foreach (string cpty in opicdeals.Select(o => o.CPTY).ToArray())
                {
                if (!deal.MA_COUTERPARTY.USERCODE.ToString().Equals(cpty.Trim()))
                {
                    return new { Result = "ERROR", Message = "Counterparty does not match." };
                }
                }

                //Build transaction each deal
                BuildTransation(sessioninfo, opicdeals, deal, ref UpdateDeals);

                LoggingHelper.Debug("End Match deal process on " + processdate.ToString());
                return new
                {
                    Result = "OK",
                    Message = String.Format("Match deal completed, between {0} and {1}.", deal.INT_DEAL_NO, OPICSNo)
                };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        /// <summary>
        /// Cancel deal by ID
        /// </summary>
        /// <param name="sessioninfo"></param>
        /// <param name="DMKDealID"></param>
        /// <returns></returns>
        public static object CancellingDeal(SessionInfo sessioninfo, Guid DMKDealID)
        {
            ReconcileBusiness _reconcileBusiness = new ReconcileBusiness();
            DealBusiness _dealBusiness = new DealBusiness();
            List<DealTranModel> DealTrans = new List<DealTranModel>();
            DA_TRN deal = null;
            DA_TRN deal2 = null;
            List<DA_TRN> dealExts = null;
            string extdealno = string.Empty;
            try
            {
                LoggingHelper.Debug("Cancelling deal");
                //find deal before
                deal = _dealBusiness.GetByID(DMKDealID);
                if (deal == null)
                    return new { Result = "ERROR", Message = String.Format("Deal {0} is not found.", deal.INT_DEAL_NO) };

                if (deal.MA_PRODUCT.LABEL.Replace(" ", string.Empty) == ProductCode.FXSWAP.ToString())
                    deal2 = _dealBusiness.GetDealByProcessDate(sessioninfo.Process.CurrentDate).FirstOrDefault(p => p.INT_DEAL_NO == deal.INT_DEAL_NO && p.VERSION == deal.VERSION && p.ID != deal.ID);
                
                if (deal.MA_PRODUCT.LABEL.Replace(" ", string.Empty) == ProductCode.FXSWAP.ToString() && deal2 == null)
                    return new { Result = "ERROR", Message = String.Format("Deal {0} is not found.", deal.INT_DEAL_NO) };

                dealExts = _dealBusiness.GetByExternalByInternalDealNo(sessioninfo.Process.NextDate, deal.INT_DEAL_NO);
                if (dealExts == null)
                    return new { Result = "ERROR", Message = String.Format("OPICS Deal is not found.") };

               // extdealno = deal.EXT_DEAL_NO;

                deal.EXT_DEAL_NO = null;
                deal.EXT_PORTFOLIO = null;
                deal.STATUS_ID = _lookupvaluesRepository.StatusRepository.GetByLabel(StatusCode.OPEN.ToString()).ID;
                deal.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                deal.LOG.MODIFYDATE = DateTime.Now;
                deal.INSERT_BY_EXT = null;
                DealTrans.Add(new DealTranModel() { ProductTransaction = (ProductCode)Enum.Parse(typeof(ProductCode), deal.MA_PRODUCT.LABEL.Replace(" ","")), Transaction = deal, UpdateStates = UpdateStates.Editing });

                if (deal2 != null)
                {
                    deal2.EXT_DEAL_NO = null;
                    deal2.EXT_PORTFOLIO = null;
                    deal2.STATUS_ID = _lookupvaluesRepository.StatusRepository.GetByLabel(StatusCode.OPEN.ToString()).ID;
                    deal2.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                    deal2.LOG.MODIFYDATE = DateTime.Now;
                    deal2.INSERT_BY_EXT = null;
                    DealTrans.Add(new DealTranModel() { ProductTransaction = (ProductCode)Enum.Parse(typeof(ProductCode), deal2.MA_PRODUCT.LABEL.Replace(" ", "")), Transaction = deal2, UpdateStates = UpdateStates.Editing });
                }

                foreach (DA_TRN dealExt in dealExts)
                {
                    DealTrans.Add(new DealTranModel() { ProductTransaction = (ProductCode)Enum.Parse(typeof(ProductCode), dealExt.MA_PRODUCT.LABEL.Replace(" ", "")), Transaction = dealExt, UpdateStates = UpdateStates.Deleting });
                }             

                _reconcileBusiness.UpdateDealReconcile(sessioninfo, DealTrans);

                LoggingHelper.Debug("End Cancelling deal");
                return new
                {
                    Result = "OK",
                    Message = String.Format("Process cancel the deal between DMK Deal no {0} and OPICS Deal no {1} completed", deal.INT_DEAL_NO, extdealno)
                };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        /// <summary>
        /// Create Deal Template
        /// </summary>
        /// <param name="sessioninfo"></param>
        /// <returns></returns>
        public static DA_TRN CreateDealTemplate(SessionInfo sessioninfo)
        {
            DA_TRN deal = new DA_TRN();
            deal.ID = Guid.NewGuid();
            deal.LOG.INSERTDATE = DateTime.Now;
            deal.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
            return deal;
        }

        public static object ImportExternalByProcessDate(SessionInfo sessioninfo)
        {
            OpicsBusiness _opicsBusiness = new OpicsBusiness();
            DealBusiness _dealBusiness = new DealBusiness();
            ReconcileBusiness _reconcileBusiness = new ReconcileBusiness();
            List<DealTranModel> DealTrans = new List<DealTranModel>();
            var results = new List<object>();

            try
            {
                LoggingHelper.Debug("Get OPICS Deal on " + sessioninfo.Process.PreviousDate.ToString());

                //Get data from OPICS
                List<DEALModel> opicsdeals = _opicsBusiness.GetOPICSDealExternal(sessioninfo.Process.PreviousDate);
                List<CASHFLOWModel> opicscashflows = _opicsBusiness.GetOPICSCashflow(sessioninfo.Process.PreviousDate);

                List<DA_TRN> importeddeals = _dealBusiness.GetImportedDealsByProcessDate(sessioninfo.Process.CurrentDate);

                //Ignore imported deals
                var query = from o in opicsdeals
                            where !importeddeals.Any(d => d.EXT_DEAL_NO == o.EXT_DEAL_NO)
                            select o;

                LoggingHelper.Debug("Import Passed OPICS Deal");
                foreach (DEALModel opicdeal in query.ToList())
                {
                    if (ValidateOPICS(opicdeal))
                        ImportPassedOPICSDeal(sessioninfo, opicdeal, sessioninfo.Process.CurrentDate, ref DealTrans);
                }

                LoggingHelper.Debug("End Import Passed OPICS Deal as " + query.Count().ToString());


                if (DealTrans.Count > 0)
                {
                    //Insert to database
                    LoggingHelper.Debug("Insert OPICS deals on " + sessioninfo.Process.PreviousDate.ToString());

                    _reconcileBusiness.UpdateDealReconcile(sessioninfo, DealTrans);
                }

                results.Add(new { Object = "OPICS Deals", Total = DealTrans.Count});

                //Import Cashflows from OPICS
                List<DA_TRN_CASHFLOW> cashflows = null;

                if (opicscashflows != null && opicscashflows.Count > 0)
                {
                    List<DA_TRN> allimporteddeals = importeddeals.Union(from t in DealTrans select t.Transaction).ToList();
                    List<DA_TRN_CASHFLOW> importedcashflows = _dealBusiness.GetFlowsByProcessDate(sessioninfo.Process.CurrentDate);

                    cashflows = GenerateCashflowObject(sessioninfo, opicscashflows, importedcashflows, allimporteddeals);

                    if (cashflows.Count > 0)
                    {
                        _reconcileBusiness.ImportCashflows(sessioninfo, cashflows);
                    }
                }
                results.Add(new { Object = "OPICS Cashflows", Total = cashflows == null ? 0 : cashflows.Count });
                
                //Return result to jTable
                return new
                {
                    Result = "OK",
                    //Records = count > 0 ? sortedRecords.Skip(startIndex).Take(count).ToList() : sortedRecords,                    
                    Records = results,
                    TotalRecordCount = results.Count
                };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        #region Private Methods
        /// <summary>
        /// Auto Match And Update Deals
        /// </summary>
        /// <param name="sessioninfo"></param>
        /// <param name="processdate"></param>
        /// <param name="opicdeals"></param>
        /// <param name="deals"></param>
        private static void AutoMatchAndUpdateDeals(SessionInfo sessioninfo, DateTime processdate, 
            List<DEALModel> opicdeals, out List<DA_TRN> deals)
        {
            DealBusiness _dealBusiness = new DealBusiness();
            List<DA_TRN> UpdateDeals = new List<DA_TRN>();
            List<DA_TRN> MatchDeals = new List<DA_TRN>();
            List<DEALModel> _opicdeals = null;

            LoggingHelper.Debug("AutoMatchAndUpdateDeals on " + processdate.ToString());

            deals = _dealBusiness.GetDealByProcessDate(processdate, StatusCode.OPEN);
            if (deals != null)
            {
                if (opicdeals != null)
                {
                    //for loop for deal on process date
                    foreach (DA_TRN deal in deals)
                    {
                        //Condition for Match deal between DMK and OPICS, INSERT_DATE = Processing Date
                        switch ((ProductCode)Enum.Parse(typeof(ProductCode), deal.MA_PRODUCT.LABEL.Replace(" ", string.Empty)))
                        {
                            case ProductCode.FXSWAP:
                                _opicdeals = opicdeals.Where<DEALModel>(p => p.INT_DEAL_NO == deal.INT_DEAL_NO && p.INSERT_DATE.Date == processdate.Date && p.FLAG_NEARFAR == deal.FLAG_NEARFAR).ToList();
                                break;
                            default:
                                _opicdeals = opicdeals.Where<DEALModel>(p => p.INT_DEAL_NO == deal.INT_DEAL_NO && p.INSERT_DATE.Date == processdate.Date).ToList();
                                break;
                        }      

                        //Build transaction each deal
                        if (_opicdeals != null && _opicdeals.Count > 0)
                        {                             
                            bool match = CheckAddonCondition(deal, _opicdeals);                            
                            if (match)
                            {
                                BuildTransation(sessioninfo, _opicdeals, deal, ref UpdateDeals);
                                MatchDeals.Add(deal);
                            }
                        }
                        else { 
                            //Not match, will be import auto for this processing date

                        }
                    }

                    foreach (DA_TRN matchdeal in MatchDeals)
                        deals.Remove(matchdeal);
                }
            }
            bool done = false;
            //include some statements
            done = true;
        }
        /// <summary>
        /// Validation OPICS deal as CPTY, INSTRUMENT
        /// </summary>
        /// <param name="opicdeal"></param>
        /// <returns></returns>
        private static bool ValidateOPICS(DEALModel opicdeal)
        {
            bool found = true;
            InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();
            CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
            MA_INSTRUMENT ins = null;

            LoggingHelper.Debug("Validate OPICS data");

            MA_COUTERPARTY counterparty = _counterpartyBusiness.GetByUsercode(Convert.ToInt32(opicdeal.CPTY));

            if (counterparty == null) //counterparty must be check whether it exist in  DMK
                throw new UIPException(new Exception(), String.Format("Counterparty {0} is not defined in Dealmaker.", opicdeal.SNAME.Trim()));

            ProductCode eProductCode = (ProductCode)Enum.Parse(typeof(ProductCode), opicdeal.PRODUCT.Replace(" ", string.Empty));

            if (eProductCode == ProductCode.BOND || eProductCode == ProductCode.REPO)
            {
                ins = _instrumentBusiness.GetByLabel(null, opicdeal.INSTRUMENT);

                if (ins == null)
                {
                    found = false;
                    throw new UIPException(new Exception(), String.Format("Instrument {0} is not defined in Dealmaker.", opicdeal.INSTRUMENT));
                }
            }
            else if (eProductCode == ProductCode.FXSPOT || eProductCode == ProductCode.FXFORWARD || eProductCode == ProductCode.FXSWAP)
            {
                ins = _instrumentBusiness.GetFXInstrumentByCCY(null, eProductCode, opicdeal.CCY1, opicdeal.CCY2);

                if (ins == null)
                {
                    found = false;
                    throw new UIPException(new Exception(), String.Format("Currency pair {0} is not defined in Dealmaker.", opicdeal.CCY1 + "/" + opicdeal.CCY2));
                }
            }

            return found;
        }
        /// <summary>
        /// Check Addon Condition for both product BONDD, SWAP
        /// </summary>
        /// <param name="deal"></param>
        /// <param name="opicdeal"></param>
        /// <returns></returns>
        private static bool CheckAddonCondition(DA_TRN deal, List<DEALModel> opicdeals)
        {
            bool match = true;
            LoggingHelper.Debug("Check Addon Condition OPICS data and DMK deal");
            List<string> notMatch = new List<string>();
            string ccy1 =deal.FIRST.CCY_ID.HasValue? _lookupvaluesRepository.CurrencyRepository.GetByID(deal.FIRST.CCY_ID.Value).LABEL:string.Empty;
            string ccy2 = deal.SECOND.CCY_ID.HasValue ? _lookupvaluesRepository.CurrencyRepository.GetByID(deal.SECOND.CCY_ID.Value).LABEL : string.Empty;
            switch ((ProductCode)Enum.Parse(typeof(ProductCode), deal.MA_PRODUCT.LABEL.Replace(" ", string.Empty)))
            {
                case ProductCode.BOND:

                    if (!opicdeals.All(o => o.TRADE_DATE.Date.Equals(deal.TRADE_DATE.Value.Date)))
                    {
                        match = false;
                        notMatch.Add("TradeDate");
                    }
                    if (!opicdeals.All(o => o.MATURITY_DATE.Date.Equals(deal.MATURITY_DATE.Value.Date)))
                    {
                        match = false;
                        notMatch.Add("SettlementDate");
                    }
                    if (!opicdeals.All(o => o.INSTRUMENT.Trim().Equals(deal.MA_INSRUMENT.LABEL)))
                    {
                        match = false;
                        notMatch.Add("Instrument");
                    }
                    if (!opicdeals.All(o => o.CPTY.Trim().Equals(deal.MA_COUTERPARTY.USERCODE.ToString())))
                    {
                        match = false;
                        notMatch.Add("Counterparty");
                    }
                    if (deal.FLAG_SETTLE.HasValue && deal.FLAG_SETTLE.Value && !opicdeals.All(o => o.BUY_SELL.Equals(deal.FLAG_BUYSELL)))
                    {
                        match = false;
                        notMatch.Add("Buy-Sell");
                    }
                    if (!deal.FIRST.NOTIONAL.Equals(opicdeals.Sum(o => o.NOTIONAL1)))
                    {
                        match = false;
                        notMatch.Add("Notional");
                    }
                    break;
                case ProductCode.SWAP:
                    if (!opicdeals.All(o => o.TRADE_DATE.Date.Equals(deal.TRADE_DATE.Value.Date)))
                    {
                        match = false;
                        notMatch.Add("TradeDate");
                    }
                    if (!opicdeals.All(o => o.START_DATE.Value.Date.Equals(deal.START_DATE.Value.Date)))
                    {
                        match = false;
                        notMatch.Add("EffectiveDate");
                    }
                    if ((deal.MATURITY_DATE.HasValue) && (!opicdeals.All(o => o.MATURITY_DATE.Date.Equals(deal.MATURITY_DATE.Value.Date))))
                    {
                        match = false;
                        notMatch.Add("MaturityDate");
                    }
                    if (!opicdeals.All(o => o.CPTY.Trim().Equals(deal.MA_COUTERPARTY.USERCODE.ToString())))
                    {
                        match = false;
                        notMatch.Add("Counterparty");
                    }
                    if (!deal.FIRST.NOTIONAL.Equals(opicdeals.Sum(o => o.PAY_REC1 == deal.FIRST.FLAG_PAYREC ? o.NOTIONAL1 : o.NOTIONAL2)))
                    {
                        match = false;
                        notMatch.Add("Notional1");
                    }
                    if (!deal.SECOND.NOTIONAL.Equals(opicdeals.Sum(o => o.PAY_REC2 == deal.SECOND.FLAG_PAYREC ? o.NOTIONAL2 : o.NOTIONAL1)))
                    {
                        match = false;
                        notMatch.Add("Notional2");
                    }
                    if (!opicdeals.All(o => (o.FIXED_FLOAT1 == CouponType.FIXED.ToString()).Equals(o.PAY_REC1 == deal.FIRST.FLAG_PAYREC ? deal.FIRST.FLAG_FIXED : deal.SECOND.FLAG_FIXED )))
                    {
                        match = false;
                        notMatch.Add("FixedFloat1");
                    }
                    if (!opicdeals.All(o => (o.FIXED_FLOAT2 == CouponType.FIXED.ToString()).Equals(o.PAY_REC2 == deal.SECOND.FLAG_PAYREC ? deal.SECOND.FLAG_FIXED : deal.FIRST.FLAG_FIXED)))
                    {
                        match = false;
                        notMatch.Add("FixedFloat2");
                    }
                    if (!(opicdeals.All(o => o.FIRST_FIXING1 == 0 && (o.PAY_REC1 == deal.FIRST.FLAG_PAYREC ? deal.FIRST.FIRSTFIXINGAMT == null : deal.SECOND.FIRSTFIXINGAMT == null)) 
                        || opicdeals.All(o => o.FIRST_FIXING1.Equals(o.PAY_REC1 == deal.FIRST.FLAG_PAYREC ? deal.FIRST.FIRSTFIXINGAMT : deal.SECOND.FIRSTFIXINGAMT))))
                    {
                        match = false;
                        notMatch.Add("Fixing1");
                    }
                    if (!(opicdeals.All(o => o.FIRST_FIXING2 == 0 && (o.PAY_REC2 == deal.SECOND.FLAG_PAYREC ? deal.SECOND.FIRSTFIXINGAMT == null : deal.FIRST.FIRSTFIXINGAMT == null)) 
                        || opicdeals.All(o => o.FIRST_FIXING2.Equals(o.PAY_REC2 == deal.SECOND.FLAG_PAYREC ? deal.SECOND.FIRSTFIXINGAMT : deal.FIRST.FIRSTFIXINGAMT))))
                    {
                        match = false;
                        notMatch.Add("Fixing2");
                    }
                    if (!opicdeals.All(o => o.RATE1.Equals(o.PAY_REC1 == deal.FIRST.FLAG_PAYREC ? deal.FIRST.RATE : deal.SECOND.RATE)))
                    {
                        match = false;
                        notMatch.Add("Rate1");
                    }
                    if (!opicdeals.All(o => o.RATE2.Equals(o.PAY_REC2 == deal.SECOND.FLAG_PAYREC ? deal.SECOND.RATE : deal.FIRST.RATE)))
                    {
                        match = false;
                        notMatch.Add("Rate2");
                    }
                    string freq1 = _lookupvaluesRepository.FrequencyRepository.GetByID(deal.FIRST.FREQTYPE_ID.Value).USERCODE;
                    string freq2 = _lookupvaluesRepository.FrequencyRepository.GetByID(deal.SECOND.FREQTYPE_ID.Value).USERCODE;

                    if (!opicdeals.All(o => o.FREQ1.Trim().Equals(o.PAY_REC1 == deal.FIRST.FLAG_PAYREC ? freq1 : freq2)))
                    {
                        match = false;
                        notMatch.Add("Freq1");
                    }
                    if (!opicdeals.All(o => o.FREQ2.Trim().Equals(o.PAY_REC2 == deal.SECOND.FLAG_PAYREC ? freq2 : freq1)))
                    {
                        match = false;
                        notMatch.Add("Freq2");
                    }
                    break;
                case ProductCode.REPO:
                    if (!opicdeals.All(o => o.TRADE_DATE.Date.Equals(deal.TRADE_DATE.Value.Date)))
                    {
                        match = false;
                        notMatch.Add("TradeDate");
                    }
                    if ((deal.START_DATE.HasValue) && (!opicdeals.All(o => o.START_DATE.Value.Date.Equals(deal.START_DATE.Value.Date))))
                    {
                        match = false;
                        notMatch.Add("EffectiveDate");
                    }
                    if ((deal.MATURITY_DATE.HasValue) && (!opicdeals.All(o => o.MATURITY_DATE.Date.Equals(deal.MATURITY_DATE.Value.Date))))
                    {
                        match = false;
                        notMatch.Add("MaturityDate");
                    }
                    if (!opicdeals.All(o => o.CPTY.Trim().Equals(deal.MA_COUTERPARTY.USERCODE.ToString())))
                    {
                        match = false;
                        notMatch.Add("Counterparty");
                    }
                    if (!opicdeals.All(o => o.BUY_SELL.Trim().Equals(deal.FLAG_BUYSELL)))
                    {
                        match = false;
                        notMatch.Add("BuySell");
                    }
                    if (!opicdeals.All(o => o.INSTRUMENT.Trim().Equals(deal.MA_INSRUMENT.LABEL)))
                    {
                        match = false;
                        notMatch.Add("Instrument");
                    }
                    if (!deal.FIRST.NOTIONAL.Equals(opicdeals.Sum(o => o.NOTIONAL1)))
                    {
                        match = false;
                        notMatch.Add("Notional");
                    }
                    break;
                case ProductCode.FXSPOT:
                    if (!opicdeals.All(o => o.TRADE_DATE.Date.Equals(deal.TRADE_DATE.Value.Date)))
                    {
                        match = false;
                        notMatch.Add("TradeDate");
                    }
                    if ((deal.MATURITY_DATE.HasValue) && (!opicdeals.All(o => o.MATURITY_DATE.Date.Equals(deal.MATURITY_DATE.Value.Date))))
                    {
                        match = false;
                        notMatch.Add("MaturityDate");
                    }
                    if (!opicdeals.All(o => o.CPTY.Trim().Equals(deal.MA_COUTERPARTY.USERCODE.ToString())))
                    {
                        match = false;
                        notMatch.Add("Counterparty");
                    }
                    if (!deal.FIRST.NOTIONAL.Equals(opicdeals.Sum(o => o.BUY_SELL == deal.FLAG_BUYSELL ? o.NOTIONAL1 : o.NOTIONAL2)))
                    {
                        match = false;
                        notMatch.Add("Notional1");
                    }
                    if (!deal.SECOND.NOTIONAL.Equals(opicdeals.Sum(o => o.BUY_SELL == deal.FLAG_BUYSELL ? o.NOTIONAL2 : o.NOTIONAL1)))
                    {
                        match = false;
                        notMatch.Add("Notional2");
                    }
                    if (!opicdeals.All(o => o.BUY_SELL == deal.FLAG_BUYSELL ? o.CCY1.Trim().Equals(ccy1) : o.CCY1.Trim().Equals(ccy2)))
                    {
                        match = false;
                        notMatch.Add("CCY1");
                    }
                    if (!opicdeals.All(o => o.BUY_SELL == deal.FLAG_BUYSELL ? o.CCY2.Trim().Equals(ccy2) : o.CCY2.Trim().Equals(ccy1)))
                    {
                        match = false;
                        notMatch.Add("CCY2");
                    }
                    break;
                case ProductCode.FXFORWARD:
                    if (!opicdeals.All(o => o.TRADE_DATE.Date.Equals(deal.TRADE_DATE.Value.Date)))
                    {
                        match = false;
                        notMatch.Add("TradeDate");
                    }
                    //if ((deal.START_DATE.HasValue) && (!opicdeals.All(o => o.START_DATE.Value.Date.Equals(deal.START_DATE.Value.Date))))
                    //{
                    //    match = false;
                    //    notMatch.Add("EffectiveDate");
                    //}
                    if ((deal.MATURITY_DATE.HasValue) && (!opicdeals.All(o => o.MATURITY_DATE.Date.Equals(deal.MATURITY_DATE.Value.Date))))
                    {
                        match = false;
                        notMatch.Add("MaturityDate");
                    }
                    if (!opicdeals.All(o => o.CPTY.Trim().Equals(deal.MA_COUTERPARTY.USERCODE.ToString())))
                    {
                        match = false;
                        notMatch.Add("Counterparty");
                    }
                    if (!deal.FIRST.NOTIONAL.Equals(opicdeals.Sum(o => o.BUY_SELL == deal.FLAG_BUYSELL ? o.NOTIONAL1 : o.NOTIONAL2)))
                    {
                        match = false;
                        notMatch.Add("Notional1");
                    }
                    if (!deal.SECOND.NOTIONAL.Equals(opicdeals.Sum(o => o.BUY_SELL == deal.FLAG_BUYSELL ? o.NOTIONAL2 : o.NOTIONAL1)))
                    {
                        match = false;
                        notMatch.Add("Notional2");
                    }
                    if (!opicdeals.All(o => o.BUY_SELL == deal.FLAG_BUYSELL ? o.CCY1.Trim().Equals(ccy1) : o.CCY1.Trim().Equals(ccy2)))
                    {
                        match = false;
                        notMatch.Add("CCY1");
                    }
                    if (!opicdeals.All(o => o.BUY_SELL == deal.FLAG_BUYSELL ? o.CCY2.Trim().Equals(ccy2) : o.CCY2.Trim().Equals(ccy1)))
                    {
                        match = false;
                        notMatch.Add("CCY2");
                    }

                    break;
                case ProductCode.FXSWAP:
                    if (!opicdeals.All(o => o.TRADE_DATE.Date.Equals(deal.TRADE_DATE.Value.Date)))
                    {
                        match = false;
                        notMatch.Add("TradeDate");
                    }
                    //if (deal.SPOT_DATE.HasValue && opicdeals.All(o => o.SPOT_DATE.HasValue && o.SPOT_DATE.Value.Equals(deal.SPOT_DATE.Value)))
                    //{
                    //    if ((deal.START_DATE.HasValue) && (!opicdeals.All(o => o.START_DATE.Value.Date.Equals(deal.START_DATE.Value.Date))))
                    //    {
                    //        match = false;
                    //        notMatch.Add("EffectiveDate");
                    //    }
                    //}
                    //if ((deal.START_DATE.HasValue) && (!opicdeals.All(o => o.START_DATE.Value.Date.Equals(deal.START_DATE.Value.Date))))
                    //{
                    //    match = false;
                    //    notMatch.Add("EffectiveDate");
                    //}
                    if ((deal.MATURITY_DATE.HasValue) && (!opicdeals.All(o => o.MATURITY_DATE.Date.Equals(deal.MATURITY_DATE.Value.Date))))
                    {
                        match = false;
                        notMatch.Add("MaturityDate");
                    }
                    if (!opicdeals.All(o => o.CPTY.Trim().Equals(deal.MA_COUTERPARTY.USERCODE.ToString())))
                    {
                        match = false;
                        notMatch.Add("Counterparty");
                    }
                    if (!deal.FIRST.NOTIONAL.Equals(opicdeals.Sum(o => o.BUY_SELL == deal.FLAG_BUYSELL ? o.NOTIONAL1 : o.NOTIONAL2)))
                    {
                        match = false;
                        notMatch.Add("Notional1");
                    }
                    if (!deal.SECOND.NOTIONAL.Equals(opicdeals.Sum(o => o.BUY_SELL == deal.FLAG_BUYSELL ? o.NOTIONAL2 : o.NOTIONAL1)))
                    {
                        match = false;
                        notMatch.Add("Notional2");
                    }
                    if (!opicdeals.All(o => o.BUY_SELL == deal.FLAG_BUYSELL ? o.CCY1.Trim().Equals(ccy1) : o.CCY1.Trim().Equals(ccy2)))
                    {
                        match = false;
                        notMatch.Add("CCY1");
                    }
                    if (!opicdeals.All(o => o.BUY_SELL == deal.FLAG_BUYSELL ? o.CCY2.Trim().Equals(ccy2) : o.CCY2.Trim().Equals(ccy1)))
                    {
                        match = false;
                        notMatch.Add("CCY1");
                    }
                    break;
            }
            deal.REMARK = string.Join(", ",notMatch);
            return match;
        }
        /// <summary>
        /// Build Transation for Matching Deal between DMK Deal No. and OPICS Deal No.
        /// </summary>
        /// <param name="sessioninfo"></param>
        /// <param name="opicdeals"></param>
        /// <param name="deal"></param>
        /// <param name="UpdateDeals"></param>
        private static void BuildTransation(SessionInfo sessioninfo, List<DEALModel> opicdeals, 
            DA_TRN deal, ref List<DA_TRN> UpdateDeals)
        {
            ReconcileBusiness _reconcileBusiness = new ReconcileBusiness();
            StaticDataBusiness _staticdataBusiness = new StaticDataBusiness();
            DealBusiness _dealBusiness = new DealBusiness();
            CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
            InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();
            List<DealTranModel> DealTrans = new List<DealTranModel>();

            DA_TRN newDeal = null;
            LoggingHelper.Debug("BuildTransation OPICS data and DMK deal");

            if (opicdeals != null)
            {
                bool flag = true;
                foreach (DEALModel opicdeal in opicdeals)
                {
                    flag = ValidateOPICS(opicdeal);
                    if( !flag ) break;
                }
                if (flag)
                {
                    //Begin transaction - Update DMK Deal No
                    deal.EXT_DEAL_NO = opicdeals[0].EXT_DEAL_NO;
                    deal.EXT_PORTFOLIO = opicdeals[0].EXT_PORTFOLIO;
                    deal.STATUS_ID = _lookupvaluesRepository.StatusRepository.GetByLabel(StatusCode.MATCHED.ToString()).ID;
                    deal.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                    deal.LOG.MODIFYDATE = DateTime.Now;
                    deal.INSERT_BY_EXT = opicdeals[0].INSERT_BY_EXT;

                    ProductCode eProduct = (ProductCode)Enum.Parse(typeof(ProductCode), opicdeals[0].PRODUCT.Replace(" ", string.Empty));

                    //Update dealmaker data
                    DealTrans.Add(new DealTranModel() { UpdateStates = UpdateStates.Editing, Transaction = deal, ProductTransaction = eProduct });

                    //Create DMK Deal from OPICS Deal
                    foreach (DEALModel opicdeal in opicdeals)
                    {
                        newDeal = GenerateTrnObject(sessioninfo, opicdeal, deal, sessioninfo.Process.NextDate);

                        //Import Opics transaction 
                        DealTrans.Add(new DealTranModel() { UpdateStates = UpdateStates.Adding, Transaction = newDeal, ProductTransaction = eProduct });
                    }
                    //Update transaction
                    _reconcileBusiness.UpdateDealReconcile(sessioninfo, DealTrans);
                }
            }
        }
        /// <summary>
        /// Rollback Transaction
        /// </summary>
        /// <param name="sessioninfo"></param>
        /// <param name="deal"></param>
        private static void RollbackTransaction(SessionInfo sessioninfo, DA_TRN deal)
        {
            DealBusiness _dealBusiness = new DealBusiness();
            deal.EXT_DEAL_NO = null;
            deal.EXT_PORTFOLIO = null;
            deal.STATUS_ID = _lookupvaluesRepository.StatusRepository.GetByLabel(StatusCode.OPEN.ToString()).ID;
            deal.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
            deal.LOG.MODIFYDATE = DateTime.Now;
            //Update data
            _dealBusiness.Update(sessioninfo, deal);
        }

        /// <summary>
        /// Generate DA_TRN object from Opics Transaction
        /// </summary>
        /// <param name="sessioninfo"></param>
        /// <param name="opicdeal"></param>
        /// <param name="processdate"></param>
        private static DA_TRN GenerateTrnObject(SessionInfo sessioninfo, DEALModel opicdeal, DA_TRN dmkDeal, DateTime processdate)
        {
            StaticDataBusiness _staticdataBusiness = new StaticDataBusiness();
            CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
            InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();
            LookupBusiness _lookupBusiness = new LookupBusiness();

            LoggingHelper.Debug("Generate transaction object : " + opicdeal.EXT_DEAL_NO);
            DA_TRN newDeal = CreateDealTemplate(sessioninfo);
            try
            { 

            newDeal.ENGINE_DATE = processdate;
            newDeal.STATUS_ID = _lookupvaluesRepository.StatusRepository.GetByLabel(StatusCode.MATCHED.ToString()).ID;
            newDeal.PRODUCT_ID = _lookupvaluesRepository.ProductRepository.GetByLabel(opicdeal.PRODUCT.Trim()).ID;
            //newDeal.INT_DEAL_NO = dmkNo;
            newDeal.EXT_DEAL_NO = opicdeal.EXT_DEAL_NO;
            //newDeal.VERSION = 1;
            newDeal.SOURCE = SourceType.EXT.ToString();
            newDeal.CTPY_ID = _counterpartyBusiness.GetByUsercode(Convert.ToInt32(opicdeal.CPTY.Trim())).ID;
            newDeal.EXT_PORTFOLIO = opicdeal.EXT_PORTFOLIO;
            newDeal.FLAG_BUYSELL = opicdeal.BUY_SELL;
            if (!String.IsNullOrEmpty(opicdeal.PORTFOLIO.Trim()))
                newDeal.PORTFOLIO_ID = _lookupvaluesRepository.PortfolioRepository.GetByLabel(opicdeal.PORTFOLIO.Trim()).ID;
            newDeal.START_DATE = opicdeal.START_DATE;
            newDeal.MATURITY_DATE = opicdeal.MATURITY_DATE;
            newDeal.TRADE_DATE = opicdeal.TRADE_DATE;
            newDeal.INSERT_BY_EXT = opicdeal.INSERT_BY_EXT;
            newDeal.FLAG_SETTLE = dmkDeal != null && dmkDeal.FLAG_SETTLE != null ? dmkDeal.FLAG_SETTLE : opicdeal.FLAG_SETTLE == 1 ? true : false;

            if (dmkDeal != null)
            {
                newDeal.INT_DEAL_NO = dmkDeal.INT_DEAL_NO;
                newDeal.VERSION = dmkDeal.VERSION;
                newDeal.REMARK = dmkDeal.REMARK + " " + opicdeal.REMARK;

                newDeal.LOG.INSERTDATE = dmkDeal.LOG.INSERTDATE;
                newDeal.LOG.INSERTBYUSERID = dmkDeal.LOG.INSERTBYUSERID;

                //newDeal.FLAG_SETTLE = dmkDeal.FLAG_SETTLE;
                newDeal.OVER_AMOUNT = dmkDeal.OVER_AMOUNT;
                newDeal.OVER_APPROVER = dmkDeal.OVER_APPROVER;
                newDeal.OVER_COMMENT = dmkDeal.OVER_COMMENT;
                newDeal.OVER_SETTL_AMOUNT = dmkDeal.OVER_SETTL_AMOUNT;
            }
            else
            {
                newDeal.VERSION = 1;
            }
            
            var spotrate = _lookupBusiness.GetSpotRateAll().AsQueryable();
            MA_SPOT_RATE firstrate = new MA_SPOT_RATE();
            MA_SPOT_RATE secondRate = new MA_SPOT_RATE();

            switch ((ProductCode)Enum.Parse(typeof(ProductCode), opicdeal.PRODUCT.Replace(" ", string.Empty)))
            {
                case ProductCode.BOND:
                    newDeal.INSTRUMENT_ID = _instrumentBusiness.GetByLabel(sessioninfo, opicdeal.INSTRUMENT.Trim()).ID;

                    //First only
                    if (!String.IsNullOrEmpty(opicdeal.CCY1))
                        newDeal.FIRST.CCY_ID = _lookupvaluesRepository.CurrencyRepository.GetByLabel(opicdeal.CCY1).ID;
                    newDeal.FIRST.NOTIONAL = opicdeal.NOTIONAL1;

                    //THB Notional
                    var firstRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == newDeal.FIRST.CCY_ID && p.PROC_DATE == newDeal.TRADE_DATE);

                    if (firstRate != null)
                    {
                        newDeal.NOTIONAL_THB = Math.Abs(newDeal.FIRST.NOTIONAL.Value * firstRate.RATE);
                    }
                    else
                    {
                        throw new UIPException(new Exception(), "Error : There is no " + opicdeal.CCY1 + " spot rate on " + newDeal.TRADE_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL));
                    }                    
                    newDeal.FLAG_PCE = dmkDeal != null && dmkDeal.FLAG_PCE != null ? dmkDeal.FLAG_PCE : false;

                    newDeal.KK_PCCF = _staticdataBusiness.GetPCCF(sessioninfo, newDeal);
                    newDeal.KK_CONTRIBUTE = Math.Ceiling(newDeal.NOTIONAL_THB.Value * newDeal.KK_PCCF.Value / 100);

                    break;
                case ProductCode.REPO:
                    newDeal.INSTRUMENT_ID = _instrumentBusiness.GetByLabel(sessioninfo, opicdeal.INSTRUMENT.Trim()).ID;

                    //First only
                    if (!String.IsNullOrEmpty(opicdeal.CCY1))
                        newDeal.FIRST.CCY_ID = _lookupvaluesRepository.CurrencyRepository.GetByLabel(opicdeal.CCY1).ID;
                    newDeal.FIRST.NOTIONAL = newDeal.NOTIONAL_THB = opicdeal.NOTIONAL1;
                    
                    newDeal.KK_PCCF = _staticdataBusiness.GetPCCF(sessioninfo, newDeal);
                    newDeal.KK_CONTRIBUTE = Math.Ceiling(newDeal.NOTIONAL_THB.Value * newDeal.KK_PCCF.Value / 100);

                    break;
                case ProductCode.SWAP:
                    newDeal.INSTRUMENT_ID = _instrumentBusiness.GetByLabel(sessioninfo, opicdeal.INSTRUMENT.Trim()).ID;

                    //First and Second
                    if (!String.IsNullOrEmpty(opicdeal.CCY1))
                        newDeal.FIRST.CCY_ID = _lookupvaluesRepository.CurrencyRepository.GetByLabel(opicdeal.CCY1).ID;

                    if (!String.IsNullOrEmpty(opicdeal.FREQ1))
                        newDeal.FIRST.FREQTYPE_ID = _lookupvaluesRepository.FrequencyRepository.GetByUsercode(opicdeal.FREQ1).ID;

                    newDeal.FIRST.FIRSTFIXINGAMT = opicdeal.FIRST_FIXING1;
                    newDeal.FIRST.FLAG_FIXED = opicdeal.FIXED_FLOAT1 == CouponType.FIXED.ToString() ? true : false;
                    newDeal.FIRST.FLAG_PAYREC = opicdeal.PAY_REC1;
                    newDeal.FIRST.NOTIONAL = newDeal.NOTIONAL_THB = opicdeal.NOTIONAL1;
                    newDeal.FIRST.RATE = opicdeal.RATE1;

                    //Second
                    if (!String.IsNullOrEmpty(opicdeal.CCY2))
                        newDeal.SECOND.CCY_ID = _lookupvaluesRepository.CurrencyRepository.GetByLabel(opicdeal.CCY2).ID;

                    if (!String.IsNullOrEmpty(opicdeal.FREQ2))
                        newDeal.SECOND.FREQTYPE_ID = _lookupvaluesRepository.FrequencyRepository.GetByUsercode(opicdeal.FREQ2).ID;

                    newDeal.SECOND.FIRSTFIXINGAMT = opicdeal.FIRST_FIXING2;
                    newDeal.SECOND.FLAG_FIXED = opicdeal.FIXED_FLOAT2 == CouponType.FIXED.ToString() ? true : false;
                    newDeal.SECOND.FLAG_PAYREC = opicdeal.PAY_REC2;
                    newDeal.SECOND.NOTIONAL = opicdeal.NOTIONAL2;
                    newDeal.SECOND.RATE = opicdeal.RATE2;

                    //THB Notional
                    if (opicdeal.CCY1 == "THB")
                        newDeal.NOTIONAL_THB = Math.Abs(newDeal.FIRST.NOTIONAL.Value);
                    else if (opicdeal.CCY2 == "THB")
                        newDeal.NOTIONAL_THB = Math.Abs(newDeal.SECOND.NOTIONAL.Value);
                    else
                    {
                        firstRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == newDeal.FIRST.CCY_ID && p.PROC_DATE == newDeal.TRADE_DATE);
                        secondRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == newDeal.SECOND.CCY_ID && p.PROC_DATE == newDeal.TRADE_DATE);

                        if (firstRate != null && secondRate != null)
                        {
                            if (Math.Abs(newDeal.FIRST.NOTIONAL.Value * firstRate.RATE) > Math.Abs(newDeal.SECOND.NOTIONAL.Value * secondRate.RATE))
                                newDeal.NOTIONAL_THB = Math.Abs(newDeal.FIRST.NOTIONAL.Value * firstRate.RATE);
                            else
                                newDeal.NOTIONAL_THB = Math.Abs(newDeal.SECOND.NOTIONAL.Value * secondRate.RATE);
                        }
                        else
                        {
                            string mess = string.Empty;
                            mess += firstRate == null ? opicdeal.CCY1 : string.Empty;
                            mess += secondRate == null ? (mess != string.Empty ? " and " + opicdeal.CCY2 : opicdeal.CCY2) : string.Empty;
                            throw new UIPException(new Exception(), "Error : There is no " + mess + " spot rate on " + newDeal.TRADE_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL));
                        }
                    }
                    
                    newDeal.KK_PCCF = _staticdataBusiness.GetPCCF(sessioninfo, newDeal);
                    newDeal.KK_CONTRIBUTE = Math.Ceiling(newDeal.NOTIONAL_THB.Value * newDeal.KK_PCCF.Value / 100);

                    //newDeal.BOT_PCCF = _staticdataBusiness.GetSwapBOTPCCF(sessioninfo, newDeal);
                    //newDeal.BOT_CONTRIBUTE = Math.Ceiling(newDeal.NOTIONAL_THB.Value * newDeal.BOT_PCCF.Value / 100);

                    break;
                case ProductCode.FXSPOT:
                    newDeal.INSTRUMENT_ID = _instrumentBusiness.GetFXInstrumentByCCY(sessioninfo, ProductCode.FXSPOT, opicdeal.CCY1, opicdeal.CCY2).ID;

                    if (!String.IsNullOrEmpty(opicdeal.CCY1))
                        newDeal.FIRST.CCY_ID = _lookupvaluesRepository.CurrencyRepository.GetByLabel(opicdeal.CCY1).ID;
                    newDeal.FIRST.FLAG_PAYREC = opicdeal.PAY_REC1;
                    newDeal.FIRST.NOTIONAL = opicdeal.NOTIONAL1;
                    newDeal.FIRST.RATE = opicdeal.RATE1;

                    if (!String.IsNullOrEmpty(opicdeal.CCY2))
                        newDeal.SECOND.CCY_ID = _lookupvaluesRepository.CurrencyRepository.GetByLabel(opicdeal.CCY2).ID;
                    newDeal.SECOND.FLAG_PAYREC = opicdeal.PAY_REC2;
                    newDeal.SECOND.NOTIONAL = opicdeal.NOTIONAL2;

                    //THB Notional
                    if (opicdeal.CCY1 == "THB")
                        newDeal.NOTIONAL_THB = Math.Abs(newDeal.FIRST.NOTIONAL.Value);
                    else if (opicdeal.CCY2 == "THB")
                        newDeal.NOTIONAL_THB = Math.Abs(newDeal.SECOND.NOTIONAL.Value);
                    else
                    {
                        firstRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == newDeal.FIRST.CCY_ID && p.PROC_DATE == newDeal.TRADE_DATE);
                        secondRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == newDeal.SECOND.CCY_ID && p.PROC_DATE == newDeal.TRADE_DATE);

                        if (firstRate != null && secondRate != null)
                        {
                            if (Math.Abs(newDeal.FIRST.NOTIONAL.Value * firstRate.RATE) > Math.Abs(newDeal.SECOND.NOTIONAL.Value * secondRate.RATE))
                                newDeal.NOTIONAL_THB = Math.Abs(newDeal.FIRST.NOTIONAL.Value * firstRate.RATE);
                            else
                                newDeal.NOTIONAL_THB = Math.Abs(newDeal.SECOND.NOTIONAL.Value * secondRate.RATE);
                        }
                        else
                        {
                            string mess = string.Empty;
                            mess += firstRate == null ? opicdeal.CCY1 : string.Empty;
                            mess += secondRate == null ? (mess != string.Empty ? " and " + opicdeal.CCY2 : opicdeal.CCY2) : string.Empty;
                            throw new UIPException(new Exception(), "Error : There is no " + mess + " spot rate on " + newDeal.TRADE_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL));
                        }
                    }

                    newDeal.KK_PCCF = _staticdataBusiness.GetPCCF(sessioninfo, newDeal);
                    newDeal.KK_CONTRIBUTE = Math.Ceiling(newDeal.NOTIONAL_THB.Value * newDeal.KK_PCCF.Value / 100);

                    break;

                case ProductCode.FXFORWARD:
                    newDeal.INSTRUMENT_ID = _instrumentBusiness.GetFXInstrumentByCCY(sessioninfo, ProductCode.FXFORWARD, opicdeal.CCY1, opicdeal.CCY2).ID;
                    newDeal.START_DATE = dmkDeal != null ? dmkDeal.START_DATE : opicdeal.START_DATE;

                    if (!String.IsNullOrEmpty(opicdeal.CCY1))
                        newDeal.FIRST.CCY_ID = _lookupvaluesRepository.CurrencyRepository.GetByLabel(opicdeal.CCY1).ID;
                    newDeal.FIRST.FLAG_PAYREC = opicdeal.PAY_REC1;
                    newDeal.FIRST.NOTIONAL = opicdeal.NOTIONAL1;
                    newDeal.FIRST.RATE = opicdeal.RATE1;
                    newDeal.FIRST.SWAP_POINT = opicdeal.SWAP_POINT1;

                    if (!String.IsNullOrEmpty(opicdeal.CCY2))
                        newDeal.SECOND.CCY_ID = _lookupvaluesRepository.CurrencyRepository.GetByLabel(opicdeal.CCY2).ID;
                    newDeal.SECOND.FLAG_PAYREC = opicdeal.PAY_REC2;
                    newDeal.SECOND.NOTIONAL = opicdeal.NOTIONAL2;
                    newDeal.SECOND.SWAP_POINT = opicdeal.SWAP_POINT2;

                    //THB Notional
                    if (opicdeal.CCY1 == "THB")
                        newDeal.NOTIONAL_THB = Math.Abs(newDeal.FIRST.NOTIONAL.Value);
                    else if (opicdeal.CCY2 == "THB")
                        newDeal.NOTIONAL_THB = Math.Abs(newDeal.SECOND.NOTIONAL.Value);
                    else
                    {
                        firstRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == newDeal.FIRST.CCY_ID && p.PROC_DATE == newDeal.TRADE_DATE);
                        secondRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == newDeal.SECOND.CCY_ID && p.PROC_DATE == newDeal.TRADE_DATE);

                        if (firstRate != null && secondRate != null)
                        {
                            if (Math.Abs(newDeal.FIRST.NOTIONAL.Value * firstRate.RATE) > Math.Abs(newDeal.SECOND.NOTIONAL.Value * secondRate.RATE))
                                newDeal.NOTIONAL_THB = Math.Abs(newDeal.FIRST.NOTIONAL.Value * firstRate.RATE);
                            else
                                newDeal.NOTIONAL_THB = Math.Abs(newDeal.SECOND.NOTIONAL.Value * secondRate.RATE);
                        }
                        else
                        {
                            string mess = string.Empty;
                            mess += firstRate == null ? opicdeal.CCY1 : string.Empty;
                            mess += secondRate == null ? (mess != string.Empty ? " and " + opicdeal.CCY2 : opicdeal.CCY2) : string.Empty;
                            throw new UIPException(new Exception(), "Error : There is no " + mess + " spot rate on " + newDeal.TRADE_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL));
                        }
                    }

                    newDeal.SPOT_DATE = opicdeal.SPOT_DATE;

                    newDeal.KK_PCCF = _staticdataBusiness.GetPCCF(sessioninfo, newDeal);
                    newDeal.KK_CONTRIBUTE = Math.Ceiling(newDeal.NOTIONAL_THB.Value * newDeal.KK_PCCF.Value / 100);

                    break;

                case ProductCode.FXSWAP:
                    newDeal.INSTRUMENT_ID = _instrumentBusiness.GetFXInstrumentByCCY(sessioninfo, ProductCode.FXSWAP, opicdeal.CCY1, opicdeal.CCY2).ID;
                    newDeal.FLAG_NEARFAR = opicdeal.FLAG_NEARFAR;
                    newDeal.START_DATE = dmkDeal != null ? dmkDeal.START_DATE : opicdeal.START_DATE;

                    if (!String.IsNullOrEmpty(opicdeal.CCY1))
                        newDeal.FIRST.CCY_ID = _lookupvaluesRepository.CurrencyRepository.GetByLabel(opicdeal.CCY1).ID;
                    newDeal.FIRST.FLAG_PAYREC = opicdeal.PAY_REC1;
                    newDeal.FIRST.NOTIONAL = opicdeal.NOTIONAL1;
                    newDeal.FIRST.RATE = opicdeal.RATE1;
                    newDeal.FIRST.SWAP_POINT = opicdeal.SWAP_POINT1;

                    if (!String.IsNullOrEmpty(opicdeal.CCY2))
                        newDeal.SECOND.CCY_ID = _lookupvaluesRepository.CurrencyRepository.GetByLabel(opicdeal.CCY2).ID;
                    newDeal.SECOND.FLAG_PAYREC = opicdeal.PAY_REC2;
                    newDeal.SECOND.NOTIONAL = opicdeal.NOTIONAL2;
                    newDeal.SECOND.SWAP_POINT = opicdeal.SWAP_POINT2;

                    //THB Notional
                    if (opicdeal.CCY1 == "THB")
                        newDeal.NOTIONAL_THB = Math.Abs(newDeal.FIRST.NOTIONAL.Value);
                    else if (opicdeal.CCY2 == "THB")
                        newDeal.NOTIONAL_THB = Math.Abs(newDeal.SECOND.NOTIONAL.Value);
                    else
                    {
                        firstRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == newDeal.FIRST.CCY_ID && p.PROC_DATE == newDeal.TRADE_DATE);
                        secondRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == newDeal.SECOND.CCY_ID && p.PROC_DATE == newDeal.TRADE_DATE);

                        if (firstRate != null && secondRate != null)
                        {
                            if (Math.Abs(newDeal.FIRST.NOTIONAL.Value * firstRate.RATE) > Math.Abs(newDeal.SECOND.NOTIONAL.Value * secondRate.RATE))
                                newDeal.NOTIONAL_THB = Math.Abs(newDeal.FIRST.NOTIONAL.Value * firstRate.RATE);
                            else
                                newDeal.NOTIONAL_THB = Math.Abs(newDeal.SECOND.NOTIONAL.Value * secondRate.RATE);
                        }
                        else
                        {
                            string mess = string.Empty;
                            mess += firstRate == null ? opicdeal.CCY1 : string.Empty;
                            mess += secondRate == null ? (mess != string.Empty ? " and " + opicdeal.CCY2 : opicdeal.CCY2) : string.Empty;
                            throw new UIPException(new Exception(), "Error : There is no " + mess + " spot rate on " + newDeal.TRADE_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL));
                        }
                    }

                    newDeal.SPOT_DATE = opicdeal.SPOT_DATE;

                    //Exposure include far leg only -> set near leg to 0
                    if (newDeal.FLAG_NEARFAR == "N")
                    {
                        newDeal.KK_PCCF = 0;
                        newDeal.KK_CONTRIBUTE = 0;
                    }
                    else
                    {
                        newDeal.KK_PCCF = _staticdataBusiness.GetPCCF(sessioninfo, newDeal);
                        newDeal.KK_CONTRIBUTE = Math.Ceiling(newDeal.NOTIONAL_THB.Value * newDeal.KK_PCCF.Value / 100);
                    }                    

                    break;
                default:
                    throw new UIPException(new Exception(), String.Format("The product is not defined in the OPICS deal no {0}.", opicdeal.EXT_DEAL_NO));
            }
            }
            
            catch (Exception ex)
            {
                throw new UIPException(new Exception(), ex.Message);
            }
            return newDeal;
        }


        /// <summary>
        /// Import Passed for OPICS Deal to DA_TRN table
        /// </summary>
        /// <param name="sessioninfo"></param>
        /// <param name="opicdeal"></param>
        private static void ImportPassedOPICSDeal(SessionInfo sessioninfo, DEALModel opicdeal, DateTime processdate, ref List<DealTranModel> DealTrans)
        {
            StaticDataBusiness _staticdataBusiness = new StaticDataBusiness();
            DealBusiness _dealBusiness = new DealBusiness();
            CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
            InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();

            DA_TRN pastimported = _dealBusiness.GetPastImportedDealsByExtNo(opicdeal.EXT_DEAL_NO);

            DA_TRN newDeal = GenerateTrnObject(sessioninfo, opicdeal, pastimported, processdate);

            DealTrans.Add(new DealTranModel() { ProductTransaction = (ProductCode)Enum.Parse(typeof(ProductCode), opicdeal.PRODUCT.Replace(" ", string.Empty)), Transaction = newDeal, UpdateStates = UpdateStates.Adding });
        }

        /// <summary>
        /// Import Cash flows to DA_TRN.DA_TRN_CASHFLOW
        /// </summary>
        private static List<DA_TRN_CASHFLOW> GenerateCashflowObject(SessionInfo sessioninfo, List<CASHFLOWModel> opicscashflows, List<DA_TRN_CASHFLOW> importedcashflows, List<DA_TRN> trns)
        {
            List<DA_TRN_CASHFLOW> cashflows = new List<DA_TRN_CASHFLOW>();
            DA_TRN_CASHFLOW newcashflow;
            DA_TRN trn = null;
            DA_TRN_CASHFLOW importedflow = null;

            foreach (CASHFLOWModel opicscashflow  in opicscashflows)
            {
                //Check whether deal is imported
                trn = trns.FirstOrDefault(p => p.EXT_DEAL_NO == opicscashflow.EXT_DEAL_NO);

                if (trn != null)
                {
                    //Check duplicate
                    importedflow = importedcashflows.FirstOrDefault(p => p.DA_TRN_ID == trn.ID 
                                                                    && p.FLAG_FIRST.Equals(opicscashflow.LEG == 1 ? true : false)
                                                                    && p.SEQ == opicscashflow.SEQ);

                    if (importedflow == null)
                    {
                        newcashflow = new DA_TRN_CASHFLOW();

                        newcashflow.ID = Guid.NewGuid();
                        newcashflow.DA_TRN_ID = trn.ID;
                        newcashflow.FLAG_FIRST = opicscashflow.LEG == 1 ? true : false;
                        newcashflow.SEQ = opicscashflow.SEQ;
                        newcashflow.RATE = opicscashflow.RATE;
                        newcashflow.FLOW_DATE = opicscashflow.FLOW_DATE;
                        newcashflow.FLOW_AMOUNT = opicscashflow.FLOW_AMOUNT;
                        newcashflow.FLOW_AMOUNT_THB = opicscashflow.FLOW_AMOUNT_THB;

                        newcashflow.LOG.INSERTDATE = DateTime.Now;
                        newcashflow.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;

                        cashflows.Add(newcashflow);
                    }
                }
            }

            return cashflows;
        }

        public static object CheckFlagReconcile(SessionInfo sessioninfo)
        {
            ProcessingDateBusiness _pcdBusiness = new ProcessingDateBusiness();
            UserBusiness _userBusiness = new UserBusiness();
            MA_PROCESS_DATE processdate;
            MA_USER user;
            try
            {
                processdate = _pcdBusiness.Get();
                if (processdate == null)
                    throw new UIPException(new Exception(), "Process date data not found!");
                if (processdate.FLAG_RECONCILE.HasValue && processdate.FLAG_RECONCILE.Value == true)
                {
                    user = _userBusiness.GetByID(sessioninfo, processdate.LOG.MODIFYBYUSERID.Value);
                    return new { Result = "UNAVAILBLE", Message = string.Format("Please wait. System is on processing by {0}", user != null ? user.NAME : string.Empty) };
                }
                else
                {
                    //processdate.FLAG_RECONCILE = true;
                    //processdate.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                    //processdate.LOG.MODIFYDATE = DateTime.Now;
                    //_pcdBusiness.Update(sessioninfo, processdate);
                    return new { Result = "AVAILBLE", Message = string.Empty };
                }
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }


        private static void FlagReconcile(SessionInfo sessioninfo,bool unlock)
        {
            ProcessingDateBusiness _pcdBusiness = new ProcessingDateBusiness();
            MA_PROCESS_DATE processdate;
            processdate = _pcdBusiness.Get();
            if (processdate == null)
                throw new UIPException(new Exception(), "Process date data not found!");
            processdate.FLAG_RECONCILE = unlock;
            processdate.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
            processdate.LOG.MODIFYDATE = DateTime.Now;
            _pcdBusiness.Update(sessioninfo, processdate);

        }
        #endregion
    }
}
