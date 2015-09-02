using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Data;

namespace KK.DealMaker.Core.Common
{
    public class FIDealModel
    { 
        public Guid ID { get; set; }
        public string DMK_NO { get; set; }
        public string OPICS_NO { get; set; }
        public string InsertState { get; set; }
        public DateTime TradeDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string Instrument { get; set; }
        public string BuySell { get; set; }
        public string Portfolio { get; set; }
        public string Counterparty { get; set; }
        public decimal Yield { get; set; }
        public decimal GrossValue { get; set; }
        public string CCY { get; set; }
        public string Trader { get; set; }
        public string Status { get; set; }
        public decimal PCE { get; set; }

        //TBMA info
        public string TBMA_Trader_ID { get; set; }
        public string Sender { get; set; }
        public int Unit { get; set; }
        public decimal CleanPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public string YieldType { get; set; }
        public string ReporyBy { get; set; }
        public string Purpose { get; set; }
        public int Term { get; set; }
        public decimal Rate { get; set; }
        public string TBMA_Remark { get; set; }
        public string SendTime { get; set; }
        public string PrimaryMarket { get; set; }
        public string NonDVP { get; set; }
    }
}
