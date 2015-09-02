using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using KK.DealMaker.UIProcessComponent.Report;
using KK.DealMaker.Core.Common;
namespace KK.DealMaker.Web.Report
{
    public partial class ExportToCSV : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {           
           switch (Request.QueryString["reportName"].ToString())
            {
                case "PCE": GetPCEExport(Request.QueryString["strReportDate"]
                                        , Request.QueryString["strTitle"]
                                        , Request.QueryString["strReportType"]
                                        , Request.QueryString["strCtpy"]
                                        , Request.QueryString["strLimit"]
                                        , Request.QueryString["strStatus"]); break;
                case "PCEDetail": GetPCEDetailExport(Request.QueryString["strReportDate"]
                                                    , Request.QueryString["strTitle"]
                                                    , Request.QueryString["strReportType"]
                                                    , Request.QueryString["strCtpy"]
                                                    , Request.QueryString["strProduct"]); break;
                case "SCE": GetSCEExport(Request.QueryString["strReportDate"]
                                        , Request.QueryString["strTitle"]
                                        , Request.QueryString["strReportType"]
                                        , Request.QueryString["strCtpy"]
                                        , Request.QueryString["strStatus"]); break;
                case "SCEDetail": GetSCEDetailExport(Request.QueryString["strReportDate"]
                                                    , Request.QueryString["strTitle"]
                                                    , Request.QueryString["strReportType"]
                                                    , Request.QueryString["strCtpy"]
                                                    , Request.QueryString["strProduct"]); break;
                case "LimitAudit": GetLimitAuditExport(Request.QueryString["strReportDate"]
                                                        , Request.QueryString["strReportDateto"]
                                                        , Request.QueryString["strCtpy"]
                                                        , Request.QueryString["strCountry"]
                                                        , Request.QueryString["strEvent"]); break;
                case "LimitOverwrite": GetLimitOverwriteExport(Request.QueryString["strReportDate"], Request.QueryString["strCtpy"]); break;
                case "DealView": GetDealViewExport(Request.QueryString["DMKNo"]
                                                , Request.QueryString["OPICNo"]
                                                , Request.QueryString["product"]
                                                , Request.QueryString["counterparty"]
                                                , Request.QueryString["portfolio"]
                                                , Request.QueryString["tradedate"]
                                                , Request.QueryString["effdate"]
                                                , Request.QueryString["matdate"]
                                                , Request.QueryString["instrument"]
                                                , Request.QueryString["user"]
                                                , Request.QueryString["status"]
                                                , Request.QueryString["overstatus"]
                                                , Request.QueryString["procdate"]
                                                , Request.QueryString["settle"]
                                                ); break;
                case "Repo": GetRepoExport(Request.QueryString["strReportDate"]
                                            , Request.QueryString["strTitle"]
                                            , Request.QueryString["strReportType"] 
                                            , Request.QueryString["strCtpy"]); break;
                case "Country": GetCountryExport(Request.QueryString["strReportDate"]
                                                 , Request.QueryString["strTitle"]
                                                 , Request.QueryString["strReportType"]
                                                 , Request.QueryString["strCountry"]
                                                 , Request.QueryString["strStatus"]); break;

           }              
        }

        private static void GetPCEExport(string strReportDate, string strTitle, string strReportType, string strCtpy, string strLimit, string strStatus)
        {
            string strFileName = "PCE_" + (strReportType != "" ? "BOD" : "Intraday") + "_" + strReportDate.Replace("/", "-");
            string[] columnheaders = { "\"Report Date\"", "\"Counterparty\"", "\"Limit\"", "\"Control\"", "\"Expired Date\"", "\"Limit Amount\"", "\"Temp Amount\"", "\"Utilization\"", "\"Available\"", "\"Status\"" };
            StringBuilder stringBuilder = new StringBuilder();           
            List<LimitCheckModel> records = ReportUIP.GetPCEExport(SessionInfo, strReportDate,  strCtpy, strLimit, strReportType, strStatus);
            foreach(LimitCheckModel record in records){
                AddComma(record.PROCESSING_DATE.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.SNAME, stringBuilder);
                AddComma(record.LIMIT_LABEL, stringBuilder);
                AddComma(record.FLAG_CONTROL == true ? "Control" : "No-Restriction" , stringBuilder);
                AddComma(record.EXPIRE_DATE.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.GEN_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.TEMP_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.UTILIZATION.ToString("#,##0"), stringBuilder);
                AddComma(record.AVAILABLE.ToString("#,##0"), stringBuilder);
                AddComma(record.STATUS, stringBuilder,true);
                stringBuilder.Append(Environment.NewLine);
            }
            WriteCSVFile(strTitle, columnheaders, stringBuilder, strFileName);
        }

