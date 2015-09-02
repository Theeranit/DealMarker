using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Business.Report;
using KK.DealMaker.Business.Deal;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.Business.Master;

namespace KK.DealMaker.UIProcessComponent.Report
{
    public class ReportUIP : BaseUIP
    {
        public static object GetPCEReport(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strLimit, string strSource, string strStatus, int jtStartIndex, int jtPageSize)
        {
            try
            {
                ReportBusiness _reportBusiness = new ReportBusiness();

                //Get data from database
                List<LimitCheckModel> limits = _reportBusiness.GetPCEReport(sessioninfo, strReportDate, strCtpy, strLimit, strSource, strStatus);

                //Return result to jTable
                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? limits.Skip(jtStartIndex).Take(jtPageSize).ToList() : limits, 
                             TotalRecordCount = limits.Count };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static List<LimitCheckModel> GetPCEExport(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strLimit, string strSource, string strStatus)
        {
            ReportBusiness _reportBusiness = new ReportBusiness();

            List<LimitCheckModel> limits = _reportBusiness.GetPCEReport(sessioninfo, strReportDate, strCtpy, strLimit, strSource, strStatus);

            return limits;
        }

        public static object GetPCEDetailReport(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strLimit, string strSource, int jtStartIndex, int jtPageSize)
        {
            try
            {
                var report = GetPCEDetailData(sessioninfo, strReportDate, strCtpy, strLimit, strSource);
                
                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? report.Skip(jtStartIndex).Take(jtPageSize).ToList() : report, 
                             TotalRecordCount = report.Count };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static List<DealViewModel> GetPCEDetailExport(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strProduct, string strSource)
        {
            return GetPCEDetailData(sessioninfo, strReportDate, strCtpy, strProduct, strSource);
        }

