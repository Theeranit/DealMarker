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
    public partial class MA_CONFIG_ATTRIBUTE
    {
        public MA_CONFIG_ATTRIBUTE()
        {
            this.LOG = new LOG();
        }
    
        #region Primitive Properties
        public string TABLE { get; set; }
        public string ATTRIBUTE { get; set; }
        public string VALUE { get; set; }
        public System.Guid ID { get; set; }
        public bool ISACTIVE { get; set; }
        public System.Guid PCCF_CONFIG_ID { get; set; }

        #endregion
    
        #region Complex Properties
        public LOG LOG { get; set; }

        #endregion
    }
    
}
