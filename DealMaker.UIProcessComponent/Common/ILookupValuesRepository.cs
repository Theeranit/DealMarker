using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KK.DealMaker.Core.Data;

namespace KK.DealMaker.UIProcessComponent.Common
{
    public interface ILookupValuesRepository
    {
        ILookupValues<MA_STATUS> StatusRepository { get; }
        ILookupValues<MA_PORTFOLIO> PortfolioRepository { get; }
        ILookupValues<MA_PRODUCT> ProductRepository { get; }
        ILookupValues<MA_FREQ_TYPE> FrequencyRepository { get; }
        ILookupValues<MA_LIMIT> LimitRepository { get; }
        ILookupValues<MA_CURRENCY> CurrencyRepository { get; }
    }
}
