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
    public partial class MA_USER
    {
        public MA_USER()
        {
            this.LOG = new LOG();
        }
    
        #region Primitive Properties
        public System.Guid ID { get; set; }
        public string USERCODE { get; set; }
        public string NAME { get; set; }
        public string DEPARTMENT { get; set; }
        public Nullable<bool> ISLOCKED { get; set; }
        public Nullable<bool> ISACTIVE { get; set; }
        public string USER_OPICS { get; set; }
        public System.Guid USER_PROFILE_ID { get; set; }

        #endregion
    
        #region Complex Properties
        public LOG LOG { get; set; }

        #endregion
    
        #region Navigation Properties
        public MA_USER_PROFILE MA_USER_PROFILE { get; set; }

        #endregion
    }
    
}