        private static void GetPCEDetailExport(string strReportDate, string strTitle, string strReportType, string strCtpy, string strProduct)
        {
            string strFileName = "PCEDetail_" + (strReportType != "" ? "BOD" : "Intraday") + "_" + strReportDate.Replace("/", "-");
            string[] columnheaders = { "\"Report Date\"", "\"DMK NO\"", "\"OPICS NO\"", "\"Source\"", "\"Product\"", "\"Portfolio\"", "\"Trade Date\"", "\"Effective Date\"", "\"Maturity Date\"", "\"Instrument\"", "\"Counterparty\"", "\"Country\"", "\"Leg 1\"", "\"Notional1\"", "\"CCY1\"", "\"Leg 2\"", "\"Notional2\"", "\"CCY2\"", "\"PCCF\"", "\"Contribute\"", "\"CSA\"" };
            StringBuilder stringBuilder = new StringBuilder();
            List<DealViewModel> records = ReportUIP.GetPCEDetailExport(SessionInfo, strReportDate, strCtpy, strProduct, strReportType);
            foreach (DealViewModel record in records)
            {
                AddComma(record.EngineDate.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.DMK_NO, stringBuilder);
                AddComma(record.OPICS_NO, stringBuilder);
                AddComma(record.Source, stringBuilder);
                AddComma(record.Product, stringBuilder);
                AddComma(record.Portfolio, stringBuilder);
                AddComma(record.TradeDate.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.EffectiveDate.HasValue ? record.EffectiveDate.Value.ToString("dd-MMM-yyyy") : string.Empty, stringBuilder);
                AddComma(record.MaturityDate.HasValue ? record.MaturityDate.Value.ToString("dd-MMM-yyyy") : string.Empty , stringBuilder);
                AddComma(record.Instrument, stringBuilder);
                AddComma(record.Counterparty, stringBuilder);
                AddComma(record.Country, stringBuilder);
                AddComma(record.FixedFloat1, stringBuilder);
                AddComma(record.Notional1.HasValue ? record.Notional1.Value.ToString("#,##0") : string.Empty, stringBuilder);
                AddComma(record.CCY1, stringBuilder);
                AddComma(record.FixedFloat2, stringBuilder);
                AddComma(record.Notional2.HasValue ? record.Notional2.Value.ToString("#,##0") : string.Empty, stringBuilder);
                AddComma(record.CCY2, stringBuilder);
                AddComma(record.KKPCCF.HasValue ? record.KKPCCF.Value.ToString("#,##0.00") : string.Empty, stringBuilder);
                AddComma(record.KKContribute.HasValue ? record.KKContribute.Value.ToString("#,##0") : string.Empty, stringBuilder);
                AddComma(record.CSA, stringBuilder);
                stringBuilder.Append(Environment.NewLine);
            }
            WriteCSVFile(strTitle, columnheaders, stringBuilder, strFileName);         
        }

