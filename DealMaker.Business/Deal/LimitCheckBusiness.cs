using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.EnterpriseServices;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.Data;
using KK.DealMaker.DataAccess.Repositories;
using KK.DealMaker.Core.SystemFramework;
using System.Configuration;
using KK.DealMaker.Core.Helper;
using System.Linq.Expressions;

namespace KK.DealMaker.Business.Deal
{
    public class LimitCheckBusiness : BaseBusiness
    {
        public List<LimitCheckModel> CheckAllPCE(DateTime ProcessingDate, DA_TRN trn, Guid ExcludeID1, Guid ExcludeID2)
        {
            CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();

            List<LimitCheckModel> results = GetPCEByCriteria(ProcessingDate, trn.CTPY_ID, trn.PRODUCT_ID.Value, "", ExcludeID1, ExcludeID2);

            //Get group exposure
            MA_COUTERPARTY ctpy = _counterpartyBusiness.GetCounterpartyAll().FirstOrDefault(p => p.ID == trn.CTPY_ID);
            if (ctpy != null && ctpy.GROUP_CTPY_ID != null)
            {
                var group = GetPCEByCriteria(ProcessingDate, ctpy.GROUP_CTPY_ID.Value, trn.PRODUCT_ID.Value, "", ExcludeID1, ExcludeID2);
                results = results.Union(group).ToList();
            }

            //Get temp limit
            //Look for temp limit when all conditions meet
            // 1. Transaction maturity date <= Temp limit maturity date
            foreach (LimitCheckModel result in results)
            {
                MA_TEMP_CTPY_LIMIT temp_limit = _counterpartyBusiness.GetActiveTempByID(ProcessingDate, trn.MATURITY_DATE.Value, result.CTPY_LIMIT_ID);

                if (temp_limit != null)
                    result.TEMP_AMOUNT = temp_limit.AMOUNT;
                    //result.AMOUNT = result.AMOUNT + temp_limit.AMOUNT;
            }

            return results.OrderBy(p => p.SNAME).ThenBy(p => p.SORT_INDEX).ToList();
        }

        public List<LimitCheckModel> CheckAllSET(DateTime ProcessingDate, DA_TRN trn, Guid ExcludeID1, Guid ExcludeID2)
        {
            CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();

            List<LimitCheckModel> results = GetSCEByCriteria(ProcessingDate, trn.CTPY_ID, trn.PRODUCT_ID.Value, "", ExcludeID1, ExcludeID2);

            //Get group exposure
            MA_COUTERPARTY ctpy = _counterpartyBusiness.GetCounterpartyAll().FirstOrDefault(p => p.ID == trn.CTPY_ID);
            if (ctpy != null && ctpy.GROUP_CTPY_ID != null)
            {
                var group = GetSCEByCriteria(ProcessingDate, ctpy.GROUP_CTPY_ID.Value, trn.PRODUCT_ID.Value, "", ExcludeID1, ExcludeID2);
                results = results.Union(group).ToList();
            }

            //Get temp limit
            //Look for temp limit when all conditions meet
            // 1. Transaction maturity date <= Temp limit maturity date
            foreach (LimitCheckModel result in results)
            {
                MA_TEMP_CTPY_LIMIT temp_limit = _counterpartyBusiness.GetActiveTempByID(ProcessingDate, trn.MATURITY_DATE.Value, result.CTPY_LIMIT_ID);

                if (temp_limit != null)
                    result.TEMP_AMOUNT = temp_limit.AMOUNT;
                    //result.AMOUNT = result.AMOUNT + temp_limit.AMOUNT;
            }

            return results;
        }

