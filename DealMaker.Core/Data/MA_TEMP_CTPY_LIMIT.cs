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
    public partial class MA_TEMP_CTPY_LIMIT
    {
        public MA_TEMP_CTPY_LIMIT()
        {
            this.LOG = new LOG();
        }
    
        #region Primitive Properties
        public System.Guid ID { get; set; }
        public System.Guid CTPY_LIMIT_ID { get; set; }
        public decimal AMOUNT { get; set; }
        public System.DateTime EFFECTIVE_DATE { get; set; }
        public System.DateTime EXPIRY_DATE { get; set; }
        public bool ISACTIVE { get; set; }

        #endregion
    
        #region Complex Properties
        public LOG LOG { get; set; }

        #endregion
    }
    
}
