using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Business.Master;
using KK.DealMaker.Core.Data;

namespace KK.DealMaker.UIProcessComponent.Common
{
    public interface ILookupValues<T>
    {
        T GetByID(Guid ID);
        T GetByUsercode(string usercode);
        T GetByLabel(string label);
    }

    public class MemoryLookupValues
    {        
        public List<MA_FREQ_TYPE> Frequencies { get; private set; }
        public List<MA_PRODUCT> Products { get; private set; }
        public List<MA_STATUS> Statuses { get; private set; }
        public List<MA_PORTFOLIO> Portfolios { get; private set; }
        public List<MA_LIMIT> Limits { get; private set; }
        public List<MA_FUNCTIONAL> Functionals { get; private set; }
        public List<MA_USER_PROFILE> UserProfiles { get; private set; }
        public List<MA_CURRENCY> Currencies { get; private set; }
        
        LookupBusiness _lookupBusiness = new LookupBusiness();    

        public MemoryLookupValues()
        {
            Frequencies = _lookupBusiness.GetFreqTypeAll();
            Products = _lookupBusiness.GetProductAll();
            Statuses = _lookupBusiness.GetStatusAll();
            Portfolios = _lookupBusiness.GetPortfolioAll();
            Limits = _lookupBusiness.GetLimitAll();
            Currencies = _lookupBusiness.GetCurrencyAll();
        }
    }
}
