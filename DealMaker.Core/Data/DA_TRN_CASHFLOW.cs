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
    public partial class DA_TRN_CASHFLOW
    {
        public DA_TRN_CASHFLOW()
        {
            this.LOG = new LOG();
        }
    
        #region Primitive Properties
        public System.Guid ID { get; set; }
        public System.Guid DA_TRN_ID { get; set; }
        public int SEQ { get; set; }
        public Nullable<decimal> RATE { get; set; }
        public Nullable<System.DateTime> FLOW_DATE { get; set; }
        public Nullable<decimal> FLOW_AMOUNT { get; set; }
        public bool FLAG_FIRST { get; set; }
        public Nullable<decimal> FLOW_AMOUNT_THB { get; set; }

        #endregion
    
        #region Complex Properties
        public LOG LOG { get; set; }

        #endregion
    
        #region Navigation Properties
        public DA_TRN DA_TRN { get; set; }

        #endregion
    }
    
}
