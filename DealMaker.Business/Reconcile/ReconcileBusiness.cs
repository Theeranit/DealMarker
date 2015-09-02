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
using KK.DealMaker.Business.Deal;

namespace KK.DealMaker.Business.Reconcile
{
    public class ReconcileBusiness : BaseBusiness 
    {
        public void UpdateDealReconcile(SessionInfo sessioninfo, List<DealTranModel> trns)
        {
            DealBusiness _dealBusiness = new DealBusiness();
            LoggingHelper.Debug("Begin UpdateDealReconcile....");
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                foreach (DealTranModel tran in trns)
                {
                    switch (tran.UpdateStates)
                    { 
                        case UpdateStates.Adding:
                            unitOfWork.DA_TRNRepository.Add(tran.Transaction);
                            LoggingHelper.Debug(String.Format("Insert DA_TRN {0} [{1}] is completed", tran.Transaction.INT_DEAL_NO, tran.Transaction.ID.ToString()));
                            break;
                        case  UpdateStates.Editing:
                            var update = unitOfWork.DA_TRNRepository.All().FirstOrDefault(p => p.ID == tran.Transaction.ID);
                            if (update == null)
                                throw this.CreateException(new Exception(), "Data not found!");
                            else
                            {
                                update.EXT_DEAL_NO = tran.Transaction.EXT_DEAL_NO;
                                update.EXT_PORTFOLIO = tran.Transaction.EXT_PORTFOLIO;
                                update.MATURITY_DATE = tran.Transaction.MATURITY_DATE;
                                update.STATUS_ID = tran.Transaction.STATUS_ID;
                                update.LOG.MODIFYBYUSERID = tran.Transaction.LOG.MODIFYBYUSERID;
                                update.LOG.MODIFYDATE = tran.Transaction.LOG.MODIFYDATE;
                                update.INSERT_BY_EXT = tran.Transaction.INSERT_BY_EXT;

                            }
                            LoggingHelper.Debug(String.Format("Update DA_TRN {0} [{1}] is completed", update.INT_DEAL_NO, update.ID.ToString()));
                            break;
                        case UpdateStates.Deleting:
                            var delete = unitOfWork.DA_TRNRepository.All().FirstOrDefault(p => p.ID == tran.Transaction.ID);
                            if (delete == null)
                                throw this.CreateException(new Exception(), "Data not found!");
                            else
                                unitOfWork.DA_TRNRepository.Delete(delete);

                            LoggingHelper.Debug(String.Format("Delete DA_TRN {0} [{1}] is completed", delete.INT_DEAL_NO, delete.ID.ToString()));
                            break;
                    }
                }

                unitOfWork.Commit();

                LoggingHelper.Debug("Commit UpdateDealReconcile....");
            }
        }

        public void ImportCashflows(SessionInfo sessioninfo, List<DA_TRN_CASHFLOW> cashflows)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                foreach (DA_TRN_CASHFLOW cashflow in cashflows)
                {
                    unitOfWork.DA_TRN_CASHFLOWRepository.Add(cashflow);
                }

                unitOfWork.Commit();
            }
        }
    }
}
