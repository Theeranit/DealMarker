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
    public partial class MA_CSA_AGREEMENT
    {
        public MA_CSA_AGREEMENT()
        {
            this.MA_CSA_PRODUCT = new HashSet<MA_CSA_PRODUCT>();
            this.LOG = new LOG();
        }
    
        #region Primitive Properties
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> CSA_TYPE_ID { get; set; }
        public bool ISACTIVE { get; set; }

        #endregion
    
        #region Complex Properties
        public LOG LOG { get; set; }

        #endregion
    
        #region Navigation Properties
        public MA_CSA_TYPE MA_CSA_TYPE { get; set; }
        public ICollection<MA_CSA_PRODUCT> MA_CSA_PRODUCT { get; set; }

        #endregion
    }
    
}