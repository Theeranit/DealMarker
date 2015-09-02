using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.Common
{
    [Serializable]
   public class CptyLimitModel
   {
        public Guid ID { get; set; }
        public string SNAME { get; set; }
        public string PCE_ALL { get; set; }
        public string PCE_FI { get; set; }
        public string PCE_IRD { get; set; }
        public string PCE_FX { get; set; }
        public string PCE_REPO { get; set; }
        public string SET_ALL { get; set; }
    }
}
