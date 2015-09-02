using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Data;

namespace KK.DealMaker.UIProcessComponent.Common
{
    #region STATUS
    public class MemoryStatusRepository : ILookupValues<MA_STATUS>
    {
        private readonly MemoryLookupValues _dataSource;

        public MemoryStatusRepository(MemoryLookupValues dataSource)
        {
            _dataSource = dataSource;
        }

        public MA_STATUS GetByID(Guid ID)
        {
            return _dataSource.Statuses.FirstOrDefault(p => p.ID == ID);
        }

        public MA_STATUS GetByUsercode(string usercode)
        {
            //Not implementation
            throw new Exception("Not Implementation!");
        }

        public MA_STATUS GetByLabel(string label)
        {
            return _dataSource.Statuses.FirstOrDefault(p => p.LABEL.Contains(label));
        }
    }
    #endregion

    #region PORTFOLIO
    public class MemoryPortfolioRepository : ILookupValues<MA_PORTFOLIO>
    {
        private readonly MemoryLookupValues _dataSource;

        public MemoryPortfolioRepository(MemoryLookupValues dataSource)
        {
            _dataSource = dataSource;
        }

        public MA_PORTFOLIO GetByID(Guid ID)
        {
            return _dataSource.Portfolios.FirstOrDefault(p => p.ID == ID);
        }

        public MA_PORTFOLIO GetByUsercode(string usercode)
        {
            //Not implementation
            throw new Exception("Not Implementation!");
        }

        public MA_PORTFOLIO GetByLabel(string label)
        {
            return _dataSource.Portfolios.FirstOrDefault(p => p.LABEL.Contains(label));
        }
    }
    #endregion

    #region PRODUCT
    public class MemoryProductRepository : ILookupValues<MA_PRODUCT>
    {
        private readonly MemoryLookupValues _dataSource;

        public MemoryProductRepository(MemoryLookupValues dataSource)
        {
            _dataSource = dataSource;
        }

        public MA_PRODUCT GetByID(Guid ID)
        {
            return _dataSource.Products.FirstOrDefault(p => p.ID == ID);
        }

        public MA_PRODUCT GetByUsercode(string usercode)
        {
            //Not implementation
            throw new Exception("Not Implementation!");
        }

        public MA_PRODUCT GetByLabel(string label)
        {
            return _dataSource.Products.FirstOrDefault(p => p.LABEL.Contains(label));
        }
    }
    #endregion

    #region LIMIT
    public class MemoryLimitRepository : ILookupValues<MA_LIMIT>
    {
        private readonly MemoryLookupValues _dataSource;

        public MemoryLimitRepository(MemoryLookupValues dataSource)
        {
            _dataSource = dataSource;
        }

        public MA_LIMIT GetByID(Guid ID)
        {
            return _dataSource.Limits.FirstOrDefault(p => p.ID == ID);
        }

        public MA_LIMIT GetByUsercode(string usercode)
        {
            //Not implementation
            throw new Exception("Not Implementation!");
        }

        public MA_LIMIT GetByLabel(string label)
        {
            return _dataSource.Limits.FirstOrDefault(p => p.LABEL.Contains(label));
        }
    }
    #endregion

    #region FREQ_TYPE
    public class MemoryFrequencyRepository : ILookupValues<MA_FREQ_TYPE>
    {
        private readonly MemoryLookupValues _dataSource;

        public MemoryFrequencyRepository(MemoryLookupValues dataSource)
        {
            _dataSource = dataSource;
        }

        public MA_FREQ_TYPE GetByID(Guid ID)
        {
            return _dataSource.Frequencies.FirstOrDefault(p => p.ID == ID);
        }

        public MA_FREQ_TYPE GetByUsercode(string usercode)
        {
            return _dataSource.Frequencies.FirstOrDefault(p => p.USERCODE.Contains(usercode));
        }

        public MA_FREQ_TYPE GetByLabel(string label)
        {
            return _dataSource.Frequencies.FirstOrDefault(p => p.LABEL.Contains(label));
        }
    }
    #endregion

    #region CURRENCY
    public class MemoryCurrencyRepository : ILookupValues<MA_CURRENCY>
    {
        private readonly MemoryLookupValues _dataSource;

        public MemoryCurrencyRepository(MemoryLookupValues dataSource)
        {
            _dataSource = dataSource;
        }

        public MA_CURRENCY GetByID(Guid ID)
        {
            return _dataSource.Currencies.FirstOrDefault(p => p.ID == ID);
        }

        public MA_CURRENCY GetByUsercode(string usercode)
        {
            throw new Exception("Not Implementation!");
        }

        public MA_CURRENCY GetByLabel(string label)
        {
            return _dataSource.Currencies.FirstOrDefault(p => p.LABEL.Contains(label));
        }
    }
    #endregion
}