        public static List<DealViewModel> GetPCEDetailData(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strProduct, string strSource)
        {
            try
            {
                ReportBusiness _reportBusiness = new ReportBusiness();
                LookupBusiness _lookupBusiness = new LookupBusiness();
                CountryBusiness _countryBusiness = new CountryBusiness();
                CounterpartyBusiness _ctpyBusiness = new CounterpartyBusiness();

                List<DA_TRN> trns = _reportBusiness.GetPCEDetailReport(sessioninfo, strReportDate, strCtpy, strProduct, strSource);
                List<MA_CURRENCY> ccys = _lookupBusiness.GetCurrencyAll();
                List<MA_CSA_PRODUCT> csaproducts = _ctpyBusiness.GetCSAProductAll(sessioninfo);
                List<MA_COUNTRY> country = _countryBusiness.GetCountryAll();

                var report = (from trn in trns
                              join ct in country on trn.MA_COUTERPARTY.COUNTRY_ID equals ct.ID
                              join ccy1 in ccys on trn.FIRST.CCY_ID equals ccy1.ID into ljccy1
                              from subccy1 in ljccy1.DefaultIfEmpty()
                              join ccy2 in ccys on trn.SECOND.CCY_ID equals ccy2.ID into ljccy2
                              from subccy2 in ljccy2.DefaultIfEmpty()
                              join csaproduct in csaproducts on new { CTPY_ID = trn.CTPY_ID, PRODUCT_ID = trn.PRODUCT_ID.Value } equals new { CTPY_ID = csaproduct.CSA_AGREEMENT_ID, PRODUCT_ID = csaproduct.PRODUCT_ID } into ljcsa
                              from subcsa in ljcsa.DefaultIfEmpty()
                              select new DealViewModel
                              {
                                  EngineDate = trn.ENGINE_DATE,
                                  DMK_NO = trn.INT_DEAL_NO,
                                  OPICS_NO = trn.EXT_DEAL_NO,
                                  Source = trn.SOURCE == "INT" ? "DMK" : "OPICS",
                                  Product = trn.MA_PRODUCT.LABEL,
                                  Portfolio = trn.MA_PORTFOLIO.LABEL,
                                  TradeDate = trn.TRADE_DATE.Value,
                                  EffectiveDate = trn.START_DATE,
                                  MaturityDate = trn.MATURITY_DATE,
                                  Instrument = trn.MA_INSRUMENT.LABEL,
                                  Counterparty = trn.MA_COUTERPARTY.SNAME,
                                  Notional1 = trn.FIRST.NOTIONAL,
                                  Notional2 = trn.SECOND.NOTIONAL,
                                  FixedFloat1 = !trn.FIRST.FLAG_FIXED.HasValue ? "-" : trn.FIRST.FLAG_FIXED.Value ? trn.FIRST.FLAG_PAYREC + "-FIXED" : trn.FIRST.FLAG_PAYREC + "-FLOAT",
                                  FixedFloat2 = !trn.SECOND.FLAG_FIXED.HasValue ? "-" : trn.SECOND.FLAG_FIXED.Value ? trn.SECOND.FLAG_PAYREC + "-FIXED" : trn.SECOND.FLAG_PAYREC + "-FLOAT",
                                  KKPCCF = trn.KK_PCCF,
                                  KKContribute = trn.KK_CONTRIBUTE,
                                  CCY1 = subccy1 != null ? subccy1.LABEL : "-",
                                  CCY2 = subccy2 != null ? subccy2.LABEL : "-",
                                  CSA = subcsa != null ? "Yes" : "No",
                                  Country = ct.LABEL
                              }).OrderBy(p => p.DMK_NO).ToList();

                return report;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object GetSCEReport(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strSource, string strStatus, int jtStartIndex, int jtPageSize)
        {
            try
            {
                ReportBusiness _reportBusiness = new ReportBusiness();

                //Get data from database
                List<LimitCheckModel> limits = _reportBusiness.GetSCEReport(sessioninfo, strReportDate, strCtpy, strSource, strStatus);

                //Return result to jTable
                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? limits.Skip(jtStartIndex).Take(jtPageSize).ToList() : limits, 
                             TotalRecordCount = limits.Count };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static List<LimitCheckModel> GetSCEExport(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strSource, string strStatus)
        {
            ReportBusiness _reportBusiness = new ReportBusiness();

            List<LimitCheckModel> limits = _reportBusiness.GetSCEReport(sessioninfo, strReportDate, strCtpy, strSource, strStatus);

            return limits;
        }

        public static object GetSCEDetailReport(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strProduct, string strSource, int jtStartIndex, int jtPageSize)
        {
            try
            {
                var report = GetSCEDetailData(sessioninfo, strReportDate, strCtpy, strProduct, strSource);

                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? report.Skip(jtStartIndex).Take(jtPageSize).ToList() : report, 
                             TotalRecordCount = report.Count };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static List<DealViewModel> GetSCEDetailExport(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strProduct, string strSource)
        {
            return GetSCEDetailData(sessioninfo, strReportDate, strCtpy, strProduct, strSource);
        }

        public static List<DealViewModel> GetSCEDetailData(SessionInfo sessioninfo, string strReportDate, string strCtpy, string strProduct, string strSource)
        {
            ReportBusiness _reportBusiness = new ReportBusiness();

            List<DA_TRN_CASHFLOW> flows = _reportBusiness.GetSCEDetailReport(sessioninfo, strReportDate, strCtpy, strProduct, strSource);

            var formattedFlows = (from flow in flows
                                  select new DealViewModel
                                  {
                                      EngineDate = flow.DA_TRN.ENGINE_DATE,
                                      DMK_NO = flow.DA_TRN.INT_DEAL_NO,
                                      OPICS_NO = flow.DA_TRN.EXT_DEAL_NO,
                                      Source = flow.DA_TRN.SOURCE == "INT" ? "DMK" : "OPICS",
                                      Product = flow.DA_TRN.MA_PRODUCT.LABEL,
                                      Counterparty = flow.DA_TRN.MA_COUTERPARTY.SNAME,
                                      TradeDate = flow.DA_TRN.TRADE_DATE.Value,
                                      EffectiveDate = flow.DA_TRN.START_DATE,
                                      MaturityDate = flow.DA_TRN.MATURITY_DATE,
                                      Notional1 = flow.DA_TRN.FIRST.NOTIONAL,
                                      Leg = flow.FLAG_FIRST ? 1 : 2,
                                      Seq = flow.SEQ,
                                      CashflowRate = flow.RATE,
                                      CashflowDate = flow.FLOW_DATE,
                                      CashflowAmount = flow.FLOW_AMOUNT_THB,
                                      KKContribute = flow.DA_TRN.FLAG_SETTLE == true ? flow.FLOW_AMOUNT_THB : 0,
                                      Instrument = flow.DA_TRN.MA_INSRUMENT.LABEL
                                  }).ToList();

            return formattedFlows;
        }

        public static object GetLimitOverwriteReport(SessionInfo sessioninfo, string strReportDate, string strCtpy, int jtStartIndex, int jtPageSize)
        {
            try
            {
                var report = GetLimitOverwriteData(sessioninfo, strReportDate, strCtpy);

                return new
                {
                    Result = "OK",
                    Records = jtPageSize > 0 ? report.Skip(jtStartIndex).Take(jtPageSize).ToList() : report,
                    TotalRecordCount = report.Count
                };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }

        }
        public static List<DealViewModel> GetDealViewExport(SessionInfo sessioninfo
                                                    , string strDMKNo
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
            return View.DealViewUIP.GetDealInquiryData(sessioninfo
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
                                                        , strSettleStatus).AsQueryable().OrderBy(t => t.DMK_NO).ThenBy(t => t.EntryDate).ToList();
        }
        public static List<DealViewModel> GetLimitOverwriteExport(SessionInfo sessioninfo, string strReportDate, string strCtpy)
        {
            return GetLimitOverwriteData(sessioninfo, strReportDate, strCtpy);
        }
        public static List<DealViewModel> GetLimitOverwriteData(SessionInfo sessioninfo, string strReportDate, string strCtpy)
        {
            ReportBusiness _reportBusiness = new ReportBusiness();
            UserBusiness _userBusiness = new UserBusiness();
            LookupBusiness _lookupBusiness = new LookupBusiness();
            char[] trimchar = { '/' };    
            List<DA_TRN> trns =  _reportBusiness.GetLimitOverwriteReport(sessioninfo,strReportDate,strCtpy);
            List<MA_USER> users = _userBusiness.GetAll();
            List<MA_CURRENCY> ccys =  _lookupBusiness.GetCurrencyAll();
            var query = (from t in trns
                            join user in users on t.LOG.INSERTBYUSERID equals user.ID into ljuser
                            join ccy in ccys on t.FIRST.CCY_ID equals ccy.ID
                            from inputuser in ljuser.DefaultIfEmpty()
                            select new DealViewModel
                            {
                                EngineDate = t.ENGINE_DATE,
                                Trader = inputuser != null ? inputuser.USERCODE : "",
                                LimitApprover = t.OVER_APPROVER,
                                Remark = t.OVER_COMMENT,
                                DMK_NO = t.INT_DEAL_NO,
                                Counterparty = t.MA_COUTERPARTY.SNAME,
                                Product = t.MA_PRODUCT.LABEL,
                                Instrument = t.MA_INSRUMENT.LABEL,
                                Notional1 = t.FIRST.NOTIONAL,
                                CCY1 = ccy.LABEL,
                                KKContribute = t.KK_CONTRIBUTE,
                                LimitOverwrite = string.Concat(t.OVER_AMOUNT > 0 ? "PCE/" : "", t.OVER_SETTL_AMOUNT > 0 ? "SET/" : "", t.OVER_COUNTRY_AMOUNT > 0 ? "COUNTRY/" : "").TrimEnd(trimchar),
                                LimitOverAmount = ((t.OVER_AMOUNT > 0 ? "PCE: " + t.OVER_AMOUNT.Value.ToString("#,##0") : string.Empty) 
                                                    + (t.OVER_SETTL_AMOUNT > 0 ? " SET: " + t.OVER_SETTL_AMOUNT.Value.ToString("#,##0") : string.Empty)
                                                    + (t.OVER_COUNTRY_AMOUNT > 0 ? " COUNTRY: " + t.OVER_COUNTRY_AMOUNT.Value.ToString("#,##0") : string.Empty)).Trim()
                            }).OrderBy(t=>t.EngineDate).ToList();

            return query;
        }

        public static object GetRepoReport(SessionInfo sessioninfo, string strReportDate, string strReportType, string strCtpy, int jtStartIndex, int jtPageSize)
        {
            try
            {
                ReportBusiness _reportBusiness = new ReportBusiness();
                var report = _reportBusiness.GetRepoReport(sessioninfo, strReportDate, strReportType, strCtpy);

                return new
                {
                    Result = "OK",
                    Records = jtPageSize > 0 ? report.Skip(jtStartIndex).Take(jtPageSize).ToList() : report,
                    TotalRecordCount = report.Count
                };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static List<RepoReportModel> GetRepoExport(SessionInfo sessioninfo, string strReportDate, string strReportType, string strCtpy)
        {
            ReportBusiness _reportBusiness = new ReportBusiness();
            return  _reportBusiness.GetRepoReport(sessioninfo, strReportDate, strReportType, strCtpy);
        }

        public static object GetCountryReport(SessionInfo sessioninfo, string strReportDate, string strCountry, string strSource, string strStatus, int jtStartIndex, int jtPageSize)
        {
            try
            {
                List<LimitCheckModel> limits = GetCountryLimitData(sessioninfo, strReportDate, strCountry, strSource, strStatus);

                //Return result to jTable
                return new
                {
                    Result = "OK",
                    Records = jtPageSize > 0 ? limits.Skip(jtStartIndex).Take(jtPageSize).ToList() : limits,
                    TotalRecordCount = limits.Count
                };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static List<LimitCheckModel> GetCountryLimitData(SessionInfo sessioninfo, string strReportDate, string strCountry, string strSource, string strStatus)
        {
            ReportBusiness _reportBusiness = new ReportBusiness();

            return _reportBusiness.GetCountryReport(sessioninfo, strReportDate, strCountry, strSource, strStatus);
        }
        
        #region Log Report

        public static object GetLimitAuditReport(SessionInfo sessioninfo, string strLogDatefrom, string strLogDateto, string strCtpy, string strCountry, string strEvent, int jtStartIndex, int jtPageSize)
        {
            try
            {
                ReportBusiness _reportBusiness = new ReportBusiness();

                //Get data from database
                List<LimitAuditReportModel> logs = _reportBusiness.GetLimitAuditReport(sessioninfo, strLogDatefrom, strLogDateto, strCtpy, strCountry, strEvent);

                //Return result to jTable
                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? logs.Skip(jtStartIndex).Take(jtPageSize).ToList() : logs, 
                             TotalRecordCount = logs.Count };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static List<LimitAuditReportModel> GetLimitAuditExport(SessionInfo sessioninfo, string strLogDatefrom, string strLogDateto, string strCtpy, string strCountry, string strEvent)
        {
            ReportBusiness _reportBusiness = new ReportBusiness();
            CounterpartyBusiness _counterpartyBusiness = new CounterpartyBusiness();
            LookupBusiness _lookupBusiness = new LookupBusiness();
            UserBusiness _userBusiness = new UserBusiness();
            List<LimitAuditReportModel> logs = _reportBusiness.GetLimitAuditReport(sessioninfo, strLogDatefrom, strLogDateto, strCtpy, strCountry, strEvent);

           return logs;
        }
        #endregion
    }
}