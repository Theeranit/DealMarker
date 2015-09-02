using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.OpicsData
{
    public class SECMModel
    {
        public string SECID {get; set;}
        public string ISSUER {get; set;}
        public string DENOM { get; set; }
        public decimal COUPRATE_8 { get; set; }
        public string MDATE { get; set; }
        public string INTCALCRULE {get; set;}
        public string PMTFREQ { get; set; }
        public string CCY { get; set; }

    }
}