        private static void GetSCEExport(string strReportDate, string strTitle, string strReportType, string strCtpy, string strStatus)
        {
            string strFileName = "SCE_" + (strReportType != "" ? "BOD" : "Intraday") + "_" + strReportDate.Replace("/", "-");
            string[] columnheaders = { "\"Report Date\"", "\"Counterparty\"", "\"Expired Date\"", "\"Limit Amount\"", "\"Temp Amount\"", "\"Utilization\"", "\"Utilization Date\"", "\"Available\"", "\"Status\"" };
            StringBuilder stringBuilder = new StringBuilder();
            List<LimitCheckModel> records = ReportUIP.GetSCEExport(SessionInfo, strReportDate, strCtpy, strReportType, strStatus);
            foreach (LimitCheckModel record in records)
            {
                AddComma(record.PROCESSING_DATE.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.SNAME, stringBuilder);
                AddComma(record.EXPIRE_DATE.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.GEN_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.TEMP_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.ORIGINAL_KK_CONTRIBUTE.ToString("#,##0"), stringBuilder);
                AddComma(record.FLOW_DATE.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.AVAILABLE.ToString("#,##0"), stringBuilder);
                AddComma(record.STATUS, stringBuilder, true);
                stringBuilder.Append(Environment.NewLine);
            }
            WriteCSVFile(strTitle, columnheaders, stringBuilder, strFileName);   

        }

        private static void GetSCEDetailExport(string strReportDate, string strTitle, string strReportType, string strCtpy, string strProduct)
        {
            string strFileName = "SCEDetail_" + (strReportType != "" ? "BOD" : "Intraday") + "_" + strReportDate.Replace("/", "-");
            string[] columnheaders = { "\"Report Date\"", "\"DMK NO\"", "\"OPICS NO\"", "\"Product\"", "\"Instrument\"", "\"Counterparty\"", "\"Notional\"", "\"Trade Date\"", "\"Effective Date\"", "\"Maturity Date\"", "\"Leg\"", "\"Seq\"", "\"Rate\"", "\"Flow Date\"", "\"Flow Amount\"", "\"Utilization\"" };
            StringBuilder stringBuilder = new StringBuilder();
            List<DealViewModel> records = ReportUIP.GetSCEDetailExport(SessionInfo, strReportDate, strCtpy, strProduct, strReportType);
            foreach (DealViewModel record in records)
            {
                AddComma(record.EngineDate.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.DMK_NO, stringBuilder);
                AddComma(record.OPICS_NO, stringBuilder);
                AddComma(record.Product, stringBuilder);
                AddComma(record.Instrument, stringBuilder);
                AddComma(record.Counterparty, stringBuilder);
                AddComma(record.Notional1.HasValue ? record.Notional1.Value.ToString("#,##0") : string.Empty, stringBuilder);
                AddComma(record.TradeDate.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.EffectiveDate.HasValue ? record.EffectiveDate.Value.ToString("dd-MMM-yyyy") : string.Empty, stringBuilder);
                AddComma(record.MaturityDate.HasValue ? record.MaturityDate.Value.ToString("dd-MMM-yyyy") : string.Empty, stringBuilder);
                AddComma(record.Leg.ToString(), stringBuilder);
                AddComma(record.Seq.ToString(), stringBuilder);
                AddComma(record.CashflowRate.HasValue ? record.CashflowRate.Value.ToString("#,##0.0000") : string.Empty, stringBuilder);
                AddComma(record.CashflowDate.HasValue ? record.CashflowDate.Value.ToString("dd-MMM-yyyy") : string.Empty, stringBuilder);
                AddComma(record.CashflowAmount.HasValue ? record.CashflowAmount.Value.ToString("#,##0.00") : string.Empty, stringBuilder);
                AddComma(record.KKContribute.Value.ToString("#,##0"), stringBuilder);
                stringBuilder.Append(Environment.NewLine);
            }

            WriteCSVFile(strTitle, columnheaders, stringBuilder, strFileName);   
        }

        private static void GetLimitAuditExport(string strReportDate, string strReportDateto, string strCtpy, string strCountry, string strEvent)
        {
            string subject = "\"Limit Audit Report\"";
            string strFileName = "LimitAudit_" + strReportDate.Replace("/", "-") +"to" +strReportDateto.Replace("/", "-");
            string[] columnheaders = { "\"Log Date\"", "\"Entity\"", "\"Limit Type\"", "\"User\"", "\"Detail\""};
            StringBuilder stringBuilder = new StringBuilder();
            var records = ReportUIP.GetLimitAuditExport(SessionInfo, strReportDate, strReportDateto, strCtpy, strCountry, strEvent);
            
            foreach (LimitAuditReportModel record in records) 
            {
                AddComma(record.LOG_DATE.ToString("dd-MMM-yyyy HH:mm"), stringBuilder);
                AddComma(record.ENTITY, stringBuilder);
                AddComma(record.LIMIT, stringBuilder);
                AddComma(record.USER, stringBuilder);
                AddComma(record.DETAIL, stringBuilder,true);
                stringBuilder.Append(Environment.NewLine);
            
            }
            WriteCSVFile(subject, columnheaders, stringBuilder, strFileName);

        }

