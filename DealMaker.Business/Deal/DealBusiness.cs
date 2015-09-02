using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.EnterpriseServices;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.DataAccess.Repositories;
using KK.DealMaker.Core.SystemFramework;
using System.Configuration;
using System.Linq.Expressions;
using KK.DealMaker.Business.Master;
using System.Globalization;
using System.IO;

namespace KK.DealMaker.Business.Deal
{
	public class DealBusiness : BaseBusiness 
	{     

		#region General
        public DA_TRN GetFXSwapPair(string strIntDealNo, int intVersion, Guid id)
		{
            DA_TRN obj = null;
			using (EFUnitOfWork unit = new EFUnitOfWork())
			{
				obj = unit.DA_TRNRepository.All().FirstOrDefault(p => p.INT_DEAL_NO == strIntDealNo && p.VERSION == intVersion && p.ID != id) ;
			}
			return obj;
		}


        public List<DA_TRN> GetDealByDealNo(string INT_DEAL_NO , int version = -1)
        {
            List<DA_TRN> obj = null;
            using (EFUnitOfWork unit = new EFUnitOfWork())
            {
                obj = unit.DA_TRNRepository.All().Where(t => t.INT_DEAL_NO == INT_DEAL_NO && (version != -1 ? t.VERSION == version : true)).ToList();
            }
            return obj;
        }

		public List<DA_TRN> GetDealByProcessDate(DateTime processdate)
		{
			List<DA_TRN> trans = null;
			using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			{
				trans = unitOfWork.DA_TRNRepository.GetAllByEngineDate(processdate);
			}
			return trans;
		}

        public List<DA_TRN> GetDealInternalByProcessDate(DateTime processdate)
        {
            List<DA_TRN> trans = null;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                trans = unitOfWork.DA_TRNRepository.GetAllInternalByEngineDate(processdate);
            }
            return trans;
        }

		public List<DA_TRN> GetDealByProcessDate(DateTime processdate, StatusCode statuscode)
		{
			List<DA_TRN> trans = null;
			using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			{
				trans = unitOfWork.DA_TRNRepository.GetAllByEngineDateStatusCode(processdate, statuscode.ToString());
			}
			return trans;
		}

		public List<DA_TRN> GetImportedDealsByProcessDate(DateTime processdate)
		{
			List<DA_TRN> trans = null;
			using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			{
				trans = unitOfWork.DA_TRNRepository.GetImportedByEngineDate(processdate);
			}
			return trans;
		}
		
		public DA_TRN GetPastImportedDealsByExtNo(string strExtNo)
		{
			DA_TRN tran = null;
			using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			{
				tran = unitOfWork.DA_TRNRepository.GetPastImportedByExtNo(strExtNo);
			}
			return tran;
		}

		public List<DA_TRN> GetDealByStatusCode(StatusCode statuscode)
		{
			List<DA_TRN> trans = null;
			using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			{
				trans = unitOfWork.DA_TRNRepository.GetAllByStatusCode(statuscode.ToString());
			}
			return trans;
		}

		public DA_TRN GetByID(Guid ID)
		{
			DA_TRN tran = null;
			using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			{
				tran = unitOfWork.DA_TRNRepository.GetById(ID, true);
			}
			return tran;
		}

		public DA_TRN GetByDealNoProcessDate(DateTime procdate, string dealno, SourceType source)
		{
			DA_TRN tran = null;
			using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			{
				tran = unitOfWork.DA_TRNRepository.GetByDealNoProcessDate(procdate, dealno, source, true);
			}
			return tran;
		}

		public List<DA_TRN> GetByExternalByInternalDealNo(DateTime procdate, string intdealno)
		{
			List<DA_TRN> trans = null;
			using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			{
				trans = unitOfWork.DA_TRNRepository.GetByExternalByInternalDealNo(procdate, intdealno);
			}
			return trans;
		}

		public string GetNewDealNo(DateTime dteEngine)
		{
			int intCount = 0;
			using (EFUnitOfWork unit = new EFUnitOfWork())
			{
				intCount = unit.DA_TRNRepository.All().Where(p => p.ENGINE_DATE == dteEngine && p.SOURCE == "INT").Count() + 1;
			}

			return dteEngine.ToString("yyyyMMdd") + intCount.ToString("000");
		}

		public int CountByProcessDate(DateTime dteProcess)
		{
			int intCount = 0;
			using (EFUnitOfWork unit = new EFUnitOfWork())
			{
				intCount = unit.DA_TRNRepository.All().Where(p => p.ENGINE_DATE == dteProcess).Count();
			}

			return intCount;
		}

		public List<DA_TRN_CASHFLOW> GetFlowsByProcessDate(DateTime ProcessDate)
		{
			List<DA_TRN_CASHFLOW> flows = null;
			using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			{
				flows = unitOfWork.DA_TRN_CASHFLOWRepository.GetAllByEngineDate(ProcessDate);
			}
			return flows;
		}

		private void SetLimitDisplayStatus(LimitCheckModel objLimit, bool blnOverwrite, ref LimitDisplayModel objLimitDisplay)
		{
			if (objLimit.STATUS == eLimitStatusCode.EXCEED.ToString() || objLimit.STATUS == eLimitStatusCode.EXPIRED.ToString())
			{
				if ((objLimit.UTILIZATION - objLimit.AMOUNT) > objLimitDisplay.OverAmount)
				{
					objLimitDisplay.OverAmount = objLimit.UTILIZATION - objLimit.AMOUNT;
				}
                
                if (!blnOverwrite || objLimit.TEMP_AMOUNT != 0)
				    objLimitDisplay.LimitCheckStatus = eLimitCheckStatus.NOTALLOW;
                else
                    objLimitDisplay.LimitCheckStatus = eLimitCheckStatus.NEEDAPPROVE;
			}
		}

        private List<LimitCheckModel> GenerateSETDisplay(List<DA_TRN_CASHFLOW> deal_conts, List<LimitCheckModel> ori_conts, bool blnOverwrite, ref LimitDisplayModel limitDisplay)
        {
            List<LimitCheckModel> tempList = null;
            List<LimitCheckModel> limits = new List<LimitCheckModel>();
            LimitCheckModel limit = null;

            foreach (var deal_cont in deal_conts)
            {
                tempList = ori_conts.Where(f => f.FLOW_DATE == deal_cont.FLOW_DATE).ToList();
                
                foreach (LimitCheckModel item in tempList)
                {
                    limit = new LimitCheckModel();

                    limit.ORIGINAL_KK_CONTRIBUTE = item.ORIGINAL_KK_CONTRIBUTE;
                    limit.SNAME = item.SNAME;
                    limit.LIMIT_LABEL = item.LIMIT_LABEL;
                    limit.FLAG_CONTROL = item.FLAG_CONTROL;
                    limit.GEN_AMOUNT = item.GEN_AMOUNT;
                    limit.TEMP_AMOUNT = item.TEMP_AMOUNT;
                    //limit.AMOUNT = item.AMOUNT;
                    limit.FLOW_DATE = deal_cont.FLOW_DATE.Value;
                    limit.EXPIRE_DATE = item.EXPIRE_DATE;
                    limit.PROCESSING_DATE = item.PROCESSING_DATE;
                    limit.DEAL_CONTRIBUTION = deal_cont.FLOW_AMOUNT_THB.Value;

                    SetLimitDisplayStatus(limit, blnOverwrite, ref limitDisplay);

                    limits.Add(limit);
                }

                //Case that original exposure has no match date with deal exposure
                if (tempList.Count == 0)
                {
                    //Get limit info that is not equal to deal flow date
                    var limitinfos = ori_conts.GroupBy(p => p.SNAME)
                                                .Select(g => new { g, count = g.Count() })
                                                .SelectMany(t => t.g.Select(b => b)
                                                    .Zip(Enumerable.Range(1, t.count), (j, i) => new { j.SNAME, j.LIMIT_LABEL, j.FLAG_CONTROL, j.GEN_AMOUNT, j.TEMP_AMOUNT, j.EXPIRE_DATE, j.PROCESSING_DATE, rn = i }))
                                                .Where(w => w.rn == 1).ToList();

                    if (limitinfos.Count() > 0)
                    {
                        foreach (var check in limitinfos)
                        {
                            limit = new LimitCheckModel();

                            limit.ORIGINAL_KK_CONTRIBUTE = 0;
                            limit.SNAME = check.SNAME;
                            limit.LIMIT_LABEL = check.LIMIT_LABEL;
                            limit.FLAG_CONTROL = check.FLAG_CONTROL;
                            limit.GEN_AMOUNT = check.GEN_AMOUNT;
                            limit.TEMP_AMOUNT = check.TEMP_AMOUNT;
                            //limit.AMOUNT = check.AMOUNT;
                            limit.FLOW_DATE = deal_cont.FLOW_DATE.Value;
                            limit.EXPIRE_DATE = check.EXPIRE_DATE;
                            limit.PROCESSING_DATE = check.PROCESSING_DATE;
                            limit.DEAL_CONTRIBUTION = deal_cont.FLOW_AMOUNT_THB.Value;

                            SetLimitDisplayStatus(limit, blnOverwrite, ref limitDisplay);

                            limits.Add(limit);
                        }
                    }
                }
            }

            return limits.OrderBy(p => p.SNAME).ThenBy(p => p.FLOW_DATE).ToList() ;
        }

        private List<LimitCheckModel> GenerateCountryLimitDisplay(List<CountryLimitModel> deal_conts, List<LimitCheckModel> ori_conts, DA_TRN trn1, DA_TRN trn2, bool blnOverwrite, ref LimitDisplayModel limitDisplay)
        {
            CountryLimitModel tempCountry = null;
            List<LimitCheckModel> limits = new List<LimitCheckModel>();
            decimal decExp = trn1.KK_CONTRIBUTE.Value + (trn2 != null ? trn2.KK_CONTRIBUTE.Value : 0);

            foreach (var ori_cont in ori_conts)
            {
                tempCountry = deal_conts.FirstOrDefault(f => f.EXPOSURE_DATE == ori_cont.FLOW_DATE);

                ori_cont.DEAL_CONTRIBUTION = tempCountry != null ? tempCountry.EXPOSURE.Value : decExp;

                SetLimitDisplayStatus(ori_cont, blnOverwrite, ref limitDisplay);

                if ((ori_cont.FLOW_DATE == trn1.TRADE_DATE || ori_cont.FLOW_DATE == trn1.MATURITY_DATE)
                    || (trn2 != null && (ori_cont.FLOW_DATE == trn2.TRADE_DATE || ori_cont.FLOW_DATE == trn2.MATURITY_DATE))
                    || (ori_cont.STATUS == eLimitStatusCode.EXCEED.ToString() || ori_cont.STATUS == eLimitStatusCode.EXPIRED.ToString()))
                {
                    limits.Add(ori_cont);
                }
            }

            return limits;
        }

		#endregion

		#region Deal Inquiry
		public List<DA_TRN> GetDealInquiryByFilter(SessionInfo sessioninfo
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
                                                    , string strProcDate)
		{
			try
			{
				List<DA_TRN> trns = null;

				using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
				{
                    DateTime procDate = DateTime.ParseExact(strProcDate, "dd/MM/yyyy", null);
                    var query = unitOfWork.DA_TRNRepository.GetAllByEngineDate(procDate).AsQueryable();
					Guid guTemp;

					//Filters
					if (!string.IsNullOrEmpty(strDMKNo)) //Dealmaker NO
					{
						query = query.Where(p => p.INT_DEAL_NO == strDMKNo);
					}
					if (!string.IsNullOrEmpty(strOPICNo)) //OPICS NO
					{
						query = query.Where(p => p.EXT_DEAL_NO == strOPICNo);
					}
					if (Guid.TryParse(strProduct, out guTemp)) //Product
					{
						query = query.Where(p => p.PRODUCT_ID == Guid.Parse(strProduct));
					}
					if (Guid.TryParse(strCtpy, out guTemp)) //Counterparty
					{
						query = query.Where(p => p.CTPY_ID == Guid.Parse(strCtpy));
					}
					if (Guid.TryParse(strPortfolio, out guTemp)) //Portfolio
					{
						query = query.Where(p => p.PORTFOLIO_ID == Guid.Parse(strPortfolio));
					}
					if (!string.IsNullOrEmpty(strTradeDate)) //Trade date
					{
						query = query.Where(p => p.TRADE_DATE == DateTime.ParseExact(strTradeDate, "dd/MM/yyyy", null));
					}
					if (!string.IsNullOrEmpty(strEffDate)) //Effective Date
					{
						query = query.Where(p => p.START_DATE == DateTime.ParseExact(strEffDate, "dd/MM/yyyy", null));
					}
					if (!string.IsNullOrEmpty(strMatDate)) //Maturity Date
					{
						query = query.Where(p => p.MATURITY_DATE == DateTime.ParseExact(strMatDate, "dd/MM/yyyy", null));
					}
					if (Guid.TryParse(strInstrument, out guTemp)) //Instrument
					{
						query = query.Where(p => p.INSTRUMENT_ID == Guid.Parse(strInstrument));
					}
					if (Guid.TryParse(strUser, out guTemp)) //User
					{
						query = query.Where(p => p.LOG.INSERTBYUSERID == Guid.Parse(strUser));
					}
					if (Guid.TryParse(strStatus, out guTemp)) //Status
					{
						query = query.Where(p => p.STATUS_ID == Guid.Parse(strStatus));
					}
					
					trns = query.OrderBy(p => p.INT_DEAL_NO).ThenBy(p => p.VERSION).ThenBy(p => p.FLAG_BUYSELL).ToList(); //Default!                                       
				}
				//Return result to jTable
				return trns;

			}
			catch (DataServicesException ex)
			{
				throw this.CreateException(ex, null);
			}
		}

