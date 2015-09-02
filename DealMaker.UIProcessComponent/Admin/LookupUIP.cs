using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Business.Master;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.Core.Helper;

namespace KK.DealMaker.UIProcessComponent.Admin
{
    public class LookupUIP : BaseUIP
    {

        #region STATUS
        public static object GetStatusByFilter(SessionInfo sessioninfo, string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                //Return result to jTable
                LookupBusiness _lookupbusiness = new LookupBusiness();
                //Get data from database
                List<MA_STATUS> status = _lookupbusiness.GetStatusByFilter(sessioninfo, name, jtSorting);

                //Return result to jTable
                return new { Result = "OK",
                             Records = jtPageSize > 0 ? status.Skip(jtStartIndex).Take(jtPageSize).ToList() : status, 
                             TotalRecordCount = status.Count };
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
        public static object CreateStatus(SessionInfo sessioninfo, MA_STATUS record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ID = Guid.NewGuid();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.LABEL = record.LABEL;
                var added = _lookupbusiness.CreateStatus(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object UpdateStatus(SessionInfo sessioninfo, MA_STATUS record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.LABEL = record.LABEL;
                var updated = _lookupbusiness.UpdateStatus(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object GetStatusOptions(SessionInfo sessioninfo)
        {
            try
            {
                LookupBusiness _lookupBusiness = new LookupBusiness();
                //Get data from database
                var products = _lookupBusiness.GetStatusAll().Select(c => new { DisplayText = c.LABEL, Value = c.ID });

                //Return result to jTable
                return new { Result = "OK", Options = products };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion

        #region PRODUCT
        public static object GetProductOptions(SessionInfo sessioninfo, string except_product = "")
        {
            try
            {
                LookupBusiness _lookupBusiness = new LookupBusiness();
                //Get data from database
                var products = _lookupBusiness.GetProductAll().Where(t => t.ISACTIVE == true);

                if (except_product != "")
                {
                    products = products.Where(p => p.LABEL != except_product);
                }

                //Return result to jTable
                return new { Result = "OK", Options = products.OrderBy(t => t.LABEL).Select(c => new { DisplayText = c.LABEL, Value = c.ID }) };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object GetProductByFilter(SessionInfo sessioninfo, string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                //Return result to jTable
                LookupBusiness _lookupbusiness = new LookupBusiness();
                //Get data from database
                List<MA_PRODUCT> products = _lookupbusiness.GetProductByFilter(sessioninfo, name, jtSorting);

                //Return result to jTable
                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? products.Skip(jtStartIndex).Take(jtPageSize).ToList() : products, 
                             TotalRecordCount = products.Count };
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
        public static object CreateProduct(SessionInfo sessioninfo, MA_PRODUCT record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ID = Guid.NewGuid();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.LABEL = record.LABEL;
                var added = _lookupbusiness.CreateProduct(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object UpdateProduct(SessionInfo sessioninfo, MA_PRODUCT record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.LABEL = record.LABEL;
                var updated = _lookupbusiness.UpdateProduct(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion

        #region PORTFOLIO
        public static object GetPortfolioOptions(SessionInfo sessioninfo)
        {
            try
            {
                LookupBusiness _lookupBusiness = new LookupBusiness();
                //Get data from database
                var portfolio = _lookupBusiness.GetPortfolioAll().Where(t => t.ISACTIVE == true).OrderBy(p => p.LABEL)
                                                .Select(c => new { DisplayText = c.LABEL, Value = c.ID, Default = c.ISDEFAULT });

                //Return result to jTable
                return new { Result = "OK", Options = portfolio };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object GetPortfolioByFilter(SessionInfo sessioninfo, string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                //Return result to jTable
                LookupBusiness _lookupbusiness = new LookupBusiness();
                //Get data from database
                List<MA_PORTFOLIO> portfolio = _lookupbusiness.GetPortfolioByFilter(sessioninfo, name, jtSorting);

                //Return result to jTable
                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? portfolio.Skip(jtStartIndex).Take(jtPageSize).ToList() : portfolio, 
                             TotalRecordCount = portfolio.Count };
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
        public static object CreatePortfolio(SessionInfo sessioninfo, MA_PORTFOLIO record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ID = Guid.NewGuid();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.ISDEFAULT = record.ISDEFAULT == null || !record.ISDEFAULT.Value ? false : true;
                record.LABEL = record.LABEL;
                var added = _lookupbusiness.CreateProfolio(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object UpdatePortfolio(SessionInfo sessioninfo, MA_PORTFOLIO record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.ISDEFAULT = record.ISDEFAULT == null || !record.ISDEFAULT.Value ? false : true;
                record.LABEL = record.LABEL;
                var updated = _lookupbusiness.UpdatePorfolio(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion PORTFOLIO

        #region LIMIT
        public static object GetLimitOptions(SessionInfo sessioninfo)
        {
            try
            {
                LookupBusiness _lookupBusiness = new LookupBusiness();
                //Get data from database
                var limits = _lookupBusiness.GetLimitAll().Select(c => new { DisplayText = c.LABEL, Value = c.ID });

                //Return result to jTable
                return new { Result = "OK", Options = limits };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object GetLimitByFilter(SessionInfo sessioninfo, string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                //Return result to jTable
                LookupBusiness _lookupbusiness = new LookupBusiness();
                //Get data from database
                List<MA_LIMIT> limits = _lookupbusiness.GetLimitByFilter(sessioninfo, name, jtSorting);

                //Return result to jTable
                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? limits.Skip(jtStartIndex).Take(jtPageSize).ToList() : limits, 
                             TotalRecordCount = limits.Count };
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
        public static object CreateLimit(SessionInfo sessioninfo, MA_LIMIT record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ID = Guid.NewGuid();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE ? false : true;
                record.LABEL = record.LABEL;
                record.LIMIT_TYPE = record.LIMIT_TYPE;
                record.INDEX = record.INDEX;
                var added = _lookupbusiness.CreateLimit(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object UpdateLimit(SessionInfo sessioninfo, MA_LIMIT record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE ? false : true;
                record.LABEL = record.LABEL;
                record.LIMIT_TYPE = record.LIMIT_TYPE;
                record.INDEX = record.INDEX;
                var updated = _lookupbusiness.UpdateLimit(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion LIMIT

        #region FREQUENCY
        public static object GetFreqTypeOptions(SessionInfo sessioninfo)
        {
            try
            {
                LookupBusiness _lookupBusiness = new LookupBusiness();
                //Get data from database
                var freqtypes = _lookupBusiness.GetFreqTypeAll().OrderBy(p => p.INDEX).Select(c => new { DisplayText = c.LABEL, Value = c.ID });

                //Return result to jTable
                return new { Result = "OK", Options = freqtypes };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object GetFreqTypeByFilter(SessionInfo sessioninfo, string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                //Return result to jTable
                LookupBusiness _lookupbusiness = new LookupBusiness();
                //Get data from database
                List<MA_FREQ_TYPE> freqtype = _lookupbusiness.GetFreqTypeByFilter(sessioninfo, name, jtSorting);

                //Return result to jTable
                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? freqtype.Skip(jtStartIndex).Take(jtPageSize).ToList() : freqtype, 
                             TotalRecordCount = freqtype.Count };
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
        public static object CreateFreqType(SessionInfo sessioninfo, MA_FREQ_TYPE record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ID = Guid.NewGuid();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.LABEL = record.LABEL;
                record.USERCODE = record.USERCODE;
                record.INDEX = record.INDEX;
                var added = _lookupbusiness.CreateFreqType(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object UpdateFreqType(SessionInfo sessioninfo, MA_FREQ_TYPE record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                //record.LABEL = record.LABEL;
                //record.USERCODE = record
                record.USERCODE = record.USERCODE;
                record.INDEX = record.INDEX;
                var updated = _lookupbusiness.UpdateFreqType(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion FREQUENCY

        #region CURRENCY
        public static object GetCurrencyOptions(SessionInfo sessioninfo)
        {
            try
            {
                LookupBusiness _lookupBusiness = new LookupBusiness();
                //Get data from database
                var currencies = _lookupBusiness.GetCurrencyAll().Select(c => new { DisplayText = c.LABEL, Value = c.ID.ToString() });
                var currenciesList = currencies.ToList();

                currenciesList.Insert(0, new { DisplayText = "Please select", Value = string.Empty });
                //Return result to jTable
                return new { Result = "OK", Options = currenciesList };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object GetCurrencyByFilter(SessionInfo sessioninfo, string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                //Return result to jTable
                LookupBusiness _lookupbusiness = new LookupBusiness();
                //Get data from database
                List<MA_CURRENCY> currencies = _lookupbusiness.GetCurrencyByFilter(sessioninfo, name, jtSorting);

                //Return result to jTable
                return new
                {
                    Result = "OK",
                    Records = jtPageSize > 0 ? currencies.Skip(jtStartIndex).Take(jtPageSize).ToList() : currencies,
                    TotalRecordCount = currencies.Count
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
        public static object CreateCurrency(SessionInfo sessioninfo, MA_CURRENCY record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ID = Guid.NewGuid();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.LABEL = record.LABEL.ToUpper();
                var added = _lookupbusiness.CreateCurrency(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object UpdateCurrency(SessionInfo sessioninfo, MA_CURRENCY record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.LABEL = record.LABEL.ToUpper();
                var updated = _lookupbusiness.UpdateCurrency(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion CURRENCY

        #region SPOT RATE

        public static object GetSpotRateByFilter(SessionInfo sessioninfo, string processdate, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                
                LookupBusiness _lookupbusiness = new LookupBusiness();
                //Get data from database
                List<MA_SPOT_RATE> SpotRates = _lookupbusiness.GetSpotRateByFilter(sessioninfo, processdate, jtSorting);

                //var jsonData = (from s in SpotRates
                //                select new
                //                {
                //                    ID = s.ID,
                //                    CurrencyLabel = s.MA_CURRENCY.LABEL,
                //                    Proc_date = s.PROC_DATE,
                //                    Rate = s.RATE
                //                }).ToList();
                //Return result to jTable
                return new
                {
                    Result = "OK",
                    Records = jtPageSize > 0 ? SpotRates.Skip(jtStartIndex).Take(jtPageSize).ToList() : SpotRates,
                    TotalRecordCount = SpotRates.Count
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

        public static object CreateSpotRate(SessionInfo sessioninfo, MA_SPOT_RATE record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ID = Guid.NewGuid();

                var added = _lookupbusiness.CreateSpotRate(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object UpdateSpotRate(SessionInfo sessioninfo, MA_SPOT_RATE record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();

                var updated = _lookupbusiness.UpdateSpotRate(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion SPOT RATE

        #region Bond Market
        public static object GetBondMarketOptions(SessionInfo sessioninfo)
        {
            try
            {
                LookupBusiness _lookupBusiness = new LookupBusiness();
                //Get data from database
                var bondMarkets = _lookupBusiness.GetBondMarketAll().Select(c => new { DisplayText = c.LABEL, Value = c.ID.ToString() }).OrderBy(p => p.DisplayText);
                var bondMarketsList = bondMarkets.ToList();
                
                bondMarketsList.Insert(0, new { DisplayText = "Please select...", Value = string.Empty });
                //Return result to jTable
                return new { Result = "OK", Options = bondMarketsList };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object GetBondMarketByFilter(SessionInfo sessionInfo, string label, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                //Get data from database
                List<MA_BOND_MARKET> market = _lookupbusiness.GetBondMarketByFilter(sessionInfo, label, jtSorting);

                return new
                {
                    Result = "OK",
                    Records = jtPageSize > 0 ? market.Skip(jtStartIndex).Take(jtPageSize).ToList() : market,
                    TotalRecordCount = market.Count
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
        public static object CreateBondMarket(SessionInfo sessioninfo, MA_BOND_MARKET record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ID = Guid.NewGuid();
                record.LABEL = record.LABEL.ToUpper();
                record.DESCRIPTION = record.DESCRIPTION.ToUpper();

                var added = _lookupbusiness.CreateBondMarket(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object UpdateBondMarket(SessionInfo sessioninfo, MA_BOND_MARKET record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.LABEL = record.LABEL.ToUpper();
                record.DESCRIPTION = record.DESCRIPTION.ToUpper();

                var updated = _lookupbusiness.UpdateBondMarket(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion

        #region FIEntry
        public static object GetYeildTypeOptions(SessionInfo sessioninfo)
        {
            try
            {
                //Get data from database
                 var values = Enum.GetValues(typeof(TBMA_YTYPE)).Cast<TBMA_YTYPE>();
                 var YeildTypes = values.Select(c => new { DisplayText = c.ToString(), Value = c.ToString() });

                //Return result to jTable
                 return new { Result = "OK", Options = YeildTypes };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object GetPurposeOptions(SessionInfo sessioninfo)
        {
            try
            {
                //Get data from database
                var values = Enum.GetValues(typeof(TBMA_PURPOSE)).Cast<TBMA_PURPOSE>();
                var Purposes = values.Select(c => new { DisplayText = c.ToString(), Value = c.ToString() });

                //Return result to jTable
                return new { Result = "OK", Options = Purposes };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object GetReportByOptions(SessionInfo sessioninfo)
        {
            try
            {
                //Get data from database
                var values = Enum.GetValues(typeof(TBMA_REPORTBY)).Cast<TBMA_REPORTBY>();
                var ReportBys = values.Select(c => new { DisplayText = c.ToString().Replace("_", " "), Value = c });

                //Return result to jTable
                return new { Result = "OK", Options = ReportBys };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion

        #region CSA Type
        public static object GetCSATypeByFilter(SessionInfo sessionInfo, string label, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                //Get data from database
                List<MA_CSA_TYPE> market = _lookupbusiness.GetCSATypeByFilter(sessionInfo, label, jtSorting);

                return new
                {
                    Result = "OK",
                    Records = jtPageSize > 0 ? market.Skip(jtStartIndex).Take(jtPageSize).ToList() : market,
                    TotalRecordCount = market.Count
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
        public static object CreateCSAType(SessionInfo sessioninfo, MA_CSA_TYPE record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.ID = Guid.NewGuid();
                record.LABEL = record.LABEL.ToUpper();
                record.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.INSERTDATE = DateTime.Now;

                var added = _lookupbusiness.CreateCSAType(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object UpdateCSAType(SessionInfo sessioninfo, MA_CSA_TYPE record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                record.LABEL = record.LABEL.ToUpper();

                var updated = _lookupbusiness.UpdateCSAType(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetCSATypeOptions(SessionInfo sessioninfo)
        {
            try
            {
                LookupBusiness _lookupBusiness = new LookupBusiness();
                //Get data from database
                var csa = _lookupBusiness.GetCSATypeAll().Select(c => new { DisplayText = c.LABEL, Value = c.ID });

                //Return result to jTable
                return new { Result = "OK", Options = csa };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion

        #region TBMA Config
        public static object GetTBMAConfigAll(SessionInfo sessionInfo)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                //Get data from database
                List<MA_TBMA_CONFIG> config = _lookupbusiness.GetTBMAConfigAll(sessionInfo);

                return new
                {
                    Result = "OK",
                    Records = config,
                    TotalRecordCount = 1
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

        public static MA_TBMA_CONFIG GetTBMAConfig(SessionInfo sessionInfo)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();
                //Get data from database
                return _lookupbusiness.GetTBMAConfig(sessionInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object UpdateTBMAConfig(SessionInfo sessioninfo, MA_TBMA_CONFIG record)
        {
            try
            {
                LookupBusiness _lookupbusiness = new LookupBusiness();

                var updated = _lookupbusiness.UpdateTBMAConfig(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion

        #region LOGGING
        public static object GetLimitEventOptions(SessionInfo sessioninfo)
        {
            try
            {
                //Get data from database
                var values = Enum.GetValues(typeof(LimitLogEvent)).Cast<LimitLogEvent>();
                var ReportBys = values.Select(c => new { DisplayText = c.ToString().Replace("_", " "), Value = c.ToString() });

                //Return result to jTable
                return new { Result = "OK", Options = ReportBys };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        #endregion
    }
}
