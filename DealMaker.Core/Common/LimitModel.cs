using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.Common
{
    public class LimitModel
    {
        public string LimitType { get; set; }
        public decimal OriginalCont { get; set; }
        public decimal DealCont { get; set; }
        public decimal Utilization { get; set; }
        public decimal UtiliPercentage { get; set; }
        public string Status { get; set; }
        public DateTime FlowDate { get; set; }
    }
}