		public DA_TRN CancelDeal(SessionInfo sessioninfo, DA_TRN trn)
		{
			LookupBusiness _lookupBusiness = new LookupBusiness();
			using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			{
				var foundTrn = unitOfWork.DA_TRNRepository.GetById(trn.ID,true);
				if (foundTrn == null)
					throw this.CreateException(new Exception(), "Data not found!");
				else if (!Guid.Equals(foundTrn.LOG.INSERTBYUSERID, sessioninfo.CurrentUserId))
				{
					throw this.CreateException(new Exception(), "You have no right to cancel this transaction");
				}
				else if (!(foundTrn.ENGINE_DATE.Date == sessioninfo.Process.CurrentDate && foundTrn.SOURCE == "INT"))
				{
					throw this.CreateException(new Exception(), "You cannot cancel the past deals");
				}
				else
				{
					//foundTrn.STATUS_ID = new Guid("1ccd7506-b98c-4afa-838e-24378d9b3c2e");
					foundTrn.REMARK = trn.REMARK;
					foundTrn.STATUS_ID = _lookupBusiness.GetStatusAll().FirstOrDefault(p => p.LABEL == StatusCode.CANCELLED.ToString()).ID;
					foundTrn.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
					foundTrn.LOG.MODIFYDATE = DateTime.Now;
					trn = foundTrn;
					unitOfWork.Commit();
				}
			}
			return trn;
		}
		#endregion
			 
		#region FI Product
		public string SubmitFIDeal(SessionInfo sessioninfo
									, DA_TRN trn
									, string strOverApprover
									, string strOverComment
									, string strProductId)
		{
			//Move to GenerateFITransactionObject()
			//trn.STATUS_ID = Guid.Parse("9161ed18-1298-44fa-ba7d-34522cb40d66");
			//trn.INT_DEAL_NO = GetNewDealNo(sessioninfo.Process.CurrentDate);

			//StaticDataBusiness _staticdataBusiness = new StaticDataBusiness();
			//trn.KK_PCCF = _staticdataBusiness.GetFIPCCF(sessioninfo, trn);
			//trn.KK_CONTRIBUTE = trn.NOTIONAL_THB * trn.KK_PCCF / 100;

			LookupBusiness _lookupBusiness = new LookupBusiness();
			if (!string.IsNullOrEmpty(strOverApprover))
			{
				trn.OVER_APPROVER = strOverApprover;
				trn.OVER_COMMENT = strOverComment;
			}
			
			using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			{
				Guid guTemp;
				if (Guid.TryParse(strProductId, out guTemp))
				{
					var foundDeal = unitOfWork.DA_TRNRepository.All().FirstOrDefault(p => p.ID == guTemp);
					if (foundDeal == null)
						throw this.CreateException(new Exception(), "Data not found!");
					else
					{
						foundDeal.STATUS_ID = _lookupBusiness.GetStatusAll().FirstOrDefault(p => p.LABEL == StatusCode.CANCELLED.ToString()).ID;
						foundDeal.REMARK = trn.REMARK;
					}
				}
				trn.REMARK = null;
				if (trn.INT_DEAL_NO == null)
				trn.INT_DEAL_NO = GetNewDealNo(sessioninfo.Process.CurrentDate);
				unitOfWork.DA_TRNRepository.Add(trn);
				unitOfWork.Commit();
			}
			return trn.INT_DEAL_NO;
		}