        public List<LimitCheckModel> CheckAllCountry(DateTime ProcessingDate, DA_TRN trn, Guid ExcludeID1, Guid ExcludeID2)
        {
            CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
            CountryBusiness _countryBusiness = new CountryBusiness();
            List<LimitCheckModel> results = new List<LimitCheckModel>();
            LimitCheckModel result;
            LimitCheckModel temp = new LimitCheckModel();
            LimitCheckModel pce = new LimitCheckModel();
            MA_COUTERPARTY ctpy = _counterpartyBusiness.GetCounterpartyAll().FirstOrDefault(p => p.ID == trn.CTPY_ID);

            List<LimitCheckModel> sets = GetCountrySETByCriteria(ProcessingDate, ctpy.COUNTRY_ID, "", ExcludeID1, ExcludeID2);
            List<LimitCheckModel> pces = GetCountryPCEByCriteria(ProcessingDate, ctpy.COUNTRY_ID, "", ExcludeID1, ExcludeID2);

            if (pces.Count != 1)
                throw new Exception("Invalid country limit data. Please contact administrator");
            else
                pce = pces.FirstOrDefault();

            MA_COUNTRY_LIMIT temp_limit = _countryBusiness.GetActiveTempByCountryID(ProcessingDate, trn.MATURITY_DATE.Value, ctpy.COUNTRY_ID);

            DateTime dateStart = trn.TRADE_DATE.Value;
            DateTime dateEnd = trn.FLAG_SETTLE.HasValue && trn.FLAG_SETTLE.Value ? trn.DA_TRN_FLOW.Max(p => p.FLOW_DATE).Value : trn.TRADE_DATE.Value;

            while (dateStart <= dateEnd)
            {
                result = new LimitCheckModel();
                                
                temp = sets.FirstOrDefault(p => p.FLOW_DATE == dateStart);

                result.COUNTRY_LABEL = pce.COUNTRY_LABEL;
                result.COUNTRY_ID = pce.COUNTRY_ID;
                result.FLAG_CONTROL = pce.FLAG_CONTROL;
                result.GEN_AMOUNT = pce.GEN_AMOUNT;
                result.TEMP_AMOUNT = temp_limit != null ? temp_limit.AMOUNT : 0;
                //result.AMOUNT = pce.AMOUNT + (temp_limit != null ? temp_limit.AMOUNT : 0);
                result.PROCESSING_DATE = pce.PROCESSING_DATE;
                result.EXPIRE_DATE = pce.EXPIRE_DATE;
                result.FLOW_DATE = dateStart;
                result.ORIGINAL_KK_CONTRIBUTE = pce.ORIGINAL_KK_CONTRIBUTE;
                
                if (temp != null)
                    result.ORIGINAL_KK_CONTRIBUTE = result.ORIGINAL_KK_CONTRIBUTE + temp.ORIGINAL_KK_CONTRIBUTE;

                results.Add(result);

                //if (dateStart.DayOfWeek == DayOfWeek.Friday)
                //    dateStart = dateStart.AddDays(3);
                //else
                dateStart = dateStart.AddDays(1);
            }

            return results;
        }

