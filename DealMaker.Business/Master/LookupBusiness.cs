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
using System.Globalization;
namespace KK.DealMaker.Business.Master
{
    public class LookupBusiness : BaseBusiness
    {

        #region STATUS
        public List<MA_STATUS> GetStatusAll()
        {
            List<MA_STATUS> statusList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                statusList = unitOfWork.MA_STATUSRepository.All().ToList();
            }
            return statusList;

        }
        public List<MA_STATUS> GetStatusByFilter(SessionInfo sessioninfo, string name, string sorting)
        {
            try
            {
                //IEnumerable<MA_USER> query;
                IEnumerable<MA_STATUS> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_STATUSRepository.GetAll().AsQueryable();

                    //Filters
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.LABEL.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_STATUS> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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
        
        public MA_STATUS CreateStatus(SessionInfo sessioninfo, MA_STATUS status)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_STATUSRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(status.LABEL.ToLower()));
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                unitOfWork.MA_STATUSRepository.Add(status);
                unitOfWork.Commit();
            }

            return status;
        }
        public MA_STATUS UpdateStatus(SessionInfo sessioninfo, MA_STATUS status)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_STATUSRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(status.LABEL.ToLower()) && p.ID != status.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var foundStatus = unitOfWork.MA_STATUSRepository.All().FirstOrDefault(p => p.ID == status.ID);
                if (foundStatus == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);
                else
                {

                    foundStatus.ID = status.ID;
                    foundStatus.LABEL = status.LABEL;
                    foundStatus.ISACTIVE = status.ISACTIVE;
                   
                    unitOfWork.Commit();

                }
            }

            return status;
        }
        #endregion

        #region PRODUCT
        public MA_PRODUCT GetProductByUsercode(string usercode)
        {
            MA_PRODUCT prod = null;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                prod = unitOfWork.MA_PRODUCTRepository.GetAll().FirstOrDefault(t => t.LABEL.Contains(usercode));
            }
            return prod;
        }
        public List<MA_PRODUCT> GetProductAll()
        {
            List<MA_PRODUCT> productList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                productList = unitOfWork.MA_PRODUCTRepository.All().ToList();
            }
            return productList;

        }
        public MA_PRODUCT GetProductByID(SessionInfo sessioninfo, Guid guID)
        {
            MA_PRODUCT prod = null;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                prod = unitOfWork.MA_PRODUCTRepository.GetAll().FirstOrDefault(p => p.ID == guID);
            }
            return prod;
        }
        public List<MA_PRODUCT> GetProductByFilter(SessionInfo sessioninfo, string name, string sorting)
        {
            try
            {
                //IEnumerable<MA_USER> query;
                IEnumerable<MA_PRODUCT> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_PRODUCTRepository.GetAll().AsQueryable();

                    //Filters
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.LABEL.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_PRODUCT> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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
        public MA_PRODUCT CreateProduct(SessionInfo sessioninfo, MA_PRODUCT product)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_PRODUCTRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(product.LABEL.ToLower()));
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                unitOfWork.MA_PRODUCTRepository.Add(product);
                unitOfWork.Commit();
            }

            return product;
        }
        public MA_PRODUCT UpdateProduct(SessionInfo sessioninfo, MA_PRODUCT product)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_PRODUCTRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(product.LABEL.ToLower()) && p.ID != product.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var foundProduct = unitOfWork.MA_PRODUCTRepository.All().FirstOrDefault(p => p.ID == product.ID);
                if (foundProduct == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);
                else
                {

                    foundProduct.ID = product.ID;
                    foundProduct.LABEL = product.LABEL;
                    foundProduct.ISACTIVE = product.ISACTIVE;

                    unitOfWork.Commit();

                }
            }

            return product;
        }
        #endregion

        #region PORTFOLIO
        public List<MA_PORTFOLIO> GetPortfolioAll()
        {
            List<MA_PORTFOLIO> portfolioList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                portfolioList = unitOfWork.MA_PORTFOLIORepository.All().ToList();
            }
            return portfolioList;

        }
        public List<MA_PORTFOLIO> GetPortfolioByFilter(SessionInfo sessioninfo, string name, string sorting)
        {
            try
            {
                //IEnumerable<MA_USER> query;
                IEnumerable<MA_PORTFOLIO> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_PORTFOLIORepository.GetAll().AsQueryable();

                    //Filters
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.LABEL.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_PORTFOLIO> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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
        public MA_PORTFOLIO CreateProfolio(SessionInfo sessioninfo, MA_PORTFOLIO portfolio)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_PORTFOLIORepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(portfolio.LABEL.ToLower()));
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                if (portfolio.ISDEFAULT.HasValue && portfolio.ISDEFAULT.Value)
                {
                    var intCount = unitOfWork.MA_PORTFOLIORepository.GetAll().Count(p => p.ISDEFAULT == true);
                    if (intCount > 0)
                        throw this.CreateException(new Exception(), Messages.DUPLICATE_DEFAULT_DATA);
                }

                unitOfWork.MA_PORTFOLIORepository.Add(portfolio);
                unitOfWork.Commit();
            }

            return portfolio;
        }
        public MA_PORTFOLIO UpdatePorfolio(SessionInfo sessioninfo, MA_PORTFOLIO portfolio)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_PORTFOLIORepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(portfolio.LABEL.ToLower()) && p.ID != portfolio.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                if (portfolio.ISDEFAULT.HasValue && portfolio.ISDEFAULT.Value)
                {
                    var intCount = unitOfWork.MA_PORTFOLIORepository.GetAll().Count(p => p.ISDEFAULT == true && p.ID != portfolio.ID);
                    if (intCount > 0)
                        throw this.CreateException(new Exception(), Messages.DUPLICATE_DEFAULT_DATA);
                }

                var foundPortfolio = unitOfWork.MA_PORTFOLIORepository.All().FirstOrDefault(p => p.ID == portfolio.ID);
                if (foundPortfolio == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);
                else
                {

                    foundPortfolio.ID = portfolio.ID;
                    foundPortfolio.LABEL = portfolio.LABEL;
                    foundPortfolio.ISACTIVE = portfolio.ISACTIVE;
                    foundPortfolio.ISDEFAULT = portfolio.ISDEFAULT;

                    unitOfWork.Commit();

                }
            }

            return portfolio;
        }
        #endregion PORTFOLIO

        #region LIMIT
        public List<MA_LIMIT> GetLimitAll()
        {
            List<MA_LIMIT> limitList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                limitList = unitOfWork.MA_LIMITRepository.All().ToList();
            }
            return limitList;

        }
        public List<MA_LIMIT> GetLimitByFilter(SessionInfo sessioninfo, string name, string sorting)
        {
            try
            {
                //IEnumerable<MA_USER> query;
                IEnumerable<MA_LIMIT> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_LIMITRepository.GetAll().AsQueryable();

                    //Filters
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.LABEL.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_LIMIT> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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
        public MA_LIMIT CreateLimit(SessionInfo sessioninfo, MA_LIMIT limit)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_LIMITRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(limit.LABEL.ToLower()));
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                unitOfWork.MA_LIMITRepository.Add(limit);
                unitOfWork.Commit();
            }

            return limit;
        }
        public MA_LIMIT UpdateLimit(SessionInfo sessioninfo, MA_LIMIT limit)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_LIMITRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(limit.LABEL.ToLower()) && p.ID != limit.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var foundLimit = unitOfWork.MA_LIMITRepository.All().FirstOrDefault(p => p.ID == limit.ID);
                if (foundLimit == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);
                else
                {

                    foundLimit.ID = limit.ID;
                    foundLimit.LABEL = limit.LABEL;
                    foundLimit.ISACTIVE = limit.ISACTIVE;
                    foundLimit.INDEX = limit.INDEX;

                    unitOfWork.Commit();

                }
            }

            return limit;
        }
        #endregion LIMIT

        #region FREQUENCY
        public MA_FREQ_TYPE GetFreqByID(Guid guID)
        {
            MA_FREQ_TYPE freq;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                freq = unitOfWork.MA_FREQ_TYPERepository.All().FirstOrDefault(p => p.ID == guID);
            }
            return freq;
        }

        public List<MA_FREQ_TYPE> GetFreqTypeAll()
        {
            List<MA_FREQ_TYPE> FreqTypeList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                FreqTypeList = unitOfWork.MA_FREQ_TYPERepository.All().OrderBy(p => p.USERCODE).ToList();
            }
            return FreqTypeList;

        }
        public List<MA_FREQ_TYPE> GetFreqTypeByFilter(SessionInfo sessioninfo, string name, string sorting)
        {
            try
            {
                //IEnumerable<MA_USER> query;
                IEnumerable<MA_FREQ_TYPE> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_FREQ_TYPERepository.GetAll().AsQueryable();

                    //Filters
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.LABEL.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_FREQ_TYPE> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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
        public MA_FREQ_TYPE CreateFreqType(SessionInfo sessioninfo, MA_FREQ_TYPE freqtype)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_FREQ_TYPERepository.GetAll().FirstOrDefault(p => p.USERCODE.ToLower().Equals(freqtype.USERCODE.ToLower()) || p.INDEX == freqtype.INDEX);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);
                
                unitOfWork.MA_FREQ_TYPERepository.Add(freqtype);
                unitOfWork.Commit();
            }

            return freqtype;
        }
        public MA_FREQ_TYPE UpdateFreqType(SessionInfo sessioninfo, MA_FREQ_TYPE freqtype)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_FREQ_TYPERepository.GetAll().FirstOrDefault(p => (p.USERCODE.ToLower().Equals(freqtype.USERCODE.ToLower()) || p.INDEX == freqtype.INDEX) && p.ID != freqtype.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);
                
                var foundFreqType = unitOfWork.MA_FREQ_TYPERepository.All().FirstOrDefault(p => p.ID == freqtype.ID);
                if (foundFreqType == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);
                else
                {

                    //foundFreqType.ID = freqtype.ID;
                    foundFreqType.LABEL = freqtype.LABEL;
                    foundFreqType.ISACTIVE = freqtype.ISACTIVE;
                    foundFreqType.USERCODE = freqtype.USERCODE;
                    foundFreqType.INDEX = freqtype.INDEX;
                    unitOfWork.Commit();

                }
            }

            return freqtype;
        }
        #endregion FREQUENCY

        #region CURRENCY
        public List<MA_CURRENCY> GetCurrencyAll()
        {
            List<MA_CURRENCY> CurrencyList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                CurrencyList = unitOfWork.MA_CURRENCYRepository.All().OrderBy(p => p.LABEL).ToList();
            }
            return CurrencyList;

        }
        public List<MA_CURRENCY> GetCurrencyByFilter(SessionInfo sessioninfo, string name, string sorting)
        {
            try
            {
                //IEnumerable<MA_USER> query;
                IEnumerable<MA_CURRENCY> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_CURRENCYRepository.GetAll().AsQueryable();

                    //Filters
                    if (!string.IsNullOrEmpty(name))
                    {
                        query = query.Where(p => p.LABEL.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_CURRENCY> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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
        public MA_CURRENCY CreateCurrency(SessionInfo sessioninfo, MA_CURRENCY currency)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_CURRENCYRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(currency.LABEL.ToLower()));
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                unitOfWork.MA_CURRENCYRepository.Add(currency);
                unitOfWork.Commit();
            }

            return currency;
        }
        public MA_CURRENCY UpdateCurrency(SessionInfo sessioninfo, MA_CURRENCY currency)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_CURRENCYRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(currency.LABEL.ToLower()) && p.ID != currency.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var foundCurrency = unitOfWork.MA_CURRENCYRepository.All().FirstOrDefault(p => p.ID == currency.ID);
                if (foundCurrency == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND );
                else
                {

                    foundCurrency.LABEL = currency.LABEL;
                    foundCurrency.ISACTIVE = currency.ISACTIVE;
                    unitOfWork.Commit();

                }
            }

            return currency;
        }
        #endregion

        #region SPOT RATE

        public List<MA_SPOT_RATE> GetSpotRateByFilter(SessionInfo sessioninfo, string processdate, string sorting)
        {
            try
            {
                //IEnumerable<MA_USER> query;
                IEnumerable<MA_SPOT_RATE> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    IQueryable<MA_SPOT_RATE> query;
                    DateTime dteProc;
                    //Filters
                    if (string.IsNullOrEmpty(processdate))
                    
                        query  = unitOfWork.MA_SPOT_RATERepository.GetAll().AsQueryable();
                    
                    else if (!DateTime.TryParseExact(processdate, "dd/MM/yyyy", null, DateTimeStyles.None, out dteProc))

                        throw this.CreateException(new Exception(), "Invalid report date.");

                    else{

                        dteProc = DateTime.ParseExact(processdate, "dd/MM/yyyy", null);
                        query  = unitOfWork.MA_SPOT_RATERepository.GetByProcessDate(dteProc).AsQueryable();

                    }
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_SPOT_RATE> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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
        public List<MA_SPOT_RATE> GetSpotRateAll()
        {
            List<MA_SPOT_RATE> CurrencyList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                CurrencyList = unitOfWork.MA_SPOT_RATERepository.All().ToList();
            }
            return CurrencyList;

        }
        public MA_SPOT_RATE CreateSpotRate(SessionInfo sessioninfo, MA_SPOT_RATE spot)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_SPOT_RATERepository.GetAll().FirstOrDefault(p => p.CURRENCY_ID == spot.CURRENCY_ID && p.PROC_DATE == spot.PROC_DATE);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                unitOfWork.MA_SPOT_RATERepository.Add(spot);
                unitOfWork.Commit();
            }

            return spot;
        }
        public MA_SPOT_RATE UpdateSpotRate(SessionInfo sessioninfo, MA_SPOT_RATE spot)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_SPOT_RATERepository.GetAll().FirstOrDefault(p => p.CURRENCY_ID == spot.CURRENCY_ID && p.PROC_DATE == spot.PROC_DATE && p.ID != spot.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var foundSpot = unitOfWork.MA_SPOT_RATERepository.All().FirstOrDefault(p => p.ID == spot.ID);
                if (foundSpot == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);
                else
                {

                    foundSpot.CURRENCY_ID = spot.CURRENCY_ID;
                    foundSpot.PROC_DATE = spot.PROC_DATE;
                    foundSpot.RATE = spot.RATE;
                    unitOfWork.Commit();

                }
            }

            return spot;
        }

        #endregion

        #region Bond Market
        public List<MA_BOND_MARKET> GetBondMarketAll()
        {
            List<MA_BOND_MARKET> BondMarket;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                BondMarket = unitOfWork.MA_BOND_MARKETRepository.All().OrderBy(p => p.LABEL).ToList();
            }
            return BondMarket;

        }
        public List<MA_BOND_MARKET> GetBondMarketByFilter(SessionInfo sessioninfo, string label, string sorting)
        {
            try
            {
                IEnumerable<MA_BOND_MARKET> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_BOND_MARKETRepository.GetAll().AsQueryable();

                    //Filters
                    if (!string.IsNullOrEmpty(label))
                    {
                        query = query.Where(p => p.LABEL.IndexOf(label, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_BOND_MARKET> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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

        public MA_BOND_MARKET CreateBondMarket(SessionInfo sessioninfo, MA_BOND_MARKET market)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_BOND_MARKETRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(market.LABEL.ToLower()));
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                unitOfWork.MA_BOND_MARKETRepository.Add(market);
                unitOfWork.Commit();
            }

            return market;
        }

        public MA_BOND_MARKET UpdateBondMarket(SessionInfo sessioninfo, MA_BOND_MARKET market)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_BOND_MARKETRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(market.LABEL.ToLower()) && p.ID != market.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var foundMarket = unitOfWork.MA_BOND_MARKETRepository.All().FirstOrDefault(p => p.ID == market.ID);
                if (foundMarket == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);

                else
                {

                    foundMarket.LABEL = market.LABEL;
                    foundMarket.DESCRIPTION = market.DESCRIPTION;
                    unitOfWork.Commit();
                }
            }

            return market;
        }
        #endregion

        #region CSAType
        public List<MA_CSA_TYPE> GetCSATypeByFilter(SessionInfo sessioninfo, string label, string sorting)
        {
            try
            {
                IEnumerable<MA_CSA_TYPE> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_CSA_TYPERepository.All();

                    //Filters
                    if (!string.IsNullOrEmpty(label))
                    {
                        query = query.Where(p => p.LABEL.ToUpper().Contains(label.ToUpper()));
                    }
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_CSA_TYPE> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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

        public MA_CSA_TYPE CreateCSAType(SessionInfo sessioninfo, MA_CSA_TYPE csatype)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_BOND_MARKETRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(csatype.LABEL.ToLower()));
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                unitOfWork.MA_CSA_TYPERepository.Add(csatype);
                unitOfWork.Commit();
            }

            return csatype;
        }

        public MA_CSA_TYPE UpdateCSAType(SessionInfo sessioninfo, MA_CSA_TYPE csatype)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_CSA_TYPERepository.All().FirstOrDefault(p => p.LABEL.ToLower().Equals(csatype.LABEL.ToLower()) && p.ID != csatype.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var foundMarket = unitOfWork.MA_CSA_TYPERepository.All().FirstOrDefault(p => p.ID == csatype.ID);
                if (foundMarket == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);

                else
                {
                    foundMarket.LABEL = csatype.LABEL;
                    foundMarket.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                    foundMarket.LOG.MODIFYDATE = DateTime.Now;
                    unitOfWork.Commit();
                }
            }

            return csatype;
        }

        public List<MA_CSA_TYPE> GetCSATypeAll()
        {
            List<MA_CSA_TYPE> csatype;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                csatype = unitOfWork.MA_CSA_TYPERepository.All().OrderBy(p => p.LABEL).ToList();
            }
            return csatype;

        }

        #endregion

        #region TBMA Config
        public List<MA_TBMA_CONFIG> GetTBMAConfigAll(SessionInfo sessioninfo)
        {
            try
            {
                List<MA_TBMA_CONFIG> query = new List<MA_TBMA_CONFIG>();
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    query = unitOfWork.MA_TBMA_CONFIGRepository.All().ToList();
                }
                
                return query;
            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public MA_TBMA_CONFIG GetTBMAConfig(SessionInfo sessioninfo)
        {
            try
            {
                MA_TBMA_CONFIG query = new MA_TBMA_CONFIG();
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    query = unitOfWork.MA_TBMA_CONFIGRepository.All().FirstOrDefault();
                }

                return query;
            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public MA_TBMA_CONFIG UpdateTBMAConfig(SessionInfo sessioninfo, MA_TBMA_CONFIG config)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var foundConfig = unitOfWork.MA_TBMA_CONFIGRepository.All().FirstOrDefault(p => p.ID == config.ID);
                if (foundConfig == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);

                else
                {
                    foundConfig.TBMA_CAL_PASSWORD = config.TBMA_CAL_PASSWORD;
                    foundConfig.TBMA_CAL_USERNAME = config.TBMA_CAL_USERNAME;
                    foundConfig.TBMA_RPT_PATH = config.TBMA_RPT_PATH;
                    foundConfig.TBMA_RPT_PREFIX = config.TBMA_RPT_PREFIX;
                    foundConfig.TBMA_RPT_TRADERID = config.TBMA_RPT_TRADERID;

                    unitOfWork.Commit();
                }
            }

            return config;
        }
        #endregion
    }
}
