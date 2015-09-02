using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.OpicsData
{
    public class CASHFLOWModel
    {
        public string EXT_DEAL_NO { get; set; }
        public int LEG { get; set; }
        public int SEQ { get; set; }
        //public string FIXED_FLOAT { get; set; }
        public decimal RATE { get; set; }
        public DateTime FLOW_DATE { get; set; }
        public decimal FLOW_AMOUNT { get; set; }
        public decimal FLOW_AMOUNT_THB { get; set; }
        public DateTime DEALDATE { get; set; }
        public DateTime MATDATE { get; set; }
    }
}