        public List<LimitCheckModel> GetPCEByCriteria(DateTime ProcessingDate, Guid CTPY_ID, Guid ProductID, string Source, Guid ExcludeID1, Guid ExcludeID2)
        {
            List<LimitCheckModel> limits = null;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                List<DA_TRN> Trans = unitOfWork.DA_TRNRepository.GetAllByEngineDate(ProcessingDate).Where(t => t.MA_STATUS.LABEL != StatusCode.CANCELLED.ToString()).ToList();
                List<MA_LIMIT_PRODUCT> limitproducts = unitOfWork.MA_LIMIT_PRODUCTRepository.GetAll();

                //Get own exposure
                var limitOwner = (from trn in Trans
                                   join lp in limitproducts on trn.PRODUCT_ID equals lp.PRODUCT_ID
                                   where (trn.SOURCE == Source || Source == "")
                                            && (trn.ID != ExcludeID1 || ExcludeID1 == Guid.Empty)
                                            && (trn.ID != ExcludeID2 || ExcludeID2 == Guid.Empty)
                                            && (trn.CTPY_ID == CTPY_ID || CTPY_ID == Guid.Empty)
                                   select new
                                   {
                                       engineDate = trn.ENGINE_DATE,
                                       ctpyID = trn.CTPY_ID,
                                       limitID = lp.LIMIT_ID,
                                       kkcontributeAmount = trn.KK_CONTRIBUTE,
                                       botcontributeAmount = trn.BOT_CONTRIBUTE
                                   }).GroupBy(tr => new { tr.engineDate, tr.ctpyID, tr.limitID })
                                 .Select(p => new PCEGroupModel
                                 {
                                     ENGINE_DATE = Convert.ToDateTime(p.Key.engineDate),
                                     CTPY_ID = p.Key.ctpyID,
                                     LIMIT_ID = p.Key.limitID,
                                     KK_CONTRIBUTE = p.Sum(x => x.kkcontributeAmount),
                                     BOT_CONTRIBUTE = p.Sum(x => x.botcontributeAmount)
                                 });

                //Get children exposure
                var limitGroup = (from trn in Trans
                                  join lp in limitproducts on trn.PRODUCT_ID equals lp.PRODUCT_ID
                                  where (trn.SOURCE == Source || Source == "")
                                           && (trn.MA_COUTERPARTY.GROUP_CTPY_ID != null)
                                           && (trn.ID != ExcludeID1 || ExcludeID1 == Guid.Empty)
                                           && (trn.ID != ExcludeID2 || ExcludeID2 == Guid.Empty)
                                           && (trn.MA_COUTERPARTY.GROUP_CTPY_ID == CTPY_ID || CTPY_ID == Guid.Empty)
                                  select new
                                  {
                                      engineDate = trn.ENGINE_DATE,
                                      limitID = lp.LIMIT_ID,
                                      kkcontributeAmount = trn.KK_CONTRIBUTE,
                                      botcontributeAmount = trn.BOT_CONTRIBUTE,
                                      groupID = trn.MA_COUTERPARTY.GROUP_CTPY_ID
                                  }).GroupBy(tr => new { tr.engineDate, tr.groupID, tr.limitID })
                                 .Select(p => new PCEGroupModel
                                 {
                                     ENGINE_DATE = Convert.ToDateTime(p.Key.engineDate),
                                     CTPY_ID = p.Key.groupID.Value,
                                     LIMIT_ID = p.Key.limitID,
                                     KK_CONTRIBUTE = p.Sum(x => x.kkcontributeAmount),
                                     BOT_CONTRIBUTE = p.Sum(x => x.botcontributeAmount)
                                 });

                //Total exposure
                var limitGroups = limitOwner.Union(limitGroup)
                                    .GroupBy(gr => new { gr.ENGINE_DATE, gr.CTPY_ID, gr.LIMIT_ID })
                                    .Select(p => new PCEGroupModel
                                    {
                                        ENGINE_DATE = p.Key.ENGINE_DATE,
                                        CTPY_ID = p.Key.CTPY_ID,
                                        LIMIT_ID = p.Key.LIMIT_ID,
                                        KK_CONTRIBUTE = p.Sum(x => x.KK_CONTRIBUTE),
                                        BOT_CONTRIBUTE = p.Sum(x => x.BOT_CONTRIBUTE)
                                    });
                
                limits = (from ctlm in unitOfWork.MA_CTPY_LIMITRepository.GetAll()
                          join ct in unitOfWork.MA_COUTERPARTYRepository.GetAll() on ctlm.CTPY_ID equals ct.ID
                          join lm in unitOfWork.MA_LIMITRepository.GetAll() on ctlm.LIMIT_ID equals lm.ID
                          join pd in limitproducts on lm.ID equals pd.LIMIT_ID
                          join lg in limitGroups on new { ctlm.CTPY_ID, ctlm.LIMIT_ID } equals new { lg.CTPY_ID, lg.LIMIT_ID } into lj
                          from sublg in lj.DefaultIfEmpty()
                          where (ctlm.CTPY_ID == CTPY_ID || CTPY_ID == Guid.Empty)
                                  && (pd.PRODUCT_ID == ProductID || ProductID == Guid.Empty)
                                  && (lm.LIMIT_TYPE == "P")
                                  && (ct.ISACTIVE.Value)
                          select new LimitCheckModel
                          {
                              SNAME = ct.SNAME,
                              CTPY_LIMIT_ID = ctlm.ID,
                              LIMIT_LABEL = lm.LABEL,
                              FLAG_CONTROL = ctlm.FLAG_CONTROL,
                              GEN_AMOUNT = ctlm.AMOUNT,
                              //AMOUNT = ctlm.AMOUNT,
                              EXPIRE_DATE = ctlm.EXPIRE_DATE,
                              PROCESSING_DATE = ProcessingDate,
                              SORT_INDEX = lm.INDEX.Value,
                              ORIGINAL_KK_CONTRIBUTE =  (sublg != null ? sublg.KK_CONTRIBUTE.Value : 0)
                          }).OrderBy(p => p.SNAME).ThenBy(p => p.SORT_INDEX).ToList();

            }

