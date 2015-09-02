using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.Common
{
    public class PermisionModel
    {
        public string FunctionalCode { get; set; }
        public string FunctionalLabel { get; set; }
        public bool IsReadable { get; set; }
        public bool IsWritable { get; set; }
        public bool IsApprovable { get; set; }
    }
}
