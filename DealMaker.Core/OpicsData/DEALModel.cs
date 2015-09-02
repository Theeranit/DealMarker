using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.OpicsData
{
    public class DEALModel
    {
        public string EXT_DEAL_NO { get; set; }
        public string CPTY {get; set;}
        public string PRODUCT {get; set;}
        public DateTime TRADE_DATE {get; set;}
        public DateTime? START_DATE {get; set;}
        public DateTime MATURITY_DATE {get; set;}
        public string BUY_SELL {get; set;}
        public string INSTRUMENT {get; set;}
        public decimal NOTIONAL1 {get; set;}
        public string CCY1 {get; set;}
        public string PAY_REC1 {get; set;}
        public string FIXED_FLOAT1 {get; set;}
        public string FREQ1 {get; set;}
        public decimal FIRST_FIXING1 {get; set;}
        public decimal RATE1 {get; set;}
        public decimal NOTIONAL2 {get; set;}
        public string CCY2 {get; set;}
        public string PAY_REC2 {get; set;}
        public string FIXED_FLOAT2 {get; set;}
        public string FREQ2 {get; set;}
        public decimal FIRST_FIXING2 {get; set;}
        public decimal RATE2 {get; set;}
        public string INSERT_BY_EXT {get; set;}
        public DateTime INSERT_DATE {get; set;}
        public string EXT_PORTFOLIO {get; set;}
        public string PORTFOLIO {get; set;}
        public string INT_DEAL_NO { get; set; }
        public string SNAME { get; set; }

        public string FLAG_NEARFAR { get; set; }
        public decimal? SWAP_POINT1 { get; set; }
        public decimal? SWAP_POINT2 { get; set; }
        //public decimal NOTIONAL_THB { get; set; }
        public DateTime? SPOT_DATE { get; set; }
        public int FLAG_SETTLE { get; set; }
        public string REMARK { get; set; }
    }
}
