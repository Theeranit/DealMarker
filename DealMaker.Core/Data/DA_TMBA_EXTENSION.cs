//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KK.DealMaker.Core.Data
{
    public partial class DA_TMBA_EXTENSION
    {
        #region Primitive Properties
        public System.Guid ID { get; set; }
        public string PURPOSE { get; set; }
        public string YIELD_TYPE { get; set; }
        public decimal CLEAN_PRICE { get; set; }
        public decimal GROSS_PRICE { get; set; }
        public int UNIT { get; set; }
        public Nullable<int> TERM { get; set; }
        public Nullable<decimal> RATE { get; set; }
        public string REMARK { get; set; }
        public Nullable<System.Guid> SENDER_ID { get; set; }
        public Nullable<System.DateTime> SEND_DATE { get; set; }
        public bool IS_REPORT_CLEAN { get; set; }

        #endregion
    }
    
}
