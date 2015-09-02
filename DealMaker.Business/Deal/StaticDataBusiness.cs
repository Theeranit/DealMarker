using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.EnterpriseServices;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.Data;
using KK.DealMaker.DataAccess.Repositories;
using KK.DealMaker.Core.SystemFramework;
using System.Configuration;
using KK.DealMaker.Core.Helper;
using System.Linq.Expressions;
using KK.DealMaker.Business.Master;

namespace KK.DealMaker.Business.Deal
{
    public class StaticDataBusiness : BaseBusiness
    {
        #region PCCF
        public List<MA_PCCF> GetPCCFAll()
        {
            List<MA_PCCF> pccfList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                pccfList = unitOfWork.MA_PCCFRepository.GetAll();
            }
            return pccfList;

        }
        public List<MA_PCCF> GetPCCFByFilter(SessionInfo sessioninfo, string label, string sorting)
        {
            try
            {
                IEnumerable<MA_PCCF> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_PCCFRepository.GetAll().AsQueryable();

                    //Filters
                    if (!string.IsNullOrEmpty(label))
                    {
                        query = query.Where(p => p.LABEL.IndexOf(label, StringComparison.OrdinalIgnoreCase) >= 0);
                    }

                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_PCCF> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
                    sortedRecords = orderedRecords.ToList();
                }
                //Return result to jTable
                return sortedRecords.ToList();

            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public MA_PCCF GetPCCFByID(SessionInfo sessioninfo, Guid guID)
        {
            try
            {
                MA_PCCF pccf = null;

                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    pccf = unitOfWork.MA_PCCFRepository.GetAll().FirstOrDefault(p => p.ID == guID);
                }

                return pccf;
            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public MA_PCCF GetPCCFByLabel(SessionInfo sessioninfo, string strLabel)
        {
            try
            {
                MA_PCCF pccf = null;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    pccf = unitOfWork.MA_PCCFRepository.GetByLabel(strLabel);
                }
                return pccf;
            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public MA_PCCF GetPCCFByLabelProduct(SessionInfo sessioninfo, string strLabel, Guid GuProduct)
        {
            try
            {
                MA_PCCF pccf = null;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    pccf = unitOfWork.MA_PCCFRepository.GetByLabelProduct(strLabel, GuProduct);
                }
                return pccf;
            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }
                
        public MA_PCCF CreatePCCF(SessionInfo sessioninfo, MA_PCCF pccf)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_PCCFRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(pccf.LABEL.ToLower()));
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                unitOfWork.MA_PCCFRepository.Add(pccf);
                unitOfWork.Commit();
            }

            return pccf;
        }