		public DA_TRN GenerateFITransactionObject(SessionInfo sessionInfo
                                                    , string strTradeDate
                                                    , string strBuySell
                                                    , string strInstrument
                                                    , string strCtpy
                                                    , string strPortfolio
                                                    , string strSettlementDate
                                                    , string strYield
                                                    , string strUnit
                                                    , string strCleanPrice
                                                    , string strGrossPrice
                                                    , string strNotional
                                                    , string strCCY
                                                    , string strPceFlag
                                                    , string strSettleFlag
                                                    , string strYeildType
                                                    , string strReportBy
                                                    , string strPurpose
                                                    , string strTerm
                                                    , string strRate
                                                    , string strTBMARemark
                                                    , string strRemark
                                                    , string strProductId)
		{
			//Validate business logics here
			//throw error to UI if fail validation
			LookupBusiness _lookupBusiness = new LookupBusiness();
			DA_TRN trn = new DA_TRN();
			DA_TRN lastrn = null;
			DateTime dteTemp;
			Decimal decTemp;
			Guid guTemp;
			if (Guid.TryParse(strProductId, out guTemp))
			{
				lastrn = new DA_TRN();
				lastrn = GetByID(guTemp);
			}
			trn.ENGINE_DATE = sessionInfo.Process.CurrentDate;
			
			if (strCtpy == "null")
				throw this.CreateException(new Exception(), "Please input counterparty.");
			else
				trn.CTPY_ID = Guid.Parse(strCtpy);

			if (String.IsNullOrEmpty(strTradeDate))
				throw this.CreateException(new Exception(), "Please input trade date.");
			else if (!DateTime.TryParseExact(strTradeDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dteTemp))
				throw this.CreateException(new Exception(), "Invalid trade date.");
			else
				trn.TRADE_DATE = DateTime.ParseExact(strTradeDate, "dd/MM/yyyy", null);

			if (String.IsNullOrEmpty(strSettlementDate))
				throw this.CreateException(new Exception(), "Please input settlement date.");
			else if (!DateTime.TryParseExact(strSettlementDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dteTemp))
				throw this.CreateException(new Exception(), "Invalid settlement date.");
			else
			{
				trn.MATURITY_DATE = DateTime.ParseExact(strSettlementDate, "dd/MM/yyyy", null);
				trn.START_DATE = trn.MATURITY_DATE;
			}

			if (strInstrument == "null")
				throw this.CreateException(new Exception(), "Please input instrument.");
			else
				trn.INSTRUMENT_ID = Guid.Parse(strInstrument);

			if (String.IsNullOrEmpty(strNotional))
				throw this.CreateException(new Exception(), "Please input notional amount.");
			else if (!Decimal.TryParse(strNotional, out decTemp))
				throw this.CreateException(new Exception(), "Invalid notional amount.");
			else if (Convert.ToDecimal(strNotional) <= 0)
				throw this.CreateException(new Exception(), "Invalid notional amount.");
			else
				trn.FIRST.NOTIONAL = Convert.ToDecimal(strNotional);
   
			if (strPortfolio == "-1")
				throw this.CreateException(new Exception(), "Please select portfolio.");
			else
				trn.PORTFOLIO_ID = Guid.Parse(strPortfolio);

			if (trn.MATURITY_DATE < trn.TRADE_DATE)
			{
				throw this.CreateException(new Exception(), "Settlement date cannot be before trade date.");
			}
            if ((strPurpose.Trim() == TBMA_PURPOSE.FIN.ToString() || strPurpose.Trim() == TBMA_PURPOSE.FINB.ToString() || strPurpose.Trim() == TBMA_PURPOSE.FINP.ToString()) && (strTBMARemark.Trim() == string.Empty || strRate.Trim() == string.Empty || strTerm.Trim() == string.Empty))
            {
				throw this.CreateException(new Exception(), "Please input Term, Rate and Remark.");
			}
			if (lastrn != null) {
				if (strRemark == "")
					throw this.CreateException(new Exception(), "Please input comment.");
				else trn.REMARK = strRemark;
			}
			trn.ID = Guid.NewGuid();
			trn.FLAG_SETTLE = strSettleFlag == "1" ? true : false;
			trn.FLAG_BUYSELL = strBuySell;
            trn.FIRST.CCY_ID = Guid.Parse(strCCY);
            trn.FIRST.RATE = Convert.ToDecimal(strYield);
			trn.PRODUCT_ID = _lookupBusiness.GetProductAll().FirstOrDefault(p => p.LABEL == ProductCode.BOND.ToString()).ID;
			trn.VERSION = lastrn == null ? 1 : lastrn.VERSION + 1;
			trn.SOURCE = SourceType.INT.ToString();
			trn.STATUS_ID = _lookupBusiness.GetStatusAll().FirstOrDefault(p => p.LABEL == StatusCode.OPEN.ToString()).ID;
			trn.INT_DEAL_NO = lastrn == null ? null : lastrn.INT_DEAL_NO;
			var spotrate = _lookupBusiness.GetSpotRateAll().AsQueryable();
			var a = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.FIRST.CCY_ID && p.PROC_DATE == trn.TRADE_DATE);
			if (a == null)           
			{
				var ccys = _lookupBusiness.GetCurrencyAll().FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID);
				throw this.CreateException(new Exception(), "Error : There is no " + ccys.LABEL + " spot rate on " + strTradeDate + " trade date.");
			}
			trn.NOTIONAL_THB = Math.Ceiling(trn.FIRST.NOTIONAL.Value * a.RATE);
            trn.FLAG_PCE = strPceFlag == "1" ? true : false;
			trn.LOG.INSERTDATE = DateTime.Now;
			trn.LOG.INSERTBYUSERID = sessionInfo.CurrentUserId;
			DA_TRN_CASHFLOW flow1 = new DA_TRN_CASHFLOW();
			flow1.ID = Guid.NewGuid();
			flow1.DA_TRN_ID = trn.ID;
			flow1.FLAG_FIRST = true;
			flow1.FLOW_DATE = trn.MATURITY_DATE;
			flow1.FLOW_AMOUNT = trn.FLAG_BUYSELL == "B" ? -trn.FIRST.NOTIONAL : trn.FIRST.NOTIONAL;
			flow1.FLOW_AMOUNT_THB = trn.FLAG_BUYSELL == "B" ? -trn.NOTIONAL_THB : trn.NOTIONAL_THB;
			flow1.LOG.INSERTDATE = trn.LOG.INSERTDATE;
			flow1.LOG.INSERTBYUSERID = trn.LOG.INSERTBYUSERID;
			trn.DA_TRN_FLOW.Add(flow1);

            DA_TMBA_EXTENSION tbma = new DA_TMBA_EXTENSION();
            tbma.ID = trn.ID;
            tbma.PURPOSE = strPurpose;
            tbma.YIELD_TYPE = strYeildType;
            tbma.CLEAN_PRICE = Convert.ToDecimal(strCleanPrice);
            tbma.GROSS_PRICE = Convert.ToDecimal(strGrossPrice);
            tbma.UNIT = Convert.ToInt32(strUnit);
            if (strTerm != string.Empty)
                tbma.TERM = Convert.ToInt32(strTerm);
            if (strRate != string.Empty)
                tbma.RATE = Convert.ToDecimal(strRate);
            if (strTBMARemark != string.Empty)
                tbma.REMARK = strTBMARemark;
            tbma.IS_REPORT_CLEAN = strReportBy == "0" ? true : false;

            if (lastrn != null)
            {
                tbma.SEND_DATE = lastrn.DA_TMBA_EXTENSION.SEND_DATE;
                tbma.SENDER_ID = lastrn.DA_TMBA_EXTENSION.SENDER_ID;
            }

            trn.DA_TMBA_EXTENSION = tbma;
			return trn;
		}

		#endregion

		#region Swap Product
		public string SubmitSwapDeal(SessionInfo sessioninfo
										, DA_TRN trn
										, string strOverApprover
										, string strOverComment
										, string strProductId)
		{            
			//trn.STATUS_ID = Guid.Parse("9161ed18-1298-44fa-ba7d-34522cb40d66");
			//trn.INT_DEAL_NO = GetNewDealNo(sessioninfo.Process.CurrentDate);

			//StaticDataBusiness _staticdataBusiness = new StaticDataBusiness();
			//trn.KK_PCCF = _staticdataBusiness.GetSwapKKPCCF(sessioninfo, trn);
			//trn.KK_CONTRIBUTE = trn.FIRST.NOTIONAL * trn.KK_PCCF / 100;
			//trn.BOT_PCCF = _staticdataBusiness.GetSwapBOTPCCF(sessioninfo, trn);
			//trn.BOT_CONTRIBUTE = trn.FIRST.NOTIONAL * trn.BOT_PCCF / 100;
			LookupBusiness _lookupBusiness = new LookupBusiness();
			if (!String.IsNullOrEmpty(strOverApprover))
			{
				trn.OVER_APPROVER = strOverApprover;
				trn.OVER_COMMENT = strOverComment;
			}

			using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			{
				Guid guTemp;
				if (Guid.TryParse(strProductId, out guTemp))
				{
					var foundDeal = unitOfWork.DA_TRNRepository.All().FirstOrDefault(p => p.ID == guTemp);
					if (foundDeal == null)
						throw this.CreateException(new Exception(), "Data not found!");
					else
					{
						foundDeal.STATUS_ID = _lookupBusiness.GetStatusAll().FirstOrDefault(p => p.LABEL == StatusCode.CANCELLED.ToString()).ID;
						foundDeal.REMARK = trn.REMARK;
					}
				}
				trn.REMARK = null;
				if (trn.INT_DEAL_NO == null)
					trn.INT_DEAL_NO = GetNewDealNo(sessioninfo.Process.CurrentDate);
				unitOfWork.DA_TRNRepository.Add(trn);
				unitOfWork.Commit();
			}
			return trn.INT_DEAL_NO;
		}

		public DA_TRN GenerateSwapTransactionObject(SessionInfo sessionInfo, string strTradeDate, string strInstrument, string strCtpy, string strPortfolio
															, string strEffDate, string strMatDate, string strNotional1
															, string strCCY1 , string strFFL1, string strFFix1, string strRate1, string strFreq1
															, string strNotional2, string strCCY2, string strFFL2, string strFFix2, string strRate2, string strFreq2
															, int intDaySpan,string strRemark, string strProductId)
		{
			LookupBusiness _lookupBusiness = new LookupBusiness();
			DA_TRN trn = new DA_TRN();
			DA_TRN lastrn = null;
			Guid guTemp;
			if (Guid.TryParse(strProductId, out guTemp))
			{
				lastrn = new DA_TRN();
				lastrn = GetByID(guTemp);
			}
			trn.ID = Guid.NewGuid();
			trn.ENGINE_DATE = sessionInfo.Process.CurrentDate;
			trn.TRADE_DATE = DateTime.ParseExact(strTradeDate, "dd/MM/yyyy", null);
			trn.INSTRUMENT_ID = Guid.Parse(strInstrument);
			trn.CTPY_ID = Guid.Parse(strCtpy);
			trn.PORTFOLIO_ID = Guid.Parse(strPortfolio);
			trn.START_DATE = DateTime.ParseExact(strEffDate, "dd/MM/yyyy", null);
			trn.MATURITY_DATE = DateTime.ParseExact(strMatDate, "dd/MM/yyyy", null);
			trn.FIRST.NOTIONAL = Convert.ToDecimal(strNotional1); 
			trn.SECOND.NOTIONAL  = Convert.ToDecimal(strNotional2);           
			//Fix Currency Type
			trn.FIRST.CCY_ID =  Guid.Parse(strCCY1);
			trn.SECOND.CCY_ID= Guid.Parse(strCCY2);
			trn.FIRST.FLAG_PAYREC = "P";
			trn.FIRST.FLAG_FIXED = strFFL1 == "1" ? true : false;

			if (!trn.FIRST.FLAG_FIXED.Value)
			{            
				trn.FIRST.FIRSTFIXINGAMT = Convert.ToDecimal(strFFix1);
			}
			trn.FIRST.RATE = Convert.ToDecimal(strRate1);
			trn.FIRST.FREQTYPE_ID = Guid.Parse(strFreq1);            
			trn.SECOND.FLAG_PAYREC = "R";
			trn.SECOND.FLAG_FIXED = strFFL2 == "1" ? true : false;

			if (!trn.SECOND.FLAG_FIXED.Value)
			{
				trn.SECOND.FIRSTFIXINGAMT = Convert.ToDecimal(strFFix2);
			}
			trn.SECOND.RATE = Convert.ToDecimal(strRate2);
			trn.SECOND.FREQTYPE_ID = Guid.Parse(strFreq2);      

			if (lastrn != null)
			{
				if (strRemark == "")
					throw this.CreateException(new Exception(), "Please input comment.");
				else trn.REMARK = strRemark;
			}
			trn.PRODUCT_ID = _lookupBusiness.GetProductAll().FirstOrDefault(p => p.LABEL == ProductCode.SWAP.ToString()).ID;
			trn.VERSION = lastrn == null ? 1 : lastrn.VERSION + 1;
			trn.SOURCE = SourceType.INT.ToString();
			trn.STATUS_ID = _lookupBusiness.GetStatusAll().FirstOrDefault(p => p.LABEL == StatusCode.OPEN.ToString()).ID;
			trn.INT_DEAL_NO = lastrn == null ? null : lastrn.INT_DEAL_NO;
            trn.FLAG_SETTLE = true;
			trn.LOG.INSERTDATE = DateTime.Now;
			trn.LOG.INSERTBYUSERID = sessionInfo.CurrentUserId;
			var ccys = _lookupBusiness.GetCurrencyAll().AsQueryable();
			var n1 = ccys.FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID && p.LABEL == "THB");
			var n2 = ccys.FirstOrDefault(p => p.ID == trn.SECOND.CCY_ID && p.LABEL == "THB");
			var spotrate = _lookupBusiness.GetSpotRateAll().AsQueryable();
			var firstRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.FIRST.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
			var secondRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.SECOND.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
			if (firstRate == null || secondRate == null)
			{
				var ccylabel = ccys.Where(p => (p.ID == trn.FIRST.CCY_ID && firstRate == null) || (p.ID == trn.SECOND.CCY_ID && secondRate == null)).Select(p => p.LABEL);
				var strCCY = string.Join(" and ",ccylabel);
				throw this.CreateException(new Exception(), "Error : There is no " + strCCY + " spot rate.");
			}              
			if (n1 != null)
				trn.NOTIONAL_THB = Math.Ceiling(trn.FIRST.NOTIONAL.Value);
			else if (n2 != null)
				trn.NOTIONAL_THB = Math.Ceiling(trn.SECOND.NOTIONAL.Value);
			else
			{
				if (Math.Ceiling(trn.FIRST.NOTIONAL.Value * firstRate.RATE) > Math.Ceiling(trn.SECOND.NOTIONAL.Value * secondRate.RATE))
					trn.NOTIONAL_THB = Math.Ceiling(trn.FIRST.NOTIONAL.Value * firstRate.RATE);
				else
					trn.NOTIONAL_THB = Math.Ceiling(trn.SECOND.NOTIONAL.Value * secondRate.RATE);                    
			}
			GenerateCashFlows(sessionInfo, trn, intDaySpan, firstRate.RATE, secondRate.RATE);

			return trn;
		}

		public void GenerateCashFlows(SessionInfo sessioninfo, DA_TRN trn, int intDaySpan, decimal firstRate, decimal secondRate)
		{
			DateTime dteActualFlow;
			DateTime dteLastActualFlow;
			DateTime dteEstFlow;
			int intCFSeq;
			decimal spotrate;
			//string strLegFreq;

			DA_TRN_CASHFLOW flow;

			LookupBusiness _lookupBusiness = new LookupBusiness();
			int addMonth = 1; //default for Month
            int addDay = 1;
			MA_FREQ_TYPE freq = null;
			InstrumentBusiness _insBusiness = new InstrumentBusiness();

			MA_INSTRUMENT ins = _insBusiness.GetByID(sessioninfo, trn.INSTRUMENT_ID.Value);

			for (int intLeg = 1; intLeg <= 2; intLeg++)
			{
				dteActualFlow = dteEstFlow = dteLastActualFlow = trn.START_DATE.Value.Date;
				
				//CCS : 4 priciple payment flows will be generated, 2 on initial payment date and 2 on maturity date
				if (ins.LABEL == "CCS")
				{
					//Initial Exchange
					flow = new DA_TRN_CASHFLOW();

                    flow.FLAG_FIRST = intLeg == 1 ? true : false;
                    flow.SEQ = 1;
                    flow.FLOW_DATE = trn.START_DATE;
                    flow.FLOW_AMOUNT = intLeg == 1 ? trn.FIRST.NOTIONAL : -trn.SECOND.NOTIONAL;
                    flow.FLOW_AMOUNT_THB = intLeg == 1 ? Math.Ceiling(flow.FLOW_AMOUNT.Value * firstRate) : Math.Ceiling(flow.FLOW_AMOUNT.Value * secondRate);
                    
					flow.ID = Guid.NewGuid();
					flow.DA_TRN_ID = trn.ID;
					flow.LOG.INSERTDATE = trn.LOG.INSERTDATE;
					flow.LOG.INSERTBYUSERID = trn.LOG.INSERTBYUSERID;
					trn.DA_TRN_FLOW.Add(flow);
				}

                intCFSeq = 2;

				freq = intLeg == 1 ? _lookupBusiness.GetFreqByID(trn.FIRST.FREQTYPE_ID.Value) : _lookupBusiness.GetFreqByID(trn.SECOND.FREQTYPE_ID.Value);

				do
				{
                    if ((FrequencyType)Enum.Parse(typeof(FrequencyType), freq.USERCODE) == FrequencyType.W)
                    {
                        dteActualFlow = dteActualFlow.AddDays(7);
                    }
                    else if ((FrequencyType)Enum.Parse(typeof(FrequencyType), freq.USERCODE) == FrequencyType.B)
                    {
                        dteActualFlow = trn.MATURITY_DATE.Value;
                    }
                    else if (((FrequencyType)Enum.Parse(typeof(FrequencyType), freq.USERCODE)).ToString().StartsWith("D"))
                    {
                        addDay = (int)Enum.Parse(typeof(FrequencyType), freq.USERCODE).GetHashCode();
                        dteActualFlow = dteActualFlow.AddDays(addDay);
                    }
                    else
                    {
                        addMonth = (int)Enum.Parse(typeof(FrequencyType), freq.USERCODE).GetHashCode();
						dteActualFlow = dteActualFlow.AddMonths(addMonth);
                    }
                    					
					for (int i = -intDaySpan; i <= intDaySpan; i++)
					{
						flow = new DA_TRN_CASHFLOW();

						dteEstFlow = dteActualFlow.AddDays(i);
                        
						flow.FLAG_FIRST = intLeg == 1 ? true : false;
						flow.SEQ = intCFSeq;

						if (intLeg == 1)
						{
							flow.RATE = trn.FIRST.FLAG_FIXED == true ? trn.FIRST.RATE : trn.FIRST.RATE + trn.FIRST.FIRSTFIXINGAMT;
							flow.FLOW_AMOUNT = trn.FIRST.NOTIONAL * flow.RATE / 100 * Convert.ToDecimal((dteEstFlow - dteLastActualFlow).TotalDays) / 365;
							spotrate = firstRate;
						}
						else
						{ 
							flow.RATE = trn.SECOND.FLAG_FIXED == true ? trn.SECOND.RATE : trn.SECOND.RATE + trn.SECOND.FIRSTFIXINGAMT;
							flow.FLOW_AMOUNT = trn.SECOND.NOTIONAL * flow.RATE / 100 * Convert.ToDecimal((dteEstFlow - dteLastActualFlow).TotalDays) / 365;
							spotrate = secondRate;
						}

						flow.FLOW_DATE = dteEstFlow;                        
						
						if (intLeg == 1) //flow amount is negative for pay leg
							flow.FLOW_AMOUNT = -flow.FLOW_AMOUNT;

						flow.FLOW_AMOUNT_THB = flow.FLOW_AMOUNT > 0 ? Math.Ceiling(flow.FLOW_AMOUNT.Value * spotrate) : Math.Floor(flow.FLOW_AMOUNT.Value * spotrate);

						flow.ID = Guid.NewGuid();
						flow.DA_TRN_ID = trn.ID;
						flow.LOG.INSERTDATE = trn.LOG.INSERTDATE;
						flow.LOG.INSERTBYUSERID = trn.LOG.INSERTBYUSERID;

						trn.DA_TRN_FLOW.Add(flow);
						intCFSeq += 1;
					}

					dteLastActualFlow = dteActualFlow;
				} while (dteActualFlow < trn.MATURITY_DATE.Value.Date);

                if (ins.LABEL == "CCS")
                {
                    //Maturity Exchange
                    flow = new DA_TRN_CASHFLOW();

                    flow.FLAG_FIRST = intLeg == 1 ?  true : false;
                    flow.SEQ = intCFSeq;
                    flow.FLOW_DATE = trn.MATURITY_DATE;
                    flow.FLOW_AMOUNT = intLeg == 1 ? -trn.FIRST.NOTIONAL : trn.SECOND.NOTIONAL;
                    flow.FLOW_AMOUNT_THB = intLeg == 1 ? Math.Ceiling(flow.FLOW_AMOUNT.Value * firstRate) : Math.Ceiling(flow.FLOW_AMOUNT.Value * secondRate);

                    flow.ID = Guid.NewGuid();
                    flow.DA_TRN_ID = trn.ID;
                    flow.LOG.INSERTDATE = trn.LOG.INSERTDATE;
                    flow.LOG.INSERTBYUSERID = trn.LOG.INSERTBYUSERID;
                    trn.DA_TRN_FLOW.Add(flow);
                }
			}
			
		}
		
		#endregion

		#region FX Spot
		 public DA_TRN GenerateFXSpotTransactionObject(SessionInfo sessionInfo, string strTradeDate, string strSpotDate, string strCtpy, string strPortfolio
														, string strCurrencyPair, string strBS, string strContractCcy, string strCounterCcy
                                                        , string strSpotRate, string strContractAmt, string strCounterAmt, string strRemark, bool settleFlag, string strProductId)
		{
			LookupBusiness _lookupBusiness = new LookupBusiness();
			DA_TRN trn = new DA_TRN();
			DA_TRN lastrn  = null;
			Guid guTemp;
			if (Guid.TryParse(strProductId, out guTemp)) {
				lastrn = new DA_TRN();
				lastrn = GetByID(guTemp);
			}
			trn.ID = Guid.NewGuid();
			trn.ENGINE_DATE = sessionInfo.Process.CurrentDate;         
			trn.TRADE_DATE = DateTime.ParseExact(strTradeDate, "dd/MM/yyyy", null);
			trn.START_DATE = trn.TRADE_DATE;
			trn.MATURITY_DATE = DateTime.ParseExact(strSpotDate, "dd/MM/yyyy", null);
			trn.CTPY_ID = Guid.Parse(strCtpy);
			trn.PORTFOLIO_ID = Guid.Parse(strPortfolio);
			trn.INSTRUMENT_ID = Guid.Parse(strCurrencyPair);
			trn.FIRST.CCY_ID = Guid.Parse(strContractCcy);
			trn.SECOND.CCY_ID = Guid.Parse(strCounterCcy);
			trn.FIRST.RATE = Convert.ToDecimal(strSpotRate);
			trn.FLAG_BUYSELL = strBS;
			trn.FIRST.NOTIONAL = trn.FLAG_BUYSELL == "B" ? Convert.ToDecimal(strContractAmt) : -Convert.ToDecimal(strContractAmt);
			trn.SECOND.NOTIONAL = trn.FLAG_BUYSELL == "B" ? -Convert.ToDecimal(strCounterAmt) : Convert.ToDecimal(strCounterAmt);
			trn.INT_DEAL_NO = lastrn == null ? null : lastrn.INT_DEAL_NO;
			trn.SOURCE = SourceType.INT.ToString();
			trn.VERSION = lastrn == null ? 1 : lastrn.VERSION + 1;
			trn.PRODUCT_ID = _lookupBusiness.GetProductAll().FirstOrDefault(p => p.LABEL.Replace(" ", string.Empty) == ProductCode.FXSPOT.ToString()).ID;
			trn.FIRST.FLAG_PAYREC = trn.FLAG_BUYSELL == "B" ? "R" : "P";
			trn.SECOND.FLAG_PAYREC = trn.FLAG_BUYSELL == "B" ? "P" : "R";
			trn.LOG.INSERTDATE = DateTime.Now;
			trn.LOG.INSERTBYUSERID = sessionInfo.CurrentUserId;
			trn.STATUS_ID = _lookupBusiness.GetStatusAll().FirstOrDefault(p => p.LABEL == StatusCode.OPEN.ToString()).ID;
            trn.FLAG_SETTLE = !settleFlag;
			trn.REMARK = strRemark;
			var ccys = _lookupBusiness.GetCurrencyAll().AsQueryable();
			var n1 = ccys.FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID  && p.LABEL == "THB");
			var n2 = ccys.FirstOrDefault(p => p.ID == trn.SECOND.CCY_ID  && p.LABEL == "THB");
			var spotrate = _lookupBusiness.GetSpotRateAll().AsQueryable();
			if (n1 != null)
				trn.NOTIONAL_THB = Math.Abs(trn.FIRST.NOTIONAL.Value);
			else if (n2 != null)
				trn.NOTIONAL_THB = Math.Abs(trn.SECOND.NOTIONAL.Value);
			else
			{
				var firstRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.FIRST.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
				var secondRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.SECOND.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);

				if (firstRate != null && secondRate != null)
				{
					if (Math.Abs(trn.FIRST.NOTIONAL.Value * firstRate.RATE) > Math.Abs(trn.SECOND.NOTIONAL.Value * secondRate.RATE))
						trn.NOTIONAL_THB = Math.Abs(trn.FIRST.NOTIONAL.Value * firstRate.RATE);
					else
						trn.NOTIONAL_THB = Math.Abs(trn.SECOND.NOTIONAL.Value * secondRate.RATE);
				}
				else
				{
					var ccylabel = ccys.FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID);
					throw this.CreateException(new Exception(), "Error : There is no " + ccylabel.LABEL + " spot rate.");
				}                
			 }

			//StaticDataBusiness _staticdataBusiness = new StaticDataBusiness();
			//trn.KK_PCCF = _staticdataBusiness.GetFXPCCF(sessionInfo, trn);
			//trn.KK_CONTRIBUTE = trn.NOTIONAL_THB.Value * trn.KK_PCCF / 100;

			DA_TRN_CASHFLOW flow1 = new DA_TRN_CASHFLOW();

			flow1.ID = Guid.NewGuid();
			flow1.DA_TRN_ID = trn.ID;
			flow1.FLAG_FIRST = true;
			flow1.RATE = trn.FIRST.RATE;
			flow1.FLOW_DATE = trn.MATURITY_DATE;
			flow1.FLOW_AMOUNT = trn.FIRST.NOTIONAL;
			if (n1 != null)
				flow1.FLOW_AMOUNT_THB = Math.Ceiling(trn.FIRST.NOTIONAL.Value);
			else
			{
				var a = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.FIRST.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
				if (a != null){
					flow1.FLOW_AMOUNT_THB = Math.Ceiling(trn.FIRST.NOTIONAL.Value * a.RATE);
				}
				else{
					var ccylabel = ccys.FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID);
					throw this.CreateException(new Exception(), "Error : There is no " + ccylabel.LABEL + " spot rate.");                
				}
				
			}
			flow1.LOG.INSERTDATE = trn.LOG.INSERTDATE;
			flow1.LOG.INSERTBYUSERID = trn.LOG.INSERTBYUSERID;

			DA_TRN_CASHFLOW flow2 = new DA_TRN_CASHFLOW();

			flow2.ID = Guid.NewGuid();
			flow2.DA_TRN_ID = trn.ID;
			flow2.FLAG_FIRST = false;
			flow2.FLOW_DATE = trn.MATURITY_DATE;
			flow2.FLOW_AMOUNT = trn.SECOND.NOTIONAL;
			if (n2 != null)
				flow2.FLOW_AMOUNT_THB = Math.Ceiling(trn.SECOND.NOTIONAL.Value);
			else
			{
				var a = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.SECOND.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
				if (a != null)
				{
					flow2.FLOW_AMOUNT_THB = Math.Ceiling(trn.SECOND.NOTIONAL.Value * a.RATE);
				}
				else
				{
					var ccylabel = ccys.FirstOrDefault(p => p.ID == trn.SECOND.CCY_ID);
					throw this.CreateException(new Exception(), "Error : There is no " + ccylabel.LABEL + " spot rate.");
				}        
			}

			flow2.LOG.INSERTDATE = trn.LOG.INSERTDATE;
			flow2.LOG.INSERTBYUSERID = trn.LOG.INSERTBYUSERID;

			trn.DA_TRN_FLOW.Add(flow1);
			trn.DA_TRN_FLOW.Add(flow2);
			return trn;
		}
		
		 public string SubmitFXDeal(SessionInfo sessioninfo
										 , DA_TRN trn
										 , string strOverApprover
										 , string strOverComment
										 , string strProductId)
		 {
			 StaticDataBusiness _staticdataBusiness = new StaticDataBusiness();
			 LookupBusiness _lookupBusiness = new LookupBusiness();
			 if (!String.IsNullOrEmpty(strOverApprover))
			 {
				 trn.OVER_APPROVER = strOverApprover;
				 trn.OVER_COMMENT = strOverComment;
			 }
		  
			 using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			 {
				 Guid guTemp;
				 if (Guid.TryParse(strProductId, out guTemp))
				 {
					 var foundDeal = unitOfWork.DA_TRNRepository.All().FirstOrDefault(p => p.ID == guTemp);
					 if (foundDeal == null)
						 throw this.CreateException(new Exception(), "Data not found!");
					 else
					 {
						 foundDeal.STATUS_ID = _lookupBusiness.GetStatusAll().FirstOrDefault(p => p.LABEL == StatusCode.CANCELLED.ToString()).ID;
						 foundDeal.REMARK = trn.REMARK;
					 }
				 }
				 trn.REMARK = null;
				 if (trn.INT_DEAL_NO == null)
					 trn.INT_DEAL_NO = GetNewDealNo(sessioninfo.Process.CurrentDate);
				 unitOfWork.DA_TRNRepository.Add(trn);
				 unitOfWork.Commit();
			 }
			 return trn.INT_DEAL_NO;
		 }
		#endregion

		#region FX Forword

		 public DA_TRN GenerateFXForwardTransactionObject(SessionInfo sessionInfo, string strTradeDate, string strSpotDate, string strSetDate, string strCtpy, string strPortfolio
																, string strCurrencyPair, string strBS, string strContractCcy, string strCounterCcy
                                                                , string strSpotRate, string strSwapPoint, string strContractAmt, string strCounterAmt, string strRemark, bool settleFlag, string strProductId)
		 {
			 LookupBusiness _lookupBusiness = new LookupBusiness();
			 DA_TRN trn = new DA_TRN();
			 DA_TRN lastrn = null;
			 Guid guTemp;
			 if (Guid.TryParse(strProductId, out guTemp))
			 {
				 lastrn = new DA_TRN();
				 lastrn = GetByID(guTemp);
			 }
			 trn.ID = Guid.NewGuid();
			 trn.ENGINE_DATE = sessionInfo.Process.CurrentDate;
			 trn.INT_DEAL_NO = lastrn == null ? null : lastrn.INT_DEAL_NO;
			 trn.SOURCE = SourceType.INT.ToString();
			 trn.VERSION = lastrn == null ? 1 : lastrn.VERSION + 1;
			 trn.PRODUCT_ID = _lookupBusiness.GetProductAll().FirstOrDefault(p => p.LABEL.Replace(" ", string.Empty) == ProductCode.FXFORWARD.ToString()).ID;
			 trn.CTPY_ID = Guid.Parse(strCtpy);          
			 trn.PORTFOLIO_ID = Guid.Parse(strPortfolio);
			 trn.INSTRUMENT_ID = Guid.Parse(strCurrencyPair);
			 trn.TRADE_DATE = DateTime.ParseExact(strTradeDate, "dd/MM/yyyy", null);
			 trn.SPOT_DATE = DateTime.ParseExact(strSpotDate, "dd/MM/yyyy", null);
			 trn.START_DATE = trn.SPOT_DATE;
			 trn.MATURITY_DATE = DateTime.ParseExact(strSetDate, "dd/MM/yyyy", null);
			 trn.FLAG_BUYSELL = strBS;
			 trn.FIRST.NOTIONAL = trn.FLAG_BUYSELL == "B" ? Convert.ToDecimal(strContractAmt) : -Convert.ToDecimal(strContractAmt);
			 trn.FIRST.CCY_ID = Guid.Parse(strContractCcy);
			 trn.FIRST.FLAG_PAYREC = trn.FLAG_BUYSELL == "B" ? "R" : "P";
			 trn.FIRST.RATE = Convert.ToDecimal(strSpotRate) + Convert.ToDecimal(strSwapPoint);
			 trn.FIRST.SWAP_POINT = Convert.ToDecimal(strSwapPoint);
			 trn.SECOND.NOTIONAL = trn.FLAG_BUYSELL == "B" ? -Convert.ToDecimal(strCounterAmt) : Convert.ToDecimal(strCounterAmt);
			 trn.SECOND.CCY_ID = Guid.Parse(strCounterCcy);
			 trn.SECOND.FLAG_PAYREC = trn.FLAG_BUYSELL == "B" ? "P" : "R";
			 trn.LOG.INSERTDATE = DateTime.Now;
			 trn.LOG.INSERTBYUSERID = sessionInfo.CurrentUserId;
			 trn.STATUS_ID = _lookupBusiness.GetStatusAll().FirstOrDefault(p => p.LABEL == StatusCode.OPEN.ToString()).ID;
             trn.FLAG_SETTLE = !settleFlag;
			 trn.REMARK = strRemark;
			 var ccys = _lookupBusiness.GetCurrencyAll().AsQueryable();
			 var n1 = ccys.FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID && p.LABEL == "THB");
			 var n2 = ccys.FirstOrDefault(p => p.ID == trn.SECOND.CCY_ID && p.LABEL == "THB");
			 var spotrate = _lookupBusiness.GetSpotRateAll().AsQueryable();
			 if (n1 != null)
				 trn.NOTIONAL_THB = Math.Abs(trn.FIRST.NOTIONAL.Value);
			 else if (n2 != null)
				 trn.NOTIONAL_THB = Math.Abs(trn.SECOND.NOTIONAL.Value);
			 else
			 {
				 var firstRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.FIRST.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
				 var secondRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.SECOND.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);

				 if (firstRate != null && secondRate != null)
				 {
					 if (Math.Abs(trn.FIRST.NOTIONAL.Value * firstRate.RATE) > Math.Abs(trn.SECOND.NOTIONAL.Value * secondRate.RATE))
						 trn.NOTIONAL_THB = Math.Abs(trn.FIRST.NOTIONAL.Value * firstRate.RATE);
					 else
						 trn.NOTIONAL_THB = Math.Abs(trn.SECOND.NOTIONAL.Value * secondRate.RATE);
				 }
				 else
				 {
					 var ccylabel = ccys.FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID);
					 throw this.CreateException(new Exception(), "Error : There is no " + ccylabel.LABEL + " spot rate.");
				 }
			 }

			 //StaticDataBusiness _staticdataBusiness = new StaticDataBusiness();
			 //trn.KK_PCCF = _staticdataBusiness.GetFXPCCF(sessionInfo, trn);
			 //trn.KK_CONTRIBUTE = Math.Abs(trn.NOTIONAL_THB.Value) * trn.KK_PCCF / 100;

			 DA_TRN_CASHFLOW flow1 = new DA_TRN_CASHFLOW();

			 flow1.ID = Guid.NewGuid();
			 flow1.DA_TRN_ID = trn.ID;
			 flow1.FLAG_FIRST = true;
			 flow1.RATE = trn.FIRST.RATE + trn.FIRST.SWAP_POINT;
			 flow1.FLOW_DATE = trn.MATURITY_DATE;
			 flow1.FLOW_AMOUNT = trn.FIRST.NOTIONAL;
			 if (n1 != null)
				 flow1.FLOW_AMOUNT_THB = Math.Ceiling(trn.FIRST.NOTIONAL.Value);
			 else
			 {
				 var a = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.FIRST.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
				 if (a != null)
				 {
					 flow1.FLOW_AMOUNT_THB = Math.Ceiling(trn.FIRST.NOTIONAL.Value * a.RATE);
				 }
				 else
				 {
					 var ccylabel = ccys.FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID);
					 throw this.CreateException(new Exception(), "Error : There is no " + ccylabel.LABEL + " spot rate.");
				 }

			 }
			 flow1.LOG.INSERTDATE = trn.LOG.INSERTDATE;
			 flow1.LOG.INSERTBYUSERID = trn.LOG.INSERTBYUSERID;

			 DA_TRN_CASHFLOW flow2 = new DA_TRN_CASHFLOW();

			 flow2.ID = Guid.NewGuid();
			 flow2.DA_TRN_ID = trn.ID;
			 flow2.FLAG_FIRST = false;
			 flow2.FLOW_DATE = trn.MATURITY_DATE;
			 flow2.FLOW_AMOUNT = trn.SECOND.NOTIONAL;
			 if (n2 != null)
				 flow2.FLOW_AMOUNT_THB = Math.Ceiling(trn.SECOND.NOTIONAL.Value);
			 else
			 {
				 var a = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.SECOND.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
				 if (a != null)
				 {
					 flow2.FLOW_AMOUNT_THB = Math.Ceiling(trn.SECOND.NOTIONAL.Value * a.RATE);
				 }
				 else
				 {
					 var ccylabel = ccys.FirstOrDefault(p => p.ID == trn.SECOND.CCY_ID);
					 throw this.CreateException(new Exception(), "Error : There is no " + ccylabel.LABEL + " spot rate.");
				 }
			 }

			 flow2.LOG.INSERTDATE = trn.LOG.INSERTDATE;
			 flow2.LOG.INSERTBYUSERID = trn.LOG.INSERTBYUSERID;


			 trn.DA_TRN_FLOW.Add(flow1);
			 trn.DA_TRN_FLOW.Add(flow2);
			 return trn;

		 }     
		 #endregion

		 #region FX Swap
		 public DA_TRN GenerateFXSwapTransactionObject1(SessionInfo sessionInfo, string strTradeDate, string strCtpy, string strPortfolio, string strCurrencyPair
																, string strContractCcy, string strCounterCcy, string strSpotRate
																, string strBSNear, string strSetDateNear, string strSwapPointNear
																, string strContractAmtNear, string strCounterAmtNear, string strSpotDate, string strRemark,bool settleFlag, string strProductId1)
		 {
			 LookupBusiness _lookupBusiness = new LookupBusiness();
			 DA_TRN trn = new DA_TRN();
			 DA_TRN lastrn = null;
			 Guid guTemp;
			 if (Guid.TryParse(strProductId1, out guTemp))
			 {
				 lastrn = new DA_TRN();
				 lastrn = GetByID(guTemp);
			 }
		  
			 trn.ID = Guid.NewGuid();
			 trn.ENGINE_DATE = sessionInfo.Process.CurrentDate;
			 trn.INT_DEAL_NO = lastrn == null ? null : lastrn.INT_DEAL_NO;
			 trn.SOURCE = SourceType.INT.ToString();
			 trn.VERSION = lastrn == null ? 1 : lastrn.VERSION + 1;
			 trn.PRODUCT_ID = _lookupBusiness.GetProductAll().FirstOrDefault(p => p.LABEL.Replace(" ", string.Empty) == ProductCode.FXSWAP.ToString()).ID;
			 trn.CTPY_ID = Guid.Parse(strCtpy);
			 trn.INSTRUMENT_ID = Guid.Parse(strCurrencyPair);
			 trn.PORTFOLIO_ID = Guid.Parse(strPortfolio);
			 trn.TRADE_DATE = DateTime.ParseExact(strTradeDate, "dd/MM/yyyy", null);
			 trn.SPOT_DATE = DateTime.ParseExact(strSpotDate, "dd/MM/yyyy", null);

			 //If settle after spot date (forward deal) then bucket is count from spot -> settlement date
			 //else (O/N or T/N deal) no need to count bucket so start date will be null
			 if (DateTime.ParseExact(strSetDateNear, "dd/MM/yyyy", null) > trn.SPOT_DATE)
				 trn.START_DATE = trn.SPOT_DATE;
			 else
				 trn.START_DATE = trn.TRADE_DATE;

			 trn.MATURITY_DATE = DateTime.ParseExact(strSetDateNear, "dd/MM/yyyy", null);
			 trn.FLAG_BUYSELL = strBSNear;
			 trn.FLAG_NEARFAR = "N";
			 trn.FIRST.NOTIONAL = trn.FLAG_BUYSELL == "B" ? Convert.ToDecimal(strContractAmtNear) : -Convert.ToDecimal(strContractAmtNear);
			 trn.FIRST.CCY_ID = Guid.Parse(strContractCcy);
			 trn.FIRST.FLAG_PAYREC = trn.FLAG_BUYSELL == "B" ? "R" : "P";
			 trn.FIRST.RATE = Convert.ToDecimal(strSpotRate) + Convert.ToDecimal(strSwapPointNear);
			 trn.FIRST.SWAP_POINT = Convert.ToDecimal(strSwapPointNear);
			 trn.SECOND.NOTIONAL = trn.FLAG_BUYSELL == "B" ? -Convert.ToDecimal(strCounterAmtNear) : Convert.ToDecimal(strCounterAmtNear);
			 trn.SECOND.CCY_ID = Guid.Parse(strCounterCcy);
			 trn.SECOND.FLAG_PAYREC = trn.FLAG_BUYSELL == "B" ? "P" : "R";
			 trn.LOG.INSERTDATE = DateTime.Now;
			 trn.LOG.INSERTBYUSERID = sessionInfo.CurrentUserId;
			 trn.STATUS_ID = _lookupBusiness.GetStatusAll().FirstOrDefault(p => p.LABEL == StatusCode.OPEN.ToString()).ID;
             trn.FLAG_SETTLE =!settleFlag;
			 trn.REMARK = strRemark;
			 var ccys = _lookupBusiness.GetCurrencyAll().AsQueryable();
			 var n1 = ccys.FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID && p.LABEL == "THB");
			 var n2 = ccys.FirstOrDefault(p => p.ID == trn.SECOND.CCY_ID && p.LABEL == "THB");
			 var spotrate = _lookupBusiness.GetSpotRateAll().AsQueryable();
			 if (n1 != null)
				 trn.NOTIONAL_THB = Math.Abs(trn.FIRST.NOTIONAL.Value);
			 else if (n2 != null)
				 trn.NOTIONAL_THB = Math.Abs(trn.SECOND.NOTIONAL.Value);
			 else
			 {
				 var firstRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.FIRST.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
				 var secondRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.SECOND.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);

				 if (firstRate != null && secondRate != null)
				 {
					 if (Math.Abs(trn.FIRST.NOTIONAL.Value * firstRate.RATE) > Math.Abs(trn.SECOND.NOTIONAL.Value * secondRate.RATE))
						 trn.NOTIONAL_THB = Math.Abs(trn.FIRST.NOTIONAL.Value * firstRate.RATE);
					 else
						 trn.NOTIONAL_THB = Math.Abs(trn.SECOND.NOTIONAL.Value * secondRate.RATE);
				 }
				 else
				 {
					 var ccylabel = ccys.FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID);
					 throw this.CreateException(new Exception(), "Error : There is no " + ccylabel.LABEL + " spot rate.");
				 }
			 }

			 //StaticDataBusiness _staticdataBusiness = new StaticDataBusiness();
			 //trn.KK_PCCF = _staticdataBusiness.GetFXPCCF(sessionInfo, trn);
			 //trn.KK_CONTRIBUTE = Math.Abs(trn.NOTIONAL_THB.Value) * trn.KK_PCCF / 100;

			 DA_TRN_CASHFLOW flow1 = new DA_TRN_CASHFLOW();

			 flow1.ID = Guid.NewGuid();
			 flow1.DA_TRN_ID = trn.ID;
			 flow1.FLAG_FIRST = true;
			 flow1.RATE = trn.FIRST.RATE + trn.FIRST.SWAP_POINT;
			 flow1.FLOW_DATE = trn.MATURITY_DATE;
			 flow1.FLOW_AMOUNT = trn.FIRST.NOTIONAL;
			 if (n1 != null)
				 flow1.FLOW_AMOUNT_THB = Math.Ceiling(trn.FIRST.NOTIONAL.Value);
			 else
			 {
				 var a = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.FIRST.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
				 if (a != null)
				 {
					 flow1.FLOW_AMOUNT_THB = Math.Ceiling(trn.FIRST.NOTIONAL.Value * a.RATE);
				 }
				 else
				 {
					 var ccylabel = ccys.FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID);
					 throw this.CreateException(new Exception(), "Error : There is no " + ccylabel.LABEL + " spot rate.");
				 }

			 }
			 flow1.LOG.INSERTDATE = trn.LOG.INSERTDATE;
			 flow1.LOG.INSERTBYUSERID = trn.LOG.INSERTBYUSERID;

			 DA_TRN_CASHFLOW flow2 = new DA_TRN_CASHFLOW();

			 flow2.ID = Guid.NewGuid();
			 flow2.DA_TRN_ID = trn.ID;
			 flow2.FLAG_FIRST = false;
			 flow2.FLOW_DATE = trn.MATURITY_DATE;
			 flow2.FLOW_AMOUNT = trn.SECOND.NOTIONAL;
			 if (n2 != null)
				 flow2.FLOW_AMOUNT_THB = Math.Ceiling(trn.SECOND.NOTIONAL.Value);
			 else
			 {
				 var a = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.SECOND.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
				 if (a != null)
				 {
					 flow2.FLOW_AMOUNT_THB = Math.Ceiling(trn.SECOND.NOTIONAL.Value * a.RATE);
				 }
				 else
				 {
					 var ccylabel = ccys.FirstOrDefault(p => p.ID == trn.SECOND.CCY_ID);
					 throw this.CreateException(new Exception(), "Error : There is no " + ccylabel.LABEL + " spot rate.");
				 }
			 }

			 flow2.LOG.INSERTDATE = trn.LOG.INSERTDATE;
			 flow2.LOG.INSERTBYUSERID = trn.LOG.INSERTBYUSERID;


			 trn.DA_TRN_FLOW.Add(flow1);
			 trn.DA_TRN_FLOW.Add(flow2);
			 return trn;
		 }

		 public DA_TRN GenerateFXSwapTransactionObject2(SessionInfo sessionInfo, string strTradeDate, string strCtpy, string strPortfolio, string strCurrencyPair
																, string strContractCcy, string strCounterCcy, string strSpotRate
																, string strBSFar, string strSetDateFar, string strSwapPointFar
                                                                , string strContractAmtFar, string strCounterAmtFar, string strSpotDate, string strRemark, bool settleFlag, int Version)
		 {
			 LookupBusiness _lookupBusiness = new LookupBusiness();
			 DA_TRN trn = new DA_TRN();

			 trn.ID = Guid.NewGuid();
			 trn.ENGINE_DATE = sessionInfo.Process.CurrentDate;
			 //trn.INT_DEAL_NO = strDealNo;
			 trn.SOURCE = SourceType.INT.ToString();
			 trn.VERSION = Version;
			 trn.PRODUCT_ID = _lookupBusiness.GetProductAll().FirstOrDefault(p => p.LABEL.Replace(" ", string.Empty) == ProductCode.FXSWAP.ToString()).ID;
			 trn.CTPY_ID = Guid.Parse(strCtpy);
			 trn.PORTFOLIO_ID = Guid.Parse(strPortfolio);
			 trn.INSTRUMENT_ID = Guid.Parse(strCurrencyPair);
			 trn.TRADE_DATE = DateTime.ParseExact(strTradeDate, "dd/MM/yyyy", null);
			 trn.SPOT_DATE = DateTime.ParseExact(strSpotDate, "dd/MM/yyyy", null);
			 
			 //If settle after spot date (forward deal) then bucket is count from spot -> settlement date
			 //else (O/N or T/N deal) no need to count bucket so start date will be null
			 if (DateTime.ParseExact(strSetDateFar, "dd/MM/yyyy", null) > trn.SPOT_DATE)
				 trn.START_DATE = trn.SPOT_DATE;
			 else
				 trn.START_DATE = trn.TRADE_DATE;

			 trn.MATURITY_DATE = DateTime.ParseExact(strSetDateFar, "dd/MM/yyyy", null);
			 trn.FLAG_BUYSELL = strBSFar;
			 trn.FLAG_NEARFAR = "F";
			 trn.FIRST.NOTIONAL = trn.FLAG_BUYSELL == "B" ? Convert.ToDecimal(strContractAmtFar) : -Convert.ToDecimal(strContractAmtFar);
			 trn.FIRST.CCY_ID = Guid.Parse(strContractCcy);
			 trn.FIRST.FLAG_PAYREC = trn.FLAG_BUYSELL == "B" ? "R" : "P";
			 trn.FIRST.RATE = Convert.ToDecimal(strSpotRate) + Convert.ToDecimal(strSwapPointFar);
			 trn.FIRST.SWAP_POINT = Convert.ToDecimal(strSwapPointFar);
			 trn.SECOND.NOTIONAL = trn.FLAG_BUYSELL == "B" ? -Convert.ToDecimal(strCounterAmtFar) : Convert.ToDecimal(strCounterAmtFar);
			 trn.SECOND.CCY_ID = Guid.Parse(strCounterCcy);
			 trn.SECOND.FLAG_PAYREC = trn.FLAG_BUYSELL == "B" ? "P" : "R";
			 trn.LOG.INSERTDATE = DateTime.Now;
			 trn.LOG.INSERTBYUSERID = sessionInfo.CurrentUserId;
			 trn.STATUS_ID = _lookupBusiness.GetStatusAll().FirstOrDefault(p => p.LABEL == StatusCode.OPEN.ToString()).ID;
             trn.FLAG_SETTLE = !settleFlag;
			 trn.REMARK = strRemark;
			 var ccys = _lookupBusiness.GetCurrencyAll().AsQueryable();
			 var n1 = ccys.FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID && p.LABEL == "THB");
			 var n2 = ccys.FirstOrDefault(p => p.ID == trn.SECOND.CCY_ID && p.LABEL == "THB");
			 var spotrate = _lookupBusiness.GetSpotRateAll().AsQueryable();
			 if (n1 != null)
				 trn.NOTIONAL_THB = Math.Abs(trn.FIRST.NOTIONAL.Value);
			 else if (n2 != null)
				 trn.NOTIONAL_THB = Math.Abs(trn.SECOND.NOTIONAL.Value);
			 else
			 {
				 var firstRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.FIRST.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
				 var secondRate = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.SECOND.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);

				 if (firstRate != null && secondRate != null)
				 {
					 if (Math.Abs(trn.FIRST.NOTIONAL.Value * firstRate.RATE) > Math.Abs(trn.SECOND.NOTIONAL.Value * secondRate.RATE))
						 trn.NOTIONAL_THB = Math.Abs(trn.FIRST.NOTIONAL.Value * firstRate.RATE);
					 else
						 trn.NOTIONAL_THB = Math.Abs(trn.SECOND.NOTIONAL.Value * secondRate.RATE);
				 }
				 else
				 {
					 var ccylabel = ccys.FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID);
					 throw this.CreateException(new Exception(), "Error : There is no " + ccylabel.LABEL + " spot rate.");
				 }
			 }

			 //StaticDataBusiness _staticdataBusiness = new StaticDataBusiness();
			 //trn.KK_PCCF = _staticdataBusiness.GetFXPCCF(sessionInfo, trn);
			 //trn.KK_CONTRIBUTE = Math.Abs(trn.NOTIONAL_THB.Value) * trn.KK_PCCF / 100;

			 DA_TRN_CASHFLOW flow1 = new DA_TRN_CASHFLOW();

			 flow1.ID = Guid.NewGuid();
			 flow1.DA_TRN_ID = trn.ID;
			 flow1.FLAG_FIRST = true;
			 flow1.RATE = trn.FIRST.RATE + trn.FIRST.SWAP_POINT;
			 flow1.FLOW_DATE = trn.MATURITY_DATE;
			 flow1.FLOW_AMOUNT = trn.FIRST.NOTIONAL;
			 if (n1 != null)
				 flow1.FLOW_AMOUNT_THB = Math.Ceiling(trn.FIRST.NOTIONAL.Value);
			 else
			 {
				 var a = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.FIRST.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
				 if (a != null)
				 {
					 flow1.FLOW_AMOUNT_THB = Math.Ceiling(trn.FIRST.NOTIONAL.Value * a.RATE);
				 }
				 else
				 {
					 var ccylabel = ccys.FirstOrDefault(p => p.ID == trn.FIRST.CCY_ID);
					 throw this.CreateException(new Exception(), "Error : There is no " + ccylabel.LABEL + " spot rate.");
				 }

			 }
			 flow1.LOG.INSERTDATE = trn.LOG.INSERTDATE;
			 flow1.LOG.INSERTBYUSERID = trn.LOG.INSERTBYUSERID;

			 DA_TRN_CASHFLOW flow2 = new DA_TRN_CASHFLOW();

			 flow2.ID = Guid.NewGuid();
			 flow2.DA_TRN_ID = trn.ID;
			 flow2.FLAG_FIRST = false;
			 flow2.FLOW_DATE = trn.MATURITY_DATE;
			 flow2.FLOW_AMOUNT = trn.SECOND.NOTIONAL;
			 if (n2 != null)
				 flow2.FLOW_AMOUNT_THB = Math.Ceiling(trn.SECOND.NOTIONAL.Value);
			 else
			 {
				 var a = spotrate.FirstOrDefault(p => p.CURRENCY_ID == trn.SECOND.CCY_ID && p.PROC_DATE == sessionInfo.Process.CurrentDate);
				 if (a != null)
				 {
					 flow2.FLOW_AMOUNT_THB = Math.Ceiling(trn.SECOND.NOTIONAL.Value * a.RATE);
				 }
				 else
				 {
					 var ccylabel = ccys.FirstOrDefault(p => p.ID == trn.SECOND.CCY_ID);
					 throw this.CreateException(new Exception(), "Error : There is no " + ccylabel.LABEL + " spot rate.");
				 }
			 }

			 flow2.LOG.INSERTDATE = trn.LOG.INSERTDATE;
			 flow2.LOG.INSERTBYUSERID = trn.LOG.INSERTBYUSERID;


			 trn.DA_TRN_FLOW.Add(flow1);
			 trn.DA_TRN_FLOW.Add(flow2);
			 return trn;
		 }         
		 #endregion

		 #region Repo Product
		 public DA_TRN GenerateRepoTransactionObject(SessionInfo sessionInfo
														   , string strTradeDate
														   , string strBuySell
														   , string strInstrument
														   , string strCtpy
														   , string strPortfolio
														   , string strEffectiveDate
														   , string strMaturityDate
														   , string strNotional
														   , string strProductId
														   , string strRemark)
		 {
			 //Validate business logics here
			 //throw error to UI if fail validation
			 LookupBusiness _lookupBusiness = new LookupBusiness();
			 DA_TRN trn = new DA_TRN();
			 DA_TRN lastrn = null;
			 Guid guTemp;
			 if (Guid.TryParse(strProductId, out guTemp))
			 {
				 lastrn = new DA_TRN();
				 lastrn = GetByID(guTemp);
			 }
			 trn.ID = Guid.NewGuid();
			 trn.ENGINE_DATE = sessionInfo.Process.CurrentDate;
			 trn.INT_DEAL_NO = lastrn == null ? null : lastrn.INT_DEAL_NO;
			 trn.VERSION = lastrn == null ? 1 : lastrn.VERSION + 1;
			 trn.PRODUCT_ID = _lookupBusiness.GetProductAll().FirstOrDefault(p => p.LABEL == ProductCode.REPO.ToString()).ID;
			 trn.SOURCE = SourceType.INT.ToString();
			 trn.CTPY_ID = Guid.Parse(strCtpy);
			 trn.INSTRUMENT_ID = Guid.Parse(strInstrument);
			 trn.PORTFOLIO_ID = Guid.Parse(strPortfolio);
			 trn.TRADE_DATE = DateTime.ParseExact(strTradeDate, "dd/MM/yyyy", null);
			 trn.START_DATE = DateTime.ParseExact(strEffectiveDate, "dd/MM/yyyy", null);     
			 trn.MATURITY_DATE = DateTime.ParseExact(strMaturityDate, "dd/MM/yyyy", null);
			 trn.FLAG_BUYSELL = strBuySell;
			 trn.FIRST.NOTIONAL = Convert.ToDecimal(strNotional);
			 trn.FIRST.CCY_ID = _lookupBusiness.GetCurrencyAll().FirstOrDefault(p => p.LABEL == "THB").ID;
			 trn.STATUS_ID = _lookupBusiness.GetStatusAll().FirstOrDefault(p => p.LABEL == StatusCode.OPEN.ToString()).ID;
			 trn.NOTIONAL_THB = Math.Ceiling(trn.FIRST.NOTIONAL.Value);
             trn.FLAG_SETTLE = false;

			 if (lastrn != null)
			 {
				 if (strRemark == "")
					 throw this.CreateException(new Exception(), "Please input comment.");
				 else trn.REMARK = strRemark;
			 }
			 trn.LOG.INSERTDATE = DateTime.Now;
			 trn.LOG.INSERTBYUSERID = sessionInfo.CurrentUserId;
			 return trn;
		 }


		 public string SubmitRepoDeal(SessionInfo sessioninfo
												 , DA_TRN trn
												 , string strOverApprover
												 , string strOverComment
												 , string strProductId)
		 {            
			 LookupBusiness _lookupBusiness = new LookupBusiness();
			 if (!String.IsNullOrEmpty(strOverApprover))
			 {
				 trn.OVER_APPROVER = strOverApprover;
				 trn.OVER_COMMENT = strOverComment;
			 }

			 using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			 {
				 Guid guTemp;
				 if (Guid.TryParse(strProductId, out guTemp))
				 {
					 var foundDeal = unitOfWork.DA_TRNRepository.All().FirstOrDefault(p => p.ID == guTemp);
					 if (foundDeal == null)
						 throw this.CreateException(new Exception(), "Data not found!");
					 else
					 {
						 foundDeal.STATUS_ID = _lookupBusiness.GetStatusAll().FirstOrDefault(p => p.LABEL == StatusCode.CANCELLED.ToString()).ID;
						 foundDeal.REMARK = trn.REMARK;
					 }
				 }
				 trn.REMARK = null;
				 if (trn.INT_DEAL_NO == null)
					 trn.INT_DEAL_NO = GetNewDealNo(sessioninfo.Process.CurrentDate);
				 unitOfWork.DA_TRNRepository.Add(trn);
				 unitOfWork.Commit();
			 }
			 return trn.INT_DEAL_NO;
		 }

		 #endregion

		#region Check Limit
		 //public LimitDisplayModel CheckPCE(SessionInfo sessioninfo, DA_TRN trn)
		 //{
		 //    StaticDataBusiness _staticDataBusiness = new StaticDataBusiness();
		 //    LimitCheckBusiness _limitCheckBusiness = new LimitCheckBusiness();
		 //    LimitDisplayModel limitDisplay = new LimitDisplayModel();

		 //    trn.KK_PCCF = _staticDataBusiness.GetPCCF(sessioninfo, trn);

		 //    if (trn.KK_PCCF != null)
		 //    {
		 //        trn.KK_CONTRIBUTE = Math.Ceiling(trn.NOTIONAL_THB.Value * trn.KK_PCCF.Value / 100);

		 //        List<LimitCheckModel> limits = _limitCheckBusiness.GetPCEByCriteria(sessioninfo.Process.CurrentDate, trn.CTPY_ID, trn.PRODUCT_ID.Value, "", Guid.Empty, Guid.Empty);

		 //        foreach (LimitCheckModel limit in limits)
		 //        {
		 //            limit.DEAL_CONTRIBUTION = trn.KK_CONTRIBUTE.Value;
		 //            SetLimitDisplayStatus(limit, ref limitDisplay);
		 //        }

		 //        limitDisplay.LimitDisplayObject = limits;
		 //    }
		 //    else
		 //    {
		 //        limitDisplay.LimitCheckStatus = eLimitCheckStatus.NOTALLOW;
		 //        limitDisplay.LimitErrorObj = new { Result = "ERROR", Message = "This deal breach allowed tenor." };
		 //    }

		 //    return limitDisplay;
		 //}

		public LimitDisplayModel CheckPCE(SessionInfo sessioninfo, DA_TRN trn, string strExcludeID)
		{
			StaticDataBusiness _staticDataBusiness = new StaticDataBusiness();
			LimitCheckBusiness _limitCheckBusiness = new LimitCheckBusiness();
			LimitDisplayModel limitDisplay = new LimitDisplayModel();
			Guid guExcludeID = Guid.Empty;

			trn.KK_PCCF = _staticDataBusiness.GetPCCF(sessioninfo, trn);

			//Find original deal for exclude it from limit calculation
			if (strExcludeID != null && Guid.TryParse(strExcludeID.Replace("\"", ""), out guExcludeID))
				guExcludeID = Guid.Parse(strExcludeID.Replace("\"", ""));

			if (trn.KK_PCCF != null)
			{
				trn.KK_CONTRIBUTE = Math.Ceiling(trn.NOTIONAL_THB.Value * trn.KK_PCCF.Value / 100);

                List<LimitCheckModel> limits = _limitCheckBusiness.CheckAllPCE(sessioninfo.Process.CurrentDate, trn, guExcludeID, Guid.Empty);

				foreach (LimitCheckModel limit in limits)
				{
					limit.DEAL_CONTRIBUTION = trn.KK_CONTRIBUTE.Value;
					SetLimitDisplayStatus(limit, sessioninfo.PCEOverwrite, ref limitDisplay);
				}

                if (limitDisplay.LimitCheckStatus == eLimitCheckStatus.NEEDAPPROVE)
                    trn.OVER_AMOUNT = limitDisplay.OverAmount;

				limitDisplay.LimitDisplayObject = limits;
			}
			else
			{
				limitDisplay.LimitCheckStatus = eLimitCheckStatus.ERROR;
                limitDisplay.Message = "This deal breach allowed tenor.";
			}

			return limitDisplay;
		}

		public LimitDisplayModel CheckSCE(SessionInfo sessioninfo, DA_TRN trn, string strExcludeID)
		{
			LimitCheckBusiness _limitCheckBusiness = new LimitCheckBusiness();
			LimitDisplayModel limitDisplay = new LimitDisplayModel();
			Guid guExcludeID = Guid.Empty;

            List<DA_TRN_CASHFLOW> deal_conts = (from f in trn.DA_TRN_FLOW
							                    where f.FLOW_AMOUNT_THB > 0
                                                select new DA_TRN_CASHFLOW
							                    {
								                    FLOW_DATE = f.FLOW_DATE.Value,
								                    FLOW_AMOUNT_THB = f.FLOW_AMOUNT_THB.Value
							                    }).ToList();

			//Find original deal for exclude it from limit calculation
			if (strExcludeID != null && Guid.TryParse(strExcludeID.Replace("\"", ""), out guExcludeID))
				guExcludeID = Guid.Parse(strExcludeID.Replace("\"", ""));

			List<LimitCheckModel> ori_conts = _limitCheckBusiness.CheckAllSET(sessioninfo.Process.CurrentDate, trn, guExcludeID, Guid.Empty);

            limitDisplay.LimitDisplayObject = GenerateSETDisplay(deal_conts, ori_conts, sessioninfo.SETOverwrite , ref limitDisplay);

            if (limitDisplay.LimitCheckStatus == eLimitCheckStatus.NEEDAPPROVE)
                trn.OVER_SETTL_AMOUNT = limitDisplay.OverAmount;

			return limitDisplay;
		}

		public LimitDisplayModel CheckFXSwapPCE(SessionInfo sessioninfo, DA_TRN trn1, DA_TRN trn2, string strExcludeID)
		{
			StaticDataBusiness _staticDataBusiness = new StaticDataBusiness();
			LimitCheckBusiness _limitCheckBusiness = new LimitCheckBusiness();
			LimitDisplayModel limitDisplay = new LimitDisplayModel();
			DA_TRN oldtrn1 = null;
			DA_TRN oldtrn2 = null;
			Guid guExcludeID1 = Guid.Empty;
			Guid guExcludeID2 = Guid.Empty;

			//Exposure include far leg only -> set near leg to 0
			trn1.KK_PCCF = 0;
			trn2.KK_PCCF = _staticDataBusiness.GetPCCF(sessioninfo, trn2);

			//Find original deals for exclude them from limit calculation
			if (strExcludeID != null && Guid.TryParse(strExcludeID.Replace("\"", ""), out guExcludeID1))
			{
				guExcludeID1 = Guid.Parse(strExcludeID.Replace("\"", ""));

				oldtrn1 = GetByID(guExcludeID1);
				oldtrn2 = GetDealByProcessDate(sessioninfo.Process.CurrentDate).FirstOrDefault(p => p.INT_DEAL_NO == oldtrn1.INT_DEAL_NO && p.VERSION == oldtrn1.VERSION && p.ID != oldtrn1.ID);
				
				if (oldtrn2 == null || oldtrn1 == null)
					throw this.CreateException(new Exception(), "Cannot find original deals.");

				guExcludeID1 = oldtrn1.ID;
				guExcludeID2 = oldtrn2.ID;
			}

			if (trn1.KK_PCCF != null && trn2.KK_PCCF != null)
			{
				trn1.KK_CONTRIBUTE = 0;
				trn2.KK_CONTRIBUTE = Math.Ceiling(trn2.NOTIONAL_THB.Value * trn2.KK_PCCF.Value / 100);

                List<LimitCheckModel> limits = _limitCheckBusiness.CheckAllPCE(sessioninfo.Process.CurrentDate, trn1, guExcludeID1, guExcludeID2);

				foreach (LimitCheckModel limit in limits)
				{
                    limit.DEAL_CONTRIBUTION = trn1.KK_CONTRIBUTE.Value + trn2.KK_CONTRIBUTE.Value;
					SetLimitDisplayStatus(limit, sessioninfo.PCEOverwrite, ref limitDisplay);
				}

                if (limitDisplay.LimitCheckStatus == eLimitCheckStatus.NEEDAPPROVE)
                {
                    trn1.OVER_AMOUNT = limitDisplay.OverAmount;
                    trn2.OVER_AMOUNT = limitDisplay.OverAmount;
                }

                limitDisplay.LimitDisplayObject = limits;
			}
			else
			{
                limitDisplay.LimitCheckStatus = eLimitCheckStatus.ERROR;
                limitDisplay.Message = "This deal breach allowed tenor.";
			}

			return limitDisplay;
		}

		public LimitDisplayModel CheckFXSwapSCE(SessionInfo sessioninfo, DA_TRN trn1, DA_TRN trn2, string strExcludeID)
		{
			LimitCheckBusiness _limitCheckBusiness = new LimitCheckBusiness();
			LimitDisplayModel limitDisplay = new LimitDisplayModel();
			DA_TRN oldtrn1 = null;
			DA_TRN oldtrn2 = null;
			Guid guExcludeID1 = Guid.Empty;
			Guid guExcludeID2 = Guid.Empty;

            List<DA_TRN_CASHFLOW> deal_conts = ((from f in trn1.DA_TRN_FLOW
							                   where f.FLOW_AMOUNT_THB > 0
                                                 select new DA_TRN_CASHFLOW
							                   {
								                   FLOW_DATE = f.FLOW_DATE.Value,
                                                   FLOW_AMOUNT_THB = f.FLOW_AMOUNT_THB.Value
							                   }).Union(
							                  (from f in trn2.DA_TRN_FLOW
							                   where f.FLOW_AMOUNT_THB > 0
                                               select new DA_TRN_CASHFLOW
							                   {
                                                   FLOW_DATE = f.FLOW_DATE.Value,
                                                   FLOW_AMOUNT_THB = f.FLOW_AMOUNT_THB.Value
							                   })
							                   )).ToList();

			//Find original deals for exclude them from limit calculation
			if (strExcludeID != null && Guid.TryParse(strExcludeID.Replace("\"", ""), out guExcludeID1))
			{
				guExcludeID1 = Guid.Parse(strExcludeID.Replace("\"", ""));

				oldtrn1 = GetByID(guExcludeID1);
				oldtrn2 = GetDealByProcessDate(sessioninfo.Process.CurrentDate).FirstOrDefault(p => p.INT_DEAL_NO == oldtrn1.INT_DEAL_NO && p.VERSION == oldtrn1.VERSION && p.ID != oldtrn1.ID);

				if (oldtrn2 == null || oldtrn1 == null)
					throw this.CreateException(new Exception(), "Cannot find original deals.");

				guExcludeID1 = oldtrn1.ID;
				guExcludeID2 = oldtrn2.ID;
			}

			List<LimitCheckModel> ori_conts = _limitCheckBusiness.CheckAllSET(sessioninfo.Process.CurrentDate, trn1, guExcludeID1, guExcludeID2);

            limitDisplay.LimitDisplayObject = GenerateSETDisplay(deal_conts, ori_conts, sessioninfo.SETOverwrite, ref limitDisplay);

            if (limitDisplay.LimitCheckStatus == eLimitCheckStatus.NEEDAPPROVE)
            {
                trn1.OVER_SETTL_AMOUNT = limitDisplay.OverAmount;
                trn2.OVER_SETTL_AMOUNT = limitDisplay.OverAmount;
            }

			return limitDisplay;
		}

		public LimitDisplayModel CheckSwapSCE(SessionInfo sessioninfo, DA_TRN trn, string strExcludeID)
		{
			LimitCheckBusiness _limitCheckBusiness = new LimitCheckBusiness();
			InstrumentBusiness _insBusiness = new InstrumentBusiness();
			LimitDisplayModel limitDisplay = new LimitDisplayModel();
			Guid guExcludeID = Guid.Empty;
			List<DA_TRN_CASHFLOW> deal_conts = new List<DA_TRN_CASHFLOW>();

			MA_INSTRUMENT ins = _insBusiness.GetByID(sessioninfo, trn.INSTRUMENT_ID.Value);

			if (ins.LABEL == "CCS")
			{
				deal_conts = (from f in trn.DA_TRN_FLOW
							  where f.FLOW_AMOUNT > 0 // Receive cash flows only
							  select new
							  {
								  Flow_Date = f.FLOW_DATE,
								  Flow_Amount_THB = f.FLOW_AMOUNT_THB
							  }).GroupBy(fl => new { fl.Flow_Date })
								.Select(p => new DA_TRN_CASHFLOW
								{
									FLOW_DATE = p.Key.Flow_Date,
									FLOW_AMOUNT_THB = p.Sum(x => x.Flow_Amount_THB)
								}).OrderBy(p => p.FLOW_DATE).ToList();
			}
			else //Net cashflow for IRS
			{
				deal_conts = (from f in trn.DA_TRN_FLOW
							  select new
							  {
								  Flow_Date = f.FLOW_DATE,
								  Flow_ccy = f.FLAG_FIRST ? trn.FIRST.CCY_ID : trn.SECOND.CCY_ID,
								  Flow_Amount_THB = f.FLOW_AMOUNT_THB
							  }).GroupBy(fl => new { fl.Flow_Date, fl.Flow_ccy })
								.Select(p => new DA_TRN_CASHFLOW
								{
									FLOW_DATE = p.Key.Flow_Date,
									FLOW_AMOUNT_THB = p.Sum(x => x.Flow_Amount_THB)
								}).Where(t => t.FLOW_AMOUNT_THB > 0).OrderBy(p => p.FLOW_DATE).ToList();
			}

			//Find original deal for exclude it from limit calculation
			if (strExcludeID != null && Guid.TryParse(strExcludeID.Replace("\"", ""), out guExcludeID))
				guExcludeID = Guid.Parse(strExcludeID.Replace("\"", ""));

            List<LimitCheckModel> ori_conts = _limitCheckBusiness.CheckAllSET(sessioninfo.Process.CurrentDate, trn, guExcludeID, Guid.Empty);

            limitDisplay.LimitDisplayObject = GenerateSETDisplay(deal_conts, ori_conts, sessioninfo.SETOverwrite, ref limitDisplay);

            if (limitDisplay.LimitCheckStatus == eLimitCheckStatus.NEEDAPPROVE)
                trn.OVER_SETTL_AMOUNT = limitDisplay.OverAmount;

			return limitDisplay;
		}
        
		#endregion

        #region CountryLimit
        public LimitDisplayModel CheckCountryLimit(SessionInfo sessioninfo, DA_TRN trn, string strExcludeID)
        {
            LimitCheckBusiness _limitCheckBusiness = new LimitCheckBusiness();
            LimitDisplayModel limitDisplay = new LimitDisplayModel();
            Guid guExcludeID = Guid.Empty;

            if (trn.KK_PCCF != null)
            {
                List<CountryLimitModel> deal_conts = new List<CountryLimitModel>();

                if (trn.FLAG_SETTLE.HasValue && trn.FLAG_SETTLE.Value)
                {
                    deal_conts = (from f in trn.DA_TRN_FLOW
                                  where f.FLOW_AMOUNT_THB > 0
                                  select new CountryLimitModel
                                  {
                                      EXPOSURE_DATE = f.FLOW_DATE.Value,
                                      EXPOSURE = f.FLOW_AMOUNT_THB.Value + trn.KK_CONTRIBUTE
                                  }).ToList();
                }

                if (trn.TRADE_DATE.Value != trn.MATURITY_DATE.Value)
                    deal_conts.Add(new CountryLimitModel { EXPOSURE_DATE = trn.TRADE_DATE.Value, EXPOSURE = trn.KK_CONTRIBUTE.Value });

                //Find original deal for exclude it from limit calculation
                if (strExcludeID != null && Guid.TryParse(strExcludeID.Replace("\"", ""), out guExcludeID))
                    guExcludeID = Guid.Parse(strExcludeID.Replace("\"", ""));

                List<LimitCheckModel> ori_conts = _limitCheckBusiness.CheckAllCountry(sessioninfo.Process.CurrentDate, trn, guExcludeID, Guid.Empty);

                limitDisplay.LimitDisplayObject = GenerateCountryLimitDisplay(deal_conts, ori_conts, trn, null, sessioninfo.CountryOverwrite, ref limitDisplay);

                if (limitDisplay.LimitCheckStatus == eLimitCheckStatus.NEEDAPPROVE)
                    trn.OVER_COUNTRY_AMOUNT = limitDisplay.OverAmount;
            }
            else
            {
                limitDisplay.LimitCheckStatus = eLimitCheckStatus.ERROR;
                limitDisplay.Message = "This deal breach allowed tenor.";
            }
            return limitDisplay;
        }

        public LimitDisplayModel CheckFXSwapCountryLimit(SessionInfo sessioninfo, DA_TRN trn1, DA_TRN trn2, string strExcludeID)
        {
            LimitCheckBusiness _limitCheckBusiness = new LimitCheckBusiness();
            LimitDisplayModel limitDisplay = new LimitDisplayModel();
            DA_TRN oldtrn1 = null;
            DA_TRN oldtrn2 = null;
            Guid guExcludeID1 = Guid.Empty;
            Guid guExcludeID2 = Guid.Empty;

            if (trn1.KK_PCCF != null && trn2.KK_PCCF != null)
            {

                List<CountryLimitModel> deal_conts = new List<CountryLimitModel>();

                if (trn1.FLAG_SETTLE.HasValue && trn1.FLAG_SETTLE.Value)
                {
                    deal_conts = (from f in trn1.DA_TRN_FLOW
                                  where f.FLOW_AMOUNT_THB > 0
                                  select new CountryLimitModel
                                  {
                                      EXPOSURE_DATE = f.FLOW_DATE.Value,
                                      EXPOSURE = f.FLOW_AMOUNT_THB.Value + trn1.KK_CONTRIBUTE
                                  }).Union(
                                  (from f in trn2.DA_TRN_FLOW
                                   where f.FLOW_AMOUNT_THB > 0
                                   select new CountryLimitModel
                                   {
                                       EXPOSURE_DATE = f.FLOW_DATE.Value,
                                       EXPOSURE = f.FLOW_AMOUNT_THB.Value + trn2.KK_CONTRIBUTE
                                   })).ToList();

                }

                if (trn1.TRADE_DATE.Value != trn1.MATURITY_DATE.Value)
                    deal_conts.Add(new CountryLimitModel { EXPOSURE_DATE = trn1.TRADE_DATE.Value, EXPOSURE = trn1.KK_CONTRIBUTE.Value + trn2.KK_CONTRIBUTE.Value });

                //Find original deals for exclude them from limit calculation
                if (strExcludeID != null && Guid.TryParse(strExcludeID.Replace("\"", ""), out guExcludeID1))
                {
                    guExcludeID1 = Guid.Parse(strExcludeID.Replace("\"", ""));

                    oldtrn1 = GetByID(guExcludeID1);
                    oldtrn2 = GetDealByProcessDate(sessioninfo.Process.CurrentDate).FirstOrDefault(p => p.INT_DEAL_NO == oldtrn1.INT_DEAL_NO && p.VERSION == oldtrn1.VERSION && p.ID != oldtrn1.ID);

                    if (oldtrn2 == null || oldtrn1 == null)
                        throw this.CreateException(new Exception(), "Cannot find original deals.");

                    guExcludeID1 = oldtrn1.ID;
                    guExcludeID2 = oldtrn2.ID;
                }

                List<LimitCheckModel> ori_conts = _limitCheckBusiness.CheckAllCountry(sessioninfo.Process.CurrentDate, trn2, guExcludeID1, guExcludeID2);

                limitDisplay.LimitDisplayObject = GenerateCountryLimitDisplay(deal_conts, ori_conts, trn1, trn2, sessioninfo.CountryOverwrite, ref limitDisplay);

                if (limitDisplay.LimitCheckStatus == eLimitCheckStatus.NEEDAPPROVE)
                {
                    trn1.OVER_COUNTRY_AMOUNT = limitDisplay.OverAmount;
                    trn2.OVER_COUNTRY_AMOUNT = limitDisplay.OverAmount;
                }
            }
            else
            {
                limitDisplay.LimitCheckStatus = eLimitCheckStatus.ERROR;
                limitDisplay.Message = "This deal breach allowed tenor.";
            }
            return limitDisplay;
        }

        public LimitDisplayModel CheckSwapCountryLimit(SessionInfo sessioninfo, DA_TRN trn, string strExcludeID)
        {
            LimitCheckBusiness _limitCheckBusiness = new LimitCheckBusiness();
            InstrumentBusiness _insBusiness = new InstrumentBusiness();
            LimitDisplayModel limitDisplay = new LimitDisplayModel();
            Guid guExcludeID = Guid.Empty;

            if (trn.KK_PCCF != null)
            {
                List<CountryLimitModel> deal_conts = new List<CountryLimitModel>();

                MA_INSTRUMENT ins = _insBusiness.GetByID(sessioninfo, trn.INSTRUMENT_ID.Value);

                if (ins.LABEL == "CCS")
                {
                    deal_conts = (from f in trn.DA_TRN_FLOW
                                  where f.FLOW_AMOUNT > 0 // Receive cash flows only
                                  select new
                                  {
                                      Flow_Date = f.FLOW_DATE,
                                      Flow_Amount_THB = f.FLOW_AMOUNT_THB
                                  }).GroupBy(fl => new { fl.Flow_Date })
                                    .Select(p => new CountryLimitModel
                                    {
                                        EXPOSURE_DATE = p.Key.Flow_Date,
                                        EXPOSURE = p.Sum(x => x.Flow_Amount_THB) + trn.KK_CONTRIBUTE.Value
                                    }).OrderBy(p => p.EXPOSURE_DATE).ToList();
                }
                else //Net cashflow for IRS
                {
                    deal_conts = (from f in trn.DA_TRN_FLOW
                                  select new
                                  {
                                      Flow_Date = f.FLOW_DATE,
                                      Flow_ccy = f.FLAG_FIRST ? trn.FIRST.CCY_ID : trn.SECOND.CCY_ID,
                                      Flow_Amount_THB = f.FLOW_AMOUNT_THB
                                  }).GroupBy(fl => new { fl.Flow_Date, fl.Flow_ccy })
                                    .Select(p => new CountryLimitModel
                                    {
                                        EXPOSURE_DATE = p.Key.Flow_Date,
                                        EXPOSURE = p.Sum(x => x.Flow_Amount_THB) + trn.KK_CONTRIBUTE.Value
                                    }).Where(t => t.EXPOSURE > 0).OrderBy(p => p.EXPOSURE_DATE).ToList();
                }

                if (trn.TRADE_DATE.Value != trn.MATURITY_DATE.Value)
                    deal_conts.Add(new CountryLimitModel { EXPOSURE_DATE = trn.TRADE_DATE.Value, EXPOSURE = trn.KK_CONTRIBUTE.Value });

                //Find original deal for exclude it from limit calculation
                if (strExcludeID != null && Guid.TryParse(strExcludeID.Replace("\"", ""), out guExcludeID))
                    guExcludeID = Guid.Parse(strExcludeID.Replace("\"", ""));

                List<LimitCheckModel> ori_conts = _limitCheckBusiness.CheckAllCountry(sessioninfo.Process.CurrentDate, trn, guExcludeID, Guid.Empty);

                limitDisplay.LimitDisplayObject = GenerateCountryLimitDisplay(deal_conts, ori_conts, trn, null, sessioninfo.CountryOverwrite, ref limitDisplay);

                if (limitDisplay.LimitCheckStatus == eLimitCheckStatus.NEEDAPPROVE)
                    trn.OVER_COUNTRY_AMOUNT = limitDisplay.OverAmount;
            }
            else
            {
                limitDisplay.LimitCheckStatus = eLimitCheckStatus.ERROR;
                limitDisplay.Message = "This deal breach allowed tenor.";
            }

            return limitDisplay;
        }
        #endregion

        #region Transaction Deal
        public DA_TRN Update(SessionInfo sessioninfo, DA_TRN trn)
		{
			using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
			{
				var foundData = unitOfWork.DA_TRNRepository.All().FirstOrDefault(p => p.ID == trn.ID);
				if (foundData == null)
					throw this.CreateException(new Exception(), "Data not found!");
				else
				{
					foundData.LOG.MODIFYBYUSERID = trn.LOG.MODIFYBYUSERID;
					foundData.LOG.MODIFYDATE = trn.LOG.MODIFYDATE;
					foundData.EXT_DEAL_NO = trn.EXT_DEAL_NO;
					foundData.EXT_PORTFOLIO = trn.EXT_PORTFOLIO;
					foundData.STATUS_ID = trn.STATUS_ID;

					unitOfWork.Commit();

				}
			}

			return trn;
		}
		#endregion

         public void UpdateFISendReport(SessionInfo sessioninfo ,Guid[] IDs)
         {
             using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
             {
                     foreach (Guid id in IDs)
                     {
                         var update = unitOfWork.DA_TMBA_EXTENSIONRepository.All().FirstOrDefault(p => p.ID == id);
                         if (update == null)
                             throw this.CreateException(new Exception(), "Data not found!");
                         else
                         {
                             update.SEND_DATE = DateTime.Now;
                             update.SENDER_ID = sessioninfo.CurrentUserId;
                         }

                     }
                     unitOfWork.Commit();
             }
         }

         public void CreatingTBMAReportFile(SessionInfo session, DA_TRN trn, MA_TBMA_CONFIG config)
         {
             string delimiter = "|";
             StringBuilder sb = new StringBuilder();
             string filePath = config.TBMA_RPT_PATH + (config.TBMA_RPT_PATH.EndsWith("\\") ? "" : "\\");

             if (!Directory.Exists(filePath))
                 Directory.CreateDirectory(filePath);

             filePath = filePath + config.TBMA_RPT_PREFIX + session.UserLogon + "-" + Guid.NewGuid().ToString().Replace("-","") + ".txt";
             
             sb.AppendLine(string.Join(delimiter, new string[]{"ORDER_NUM"
                                                                ,"TRADER_ID"
                                                                ,"PURPOSE"
                                                                ,"TRADE_TIME"
                                                                ,"TRADE_DATE"
                                                                ,"SETTLEMENT_DATE"
                                                                ,"TYPE","ISSUE_SYMBOL"
                                                                ,"YIELD","YIELD_TYPE"
                                                                ,"PRICE","VOLUME"
                                                                ,"COUNTER_PARTY"
                                                                ,"TERM"
                                                                ,"RATE"
                                                                ,"REMARK"}));

             sb.AppendLine(string.Join(delimiter, new string[]{"1"
                                                            , config.TBMA_RPT_TRADERID
                                                            , trn.DA_TMBA_EXTENSION.PURPOSE
                                                            , trn.LOG.INSERTDATE.ToString(FormatTemplate.TIMESTAMP_LABEL)
                                                            , trn.TRADE_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL)
                                                            , trn.MATURITY_DATE.Value.ToString(FormatTemplate.DATE_DMY_LABEL)
                                                            , trn.FLAG_BUYSELL
                                                            , trn.MA_INSRUMENT.LABEL
                                                            , trn.FIRST.RATE.Value.ToString("0.000000")
                                                            , trn.DA_TMBA_EXTENSION.YIELD_TYPE
                                                            , trn.DA_TMBA_EXTENSION.IS_REPORT_CLEAN ? trn.DA_TMBA_EXTENSION.CLEAN_PRICE.ToString() : trn.DA_TMBA_EXTENSION.GROSS_PRICE.ToString()
                                                            , trn.DA_TMBA_EXTENSION.UNIT.ToString()
                                                            , trn.MA_COUTERPARTY.TBMA_NAME
                                                            , trn.DA_TMBA_EXTENSION.TERM.ToString()
                                                            , trn.DA_TMBA_EXTENSION.RATE.ToString()
                                                            , trn.DA_TMBA_EXTENSION.REMARK }));

            File.AppendAllText(filePath, sb.ToString());

             if (!File.Exists(filePath))
                 throw this.CreateException(new Exception(), "Create file fail");
         }
	}
}
