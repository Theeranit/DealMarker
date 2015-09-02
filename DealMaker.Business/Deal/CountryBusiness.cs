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
    public class CountryBusiness : BaseBusiness
    {
        public List<MA_COUNTRY> GetCountryAll()
        {
            List<MA_COUNTRY> countries;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                countries = unitOfWork.MA_COUNTRYRepository.All().ToList();
            }
            return countries;

        }

        public List<MA_COUNTRY> GetByFilter(SessionInfo sessioninfo, string label, string sorting)
        {
            try
            {
                IEnumerable<MA_COUNTRY> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_COUNTRYRepository.All().AsQueryable();
                    //Filters
                    if (!string.IsNullOrEmpty(label))
                    {
                        query = query.Where(p => p.LABEL.ToUpper().Contains(label.ToUpper()));
                    }
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_COUNTRY> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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

        public MA_COUNTRY Create(SessionInfo sessioninfo, MA_COUNTRY country)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate1 = unitOfWork.MA_COUNTRYRepository.All().FirstOrDefault(p => p.LABEL == country.LABEL);
                if (checkDuplicate1 != null)
                    throw this.CreateException(new Exception(), "Short name is duplicated");

                //Prepare Country-Limit data
                MA_COUNTRY_LIMIT ctLimit = new MA_COUNTRY_LIMIT();

                ctLimit.ID = Guid.NewGuid();
                ctLimit.COUNTRY_ID = country.ID;
                ctLimit.AMOUNT = 0;
                ctLimit.EFFECTIVE_DATE = sessioninfo.Process.CurrentDate;
                ctLimit.EXPIRY_DATE = sessioninfo.Process.CurrentDate;
                ctLimit.ISACTIVE = true;
                ctLimit.ISTEMP = false;
                ctLimit.FLAG_CONTROL = true;
                ctLimit.LOG.INSERTDATE = DateTime.Now;
                ctLimit.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;

                unitOfWork.MA_COUNTRYRepository.Add(country);
                unitOfWork.MA_COUNTRY_LIMITRepository.Add(ctLimit);
                unitOfWork.Commit();
            }

            return country;
        }

        public MA_COUNTRY Update(SessionInfo sessioninfo, MA_COUNTRY country)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate1 = unitOfWork.MA_COUNTRYRepository.All().FirstOrDefault(p => p.LABEL == country.LABEL && p.ID != country.ID);
                if (checkDuplicate1 != null)
                    throw this.CreateException(new Exception(), "Short name is duplicated");

                var foundData = unitOfWork.MA_COUNTRYRepository.All().FirstOrDefault(p => p.ID == country.ID);
                if (foundData == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {
                    foundData.LABEL = country.LABEL;
                    foundData.DESCRIPTION = country.DESCRIPTION;

                    unitOfWork.Commit();

                }
            }

            return country;
        }

        public List<MA_COUNTRY_LIMIT> GetCountryLimitByCountryID(SessionInfo sessioninfo, Guid ID)
        {
            List<MA_COUNTRY_LIMIT> countryLimits;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                countryLimits = unitOfWork.MA_COUNTRY_LIMITRepository.All().Where(p => p.COUNTRY_ID == ID && p.ISTEMP == false).ToList();
            }
            return countryLimits;

        }
        
        public MA_COUNTRY_LIMIT UpdateCountryLimit(SessionInfo sessioninfo, MA_COUNTRY_LIMIT countryLimit)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                if (countryLimit.EXPIRY_DATE < sessioninfo.Process.CurrentDate)
                    throw this.CreateException(new Exception(), "Expiry date cannot be set to past date.");

                var foundData = unitOfWork.MA_COUNTRY_LIMITRepository.All().FirstOrDefault(p => p.ID == countryLimit.ID);
                if (foundData == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {
                    LogBusiness logBusiness = new LogBusiness();
                    var oldRecord = new { AMOUNT = foundData.AMOUNT.ToString("#,##0"), EXPIRE_DATE = foundData.EXPIRY_DATE };
                    var newRecord = new { AMOUNT = countryLimit.AMOUNT.ToString("#,##0"), EXPIRE_DATE = countryLimit.EXPIRY_DATE };
                    var log = logBusiness.UpdateLogging(sessioninfo, foundData.COUNTRY_ID, LimitLogEvent.COUNTRY_LIMIY_AUDIT.ToString(), LookupFactorTables.MA_COUNTRY, oldRecord, newRecord, "Country Limit");
                    if (log != null) unitOfWork.DA_LOGGINGRepository.Add(log);

                    foundData.AMOUNT = countryLimit.AMOUNT;
                    foundData.FLAG_CONTROL = countryLimit.FLAG_CONTROL;
                    foundData.EXPIRY_DATE = countryLimit.EXPIRY_DATE;
                    foundData.LOG.MODIFYBYUSERID = countryLimit.LOG.MODIFYBYUSERID;
                    foundData.LOG.MODIFYDATE = countryLimit.LOG.MODIFYDATE;

                    unitOfWork.Commit();
                }
            }

            return countryLimit;
        }

        public MA_COUNTRY_LIMIT CreateTempLimit(SessionInfo sessioninfo, MA_COUNTRY_LIMIT record)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                if (record.EFFECTIVE_DATE < sessioninfo.Process.CurrentDate)
                    throw this.CreateException(new Exception(), "Effective date cannot be set to past date.");

                if (record.EXPIRY_DATE < sessioninfo.Process.CurrentDate)
                    throw this.CreateException(new Exception(), "Expiry date cannot be set to past date.");

                if (record.EXPIRY_DATE <= record.EFFECTIVE_DATE)
                    throw this.CreateException(new Exception(), "Expiry date must be after effective date.");

                var duplicate = unitOfWork.MA_COUNTRY_LIMITRepository.All().FirstOrDefault(p => p.ISTEMP == true && p.ISACTIVE == true
                                                                                                && p.COUNTRY_ID == record.COUNTRY_ID && p.EXPIRY_DATE >= record.EFFECTIVE_DATE);
                if (duplicate != null)
                    throw this.CreateException(new Exception(), "Duplicate temp limit info");

                LogBusiness logBusiness = new LogBusiness();
                var newRecord = new { AMOUNT = record.AMOUNT, EFFECTIVE_DATE = record.EFFECTIVE_DATE, EXPIRY_DATE = record.EXPIRY_DATE };
                unitOfWork.DA_LOGGINGRepository.Add(logBusiness.CreateLogging(sessioninfo, record.COUNTRY_ID, LimitLogEvent.TEMP_COUNTRY_LIMIT_AUDIT.ToString(), LookupFactorTables.MA_COUNTRY_LIMIT, "Temp Country Limit", newRecord));
                
                unitOfWork.MA_COUNTRY_LIMITRepository.Add(record);
                unitOfWork.Commit();
            }

            return record;
        }

        public MA_COUNTRY_LIMIT UpdateTempCountryLimit(SessionInfo sessioninfo, MA_COUNTRY_LIMIT countryLimit)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                if (countryLimit.EFFECTIVE_DATE < sessioninfo.Process.CurrentDate)
                    throw this.CreateException(new Exception(), "Effective date cannot be set to past date.");

                if (countryLimit.EXPIRY_DATE < sessioninfo.Process.CurrentDate)
                    throw this.CreateException(new Exception(), "Expiry date cannot be set to past date.");

                if (countryLimit.EXPIRY_DATE <= countryLimit.EFFECTIVE_DATE)
                    throw this.CreateException(new Exception(), "Expiry date must be after effective date.");

                var duplicate = unitOfWork.MA_COUNTRY_LIMITRepository.All().FirstOrDefault(p => p.ID != countryLimit.ID && p.ISTEMP == true && p.ISACTIVE == true
                                                                                                && p.COUNTRY_ID == countryLimit.COUNTRY_ID && p.EXPIRY_DATE >= countryLimit.EFFECTIVE_DATE);
                if (duplicate != null)
                    throw this.CreateException(new Exception(), "Duplicate temp limit info");

                var foundData = unitOfWork.MA_COUNTRY_LIMITRepository.All().FirstOrDefault(p => p.ID == countryLimit.ID);
                if (foundData == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {
                    LogBusiness logBusiness = new LogBusiness();
                    var oldRecord = new { AMOUNT = foundData.AMOUNT.ToString("#,##0"), EFFECTIVE_DATE = foundData.EFFECTIVE_DATE, EXPIRE_DATE = foundData.EXPIRY_DATE, ISACTIVE = foundData.ISACTIVE };
                    var newRecord = new { AMOUNT = countryLimit.AMOUNT.ToString("#,##0"), EFFECTIVE_DATE = countryLimit.EFFECTIVE_DATE, EXPIRE_DATE = countryLimit.EXPIRY_DATE, ISACTIVE = countryLimit.ISACTIVE };
                    var log = logBusiness.UpdateLogging(sessioninfo, foundData.COUNTRY_ID, LimitLogEvent.TEMP_COUNTRY_LIMIT_AUDIT.ToString(), LookupFactorTables.MA_COUNTRY_LIMIT, oldRecord, newRecord, "Temp Country Limit");
                    if (log != null) unitOfWork.DA_LOGGINGRepository.Add(log);

                    foundData.AMOUNT = countryLimit.AMOUNT;
                    foundData.ISACTIVE = countryLimit.ISACTIVE;
                    foundData.EFFECTIVE_DATE = countryLimit.EFFECTIVE_DATE;
                    foundData.EXPIRY_DATE = countryLimit.EXPIRY_DATE;
                    foundData.LOG.MODIFYBYUSERID = countryLimit.LOG.MODIFYBYUSERID;
                    foundData.LOG.MODIFYDATE = countryLimit.LOG.MODIFYDATE;

                    unitOfWork.Commit();
                }
            }

            return countryLimit;
        }

        public List<MA_COUNTRY_LIMIT> GetTempLimitByFilter(SessionInfo sessioninfo, string strCountry, string strEffDateFrom, string strEffDateTo
                                                                , string strExpDateFrom, string strExpDateTo, string jtSorting)
        {
            try
            {
                IEnumerable<MA_COUNTRY_LIMIT> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_COUNTRY_LIMITRepository.All().Where(p => p.ISTEMP == true).AsQueryable();
                    DateTime dteEffFrom;
                    DateTime dteEffTo;
                    DateTime dteExpFrom;
                    DateTime dteExpTo;

                    //Filters
                    if (!string.IsNullOrEmpty(strCountry)) //Country
                    {
                        query = query.Where(p => p.MA_COUNTRY.LABEL.ToUpper().Contains(strCountry.ToUpper()));
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
                    IQueryable<MA_COUNTRY_LIMIT> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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

        public MA_COUNTRY_LIMIT GetActiveTempByCountryID(DateTime ProcessingDate, DateTime MaturityDate, Guid ID)
        {
            MA_COUNTRY_LIMIT temp_limit = null;

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                temp_limit = unitOfWork.MA_COUNTRY_LIMITRepository.GetAll().FirstOrDefault(p => p.ISACTIVE == true && p.ISTEMP == true && p.COUNTRY_ID == ID && p.EFFECTIVE_DATE <= ProcessingDate && p.EXPIRY_DATE >= MaturityDate);
            }

            return temp_limit;
        }

        public List<MA_COUNTRY_LIMIT> GetAllActiveTemp(DateTime ProcessingDate)
        {
            List<MA_COUNTRY_LIMIT> temp_limits = new List<MA_COUNTRY_LIMIT>();

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                temp_limits = unitOfWork.MA_COUNTRY_LIMITRepository.GetAll().Where(p => p.ISACTIVE == true && p.ISTEMP == true && p.EFFECTIVE_DATE <= ProcessingDate).ToList();
            }

            return temp_limits;
        }
    }
}
