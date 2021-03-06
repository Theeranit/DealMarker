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
    public partial class MA_PCCF_CONFIG
    {
        public MA_PCCF_CONFIG()
        {
            this.MA_CONFIG_ATTRIBUTE = new HashSet<MA_CONFIG_ATTRIBUTE>();
            this.LOG = new LOG();
        }
    
        #region Primitive Properties
        public System.Guid ID { get; set; }
        public string LABEL { get; set; }
        public string DESCRIPTION { get; set; }
        public System.Guid PRODUCT_ID { get; set; }
        public System.Guid PCCF_ID { get; set; }
        public Nullable<bool> ISACTIVE { get; set; }
        public Nullable<int> PRIORITY { get; set; }

        #endregion
    
        #region Complex Properties
        public LOG LOG { get; set; }

        #endregion
    
        #region Navigation Properties
        public MA_PCCF MA_PCCF { get; set; }
        public ICollection<MA_CONFIG_ATTRIBUTE> MA_CONFIG_ATTRIBUTE { get; set; }

        #endregion
    }
    
}