        public void UpdatePCCF(SessionInfo sessioninfo, MA_PCCF pccf)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_PCCFRepository.GetAll().FirstOrDefault(p => p.LABEL.ToLower().Equals(pccf.LABEL.ToLower()) && p.ID != pccf.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), Messages.DUPLICATE_DATA);

                var foundPCCF = unitOfWork.MA_PCCFRepository.All().FirstOrDefault(p => p.ID == pccf.ID);
                if (foundPCCF == null)
                    throw this.CreateException(new Exception(), Messages.DATA_NOT_FOUND);
                else
                {                  
                    foundPCCF.LABEL = pccf.LABEL;
                    foundPCCF.C1 = pccf.C1;
                    foundPCCF.C2 = pccf.C2;
                    foundPCCF.C3 = pccf.C3;
                    foundPCCF.C4 = pccf.C4;
                    foundPCCF.C5 = pccf.C5;
                    foundPCCF.C6 = pccf.C6;
                    foundPCCF.C7 = pccf.C7;
                    foundPCCF.C8 = pccf.C8;
                    foundPCCF.C9 = pccf.C9;
                    foundPCCF.C10 = pccf.C10;
                    foundPCCF.C11 = pccf.C11;
                    foundPCCF.C12 = pccf.C12;
                    foundPCCF.C13 = pccf.C13;
                    foundPCCF.C14 = pccf.C14;
                    foundPCCF.C15 = pccf.C15;
                    foundPCCF.C16 = pccf.C16;
                    foundPCCF.C17 = pccf.C17;
                    foundPCCF.C18 = pccf.C18;
                    foundPCCF.C19 = pccf.C19;
                    foundPCCF.C20 = pccf.C20;
                    foundPCCF.C1D = pccf.C1D;
                    foundPCCF.C0D = pccf.C0D;
                    foundPCCF.C21 = pccf.C21;
                    foundPCCF.C22 = pccf.C22;
                    foundPCCF.C23 = pccf.C23;
                    foundPCCF.C24 = pccf.C24;
                    foundPCCF.DEFAULT = pccf.DEFAULT;
                    foundPCCF.more20 = pccf.more20;
                    foundPCCF.ISACTIVE = pccf.ISACTIVE;
                    foundPCCF.LOG.MODIFYBYUSERID = pccf.LOG.MODIFYBYUSERID;
                    foundPCCF.LOG.MODIFYDATE = pccf.LOG.MODIFYDATE;
                    unitOfWork.Commit();
                }
            }

        }        
      
        public decimal? GetPCCF(SessionInfo sessioninfo, DA_TRN trn)
        {
            try
            {
                decimal? decPCCF = null;
                LookupBusiness _lookupBusiness = new LookupBusiness();
                PCCFConfigBusiness _pccfBusiness = new PCCFConfigBusiness();

                MA_PRODUCT product = _lookupBusiness.GetProductByID(sessioninfo, trn.PRODUCT_ID.Value);
                ProductCode nProduct = (ProductCode)Enum.Parse(typeof(ProductCode), product.LABEL.Replace(" ", string.Empty));

                MA_PCCF pccf = _pccfBusiness.ValidatePCCFConfig(sessioninfo, trn);

                //if (pccf == null)
                //    throw this.CreateException(new Exception(), "Cannot find PCCF for transaction #" + trn.EXT_DEAL_NO);

                if (nProduct == ProductCode.BOND || nProduct == ProductCode.REPO)
                {
                    InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();

                    //------Check whether seleted bond can be used.---------
                    MA_INSTRUMENT ins = _instrumentBusiness.GetByID(sessioninfo, trn.INSTRUMENT_ID.Value);

                    if (trn.MATURITY_DATE >= ins.MATURITY_DATE)
                    {
                        throw this.CreateException(new Exception(), "Settlement date cannot be equal or after bond maturity date.");
                    }

                    if (pccf == null)
                        throw this.CreateException(new Exception(), "Selected instrument cannot be used.");
                    //-------------------------------------------------------

                    decPCCF = LimitHelper.GetPCCFValue(pccf, LimitHelper.GetYearBucket(trn.START_DATE.Value, ins.MATURITY_DATE.Value));
                }
                else
                {
                    if (pccf == null)
                        throw this.CreateException(new Exception(), "PCCF is not defined in the system." + "RF");

                    if (nProduct == ProductCode.SWAP)
                    {
                        decPCCF = LimitHelper.GetPCCFValue(pccf, LimitHelper.GetYearBucket(trn.START_DATE.Value, trn.MATURITY_DATE.Value));
                    }
                    else if (nProduct == ProductCode.FXSPOT)
                    {
                        decPCCF = pccf.DEFAULT;
                    }
                    else if (nProduct == ProductCode.FXFORWARD)
                    {
                        decPCCF = LimitHelper.GetPCCFValue(pccf, LimitHelper.GetMonthBucket(trn.START_DATE.Value, trn.MATURITY_DATE.Value));
                    }
                    else if (nProduct == ProductCode.FXSWAP)
                    {
                        if (trn.TRADE_DATE == trn.MATURITY_DATE)
                        {
                            decPCCF = pccf.C0D;
                        }
                        else if (trn.MATURITY_DATE < trn.SPOT_DATE)
                        {
                            decPCCF = pccf.C1D;
                        }
                        else if (trn.MATURITY_DATE == trn.SPOT_DATE)
                        {
                            decPCCF = pccf.DEFAULT;
                        }
                        else
                        {
                            decPCCF = LimitHelper.GetPCCFValue(pccf, LimitHelper.GetMonthBucket(trn.START_DATE.Value, trn.MATURITY_DATE.Value));
                        }
                    }
                }       
         
                return decPCCF;
            }
            catch (Exception ex)
            {
                throw this.CreateException(ex, null);
            }
        }
        #endregion

        #region CONFIG PCCF

        #endregion
    }
}
