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
    public partial class MA_PROCESS_DATE
    {
        public MA_PROCESS_DATE()
        {
            this.LOG = new LOG();
        }
    
        #region Primitive Properties
        public System.DateTime PROC_DATE { get; set; }
        public System.DateTime PREV_PROC_DATE { get; set; }
        public System.DateTime NEXT_PROC_DATE { get; set; }
        public Nullable<bool> FLAG_RECONCILE { get; set; }

        #endregion
    
        #region Complex Properties
        public LOG LOG { get; set; }

        #endregion
    }
    
}