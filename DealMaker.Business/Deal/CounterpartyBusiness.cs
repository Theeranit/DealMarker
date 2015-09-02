using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.EnterpriseServices;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.DataAccess.Repositories;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.Business.Log;
using System.Configuration;
using System.Linq.Expressions;
using System.Collections;
using System.Globalization;

namespace KK.DealMaker.Business.Deal
{
    public class CounterpartyBusiness : BaseBusiness 
    {
        public List<MA_COUTERPARTY> GetByFilter(SessionInfo sessioninfo, string name, string sorting)
        {
            try
            {
                IEnumerable<MA_COUTERPARTY> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_COUTERPARTYRepository.GetAll().AsQueryable();
                    //Filters
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.SNAME.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_COUTERPARTY> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
                    sortedRecords = orderedRecords.ToList();
                    //if (sortsp[1].ToLower() == "desc") sortedRecords = sortedRecords.Reverse();
                }
                //Return result to jTable
                return sortedRecords.ToList();

            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public List<CptyLimitModel> GetGroupViewByFilter(SessionInfo sessioninfo, string name, string sorting)
        {
            try
            {
                List<CptyLimitModel> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_COUTERPARTYRepository.GetAll().AsQueryable();
                    var GroupIDList = query.Where(c => c.GROUP_CTPY_ID.HasValue).Select(c => c.GROUP_CTPY_ID.Value).ToList();
                    query = query.Where(t => GroupIDList.Contains(t.ID));
                    //Filters
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.SNAME.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    }

                    var Cptylimits = unitOfWork.MA_CTPY_LIMITRepository.GetAll().AsQueryable();
                    var limits = unitOfWork.MA_LIMITRepository.GetAll().AsQueryable();
                    var temp = (from o1 in Cptylimits
                               join o2 in limits on o1.LIMIT_ID equals o2.ID
                               where GroupIDList.Contains(o1.CTPY_ID)
                               select new { o1.CTPY_ID,o1.FLAG_CONTROL,o2.LABEL, o1.AMOUNT  }).ToList();
                                        

                    var result = from o1 in query
                                 select new CptyLimitModel
                                 {
                                     ID = o1.ID,
                                     SNAME = o1.SNAME,
                                     PCE_ALL = temp.Where(l => l.CTPY_ID == o1.ID && l.LABEL=="PCE-ALL").Select(l => l.AMOUNT.ToString("#,##0") + (l.FLAG_CONTROL ? "(C)":"(N)") ).FirstOrDefault(),
                                     PCE_FI = temp.Where(l => l.CTPY_ID == o1.ID && l.LABEL == "PCE-FI").Select(l => l.AMOUNT.ToString("#,##0") + (l.FLAG_CONTROL ? "(C)" : "(N)")).FirstOrDefault(),
                                     PCE_IRD = temp.Where(l => l.CTPY_ID == o1.ID && l.LABEL == "PCE-IRD").Select(l => l.AMOUNT.ToString("#,##0") + (l.FLAG_CONTROL ? "(C)" : "(N)")).FirstOrDefault(),
                                     PCE_FX = temp.Where(l => l.CTPY_ID == o1.ID && l.LABEL == "PCE-FX").Select(l => l.AMOUNT.ToString("#,##0") + (l.FLAG_CONTROL ? "(C)" : "(N)")).FirstOrDefault(),
                                     PCE_REPO = temp.Where(l => l.CTPY_ID == o1.ID && l.LABEL == "PCE-REPO").Select(l => l.AMOUNT.ToString("#,##0") + (l.FLAG_CONTROL ? "(C)" : "(N)")).FirstOrDefault(),
                                     SET_ALL = temp.Where(l => l.CTPY_ID == o1.ID && l.LABEL == "SET-ALL").Select(l => l.AMOUNT.ToString("#,##0") + (l.FLAG_CONTROL ? "(C)" : "(N)")).FirstOrDefault()
                                 };
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<CptyLimitModel> orderedRecords = result.OrderBy(sortsp[0], sortsp[1]);
                    sortedRecords = orderedRecords.ToList();
                    //if (sortsp[1].ToLower() == "desc") sortedRecords = sortedRecords.Reverse();
                }
                //Return result to jTable
                return sortedRecords;

            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }
        public List<CptyLimitModel> GetCtpyLimitGroupViewByCtpyID(SessionInfo sessioninfo, Guid ID)
        {
            try
            {
                List<CptyLimitModel> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_COUTERPARTYRepository.GetAll().AsQueryable();
                    query = query.Where(t => t.GROUP_CTPY_ID == ID);
                    var GroupIDList = query.Select(c => c.ID).ToList();
                    var Cptylimits = unitOfWork.MA_CTPY_LIMITRepository.GetAll().AsQueryable();
                    var limits = unitOfWork.MA_LIMITRepository.GetAll().AsQueryable();
                    var temp = (from o1 in Cptylimits
                                join o2 in limits on o1.LIMIT_ID equals o2.ID
                                where GroupIDList.Contains(o1.CTPY_ID)
                                select new { o1.CTPY_ID, o1.FLAG_CONTROL, o2.LABEL, o1.AMOUNT }).ToList();

                    
                    var result = from o1 in query
                                 select new CptyLimitModel
                                 {
                                     SNAME = o1.SNAME,
                                     PCE_ALL = temp.Where(l => l.CTPY_ID == o1.ID && l.LABEL == "PCE-ALL").Select(l => l.AMOUNT.ToString("#,##0") + (l.FLAG_CONTROL ? "(C)" : "(N)")).FirstOrDefault(),
                                     PCE_FI = temp.Where(l => l.CTPY_ID == o1.ID && l.LABEL == "PCE-FI").Select(l => l.AMOUNT.ToString("#,##0") + (l.FLAG_CONTROL ? "(C)" : "(N)")).FirstOrDefault(),
                                     PCE_IRD = temp.Where(l => l.CTPY_ID == o1.ID && l.LABEL == "PCE-IRD").Select(l => l.AMOUNT.ToString("#,##0") + (l.FLAG_CONTROL ? "(C)" : "(N)")).FirstOrDefault(),
                                     PCE_FX = temp.Where(l => l.CTPY_ID == o1.ID && l.LABEL == "PCE-FX").Select(l => l.AMOUNT.ToString("#,##0") + (l.FLAG_CONTROL ? "(C)" : "(N)")).FirstOrDefault(),
                                     PCE_REPO = temp.Where(l => l.CTPY_ID == o1.ID && l.LABEL == "PCE-REPO").Select(l => l.AMOUNT.ToString("#,##0") + (l.FLAG_CONTROL ? "(C)" : "(N)")).FirstOrDefault(),
                                     SET_ALL = temp.Where(l => l.CTPY_ID == o1.ID && l.LABEL == "SET-ALL").Select(l => l.AMOUNT.ToString("#,##0") + (l.FLAG_CONTROL ? "(C)" : "(N)")).FirstOrDefault()
                                 };
                    IQueryable<CptyLimitModel> orderedRecords = result.OrderBy(r=>r.SNAME);
                    sortedRecords = orderedRecords.ToList();
                    //if (sortsp[1].ToLower() == "desc") sortedRecords = sortedRecords.Reverse();
                }
                //Return result to jTable
                return sortedRecords;

            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }

        }
        