            return limits;
        }

        public List<LimitCheckModel> GetSCEByCriteria(DateTime ProcessingDate, Guid CTPY_ID, Guid ProductID, string Source, Guid ExcludeID1, Guid ExcludeID2)
        {
            List<LimitCheckModel> limits = null;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                //List<DA_TRN> Trans = unitOfWork.DA_TRNRepository.GetAll().Where(t => t.MA_STATUS.LABEL != StatusCode.CANCELLED.ToString()).ToList();
                List<DA_TRN_CASHFLOW> Cashflows = unitOfWork.DA_TRN_CASHFLOWRepository.GetAllByEngineDate(ProcessingDate).Where(t => t.DA_TRN.MA_STATUS.LABEL != StatusCode.CANCELLED.ToString()) .ToList();
                List<MA_LIMIT_PRODUCT> limitproducts = unitOfWork.MA_LIMIT_PRODUCTRepository.GetAll();

                //Get NON-IRS own exposure
                var nonIRSLimits = (from cashflow in Cashflows
                                   join lp in limitproducts on cashflow.DA_TRN.PRODUCT_ID equals lp.PRODUCT_ID
                                    where (cashflow.FLOW_AMOUNT_THB > 0)
                                            && (cashflow.DA_TRN.SOURCE == Source || Source == "")
                                            && (cashflow.DA_TRN.FLAG_SETTLE == true)
                                            && (cashflow.DA_TRN.MA_INSRUMENT.LABEL != "IRS")
                                            && (cashflow.DA_TRN.CTPY_ID == CTPY_ID || CTPY_ID == Guid.Empty)
                                            && (cashflow.DA_TRN_ID != ExcludeID1 || ExcludeID1 == Guid.Empty)
                                            && (cashflow.DA_TRN_ID != ExcludeID2 || ExcludeID2 == Guid.Empty)
                                   select new
                                   {
                                       engineDate = cashflow.DA_TRN.ENGINE_DATE,
                                       ctpyID = cashflow.DA_TRN.CTPY_ID,
                                       limitID = lp.LIMIT_ID,
                                       flowDate = cashflow.FLOW_DATE,
                                       flowAmount = cashflow.FLOW_AMOUNT_THB
                                   }).GroupBy(tr => new { tr.engineDate, tr.ctpyID, tr.limitID, tr.flowDate })
                                 .Select(p => new SCEGroupModel
                                 {
                                     ENGINE_DATE = Convert.ToDateTime(p.Key.engineDate),
                                     CTPY_ID = p.Key.ctpyID,
                                     LIMIT_ID = p.Key.limitID,
                                     FLOW_DATE = p.Key.flowDate,
                                     FLOW_AMOUNT = p.Sum(x => x.flowAmount)
                                 });

                //Get IRS own exposure
                var irsLimits = (from cashflow in Cashflows
                                 join lp in limitproducts on cashflow.DA_TRN.PRODUCT_ID equals lp.PRODUCT_ID
                                 where (cashflow.DA_TRN.SOURCE == Source || Source == "")
                                          && (cashflow.DA_TRN.FLAG_SETTLE == true)
                                          && (cashflow.DA_TRN.MA_INSRUMENT.LABEL == "IRS")
                                          && (cashflow.DA_TRN_ID != ExcludeID1 || ExcludeID1 == Guid.Empty)
                                          && (cashflow.DA_TRN_ID != ExcludeID2 || ExcludeID2 == Guid.Empty)
                                 select new
                                 {
                                     engineDate = cashflow.DA_TRN.ENGINE_DATE,
                                     ctpyID = cashflow.DA_TRN.CTPY_ID,
                                     limitID = lp.LIMIT_ID,
                                     flowDate = cashflow.FLOW_DATE,
                                     flowCCY = cashflow.FLAG_FIRST ? cashflow.DA_TRN.FIRST.CCY_ID.Value : cashflow.DA_TRN.SECOND.CCY_ID.Value,
                                     flowAmount = cashflow.FLOW_AMOUNT_THB
                                 }).GroupBy(tr => new { tr.engineDate, tr.ctpyID, tr.limitID, tr.flowDate, tr.flowCCY })
                                 .Select(p => new SCEGroupModel
                                 {
                                     ENGINE_DATE = Convert.ToDateTime(p.Key.engineDate),
                                     CTPY_ID = p.Key.ctpyID,
                                     LIMIT_ID = p.Key.limitID,
                                     FLOW_DATE = p.Key.flowDate,
                                     FLOW_CCY = p.Key.flowCCY,
                                     FLOW_AMOUNT = p.Sum(x => x.flowAmount)
                                 });

                //Get NON-IRS children exposure
                var nonIRSGroup = (from cashflow in Cashflows
                                   join lp in limitproducts on cashflow.DA_TRN.PRODUCT_ID equals lp.PRODUCT_ID
                                   where (cashflow.FLOW_AMOUNT_THB > 0)
                                           && (cashflow.DA_TRN.SOURCE == Source || Source == "")
                                           && (cashflow.DA_TRN.FLAG_SETTLE == true)
                                           && (cashflow.DA_TRN.MA_INSRUMENT.LABEL != "IRS")
                                           && (cashflow.DA_TRN.MA_COUTERPARTY.GROUP_CTPY_ID != null)
                                           && (cashflow.DA_TRN.MA_COUTERPARTY.GROUP_CTPY_ID == CTPY_ID || CTPY_ID == Guid.Empty)
                                           && (cashflow.DA_TRN_ID != ExcludeID1 || ExcludeID1 == Guid.Empty)
                                           && (cashflow.DA_TRN_ID != ExcludeID2 || ExcludeID2 == Guid.Empty)
                                   select new
                                   {
                                       engineDate = cashflow.DA_TRN.ENGINE_DATE,
                                       groupID = cashflow.DA_TRN.MA_COUTERPARTY.GROUP_CTPY_ID.Value,
                                       limitID = lp.LIMIT_ID,
                                       flowDate = cashflow.FLOW_DATE,
                                       flowAmount = cashflow.FLOW_AMOUNT_THB
                                   }).GroupBy(tr => new { tr.engineDate, tr.groupID, tr.limitID, tr.flowDate })
                                 .Select(p => new SCEGroupModel
                                 {
                                     ENGINE_DATE = Convert.ToDateTime(p.Key.engineDate),
                                     CTPY_ID = p.Key.groupID,
                                     LIMIT_ID = p.Key.limitID,
                                     FLOW_DATE = p.Key.flowDate,
                                     FLOW_AMOUNT = p.Sum(x => x.flowAmount)
                                 });

                //Get IRS children exposure
                var irsGroup = (from cashflow in Cashflows
                                 join lp in limitproducts on cashflow.DA_TRN.PRODUCT_ID equals lp.PRODUCT_ID
                                 where (cashflow.DA_TRN.SOURCE == Source || Source == "")
                                          && (cashflow.DA_TRN.FLAG_SETTLE == true)
                                          && (cashflow.DA_TRN.MA_INSRUMENT.LABEL == "IRS")
                                          && (cashflow.DA_TRN_ID != ExcludeID1 || ExcludeID1 == Guid.Empty)
                                          && (cashflow.DA_TRN_ID != ExcludeID2 || ExcludeID2 == Guid.Empty)
                                          && (cashflow.DA_TRN.MA_COUTERPARTY.GROUP_CTPY_ID != null)
                                          && (cashflow.DA_TRN.MA_COUTERPARTY.GROUP_CTPY_ID == CTPY_ID || CTPY_ID == Guid.Empty)
                                 select new
                                 {
                                     engineDate = cashflow.DA_TRN.ENGINE_DATE,
                                     groupID = cashflow.DA_TRN.MA_COUTERPARTY.GROUP_CTPY_ID.Value,
                                     limitID = lp.LIMIT_ID,
                                     flowDate = cashflow.FLOW_DATE,
                                     flowCCY = cashflow.FLAG_FIRST ? cashflow.DA_TRN.FIRST.CCY_ID.Value : cashflow.DA_TRN.SECOND.CCY_ID.Value,
                                     flowAmount = cashflow.FLOW_AMOUNT_THB
                                 }).GroupBy(tr => new { tr.engineDate, tr.groupID, tr.limitID, tr.flowDate, tr.flowCCY })
                                 .Select(p => new SCEGroupModel
                                 {
                                     ENGINE_DATE = Convert.ToDateTime(p.Key.engineDate),
                                     CTPY_ID = p.Key.groupID,
                                     LIMIT_ID = p.Key.limitID,
                                     FLOW_DATE = p.Key.flowDate,
                                     FLOW_CCY = p.Key.flowCCY,
                                     FLOW_AMOUNT = p.Sum(x => x.flowAmount)
                                 });

                var limitGroups = nonIRSLimits.Union(irsLimits).Union(nonIRSGroup).Union(irsGroup)
                                    .Where(p => p.FLOW_AMOUNT > 0)
                                    .GroupBy(tr => new { tr.ENGINE_DATE, tr.CTPY_ID, tr.LIMIT_ID, tr.FLOW_DATE })
                                    .Select(t => new SCEGroupModel
                                    {
                                        ENGINE_DATE = t.Key.ENGINE_DATE,
                                        CTPY_ID = t.Key.CTPY_ID,
                                        LIMIT_ID = t.Key.LIMIT_ID,
                                        FLOW_DATE = t.Key.FLOW_DATE,
                                        FLOW_AMOUNT = t.Sum(x => x.FLOW_AMOUNT)
                                    });

                limits = (from ctlm in unitOfWork.MA_CTPY_LIMITRepository.GetAll()
                          join ct in unitOfWork.MA_COUTERPARTYRepository.GetAll() on ctlm.CTPY_ID equals ct.ID
                          join lm in unitOfWork.MA_LIMITRepository.GetAll() on ctlm.LIMIT_ID equals lm.ID
                          join pd in limitproducts on lm.ID equals pd.LIMIT_ID
                          join lg in limitGroups on new { ctlm.CTPY_ID, ctlm.LIMIT_ID } equals new { lg.CTPY_ID, lg.LIMIT_ID } into lj
                          from sublg in lj.DefaultIfEmpty()
                          where (ctlm.CTPY_ID == CTPY_ID || CTPY_ID == Guid.Empty)
                                  && (pd.PRODUCT_ID == ProductID || ProductID == Guid.Empty)
                                  && (lm.LIMIT_TYPE == "S")
                                  && (ct.ISACTIVE.Value)
                                  //&& (lg.ENGINE_DATE == ProcessingDate)
                          select new LimitCheckModel
                          {
                              SNAME = ct.SNAME,
                              LIMIT_LABEL = lm.LABEL,
                              CTPY_LIMIT_ID = ctlm.ID,
                              FLAG_CONTROL = ctlm.FLAG_CONTROL,
                              GEN_AMOUNT = ctlm.AMOUNT,
                              //AMOUNT = ctlm.AMOUNT,
                              EXPIRE_DATE = ctlm.EXPIRE_DATE,
                              PROCESSING_DATE = ProcessingDate,
                              FLOW_DATE = (sublg != null ? sublg.FLOW_DATE.Value : ProcessingDate),
                              ORIGINAL_KK_CONTRIBUTE = (sublg != null ? sublg.FLOW_AMOUNT.Value : 0)
                          }).ToList();
            }

