using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.Common
{
    public class RepoReportModel : LimitCheckModel
    {
        public decimal REV_AMOUNT { get; set; }
        public decimal REP_GOV_5_AMOUNT { get; set; }
        public decimal REP_GOV_10_AMOUNT { get; set; }
        public decimal REP_GOV_20_AMOUNT { get; set; }
        public decimal REP_GOV_20s_AMOUNT { get; set; }
        public decimal REP_SOE_5_AMOUNT { get; set; }
        public decimal REP_SOE_10_AMOUNT { get; set; }
        public decimal REP_SOE_20_AMOUNT { get; set; }
        public decimal REP_SOE_20s_AMOUNT { get; set; }
    }
}
