using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Business.Deal;
using KK.DealMaker.Business.Master;
using KK.DealMaker.Core.SystemFramework;

namespace KK.DealMaker.UIProcessComponent.Deal
{
    public class CountryUIP : BaseUIP
    {
        public static object GetCountryOptions(SessionInfo sessioninfo)
        {
            try
            {
                CountryBusiness _countryBusiness = new CountryBusiness();
                //Get data from database
                var countries = _countryBusiness.GetCountryAll()
                                            .OrderBy(p => p.LABEL)
                                            .Select(c => new { DisplayText = c.LABEL, Value = c.ID });

                //Return result to jTable
                return new { Result = "OK", Options = countries };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetByFilter(SessionInfo sessioninfo, string label, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                CountryBusiness _countryBusiness = new CountryBusiness();
                //Get data from database
                List<MA_COUNTRY> countries = _countryBusiness.GetByFilter(sessioninfo, label, jtSorting);

                //Return result to jTable
                return new
                {
                    Result = "OK",
                    Records = jtPageSize > 0 ? countries.Skip(jtStartIndex).Take(jtPageSize).ToList() : countries,
                    TotalRecordCount = countries.Count
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

        public static object Create(SessionInfo sessioninfo, MA_COUNTRY record)
        {
            try
            {
                CountryBusiness _countryBusiness = new CountryBusiness();

                record.ID = Guid.NewGuid();
                record.LABEL = record.LABEL.ToUpper();
                record.DESCRIPTION = record.DESCRIPTION.ToUpper();
                
                var addedRecord = _countryBusiness.Create(sessioninfo, record);
                
                return new { Result = "OK", Record = addedRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object Update(SessionInfo sessioninfo, MA_COUNTRY record)
        {
            try
            {
                CountryBusiness _countryBusiness = new CountryBusiness();
                record.LABEL = record.LABEL.ToUpper();
                record.DESCRIPTION = record.DESCRIPTION.ToUpper();

                var updateRecord = _countryBusiness.Update(sessioninfo, record);

                return new { Result = "OK", Record = updateRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetCountryLimitByCountryID(SessionInfo sessioninfo, Guid ID)
        {
            try
            {
                CountryBusiness _countryBusiness = new CountryBusiness();
                //Get data from database
                List<MA_COUNTRY_LIMIT> limits = _countryBusiness.GetCountryLimitByCountryID(sessioninfo, ID);

                //Return result to jTable
                return new { Result = "OK", Records = limits, TotalRecordCount = limits.Count };
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

        public static object UpdateCountryLimit(SessionInfo sessioninfo, MA_COUNTRY_LIMIT record)
        {
            try
            {
                CountryBusiness _countryBusiness = new CountryBusiness();

                if (!record.FLAG_CONTROL)
                {
                    record.AMOUNT = 0;
                    record.EFFECTIVE_DATE = sessioninfo.Process.CurrentDate;
                    record.EXPIRY_DATE = sessioninfo.Process.CurrentDate;
                }

                record.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.MODIFYDATE = DateTime.Now;

                var updateRecord = _countryBusiness.UpdateCountryLimit(sessioninfo, record);

                return new { Result = "OK" , Record = updateRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetTempLimitByFilter(SessionInfo sessioninfo, string strCountry, string strEffDateFrom, string strEffDateTo
                                                   , string strExpDateFrom, string strExpDateTo, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                CountryBusiness _countryBusiness = new CountryBusiness();
                //Get data from database
                List<MA_COUNTRY_LIMIT> limits = _countryBusiness.GetTempLimitByFilter(sessioninfo, strCountry, strEffDateFrom, strEffDateTo
                                                                                      , strExpDateFrom, strExpDateTo, jtSorting);

                //Return result to jTable
                return new
                {
                    Result = "OK",
                    Records = jtPageSize > 0 ? limits.Skip(jtStartIndex).Take(jtPageSize).ToList() : limits,
                    TotalRecordCount = limits.Count
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

        public static object CreateTempLimit(SessionInfo sessioninfo, MA_COUNTRY_LIMIT record)
        {
            try
            {
                CountryBusiness _countryBusiness = new CountryBusiness();

                record.ID = Guid.NewGuid();
                record.ISTEMP = true;
                record.FLAG_CONTROL = true;
                record.LOG.INSERTDATE = DateTime.Now;
                record.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;

                var addedRecord = _countryBusiness.CreateTempLimit(sessioninfo, record);

                return new { Result = "OK", Record = addedRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object UpdateTempLimit(SessionInfo sessioninfo, MA_COUNTRY_LIMIT record)
        {
            try
            {
                CountryBusiness _countryBusiness = new CountryBusiness();
                record.LOG.MODIFYDATE = DateTime.Now;
                record.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;

                var updateRecord = _countryBusiness.UpdateTempCountryLimit(sessioninfo, record);

                return new { Result = "OK", Record = updateRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
    }
}