        private static void GetLimitOverwriteExport(string strReportDate, string strCtpy)
        {
            string subject = "\"Limit Overwrite Report\"";
            string strFileName = "LimitOverwrite_" + strReportDate.Replace("/", "-");
            string[] columnheaders = { "\"Date\"", "\"Trader\"", "\"Approver\"", "\"Comment\"", "\"DMK NO\"", "\"Counterparty\"", "\"Product\"", "\"Instrument\"", "\"Notional\"", "\"Currency\"", "\"Utilization (PCE)\"", "\"Over Limit\"", "\"Over amount\"" };
            StringBuilder stringBuilder = new StringBuilder();
            var records = ReportUIP.GetLimitOverwriteExport(SessionInfo, strReportDate, strCtpy);
            foreach ( DealViewModel record in records)
            {
                AddComma(record.EngineDate.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.Trader, stringBuilder);
                AddComma(record.LimitApprover, stringBuilder);
                AddComma(record.Remark, stringBuilder);
                AddComma(record.DMK_NO, stringBuilder);
                AddComma(record.Counterparty, stringBuilder);
                AddComma(record.Product, stringBuilder);
                AddComma(record.Instrument, stringBuilder);
                AddComma(record.Notional1.Value.ToString("#,##0"), stringBuilder);
                AddComma(record.CCY1, stringBuilder);
                AddComma(record.KKContribute.Value.ToString("#,##0"), stringBuilder);
                AddComma(record.LimitOverwrite, stringBuilder);
                AddComma(record.LimitOverAmount, stringBuilder,true);
                stringBuilder.Append(Environment.NewLine);

            }
            WriteCSVFile(subject, columnheaders, stringBuilder, strFileName);
        }

