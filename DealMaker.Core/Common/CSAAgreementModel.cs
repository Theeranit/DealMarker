using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.Common
{
    [Serializable]
    public class CSAAgreementModel : KK.DealMaker.Core.Data.MA_CSA_AGREEMENT
    {
        public string PRODUCTS { get; set; }
    }
}
