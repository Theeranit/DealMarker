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
    public partial class DA_TRN
    {
        public DA_TRN()
        {
            this.DA_TRN_FLOW = new HashSet<DA_TRN_CASHFLOW>();
            this.FIRST = new TRN_TYPE();
            this.SECOND = new TRN_TYPE();
            this.LOG = new LOG();
        }
    
        #region Primitive Properties
        public System.Guid ID { get; set; }
        public System.DateTime ENGINE_DATE { get; set; }
        public string INT_DEAL_NO { get; set; }
        public string EXT_DEAL_NO { get; set; }
        public int VERSION { get; set; }
        public string SOURCE { get; set; }
        public Nullable<System.Guid> PRODUCT_ID { get; set; }
        public Nullable<System.Guid> INSTRUMENT_ID { get; set; }
        public Nullable<System.Guid> PORTFOLIO_ID { get; set; }
        public string EXT_PORTFOLIO { get; set; }
        public Nullable<System.DateTime> TRADE_DATE { get; set; }
        public Nullable<System.DateTime> START_DATE { get; set; }
        public Nullable<System.DateTime> MATURITY_DATE { get; set; }
        public string FLAG_BUYSELL { get; set; }
        public Nullable<decimal> KK_PCCF { get; set; }
        public Nullable<decimal> KK_CONTRIBUTE { get; set; }
        public Nullable<decimal> BOT_PCCF { get; set; }
        public Nullable<decimal> BOT_CONTRIBUTE { get; set; }
        public string OVER_APPROVER { get; set; }
        public Nullable<decimal> OVER_AMOUNT { get; set; }
        public Nullable<decimal> OVER_SETTL_AMOUNT { get; set; }
        public string OVER_COMMENT { get; set; }
        public System.Guid STATUS_ID { get; set; }
        public System.Guid CTPY_ID { get; set; }
        public string INSERT_BY_EXT { get; set; }
        public string FLAG_NEARFAR { get; set; }
        public Nullable<decimal> NOTIONAL_THB { get; set; }
        public Nullable<System.DateTime> SPOT_DATE { get; set; }
        public Nullable<bool> FLAG_SETTLE { get; set; }
        public string REMARK { get; set; }
        public Nullable<bool> FLAG_PCE { get; set; }
        public Nullable<decimal> OVER_COUNTRY_AMOUNT { get; set; }

        #endregion
    
        #region Complex Properties
        public TRN_TYPE FIRST { get; set; }
        public TRN_TYPE SECOND { get; set; }
        public LOG LOG { get; set; }

        #endregion
    
        #region Navigation Properties
        public ICollection<DA_TRN_CASHFLOW> DA_TRN_FLOW { get; set; }
        public MA_PORTFOLIO MA_PORTFOLIO { get; set; }
        public MA_PRODUCT MA_PRODUCT { get; set; }
        public MA_STATUS MA_STATUS { get; set; }
        public MA_INSTRUMENT MA_INSRUMENT { get; set; }
        public MA_COUTERPARTY MA_COUTERPARTY { get; set; }
        public DA_TMBA_EXTENSION DA_TMBA_EXTENSION { get; set; }

        #endregion
    }
    
}