        private static void GetDealViewExport(string strDMKNo
                                    , string strOPICNo
                                    , string strProduct
                                    , string strCtpy
                                    , string strPortfolio
                                    , string strTradeDate
                                    , string strEffDate
                                    , string strMatDate
                                    , string strInstrument
                                    , string strUser
                                    , string strStatus
                                    , string strOverStatus
                                    , string strProcDate
                                    , string strSettleStatus)
        {
            string subject = string.Empty;
            string strFileName = "DealReport";
            string[] columnheaders = { "\"LimitOverwrite\"", "\"LimitApprover\"", "\"Entry Date\"",
                                         "\"DMK NO\"", "\"OPICS NO\"", "\"Trade Date\"",
                                         "\"Effective Date\"", "\"Instrument\"", "\"Limit End Date\"" ,
                                         "\"B/S\"", "\"Product\"", "\"Portfolio\"" ,
                                         "\"CTPY\"", "\"Notional1\"", "\"Ccy1\"", "\"PayRec1\"" ,
                                         "\"FixedFloat1\"", "\"Freq1\"", "\"Fixing1\"", "\"Rate1\"" ,
                                         "\"SwapPoint1\"", "\"Notional2\"", "\"Ccy2\"", "\"PayRec2\"" ,
                                         "\"FixedFloat2\"", "\"Freq2\"", "\"Fixing2\"", "\"Rate2\"" ,
                                         "\"SwapPoint2\"", "\"Trader\"", "\"OpicsTrader\"", "\"Status\"" ,
                                         "\"PCE Amount\"", "\"SET\"", "\"TBMA\"", "\"Comment\""};

            StringBuilder stringBuilder = new StringBuilder();
            List<DealViewModel> records = ReportUIP.GetDealViewExport(SessionInfo
                                                                            , strDMKNo
                                                                            , strOPICNo
                                                                            , strProduct
                                                                            , strCtpy
                                                                            , strPortfolio
                                                                            , strTradeDate
                                                                            , strEffDate
                                                                            , strMatDate
                                                                            , strInstrument
                                                                            , strUser
                                                                            , strStatus
                                                                            , strOverStatus
                                                                            , strProcDate
                                                                            , strSettleStatus);
            foreach (DealViewModel record in records)
            {
                AddComma(record.LimitOverwrite, stringBuilder);
                AddComma(record.LimitApprover, stringBuilder);
                AddComma(record.EntryDate.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.DMK_NO, stringBuilder);
                AddComma(record.OPICS_NO, stringBuilder);
                AddComma(record.TradeDate.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.EffectiveDate.HasValue ? record.EffectiveDate.Value.ToString("dd-MMM-yyyy") : string.Empty, stringBuilder);
                AddComma(record.Instrument, stringBuilder);
                AddComma(record.MaturityDate.HasValue ? record.MaturityDate.Value.ToString("dd-MMM-yyyy") : string.Empty, stringBuilder);
                AddComma(record.BuySell, stringBuilder);
                AddComma(record.Product, stringBuilder);
                AddComma(record.Portfolio, stringBuilder);
                AddComma(record.Counterparty, stringBuilder);
                AddComma(record.Notional1.HasValue ? record.Notional1.Value.ToString("#,##0") : string.Empty, stringBuilder);
                AddComma(record.CCY1, stringBuilder);
                AddComma(record.PayRec1, stringBuilder);
                AddComma(record.FixedFloat1, stringBuilder);
                AddComma(record.Freq1, stringBuilder);
                AddComma(record.Fixing1.HasValue ? record.Fixing1.Value.ToString("#,##0.00") : string.Empty, stringBuilder);
                AddComma(record.Rate1.HasValue ? record.Rate1.Value.ToString("#,##0.00") : string.Empty, stringBuilder);
                AddComma(record.SwapPoint1.HasValue ? record.SwapPoint1.Value.ToString("#,##0.00") : string.Empty, stringBuilder);
                AddComma(record.Notional2.HasValue ? record.Notional2.Value.ToString("#,##0") : string.Empty, stringBuilder);
                AddComma(record.CCY2, stringBuilder);
                AddComma(record.PayRec2, stringBuilder);
                AddComma(record.FixedFloat2, stringBuilder);
                AddComma(record.Freq2, stringBuilder);
                AddComma(record.Fixing2.HasValue ? record.Fixing2.Value.ToString("#,##0.00") : string.Empty, stringBuilder);
                AddComma(record.Rate2.HasValue ? record.Rate2.Value.ToString("#,##0.00") : string.Empty, stringBuilder);
                AddComma(record.SwapPoint2.HasValue ? record.SwapPoint2.Value.ToString("#,##0.00") : string.Empty, stringBuilder);
                AddComma(record.Trader, stringBuilder);
                AddComma(record.OpicsTrader, stringBuilder);
                AddComma(record.Status, stringBuilder);
                AddComma(record.KKContribute.HasValue ? record.KKContribute.Value.ToString("#,##0") : string.Empty, stringBuilder);
                AddComma(record.SettlementLimit, stringBuilder);
                AddComma(record.TBMA_SENT, stringBuilder);
                AddComma(record.Remark, stringBuilder);
                stringBuilder.Append(Environment.NewLine);
            }
            WriteCSVFile(subject, columnheaders, stringBuilder, strFileName);
        }

