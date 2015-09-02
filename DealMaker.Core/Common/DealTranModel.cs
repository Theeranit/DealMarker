using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;

namespace KK.DealMaker.Core.Common
{
    [Serializable]
    public class DealTranModel
    {
        public UpdateStates UpdateStates { get; set; }
        public ProductCode ProductTransaction { get; set; }
        public DA_TRN Transaction { get; set; }
    }
}
