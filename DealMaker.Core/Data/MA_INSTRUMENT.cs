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
    public partial class MA_INSTRUMENT
    {
        public MA_INSTRUMENT()
        {
            this.DA_TRN = new HashSet<DA_TRN>();
            this.LOG = new LOG();
        }
    
        #region Primitive Properties
        public System.Guid ID { get; set; }
        public string LABEL { get; set; }
        public string ISSUER { get; set; }
        public Nullable<int> LOT_SIZE { get; set; }
        public Nullable<decimal> COUPON { get; set; }
        public Nullable<System.DateTime> MATURITY_DATE { get; set; }
        public string CAL_METHOD { get; set; }
        public bool ISACTIVE { get; set; }
        public System.Guid PRODUCT_ID { get; set; }
        public Nullable<bool> FLAG_FIXED { get; set; }
        public Nullable<System.Guid> COUPON_FREQ_TYPE_ID { get; set; }
        public Nullable<System.Guid> INS_MKT { get; set; }
        public Nullable<bool> FLAG_MULTIPLY { get; set; }
        public Nullable<System.Guid> CURRENCY_ID1 { get; set; }
        public Nullable<System.Guid> CURRENCY_ID2 { get; set; }

        #endregion
    
        #region Complex Properties
        public LOG LOG { get; set; }

        #endregion
    
        #region Navigation Properties
        public ICollection<DA_TRN> DA_TRN { get; set; }
        public MA_PRODUCT MA_PRODUCT { get; set; }
        public MA_FREQ_TYPE MA_FREQ_TYPE { get; set; }
        public MA_BOND_MARKET MA_BOND_MARKET { get; set; }
        public MA_CURRENCY MA_CURRENCY { get; set; }
        public MA_CURRENCY MA_CURRENCY2 { get; set; }

        #endregion
    }
    
}