        private static void GetRepoExport(string strReportDate, string strTitle, string strReportType, string strCtpy)
        {
            string strFileName = "Repo_" + (strReportType != "" ? "BOD_" : "Intraday_") + strReportDate.Replace("/", "-");
            string[] columnheaders = { "\"Report Date\"", "\"CTPY\"", "\"Limit\"", "\"Limit Amt\""//, "\"Expiry\"", "\"OS\""
                                         , "\"Available\"", "\"RRP\""
                                        , "\"RP_GOV_0-5\"", "\"RP_GOV_5-10\"", "\"RP_GOV_10-20\"", "\"RP_GOV_20+\""
                                        , "\"RP_S0E_0-5\"", "\"RP_SOE_5-10\"", "\"RP_SOE_10-20\"", "\"RP_SOE_20+\""};
            StringBuilder stringBuilder = new StringBuilder();
            var records = ReportUIP.GetRepoExport(SessionInfo, strReportDate, strReportType, strCtpy);

            foreach (RepoReportModel record in records)
            {
                AddComma(record.PROCESSING_DATE.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.SNAME, stringBuilder);
                AddComma(record.LIMIT_LABEL, stringBuilder);
                AddComma(record.AMOUNT.ToString("#,##0"), stringBuilder);
                //AddComma(record.EXPIRE_DATE.ToString("dd-MMM-yyyy"), stringBuilder);
                //AddComma(record.ORIGINAL_KK_CONTRIBUTE.ToString("#,##0"), stringBuilder);
                AddComma(record.AVAILABLE.ToString("#,##0"), stringBuilder);
                AddComma(record.REV_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.REP_GOV_5_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.REP_GOV_10_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.REP_GOV_20_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.REP_GOV_20s_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.REP_SOE_5_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.REP_SOE_10_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.REP_SOE_20_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.REP_SOE_20s_AMOUNT.ToString("#,##0"), stringBuilder);
                stringBuilder.Append(Environment.NewLine);

            }
            WriteCSVFile(strTitle, columnheaders, stringBuilder, strFileName);
        }

        private static void GetCountryExport(string strReportDate, string strTitle, string strReportType, string strCountry, string strStatus)
        {
            string strFileName = "Country_" + (strReportType != "" ? "BOD" : "Intraday") + "_" + strReportDate.Replace("/", "-");
            string[] columnheaders = { "\"Report Date\"", "\"Country\"", "\"Expired Date\"", "\"Limit Amount\"", "\"Temp Amount\"", "\"PCE Utilization\"", "\"SET Utilization\"", "\"Utilization Date\"", "\"Available\"", "\"Status\"" };
            StringBuilder stringBuilder = new StringBuilder();
            List<LimitCheckModel> records = ReportUIP.GetCountryLimitData(SessionInfo, strReportDate, strCountry, strReportType, strStatus);
            foreach (LimitCheckModel record in records)
            {
                AddComma(record.PROCESSING_DATE.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.COUNTRY_LABEL, stringBuilder);
                AddComma(record.EXPIRE_DATE.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.GEN_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.TEMP_AMOUNT.ToString("#,##0"), stringBuilder);
                AddComma(record.PCE_CONTRIBUTE.ToString("#,##0"), stringBuilder);
                AddComma(record.SET_CONTRIBUTE.ToString("#,##0"), stringBuilder);
                AddComma(record.FLOW_DATE.ToString("dd-MMM-yyyy"), stringBuilder);
                AddComma(record.AVAILABLE.ToString("#,##0"), stringBuilder);
                AddComma(record.STATUS, stringBuilder, true);
                stringBuilder.Append(Environment.NewLine);
            }
            WriteCSVFile(strTitle, columnheaders, stringBuilder, strFileName);

        }

        private static void WriteCSVFile(string subject, string[] columnheaders, StringBuilder body, string strFileName)
        {
            string attachment = "attachment; filename=" + strFileName + ".csv";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");
            string strcolumnheaders = string.Join(",", columnheaders);
            if (subject != string.Empty)
            {
                HttpContext.Current.Response.Write(subject);
                HttpContext.Current.Response.Write(Environment.NewLine);
            }
            HttpContext.Current.Response.Write(strcolumnheaders);
            HttpContext.Current.Response.Write(Environment.NewLine);
            HttpContext.Current.Response.Write(body.ToString());
            HttpContext.Current.Response.End();
        }

        private static void AddComma(string value, StringBuilder stringBuilder,bool last = false)
        {  
            stringBuilder.Append('"');
            stringBuilder.Append(value);
            stringBuilder.Append('"');
            if (!last)
            stringBuilder.Append(',');
        }
    }

}