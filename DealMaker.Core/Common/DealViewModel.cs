using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KK.DealMaker.Core.Data;

namespace KK.DealMaker.Core.Common
{
    public class DealViewModel
    {
        public Guid ID { get; set; }
        public DateTime EngineDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string DMK_NO { get; set; }
        public string OPICS_NO { get; set; }
        public string Source { get; set; }
        public DateTime TradeDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string Instrument { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string BuySell { get; set; }
        public string Product { get; set; }
        public string Portfolio { get; set; }
        public string Counterparty { get; set; }
        public decimal? Notional1 { get; set; }
        public string PayRec1 { get; set; }
        public string FixedFloat1 { get; set; }
        public string Freq1 { get; set; }
        public decimal? Fixing1 { get; set; }
        public decimal? Rate1 { get; set; }
        public decimal? SwapPoint1 { get; set; }
        public decimal? Notional2 { get; set; }
        public string PayRec2 { get; set; }
        public string FixedFloat2 { get; set; }
        public string Freq2 { get; set; }
        public decimal? Fixing2 { get; set; }
        public decimal? Rate2 { get; set; }
        public decimal? SwapPoint2 { get; set; }
        public string Status { get; set; }
        public decimal? BotPCCF { get; set; }
        public decimal? KKPCCF { get; set; }
        public decimal? BotContribute { get; set; }
        public decimal? KKContribute { get; set; }
        public string LimitOverwrite { get; set; }
        public string LimitOverAmount { get; set; }
        public string LimitApprover { get; set; }
        public string CCY1 { get; set; }
        public string CCY2 { get; set; }
        public string Trader { get; set; }
        public string Remark { get; set; }
        public string PCE { get; set; }
        public string SettlementLimit { get; set; }
        public string OpicsTrader { get; set; }
        public string CSA { get; set; }
        public string TBMA_SENT { get; set; }
        public string Country { get; set; }

        //Cashflow info
        public int Leg { get; set; }
        public int Seq { get; set; }
        public decimal? CashflowRate { get; set; }
        public DateTime? CashflowDate { get; set; }
        public decimal? CashflowAmount { get; set; }
    }
}