            return limits;
        }

        public List<LimitCheckModel> GetCountrySETByCriteria(DateTime ProcessingDate, Guid CountryID, string Source, Guid ExcludeID1, Guid ExcludeID2)
        {
            List<LimitCheckModel> limits = null;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                List<DA_TRN_CASHFLOW> Cashflows = unitOfWork.DA_TRN_CASHFLOWRepository.GetAllByEngineDate(ProcessingDate).Where(t => t.DA_TRN.MA_STATUS.LABEL != StatusCode.CANCELLED.ToString()).ToList();
                List<DA_TRN> Trans = unitOfWork.DA_TRNRepository.GetAllByEngineDate(ProcessingDate).Where(t => t.MA_STATUS.LABEL != StatusCode.CANCELLED.ToString()).ToList();

                //SETTLEMENT LIMIT PART
                //NON-IRS settlement exposure
                var nonIRSSETs = (from cashflow in Cashflows
                                    where (cashflow.FLOW_AMOUNT_THB > 0)
                                            && (cashflow.DA_TRN.SOURCE == Source || Source == "")
                                            && (cashflow.DA_TRN.FLAG_SETTLE == true)
                                            && (cashflow.DA_TRN.MA_INSRUMENT.LABEL != "IRS")
                                            && (cashflow.DA_TRN.MA_COUTERPARTY.COUNTRY_ID == CountryID || CountryID == Guid.Empty)
                                            && (cashflow.DA_TRN_ID != ExcludeID1 || ExcludeID1 == Guid.Empty)
                                            && (cashflow.DA_TRN_ID != ExcludeID2 || ExcludeID2 == Guid.Empty)
                                    select new
                                    {
                                        engineDate = cashflow.DA_TRN.ENGINE_DATE,
                                        countryID = cashflow.DA_TRN.MA_COUTERPARTY.COUNTRY_ID,
                                        expDate = cashflow.FLOW_DATE,
                                        exp = cashflow.FLOW_AMOUNT_THB
                                    }).GroupBy(tr => new { tr.engineDate, tr.countryID, tr.expDate })
                                 .Select(p => new CountryLimitModel
                                 {
                                     ENGINE_DATE = Convert.ToDateTime(p.Key.engineDate),
                                     COUNTRY_ID = p.Key.countryID,
                                     EXPOSURE_DATE = p.Key.expDate,
                                     EXPOSURE = p.Sum(x => x.exp)
                                 });

                //IRS settlement exposure
                var irsSETs = (from cashflow in Cashflows
                                 where (cashflow.DA_TRN.SOURCE == Source || Source == "")
                                          && (cashflow.DA_TRN.FLAG_SETTLE == true)
                                          && (cashflow.DA_TRN.MA_INSRUMENT.LABEL == "IRS")
                                          && (cashflow.DA_TRN.MA_COUTERPARTY.COUNTRY_ID == CountryID || CountryID == Guid.Empty)
                                          && (cashflow.DA_TRN_ID != ExcludeID1 || ExcludeID1 == Guid.Empty)
                                          && (cashflow.DA_TRN_ID != ExcludeID2 || ExcludeID2 == Guid.Empty)
                                 select new
                                 {
                                     engineDate = cashflow.DA_TRN.ENGINE_DATE,
                                     countryID = cashflow.DA_TRN.MA_COUTERPARTY.COUNTRY_ID,
                                     expDate = cashflow.FLOW_DATE,
                                     ccy = cashflow.FLAG_FIRST ? cashflow.DA_TRN.FIRST.CCY_ID.Value : cashflow.DA_TRN.SECOND.CCY_ID.Value,
                                     exp = cashflow.FLOW_AMOUNT_THB
                                 }).GroupBy(tr => new { tr.engineDate, tr.countryID, tr.expDate, tr.ccy })
                                 .Select(p => new CountryLimitModel
                                 {
                                     ENGINE_DATE = Convert.ToDateTime(p.Key.engineDate),
                                     COUNTRY_ID = p.Key.countryID,
                                     EXPOSURE_DATE = p.Key.expDate,
                                     CCY = p.Key.ccy,
                                     EXPOSURE = p.Sum(x => x.exp)
                                 });

                var allSETs = nonIRSSETs.Union(irsSETs).Where(p => p.EXPOSURE > 0)
                                    .GroupBy(tr => new { tr.ENGINE_DATE, tr.COUNTRY_ID, tr.EXPOSURE_DATE })
                                    .Select(t => new CountryLimitModel
                                    {
                                        ENGINE_DATE = t.Key.ENGINE_DATE,
                                        COUNTRY_ID = t.Key.COUNTRY_ID,
                                        EXPOSURE_DATE = t.Key.EXPOSURE_DATE,
                                        EXPOSURE = t.Sum(x => x.EXPOSURE)
                                    });
                                
                limits = (from countrylimit in unitOfWork.MA_COUNTRY_LIMITRepository.GetAll()
                          join exp in allSETs on countrylimit.COUNTRY_ID equals exp.COUNTRY_ID into exptemp
                          from subexp in exptemp.DefaultIfEmpty()
                          where (countrylimit.COUNTRY_ID == CountryID || CountryID == Guid.Empty)
                                 && (countrylimit.ISTEMP == false)
                                 && countrylimit.ISACTIVE
                                 //&& countrylimit.EFFECTIVE_DATE.Date <= ProcessingDate
                          select new LimitCheckModel
                          {
                              COUNTRY_LABEL = countrylimit.MA_COUNTRY.LABEL,
                              COUNTRY_ID = countrylimit.COUNTRY_ID,
                              FLAG_CONTROL = countrylimit.FLAG_CONTROL,
                              GEN_AMOUNT = countrylimit.EFFECTIVE_DATE.Date <= ProcessingDate ? countrylimit.AMOUNT : 0,
                              //AMOUNT = countrylimit.EFFECTIVE_DATE.Date <= ProcessingDate ? countrylimit.AMOUNT : 0,
                              PROCESSING_DATE = ProcessingDate,
                              EXPIRE_DATE = countrylimit.EXPIRY_DATE,
                              FLOW_DATE = subexp != null ? subexp.EXPOSURE_DATE.Value : ProcessingDate,
                              ORIGINAL_KK_CONTRIBUTE = subexp != null ? subexp.EXPOSURE.Value : 0,
                              SET_CONTRIBUTE = subexp != null ? subexp.EXPOSURE.Value : 0
                          }).OrderBy(p => p.COUNTRY_LABEL).ThenBy(p => p.FLOW_DATE).ToList();
            }

            return limits;
        }

        public List<LimitCheckModel> GetCountryPCEByCriteria(DateTime ProcessingDate, Guid CountryID, string Source, Guid ExcludeID1, Guid ExcludeID2)
        {
            List<LimitCheckModel> limits = null;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                List<DA_TRN> Trans = unitOfWork.DA_TRNRepository.GetAllByEngineDate(ProcessingDate).Where(t => t.MA_STATUS.LABEL != StatusCode.CANCELLED.ToString()).ToList();

                //PRE-SETTLEMENT LIMIT PART
                var allPCEs = (from trn in Trans
                               where (trn.SOURCE == Source || Source == "")
                                        && (trn.ID != ExcludeID1 || ExcludeID1 == Guid.Empty)
                                        && (trn.ID != ExcludeID2 || ExcludeID2 == Guid.Empty)
                                        && (trn.MA_COUTERPARTY.COUNTRY_ID == CountryID || CountryID == Guid.Empty)
                               select new
                               {
                                   engineDate = trn.ENGINE_DATE,
                                   countryID = trn.MA_COUTERPARTY.COUNTRY_ID,
                                   exp = trn.KK_CONTRIBUTE
                               }).GroupBy(tr => new { tr.engineDate, tr.countryID })
                                 .Select(p => new CountryLimitModel
                                 {
                                     ENGINE_DATE = Convert.ToDateTime(p.Key.engineDate),
                                     COUNTRY_ID = p.Key.countryID,
                                     EXPOSURE = p.Sum(x => x.exp)
                                 });

                limits = (from countrylimit in unitOfWork.MA_COUNTRY_LIMITRepository.GetAll()
                          join exp in allPCEs on countrylimit.COUNTRY_ID equals exp.COUNTRY_ID into exptemp
                          from subexp in exptemp.DefaultIfEmpty()
                          where (countrylimit.COUNTRY_ID == CountryID || CountryID == Guid.Empty)
                                 && (countrylimit.ISTEMP == false)
                                 && countrylimit.ISACTIVE
                          //&& countrylimit.EFFECTIVE_DATE.Date <= ProcessingDate
                          select new LimitCheckModel
                          {
                              COUNTRY_LABEL = countrylimit.MA_COUNTRY.LABEL,
                              COUNTRY_ID = countrylimit.COUNTRY_ID,
                              FLAG_CONTROL = countrylimit.FLAG_CONTROL,
                              GEN_AMOUNT = countrylimit.EFFECTIVE_DATE.Date <= ProcessingDate ? countrylimit.AMOUNT : 0,
                              //AMOUNT = countrylimit.EFFECTIVE_DATE.Date <= ProcessingDate ? countrylimit.AMOUNT : 0,
                              PROCESSING_DATE = ProcessingDate,
                              EXPIRE_DATE = countrylimit.EXPIRY_DATE,
                              FLOW_DATE = ProcessingDate,
                              ORIGINAL_KK_CONTRIBUTE = subexp != null ? subexp.EXPOSURE.Value : 0,
                              PCE_CONTRIBUTE = subexp != null ? subexp.EXPOSURE.Value : 0
                          }).OrderBy(p => p.COUNTRY_LABEL).ThenBy(p => p.FLOW_DATE).ToList();
            }

            return limits;
        }
    }
}