        public List<MA_COUTERPARTY> GetCounterpartyAll()
        {
            List<MA_COUTERPARTY> counterpartyList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                counterpartyList = unitOfWork.MA_COUTERPARTYRepository.All().ToList();
            }
            return counterpartyList;

        }

        public MA_COUTERPARTY GetByUsercode(int usercode)
        {
            MA_COUTERPARTY counterparty;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                counterparty = unitOfWork.MA_COUTERPARTYRepository.GetByUsercode(usercode);
            }
            return counterparty;

        }

        public MA_COUTERPARTY GetByShortName(string shortName)
        {
            MA_COUTERPARTY counterparty;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                counterparty = unitOfWork.MA_COUTERPARTYRepository.GetByShortName(shortName);
            }
            return counterparty;

        }

        public MA_COUTERPARTY Create(SessionInfo sessioninfo, MA_COUTERPARTY counterparty)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate1 = unitOfWork.MA_COUTERPARTYRepository.All().FirstOrDefault(p => p.USERCODE == counterparty.USERCODE);
                if (checkDuplicate1 != null)
                    throw this.CreateException(new Exception(), "OPICS ID is duplicated");
                var checkDuplicate2 = unitOfWork.MA_COUTERPARTYRepository.All().FirstOrDefault(p => p.SNAME == counterparty.SNAME);
                if (checkDuplicate2 != null)
                    throw this.CreateException(new Exception(), "Short name is duplicated");
                LogBusiness logBusiness = new LogBusiness();
                unitOfWork.DA_LOGGINGRepository.Add(logBusiness.CreateLogging(sessioninfo, counterparty.ID, LogEvent.COUNTERPARTY_AUDIT.ToString(), LookupFactorTables.MA_COUTERPARTY, "Counterparty", new { }));
                unitOfWork.MA_COUTERPARTYRepository.Add(counterparty);
                unitOfWork.Commit();
            }

            return counterparty;
        }

        public MA_COUTERPARTY Update(SessionInfo sessioninfo, MA_COUTERPARTY counterparty)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate1 = unitOfWork.MA_COUTERPARTYRepository.All().FirstOrDefault(p => p.USERCODE == counterparty.USERCODE && p.ID != counterparty.ID);
                if (checkDuplicate1 != null)
                    throw this.CreateException(new Exception(), "OPICS ID is duplicated");
                var checkDuplicate2 = unitOfWork.MA_COUTERPARTYRepository.All().FirstOrDefault(p => p.SNAME == counterparty.SNAME && p.ID != counterparty.ID);
                if (checkDuplicate2 != null)
                    throw this.CreateException(new Exception(), "Short name is duplicated");

                var foundData = unitOfWork.MA_COUTERPARTYRepository.All().FirstOrDefault(p => p.ID == counterparty.ID);
                if (foundData == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {
                    LogBusiness logBusiness = new LogBusiness();
                    string strOldGroupName = "";
                    string strNewGroupName = "";

                    if (foundData.GROUP_CTPY_ID != null)
                    {
                        var oldGroup = unitOfWork.MA_COUTERPARTYRepository.All().FirstOrDefault(p => p.GROUP_CTPY_ID == foundData.GROUP_CTPY_ID);
                        strOldGroupName = oldGroup != null ? oldGroup.SNAME : "";
                    }

                    if (counterparty.GROUP_CTPY_ID != null)
                    {
                        var newGroup = unitOfWork.MA_COUTERPARTYRepository.All().FirstOrDefault(p => p.GROUP_CTPY_ID == counterparty.GROUP_CTPY_ID);
                        strNewGroupName = newGroup != null ? newGroup.SNAME : "";
                    }

                    var oldRecord = new { USERCODE = foundData.USERCODE
                                        , BUSINESS = foundData.BUSINESS
                                        , FNAME = foundData.FNAME
                                        , SNAME = foundData.SNAME
                                        , TBMA_NAME = foundData.TBMA_NAME
                                        , GROUP = strOldGroupName
                                        , OUTLOOK = foundData.OUTLOOK
                                        , RATE = foundData.RATE
                                        , ISACTIVE = foundData.ISACTIVE };
                    var newRecord = new { USERCODE = counterparty.USERCODE
                                        , BUSINESS = counterparty.BUSINESS
                                        , FNAME = counterparty.FNAME
                                        , SNAME = counterparty.SNAME
                                        , TBMA_NAME = counterparty.TBMA_NAME
                                        , GROUP = strNewGroupName
                                        , OUTLOOK = counterparty.OUTLOOK
                                        , RATE = counterparty.RATE
                                        , ISACTIVE = counterparty.ISACTIVE };
                    
                    var log = logBusiness.UpdateLogging(sessioninfo, foundData.ID, LogEvent.COUNTERPARTY_AUDIT.ToString(), LookupFactorTables.MA_COUTERPARTY, oldRecord, newRecord);
                    if(log != null) unitOfWork.DA_LOGGINGRepository.Add(log);
                    foundData.ID = counterparty.ID;
                    foundData.USERCODE = counterparty.USERCODE;
                    foundData.ISACTIVE = counterparty.ISACTIVE;
                    foundData.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                    foundData.LOG.MODIFYDATE = DateTime.Now;
                    foundData.BUSINESS = counterparty.BUSINESS;
                    foundData.FNAME = counterparty.FNAME;
                    foundData.SNAME = counterparty.SNAME;
                    foundData.OUTLOOK = counterparty.OUTLOOK;
                    foundData.RATE = counterparty.RATE;
                    foundData.TBMA_NAME = counterparty.TBMA_NAME;
                    foundData.GROUP_CTPY_ID = counterparty.GROUP_CTPY_ID;
                    foundData.COUNTRY_ID = counterparty.COUNTRY_ID;
                    
                    unitOfWork.Commit();

                }
            }

            return counterparty;
        }

        public List<MA_CTPY_LIMIT> GetCounterpartyLimitByCtpyID(SessionInfo sessioninfo, Guid ID)
        {
            List<MA_CTPY_LIMIT> counterpartyLimits;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                counterpartyLimits = unitOfWork.MA_CTPY_LIMITRepository.GetAll().Where(p => p.CTPY_ID == ID).OrderBy(p => p.MA_LIMIT.INDEX).ToList();
            }
            return counterpartyLimits;

        }
        
        public List<MA_CTPY_LIMIT> GetCounterpartyLimitAll()
        {
            List<MA_CTPY_LIMIT> counterpartyLimits;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                counterpartyLimits = unitOfWork.MA_CTPY_LIMITRepository.GetAll().ToList();
            }
            return counterpartyLimits;

        }

        public MA_CTPY_LIMIT CreateCounterpartyLimit(SessionInfo sessioninfo, MA_CTPY_LIMIT counterpartyLimit)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                unitOfWork.MA_CTPY_LIMITRepository.Add(counterpartyLimit);
                unitOfWork.Commit();
            }

            return counterpartyLimit;
        }

        public MA_CTPY_LIMIT UpdateCounterpartyLimit(SessionInfo sessioninfo, MA_CTPY_LIMIT counterpartyLimit)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var foundData = unitOfWork.MA_CTPY_LIMITRepository.GetAll().FirstOrDefault(p => p.ID == counterpartyLimit.ID);
                if (foundData == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {
                    LogBusiness logBusiness = new LogBusiness();
                    var oldRecord = new{AMOUNT= foundData.AMOUNT.ToString("#,##0"), FLAG_CONTROL = foundData.FLAG_CONTROL,EXPIRE_DATE=foundData.EXPIRE_DATE};
                    var newRecord = new { AMOUNT = counterpartyLimit.AMOUNT.ToString("#,##0"), FLAG_CONTROL = counterpartyLimit.FLAG_CONTROL, EXPIRE_DATE = counterpartyLimit.EXPIRE_DATE };
                    var log =logBusiness.UpdateLogging(sessioninfo,foundData.ID, LimitLogEvent.LIMIT_AUDIT.ToString(),LookupFactorTables.MA_CTPY_LIMIT, oldRecord, newRecord);
                    if (log != null) unitOfWork.DA_LOGGINGRepository.Add(log);
                    foundData.ID = counterpartyLimit.ID;
                    foundData.AMOUNT = counterpartyLimit.AMOUNT;
                    foundData.FLAG_CONTROL = counterpartyLimit.FLAG_CONTROL;
                    foundData.EXPIRE_DATE = counterpartyLimit.EXPIRE_DATE;
                    foundData.LOG.MODIFYBYUSERID = counterpartyLimit.LOG.MODIFYBYUSERID;
                    foundData.LOG.MODIFYDATE = DateTime.Now;
                    foundData.LIMIT_ID = counterpartyLimit.LIMIT_ID;
                    unitOfWork.Commit();

                }
            }

            return counterpartyLimit;
        }

        public bool DeleteCounterpartyLimitByID(SessionInfo sessioninfo, Guid ID)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var foundData = unitOfWork.MA_CTPY_LIMITRepository.All().FirstOrDefault(p => p.ID == ID);
                if (foundData == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {
                    unitOfWork.MA_CTPY_LIMITRepository.Delete(foundData);
                    unitOfWork.Commit();
                }
            }

            return true;
        }

        public List<MA_TEMP_CTPY_LIMIT> GetTempLimitByFilter(SessionInfo sessioninfo, string strCpty, string strLimit, string strEffDateFrom, string strEffDateTo
                                                                , string strExpDateFrom, string strExpDateTo, string jtSorting)
        {
            try
            {
                IEnumerable<MA_TEMP_CTPY_LIMIT> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_TEMP_CTPY_LIMITRepository.GetAll().AsQueryable();
                    DateTime dteEffFrom;
                    DateTime dteEffTo;
                    DateTime dteExpFrom;
                    DateTime dteExpTo;

                    //Filters
                    if (!string.IsNullOrEmpty(strCpty) || !string.IsNullOrEmpty(strLimit))
                    {
                        var ctpylimit = unitOfWork.MA_CTPY_LIMITRepository.GetAll().AsQueryable();

                        if (!string.IsNullOrEmpty(strLimit))
                        {
                            var limit = ctpylimit.Where(p => p.MA_LIMIT.LABEL.ToUpper().Contains(strLimit.ToUpper())).Select(p => p.ID).ToArray();
                            query = query.Where(p => limit.Contains(p.CTPY_LIMIT_ID));
                        }

                        if (!string.IsNullOrEmpty(strCpty))
                        {
                            var ctpyid = unitOfWork.MA_COUTERPARTYRepository.GetAll().Where(p => p.SNAME.ToUpper().Contains(strCpty.ToUpper())).Select(p => p.ID).ToArray();
                            var ctpy = ctpylimit.Where(p => ctpyid.Contains(p.CTPY_ID)).Select(p => p.ID).ToArray();
                            query = query.Where(p => ctpy.Contains(p.CTPY_LIMIT_ID));
                        }
                    }
                    if (!string.IsNullOrEmpty(strEffDateFrom) && !string.IsNullOrEmpty(strEffDateTo)) //Effective date
                    {
                        if (!DateTime.TryParseExact(strEffDateFrom, "dd/MM/yyyy", null, DateTimeStyles.None, out dteEffFrom))
                        {
                            throw this.CreateException(new Exception(), "Invalid start effective date.");
                        }
                        else
                            dteEffFrom = DateTime.ParseExact(strEffDateFrom, "dd/MM/yyyy", null);

                        if (!DateTime.TryParseExact(strEffDateTo, "dd/MM/yyyy", null, DateTimeStyles.None, out dteEffTo))
                        {
                            throw this.CreateException(new Exception(), "Invalid end effective date.");
                        }
                        else
                            dteEffTo = DateTime.ParseExact(strEffDateTo, "dd/MM/yyyy", null);

                        query = query.Where(p => p.EFFECTIVE_DATE >= dteEffFrom && p.EFFECTIVE_DATE <= dteEffTo);
                    }
                    if (!string.IsNullOrEmpty(strExpDateFrom) && !string.IsNullOrEmpty(strExpDateTo)) //Expiry date
                    {
                        if (!DateTime.TryParseExact(strExpDateFrom, "dd/MM/yyyy", null, DateTimeStyles.None, out dteExpFrom))
                        {
                            throw this.CreateException(new Exception(), "Invalid start expiry date.");
                        }
                        else
                            dteExpFrom = DateTime.ParseExact(strExpDateFrom, "dd/MM/yyyy", null);

                        if (!DateTime.TryParseExact(strExpDateTo, "dd/MM/yyyy", null, DateTimeStyles.None, out dteExpTo))
                        {
                            throw this.CreateException(new Exception(), "Invalid end expiry date.");
                        }
                        else
                            dteExpTo = DateTime.ParseExact(strExpDateTo, "dd/MM/yyyy", null);

                        query = query.Where(p => p.EXPIRY_DATE >= dteExpFrom && p.EFFECTIVE_DATE <= dteExpTo);
                    }

                    //Sorting
                    string[] sortsp = jtSorting.Split(' ');
                    IQueryable<MA_TEMP_CTPY_LIMIT> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
                    sortedRecords = orderedRecords.ToList();
                }
                //Return result to jTable
                return sortedRecords.ToList();

            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }

        }

        public MA_TEMP_CTPY_LIMIT CreateTempLimit(SessionInfo sessioninfo, MA_TEMP_CTPY_LIMIT record)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                if (record.EFFECTIVE_DATE < sessioninfo.Process.CurrentDate)
                    throw this.CreateException(new Exception(), "Effective date cannot be set to past date.");

                if (record.EXPIRY_DATE < sessioninfo.Process.CurrentDate)
                    throw this.CreateException(new Exception(), "Expiry date cannot be set to past date.");

                if (record.EXPIRY_DATE <= record.EFFECTIVE_DATE)
                    throw this.CreateException(new Exception(), "Expiry date must be after effective date.");

                var duplicate = unitOfWork.MA_TEMP_CTPY_LIMITRepository.All().FirstOrDefault(p => p.CTPY_LIMIT_ID == record.CTPY_LIMIT_ID && p.ISACTIVE == true
                                                                                                && p.EXPIRY_DATE >= record.EFFECTIVE_DATE);
                if (duplicate != null)
                    throw this.CreateException(new Exception(), "Duplicate temp limit info");

                LogBusiness logBusiness = new LogBusiness();
                var newRecord = new { AMOUNT = record.AMOUNT, EFFECTIVE_DATE =  record.EFFECTIVE_DATE, EXPIRY_DATE = record.EXPIRY_DATE};

                unitOfWork.DA_LOGGINGRepository.Add(logBusiness.CreateLogging(sessioninfo, record.CTPY_LIMIT_ID, LimitLogEvent.TEMP_LIMIT_AUDIT.ToString(), LookupFactorTables.MA_TEMP_CTPY_LIMIT, "Temp Limit", newRecord));
                unitOfWork.MA_TEMP_CTPY_LIMITRepository.Add(record);
                unitOfWork.Commit();
            }

            return record;
        }

        public MA_TEMP_CTPY_LIMIT UpdateTempLimit(SessionInfo sessioninfo, MA_TEMP_CTPY_LIMIT record)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                if (record.EFFECTIVE_DATE < sessioninfo.Process.CurrentDate)
                    throw this.CreateException(new Exception(), "Effective date cannot be set to past date.");

                if (record.EXPIRY_DATE < sessioninfo.Process.CurrentDate)
                    throw this.CreateException(new Exception(), "Expiry date cannot be set to past date.");

                if (record.EXPIRY_DATE <= record.EFFECTIVE_DATE)
                    throw this.CreateException(new Exception(), "Expiry date must be after effective date.");

                var duplicate = unitOfWork.MA_TEMP_CTPY_LIMITRepository.All().FirstOrDefault(p => p.CTPY_LIMIT_ID == record.CTPY_LIMIT_ID && p.ISACTIVE == true && p.ID != record.ID && p.EXPIRY_DATE >= record.EFFECTIVE_DATE);
                if (duplicate != null)
                    throw this.CreateException(new Exception(), "Duplicate temp limit info");

                var foundData = unitOfWork.MA_TEMP_CTPY_LIMITRepository.GetAll().FirstOrDefault(p => p.ID == record.ID);
                if (foundData == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {
                    LogBusiness logBusiness = new LogBusiness();
                    var oldRecord = new { AMOUNT = foundData.AMOUNT.ToString("#,##0"), EFFECTIVE_DATE = foundData.EFFECTIVE_DATE, EXPIRY_DATE = foundData.EXPIRY_DATE, ISACTIVE = foundData.ISACTIVE };
                    var newRecord = new { AMOUNT = record.AMOUNT.ToString("#,##0"), EFFECTIVE_DATE = record.EFFECTIVE_DATE, EXPIRY_DATE = record.EXPIRY_DATE, ISACTIVE = record.ISACTIVE };
                    var log = logBusiness.UpdateLogging(sessioninfo, foundData.CTPY_LIMIT_ID, LimitLogEvent.TEMP_LIMIT_AUDIT.ToString(), LookupFactorTables.MA_TEMP_CTPY_LIMIT, oldRecord, newRecord, "Temp Limit");
                    
                    if (log != null) unitOfWork.DA_LOGGINGRepository.Add(log);

                    foundData.AMOUNT = record.AMOUNT;
                    foundData.CTPY_LIMIT_ID = record.CTPY_LIMIT_ID;
                    foundData.EFFECTIVE_DATE = record.EFFECTIVE_DATE;
                    foundData.EXPIRY_DATE = record.EXPIRY_DATE;
                    foundData.ISACTIVE = record.ISACTIVE;
                    foundData.LOG.MODIFYBYUSERID = record.LOG.MODIFYBYUSERID;
                    foundData.LOG.MODIFYDATE = DateTime.Now;
                    unitOfWork.Commit();

                }
            }

            return record;
        }

        public MA_TEMP_CTPY_LIMIT GetActiveTempByID(DateTime ProcessingDate, DateTime MaturityDate, Guid ID)
        {
            MA_TEMP_CTPY_LIMIT temp_limit = null;

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                temp_limit = unitOfWork.MA_TEMP_CTPY_LIMITRepository.GetAll().FirstOrDefault(p => p.ISACTIVE == true 
                                                                                            && p.CTPY_LIMIT_ID == ID 
                                                                                            && p.EFFECTIVE_DATE <= ProcessingDate 
                                                                                            && p.EXPIRY_DATE >= MaturityDate);
            }

            return temp_limit;
        }

        public MA_CSA_AGREEMENT GetCSAByCtpyID(SessionInfo sessioninfo, Guid ID)
        {
            MA_CSA_AGREEMENT csa;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                csa = unitOfWork.MA_CSA_AGREEMENTRepository.GetAll().FirstOrDefault(p => p.ID == ID);
            }
            return csa;
        }

        public MA_CSA_AGREEMENT CreateCSA(SessionInfo sessioninfo, MA_CSA_AGREEMENT record)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_CSA_AGREEMENTRepository.All().FirstOrDefault(p => p.ID == record.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), "Data is duplicated");

                unitOfWork.MA_CSA_AGREEMENTRepository.Add(record);
                unitOfWork.Commit();
            }

            return record;
        }

        public MA_CSA_AGREEMENT UpdateCSA(SessionInfo sessioninfo, MA_CSA_AGREEMENT record)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var foundData = unitOfWork.MA_CSA_AGREEMENTRepository.All().FirstOrDefault(p => p.ID == record.ID);
                if (foundData == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {
                    foundData.CSA_TYPE_ID = record.CSA_TYPE_ID;
                    foundData.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                    foundData.LOG.MODIFYDATE = DateTime.Now;
                    foundData.ISACTIVE = record.ISACTIVE;

                    unitOfWork.Commit();
                }
            }

            return record;
        }

        public List<MA_CSA_PRODUCT> GetCSAProducts(SessionInfo sessioninfo, Guid ID)
        {
            List<MA_CSA_PRODUCT> csaproducts;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                csaproducts = unitOfWork.MA_CSA_PRODUCTRepository.GetAll().Where(p => p.CSA_AGREEMENT_ID == ID).ToList();
            }
            return csaproducts;
        }

        public List<MA_CSA_PRODUCT> GetCSAProductAll(SessionInfo sessioninfo)
        {
            List<MA_CSA_PRODUCT> csaproducts;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                csaproducts = unitOfWork.MA_CSA_PRODUCTRepository.All().ToList();
            }
            return csaproducts;
        }

        public MA_CSA_PRODUCT CreateCSAProduct(SessionInfo sessioninfo, MA_CSA_PRODUCT record)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_CSA_PRODUCTRepository.All().FirstOrDefault(p => p.CSA_AGREEMENT_ID == record.CSA_AGREEMENT_ID && p.PRODUCT_ID == record.PRODUCT_ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), "Data is duplicated");

                unitOfWork.MA_CSA_PRODUCTRepository.Add(record);
                unitOfWork.Commit();
            }

            return record;
        }

        public void DeleteCSAProduct(SessionInfo sessioninfo, Guid CSA_AGREEMENT_ID, Guid PRODUCT_ID)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var foundData = unitOfWork.MA_CSA_PRODUCTRepository.All().FirstOrDefault(p => p.CSA_AGREEMENT_ID == CSA_AGREEMENT_ID && p.PRODUCT_ID == PRODUCT_ID);
                if (foundData == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {
                    unitOfWork.MA_CSA_PRODUCTRepository.Delete(foundData);
                    unitOfWork.Commit();
                }
            }
        }
    }
}
