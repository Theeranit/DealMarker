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
using KK.DealMaker.Business.Deal;
using KK.DealMaker.Business.Log;
using KK.DealMaker.Business.Master;
using System.Globalization;

namespace KK.DealMaker.Business.Report
{
    public class ReportBusiness : BaseBusiness
    {
        public List<LimitCheckModel> GetPCEReport(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strLimit, string strSource, string strStatus)
        {
            try
            {
                DateTime dteReport;
                LimitCheckBusiness _limitBusiness = new LimitCheckBusiness();
                DealBusiness _dealBusiness = new DealBusiness();
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                Guid guCtpyID = Guid.Empty;
                Guid guLimitID = Guid.Empty;

                if (String.IsNullOrEmpty(strReportDate))
                    throw this.CreateException(new Exception(), "Please input report date.");
                else if (!DateTime.TryParseExact(strReportDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dteReport))
                    throw this.CreateException(new Exception(), "Invalid report date.");
                else
                    dteReport = DateTime.ParseExact(strReportDate, "dd/MM/yyyy", null);

                if (Guid.TryParse(strCtpy, out guCtpyID))
                {
                    guCtpyID = Guid.Parse(strCtpy);
                }
                              
                if (_dealBusiness.CountByProcessDate(dteReport) == 0)
                {
                    throw this.CreateException(new Exception(), "No data for selected report date.");
                }

                var limits = _limitBusiness.GetPCEByCriteria(dteReport, guCtpyID, Guid.Empty, strSource, Guid.Empty, Guid.Empty).Distinct(new LimitCheckComparer()).AsQueryable();

                //Get temp limit
                //Look for temp limit when all conditions meet
                // 1. Transaction maturity date <= Temp limit maturity date
                foreach (LimitCheckModel limit in limits)
                {
                    MA_TEMP_CTPY_LIMIT temp_limit = _counterpartyBusiness.GetActiveTempByID(sessioninfo.Process.CurrentDate, sessioninfo.Process.CurrentDate, limit.CTPY_LIMIT_ID);

                    if (temp_limit != null)
                        limit.TEMP_AMOUNT = temp_limit.AMOUNT;
                }

                //Additional filter on limit name
                if (Guid.TryParse(strLimit, out guLimitID))
                {
                    LookupBusiness _lookupBusiness = new LookupBusiness();
                    MA_LIMIT limit = _lookupBusiness.GetLimitAll().FirstOrDefault(t => t.ID == Guid.Parse(strLimit));

                    limits = limits.Where(t => t.LIMIT_LABEL.IndexOf(limit.LABEL, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                if (strStatus != "")
                {
                    limits = limits.Where(t => t.STATUS.IndexOf(strStatus, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                return limits.ToList();
            }

            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public List<DA_TRN> GetPCEDetailReport(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strProduct, string strSource)
        {
            try
            {
                DateTime dteReport;
                DealBusiness _dealBusiness = new DealBusiness();
                Guid guTemp;

                if (String.IsNullOrEmpty(strReportDate))
                    throw this.CreateException(new Exception(), "Please input report date.");
                else if (!DateTime.TryParseExact(strReportDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dteReport))
                    throw this.CreateException(new Exception(), "Invalid report date.");
                else
                    dteReport = DateTime.ParseExact(strReportDate, "dd/MM/yyyy", null);

                if (_dealBusiness.CountByProcessDate(dteReport) == 0)
                {
                    throw this.CreateException(new Exception(), "No data for selected report date.");
                }

                var trns = _dealBusiness.GetDealByProcessDate(dteReport).Where(p => p.MA_STATUS.LABEL.ToString() != StatusCode.CANCELLED.ToString()).AsQueryable();

                if (!string.IsNullOrEmpty(strSource))
                {
                    trns = trns.Where(p => p.SOURCE == strSource);
                }

                if (Guid.TryParse(strCtpy, out guTemp))
                {
                    trns = trns.Where(t => t.CTPY_ID == Guid.Parse(strCtpy));
                }

                if (Guid.TryParse(strProduct, out guTemp))
                {
                    trns = trns.Where(s => s.PRODUCT_ID == Guid.Parse(strProduct));
                }

                return trns.ToList();
            }

            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public List<LimitCheckModel> GetSCEReport(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strSource, string strStatus)
        {
            try
            {
                DateTime dteReport;
                LimitCheckBusiness _limitBusiness = new LimitCheckBusiness();
                DealBusiness _dealBusiness = new DealBusiness();
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                Guid guCtpyID = Guid.Empty;

                if (String.IsNullOrEmpty(strReportDate))
                    throw this.CreateException(new Exception(), "Please input report date.");
                else if (!DateTime.TryParseExact(strReportDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dteReport))
                    throw this.CreateException(new Exception(), "Invalid report date.");
                else
                    dteReport = DateTime.ParseExact(strReportDate, "dd/MM/yyyy", null);

                if (_dealBusiness.CountByProcessDate(dteReport) == 0)
                {
                    throw this.CreateException(new Exception(), "No data for selected report date.");
                }

                if (Guid.TryParse(strCtpy, out guCtpyID))
                {
                    guCtpyID = Guid.Parse(strCtpy);
                }

                var limits = _limitBusiness.GetSCEByCriteria(dteReport, guCtpyID, Guid.Empty, strSource, Guid.Empty, Guid.Empty)
                                            .OrderBy(p => p.SNAME).ThenBy(p => p.FLOW_DATE)
                                            .Distinct(new LimitCheckComparer()).AsQueryable();

                //Get temp limit
                //Look for temp limit when all conditions meet
                // 1. Transaction maturity date <= Temp limit maturity date
                foreach (LimitCheckModel limit in limits)
                {
                    MA_TEMP_CTPY_LIMIT temp_limit = _counterpartyBusiness.GetActiveTempByID(sessioninfo.Process.CurrentDate, limit.FLOW_DATE, limit.CTPY_LIMIT_ID);

                    if (temp_limit != null)
                        limit.TEMP_AMOUNT = temp_limit.AMOUNT;
                }

                if (strStatus != "" )
                {
                    limits = limits.Where(t => t.STATUS.IndexOf(strStatus, StringComparison.OrdinalIgnoreCase) >= 0);
                }                      

                return limits.ToList();
            }

            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public List<DA_TRN_CASHFLOW> GetSCEDetailReport(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strProduct, string strSource)
        {
            try
            {
                DateTime dteReport;
                DealBusiness _dealBusiness = new DealBusiness();
                Guid guTemp;

                if (String.IsNullOrEmpty(strReportDate))
                    throw this.CreateException(new Exception(), "Please input report date.");
                else if (!DateTime.TryParseExact(strReportDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dteReport))
                    throw this.CreateException(new Exception(), "Invalid report date.");
                else
                    dteReport = DateTime.ParseExact(strReportDate, "dd/MM/yyyy", null);

                if (_dealBusiness.CountByProcessDate(dteReport) == 0)
                {
                    throw this.CreateException(new Exception(), "No data for selected report date.");
                }

                var cashflows = _dealBusiness.GetFlowsByProcessDate(dteReport).AsQueryable();

                if (!string.IsNullOrEmpty(strSource))
                {
                    cashflows = cashflows.Where(p => p.DA_TRN.SOURCE == strSource);
                }

                if (Guid.TryParse(strCtpy, out guTemp))
                {
                    cashflows = cashflows.Where(p => p.DA_TRN.CTPY_ID == Guid.Parse(strCtpy));
                }

                if (Guid.TryParse(strProduct, out guTemp))
                {
                    cashflows = cashflows.Where(p => p.DA_TRN.PRODUCT_ID == Guid.Parse(strProduct));
                }

                return cashflows.OrderBy(p => p.DA_TRN.INT_DEAL_NO).ThenByDescending(p => p.FLAG_FIRST).ThenBy(p => p.SEQ).ToList();
            }

            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public List<LimitAuditReportModel> GetLimitAuditReport(SessionInfo sessionInfo, string strLogDatefrom, string strLogDateto, string strCtpy, string strCountry, string strEvent)
        {
            try
            {
                DateTime dteReportfrom;
                DateTime dteReportto;
                Guid guCtpyID = Guid.Empty;
                Guid guCountryID = Guid.Empty;
                LogBusiness _logBusiness = new LogBusiness();
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                CountryBusiness _countryBusiness = new CountryBusiness();
                UserBusiness _userBusiness = new UserBusiness();

                var log = _logBusiness.GetLogAll().Where(l => l.EVENT == LimitLogEvent.LIMIT_AUDIT.ToString()
                                                        || l.EVENT == LimitLogEvent.TEMP_LIMIT_AUDIT.ToString()
                                                        || l.EVENT == LimitLogEvent.COUNTRY_LIMIY_AUDIT.ToString()
                                                        || l.EVENT == LimitLogEvent.TEMP_COUNTRY_LIMIT_AUDIT.ToString()).AsQueryable();

                var ctpyLimit = _counterpartyBusiness.GetCounterpartyLimitAll().AsQueryable();
                var ctpys = _counterpartyBusiness.GetCounterpartyAll().AsQueryable();
                var countrys = _countryBusiness.GetCountryAll().AsQueryable();
                var users = _userBusiness.GetAll().AsQueryable();

                if (String.IsNullOrEmpty(strLogDatefrom))
                    throw this.CreateException(new Exception(), "Please input start log date.");
                else if (!DateTime.TryParseExact(strLogDatefrom, "dd/MM/yyyy", null, DateTimeStyles.None, out dteReportfrom))
                    throw this.CreateException(new Exception(), "Invalid start log date.");
                else
                    dteReportfrom = DateTime.ParseExact(strLogDatefrom, "dd/MM/yyyy", null);

                if (String.IsNullOrEmpty(strLogDateto))
                    throw this.CreateException(new Exception(), "Please input end log date.");
                else if (!DateTime.TryParseExact(strLogDateto, "dd/MM/yyyy", null, DateTimeStyles.None, out dteReportto))
                    throw this.CreateException(new Exception(), "Invalid end log date.");
                else
                    dteReportto = DateTime.ParseExact(strLogDateto, "dd/MM/yyyy", null);
                if (dteReportto < dteReportfrom) throw this.CreateException(new Exception(), "Start log date must before end log date.");
                                
                var limits = from limit in ctpyLimit
                             join ctpy in ctpys on limit.CTPY_ID equals ctpy.ID
                             select new
                             {
                                 LIMIT_ID = limit.ID,
                                 CTPY_ID = ctpy.ID,
                                 SNAME = ctpy.SNAME,
                                 LIMIT = limit.MA_LIMIT.LABEL
                             };

                log = log.Where(l => l.LOG_DATE.Date >= dteReportfrom.Date && l.LOG_DATE.Date <= dteReportto.Date);

                if (strEvent != "-1")
                    log = log.Where(p => p.EVENT == strEvent);

                var limitAudits = from l in log
                                  join user in users on l.LOG.INSERTBYUSERID equals user.ID
                                  join c in limits on l.RECORD_ID equals c.LIMIT_ID into templimit
                                  from sublimit in templimit.DefaultIfEmpty()
                                  join country in countrys on l.RECORD_ID equals country.ID into tempcountry
                                  from subcountry in tempcountry.DefaultIfEmpty()
                                  orderby l.LOG_DATE
                                  select new LimitAuditReportModel
                                  {
                                      ENTITY = sublimit != null ? sublimit.SNAME : subcountry != null ? subcountry.LABEL : "",
                                      ENTITY_ID = sublimit != null ? sublimit.CTPY_ID : (subcountry != null ? subcountry.ID : Guid.Empty),
                                      LIMIT = (l.EVENT.Contains("TEMP") ? "TEMP-" : "") + (sublimit != null ? sublimit.LIMIT : "COUNTRY-LIMIT"), //sublimit != null ? sublimit.LIMIT : "Country Limit",
                                      USER = user.USERCODE,
                                      LOG_DATE = l.LOG_DATE,
                                      LOG_DATE_STR = l.LOG_DATE.ToString("dd-MMM-yyyy HH:mm"),
                                      DETAIL = l.LOG_DETAIL
                                  };

                if (Guid.TryParse(strCtpy, out guCtpyID))
                {
                    guCtpyID = Guid.Parse(strCtpy);
                    limitAudits = limitAudits.Where(p => p.ENTITY_ID == guCtpyID);
                }

                if (Guid.TryParse(strCountry, out guCountryID))
                {
                    guCountryID = Guid.Parse(strCountry);
                    limitAudits = limitAudits.Where(t => t.ENTITY_ID == guCountryID);
                }
                
                return limitAudits.ToList();
            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public List<DA_TRN> GetLimitOverwriteReport(SessionInfo sessionInfo, string strReportDate, string strCtpy)
        {
            try
            {
                DateTime dteReport;
                Guid guCtpyID = Guid.Empty;
                DealBusiness _dealBusiness = new DealBusiness();
                if (String.IsNullOrEmpty(strReportDate))
                    throw this.CreateException(new Exception(), "Please input date.");
                else if (!DateTime.TryParseExact(strReportDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dteReport))
                    throw this.CreateException(new Exception(), "Invalid date.");
                else
                    dteReport = DateTime.ParseExact(strReportDate, "dd/MM/yyyy", null);

                var trns = _dealBusiness.GetDealByProcessDate(dteReport).AsQueryable();
                trns = trns.Where(t => t.OVER_APPROVER != null && t.SOURCE == "INT" && t.MA_STATUS.LABEL != StatusCode.CANCELLED.ToString());
                if (Guid.TryParse(strCtpy, out guCtpyID))
                {
                    trns = trns.Where(a => a.CTPY_ID == guCtpyID);
                }
                return trns.ToList();
            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public List<RepoReportModel> GetRepoReport(SessionInfo sessioninfo, string strReportDate, string strSource, string strCtpy)
        {
            try
            {
                DateTime dteReport;
                LimitCheckBusiness _limitBusiness = new LimitCheckBusiness();
                DealBusiness _dealBusiness = new DealBusiness();
                LimitProductBusiness _limitProductBusiness = new LimitProductBusiness();
                LookupBusiness _lookupBusiness = new LookupBusiness();
                StaticDataBusiness _staticBusiness = new StaticDataBusiness();
                List<RepoReportModel> reports = new List<RepoReportModel>();
                RepoReportModel report;
                Guid guCtpyID = Guid.Empty;

                if (String.IsNullOrEmpty(strReportDate))
                    throw this.CreateException(new Exception(), "Please input report date.");
                else if (!DateTime.TryParseExact(strReportDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dteReport))
                    throw this.CreateException(new Exception(), "Invalid report date.");
                else
                    dteReport = DateTime.ParseExact(strReportDate, "dd/MM/yyyy", null);

                if (Guid.TryParse(strCtpy, out guCtpyID))
                {
                    guCtpyID = Guid.Parse(strCtpy);
                }

                Guid guProductID = _lookupBusiness.GetProductByUsercode(ProductCode.REPO.ToString()).ID;
                MA_PCCF rev_pccf = _staticBusiness.GetPCCFByID(sessioninfo, Guid.Parse("84f608c9-8b58-48eb-a6dd-a5407548784a"));
                MA_PCCF rep_gov_pccf = _staticBusiness.GetPCCFByID(sessioninfo, Guid.Parse("28b24b19-e81d-4f82-a2f0-7c834ee3f91c"));
                MA_PCCF rep_soe_pccf = _staticBusiness.GetPCCFByID(sessioninfo, Guid.Parse("7014b05e-198f-4f62-94b0-1d54322efbca"));

                var limits = _limitBusiness.GetPCEByCriteria(dteReport, guCtpyID, guProductID, strSource, Guid.Empty, Guid.Empty).Distinct(new LimitCheckComparer()).ToList();

                foreach (LimitCheckModel limit in limits)
                {
                    report = new RepoReportModel();

                    report.PROCESSING_DATE = limit.PROCESSING_DATE;
                    report.SNAME = limit.SNAME;
                    report.LIMIT_LABEL = limit.LIMIT_LABEL;
                    report.GEN_AMOUNT = limit.GEN_AMOUNT;
                    report.TEMP_AMOUNT = limit.TEMP_AMOUNT;
                    //report.AMOUNT = limit.AMOUNT;
                    report.EXPIRE_DATE = limit.EXPIRE_DATE;
                    report.ORIGINAL_KK_CONTRIBUTE = limit.ORIGINAL_KK_CONTRIBUTE;
                    report.DEAL_CONTRIBUTION = 0;
                    report.REV_AMOUNT = report.AVAILABLE / rev_pccf.C1.Value * 100;
                    report.REP_GOV_5_AMOUNT = report.AVAILABLE / rep_gov_pccf.C5.Value * 100;
                    report.REP_GOV_10_AMOUNT = report.AVAILABLE / rep_gov_pccf.C10.Value * 100;
                    report.REP_GOV_20_AMOUNT = report.AVAILABLE / rep_gov_pccf.C20.Value * 100;
                    report.REP_GOV_20s_AMOUNT = report.AVAILABLE / rep_gov_pccf.more20.Value * 100;
                    report.REP_SOE_5_AMOUNT = report.AVAILABLE / rep_soe_pccf.C5.Value * 100;
                    report.REP_SOE_10_AMOUNT = report.AVAILABLE / rep_soe_pccf.C10.Value * 100;
                    report.REP_SOE_20_AMOUNT = report.AVAILABLE / rep_soe_pccf.C20.Value * 100;
                    report.REP_SOE_20s_AMOUNT = report.AVAILABLE / rep_soe_pccf.more20.Value * 100;

                    reports.Add(report);
                }

                return reports;
            }

            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public List<LimitCheckModel> GetCountryReport(SessionInfo sessioninfo, string strReportDate, string strCountry, string strSource, string strStatus)
        {
            try
            {
                DateTime dteReport;
                LimitCheckBusiness _limitBusiness = new LimitCheckBusiness();
                DealBusiness _dealBusiness = new DealBusiness();
                CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
                CountryBusiness _countryBusiness = new CountryBusiness();
                Guid guCountryID = Guid.Empty;
                MA_COUNTRY_LIMIT temp_limit = null;

                if (String.IsNullOrEmpty(strReportDate))
                    throw this.CreateException(new Exception(), "Please input report date.");
                else if (!DateTime.TryParseExact(strReportDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dteReport))
                    throw this.CreateException(new Exception(), "Invalid report date.");
                else
                    dteReport = DateTime.ParseExact(strReportDate, "dd/MM/yyyy", null);

                if (_dealBusiness.CountByProcessDate(dteReport) == 0)
                {
                    throw this.CreateException(new Exception(), "No data for selected report date.");
                }

                if (Guid.TryParse(strCountry, out guCountryID))
                {
                    guCountryID = Guid.Parse(strCountry);
                }

                List<LimitCheckModel>  sets = _limitBusiness.GetCountrySETByCriteria(dteReport, guCountryID, strSource, Guid.Empty, Guid.Empty);
                List<LimitCheckModel> pces = _limitBusiness.GetCountryPCEByCriteria(dteReport, guCountryID, strSource, Guid.Empty, Guid.Empty);
               
                var reports = (from report in sets.Union(pces)
                               join pce in pces on report.COUNTRY_ID equals pce.COUNTRY_ID
                               select new LimitCheckModel
                               {
                                   COUNTRY_LABEL = report.COUNTRY_LABEL,
                                   COUNTRY_ID = report.COUNTRY_ID,
                                   FLAG_CONTROL = report.FLAG_CONTROL,
                                   GEN_AMOUNT = report.AMOUNT,
                                   PROCESSING_DATE = report.PROCESSING_DATE,
                                   EXPIRE_DATE = report.EXPIRE_DATE,
                                   FLOW_DATE = report.FLOW_DATE,
                                   SET_CONTRIBUTE = report.SET_CONTRIBUTE,
                                   PCE_CONTRIBUTE = pce.PCE_CONTRIBUTE
                               }).GroupBy(g => new
                               {
                                   g.COUNTRY_ID,
                                   g.COUNTRY_LABEL,
                                   g.FLAG_CONTROL,
                                   g.GEN_AMOUNT,
                                   g.PROCESSING_DATE,
                                   g.EXPIRE_DATE,
                                   g.FLOW_DATE,
                                   g.PCE_CONTRIBUTE
                               }).Select(s => new LimitCheckModel
                               {
                                   COUNTRY_LABEL = s.Key.COUNTRY_LABEL,
                                   COUNTRY_ID = s.Key.COUNTRY_ID,
                                   FLAG_CONTROL = s.Key.FLAG_CONTROL,
                                   GEN_AMOUNT = s.Key.GEN_AMOUNT,
                                   PROCESSING_DATE = s.Key.PROCESSING_DATE,
                                   EXPIRE_DATE = s.Key.EXPIRE_DATE,
                                   FLOW_DATE = s.Key.FLOW_DATE,
                                   PCE_CONTRIBUTE = s.Key.PCE_CONTRIBUTE,
                                   SET_CONTRIBUTE = s.Sum(x => x.SET_CONTRIBUTE),
                                   ORIGINAL_KK_CONTRIBUTE = s.Key.PCE_CONTRIBUTE + s.Sum(y => y.SET_CONTRIBUTE)
                               }).ToList();

                foreach (var report in reports)
                {
                    temp_limit = _countryBusiness.GetActiveTempByCountryID(sessioninfo.Process.CurrentDate, report.FLOW_DATE, report.COUNTRY_ID);

                    if (temp_limit != null)
                        report.TEMP_AMOUNT = temp_limit.AMOUNT;
                }

                if (strStatus != "")
                {
                    reports = reports.Where(t => t.STATUS.IndexOf(strStatus, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }

                return reports.OrderBy(p => p.COUNTRY_LABEL).ThenBy(t => t.FLOW_DATE).ToList();
            }

            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }
    }
}
