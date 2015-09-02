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
    public partial class MA_COUTERPARTY
    {
        public MA_COUTERPARTY()
        {
            this.MA_CTPY_LIMIT = new HashSet<MA_CTPY_LIMIT>();
            this.LOG = new LOG();
        }
    
        #region Primitive Properties
        public int USERCODE { get; set; }
        public string SNAME { get; set; }
        public string FNAME { get; set; }
        public string BUSINESS { get; set; }
        public string RATE { get; set; }
        public string OUTLOOK { get; set; }
        public Nullable<bool> ISACTIVE { get; set; }
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> GROUP_CTPY_ID { get; set; }
        public string TBMA_NAME { get; set; }
        public System.Guid COUNTRY_ID { get; set; }

        #endregion
    
        #region Complex Properties
        public LOG LOG { get; set; }

        #endregion
    
        #region Navigation Properties
        public ICollection<MA_CTPY_LIMIT> MA_CTPY_LIMIT { get; set; }
        public MA_COUTERPARTY GROUP_CTPY { get; set; }
        public MA_COUNTRY MA_COUNTRY { get; set; }
        public MA_CSA_AGREEMENT MA_CSA_AGREEMENT { get; set; }

        #endregion
    }
    
}