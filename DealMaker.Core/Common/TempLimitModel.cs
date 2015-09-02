using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KK.DealMaker.Core.Data;

namespace KK.DealMaker.Core.Common
{
    public class TempLimitModel
    {
        public string CTPY_NAME { get; set; }
        public string LIMIT_NAME { get; set; }
        public DateTime EFF_DATE { get; set; }
        public DateTime EXP_DATE { get; set; }
        public decimal AMOUNT { get; set; }
        public bool STATUS { get; set; }
    }
}
