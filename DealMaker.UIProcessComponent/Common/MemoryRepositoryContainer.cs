using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Data;


namespace KK.DealMaker.UIProcessComponent.Common
{
    public class MemoryRepositoryContainer : ILookupValuesRepository
    {
        private readonly MemoryLookupValues _dataSource;

        public MemoryRepositoryContainer(MemoryLookupValues dataSource)
        {
            _dataSource = dataSource;
        }

        public ILookupValues<MA_STATUS> StatusRepository
        {
            get { return new MemoryStatusRepository(_dataSource); }
        }

        public ILookupValues<MA_PORTFOLIO> PortfolioRepository
        {
            get { return new MemoryPortfolioRepository(_dataSource); }
        }

        public ILookupValues<MA_PRODUCT> ProductRepository
        {
            get { return new MemoryProductRepository(_dataSource); }
        }

        public ILookupValues<MA_LIMIT> LimitRepository
        {
            get { return new MemoryLimitRepository(_dataSource); }
        }

        public ILookupValues<MA_FREQ_TYPE> FrequencyRepository
        {
            get { return new MemoryFrequencyRepository(_dataSource); }
        }

        public ILookupValues<MA_CURRENCY> CurrencyRepository
        {
            get { return new MemoryCurrencyRepository(_dataSource); }
        }
    }
}
