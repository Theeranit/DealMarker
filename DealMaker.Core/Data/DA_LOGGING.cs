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
    public partial class DA_LOGGING
    {
        public DA_LOGGING()
        {
            this.LOG = new LOG();
        }
    
        #region Primitive Properties
        public System.Guid ID { get; set; }
        public string EVENT { get; set; }
        public string TABLE_NAME { get; set; }
        public System.DateTime LOG_DATE { get; set; }
        public Nullable<System.Guid> RECORD_ID { get; set; }
        public string LOG_DETAIL { get; set; }

        #endregion
    
        #region Complex Properties
        public LOG LOG { get; set; }

        #endregion
    }
    
}
