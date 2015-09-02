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
using KK.DealMaker.Business.Log;
namespace KK.DealMaker.Business.Deal
{
    public class InstrumentBusiness : BaseBusiness 
    {
        public List<MA_INSTRUMENT> GetNoBondsByFilter(SessionInfo sessioninfo, string label, string product, string sorting)
        {
            try
            {
                IEnumerable<MA_INSTRUMENT> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_INSTRUMENTRepository.GetAll().Where(p => p.MA_PRODUCT.LABEL != ProductCode.BOND.ToString()).AsQueryable();
                    //Filters
                    if (!string.IsNullOrEmpty(label))
                    {
                        query = query.Where(p => p.LABEL.IndexOf(label, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    if (product != "-1")
                    {
                        query = query.Where(t => t.PRODUCT_ID == Guid.Parse(product) );
                    }
                    
                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_INSTRUMENT> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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

        public List<MA_INSTRUMENT> GetBondsByFilter(SessionInfo sessioninfo, string label, string sorting)
        {
            try
            {
                IEnumerable<MA_INSTRUMENT> sortedRecords;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    var query = unitOfWork.MA_INSTRUMENTRepository.GetAll().Where(p => p.MA_PRODUCT.LABEL.Equals(ProductCode.BOND.ToString())).AsQueryable();
                    //Filters
                    if (!string.IsNullOrEmpty(label))
                    {
                        query = query.Where(p => p.LABEL.IndexOf(label, StringComparison.OrdinalIgnoreCase) >= 0);
                    }

                    //Sorting
                    string[] sortsp = sorting.Split(' ');
                    IQueryable<MA_INSTRUMENT> orderedRecords = query.OrderBy(sortsp[0], sortsp[1]);
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

        public MA_INSTRUMENT GetFXInstrumentByCCY(SessionInfo sessioninfo, ProductCode eProductCode, string strCCY1, string strCCY2)
        {
            try
            {
                MA_INSTRUMENT instrument = null;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    instrument = unitOfWork.MA_INSTRUMENTRepository
                                        .GetAllByProductCode(eProductCode.ToString())
                                        .FirstOrDefault(p => p.LABEL.Equals(strCCY1 + "/" + strCCY2) || p.LABEL.Equals(strCCY2 + "/" + strCCY1))  ;
                }
                return instrument;
            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public MA_INSTRUMENT GetByLabel(SessionInfo sessioninfo, string strLabel)
        {
            try
            {
                MA_INSTRUMENT instrument = null;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    instrument = unitOfWork.MA_INSTRUMENTRepository.GetByLabel(strLabel);
                }

                //if (instruments.Count == 1)
                //{
                //    return instruments[0];
                //}
                //else
                //{
                //    throw this.CreateException(new Exception() , "Found multiple instruments with label " + strLabel);
                //}
                return instrument;
            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public MA_INSTRUMENT GetByID(SessionInfo sessioninfo, Guid guID)
        {
            try
            {
                MA_INSTRUMENT instrument = null;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    instrument = unitOfWork.MA_INSTRUMENTRepository.GetByID(guID);
                }

                //if (instruments.Count == 1)
                //{
                //    return instruments[0];
                //}
                //else
                //{
                //    throw this.CreateException(new Exception() , "Found multiple instruments with label " + strLabel);
                //}
                return instrument;
            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        //public List<MA_INSTRUMENT> GetByProduct(SessionInfo sessioninfo, Guid productID)
        //{
        //    try
        //    {
        //        List<MA_INSTRUMENT> instruments = null;
        //        using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
        //        {
        //            instruments = unitOfWork.MA_INSTRUMENTRepository.Where(p => p.PRODUCT_ID == productID).ToList();                    
        //        }
        //        //Return result to jTable
        //        return instruments;

        //    }
        //    catch (DataServicesException ex)
        //    {
        //        throw this.CreateException(ex, null);
        //    }
        //}
        public List<MA_INSTRUMENT> GetByProduct(SessionInfo sessioninfo, ProductCode productcode)
        {
            try
            {
                List<MA_INSTRUMENT> instruments = null;
                using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
                {
                    instruments = unitOfWork.MA_INSTRUMENTRepository.GetAllByProductCode(productcode.ToString());
                }
                //Return result to jTable
                return instruments;

            }
            catch (DataServicesException ex)
            {
                throw this.CreateException(ex, null);
            }
        }

        public List<MA_INSTRUMENT> GetInstrumentAll()
        {
            List<MA_INSTRUMENT> instrumentList;
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                instrumentList = unitOfWork.MA_INSTRUMENTRepository.GetAll().ToList();
            }
            return instrumentList;

        }

        public MA_INSTRUMENT Create(SessionInfo sessioninfo, MA_INSTRUMENT instrument, ProductCode product)
        {
            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_INSTRUMENTRepository.GetAllByProductCode(product.ToString()).FirstOrDefault(p => p.LABEL == instrument.LABEL);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), "Label is duplicated");
                if (product == ProductCode.BOND)
                {
                    LogBusiness logBusiness = new LogBusiness();
                    unitOfWork.DA_LOGGINGRepository.Add(logBusiness.CreateLogging(sessioninfo, instrument.ID, LogEvent.INSTRUMENT_AUDIT.ToString(), LookupFactorTables.MA_INSTRUMENT, "BOND", new { }));
                }
                unitOfWork.MA_INSTRUMENTRepository.Add(instrument);
                unitOfWork.Commit();
            }

            return instrument;
        }

        public MA_INSTRUMENT Update(SessionInfo sessioninfo, MA_INSTRUMENT instrument, ProductCode product)
        {

            using (EFUnitOfWork unitOfWork = new EFUnitOfWork())
            {
                var checkDuplicate = unitOfWork.MA_INSTRUMENTRepository.GetAllByProductCode(product.ToString()).FirstOrDefault(p => p.LABEL == instrument.LABEL && p.ID != instrument.ID);
                if (checkDuplicate != null)
                    throw this.CreateException(new Exception(), "Label is duplicated");
                var foundData = unitOfWork.MA_INSTRUMENTRepository.GetAll().FirstOrDefault(p => p.ID == instrument.ID);
                if (foundData == null)
                    throw this.CreateException(new Exception(), "Data not found!");
                else
                {
                    if (product == ProductCode.BOND) 
                    {
                        LogBusiness logBusiness = new LogBusiness();
                        var oldRecord = new 
                        { 
                                    ISACTIVE = foundData.ISACTIVE, 
                                    CAL_METHOD = foundData.CAL_METHOD,
                                    COUPON = foundData.COUPON,
                                    COUPON_FREQ_TYPE = foundData.MA_FREQ_TYPE!= null ?foundData.MA_PRODUCT.LABEL:string.Empty,
                                    FLAG_FIXED = foundData.FLAG_FIXED,
                                    INS_MKT = foundData.INS_MKT,
                                    ISSUER = foundData.ISSUER,
                                    LABEL = foundData.LABEL,
                                    LOT_SIZE = foundData.LOT_SIZE,
                                    MATURITY_DATE = foundData.MATURITY_DATE,
                                    PRODUCT = foundData.MA_PRODUCT!= null ? foundData.MA_PRODUCT.LABEL:string.Empty,
                                    CURRENCY = foundData.MA_CURRENCY != null ? foundData.MA_CURRENCY.LABEL:string.Empty
                        };
                        var newRecord = new
                        {
                                    ISACTIVE = instrument.ISACTIVE,
                                    CAL_METHOD = instrument.CAL_METHOD,
                                    COUPON = instrument.COUPON,
                                    COUPON_FREQ_TYPE =unitOfWork.MA_FREQ_TYPERepository.All().FirstOrDefault(f=>f.ID==instrument.COUPON_FREQ_TYPE_ID).LABEL,
                                    FLAG_FIXED = instrument.FLAG_FIXED,
                                    INS_MKT = instrument.INS_MKT,
                                    ISSUER = instrument.ISSUER,
                                    LABEL = instrument.LABEL,
                                    LOT_SIZE = instrument.LOT_SIZE,
                                    MATURITY_DATE = instrument.MATURITY_DATE,
                                    PRODUCT = unitOfWork.MA_PRODUCTRepository.All().FirstOrDefault(p => p.ID == instrument.PRODUCT_ID).LABEL,
                                    CURRENCY = unitOfWork.MA_CURRENCYRepository.All().FirstOrDefault(c => c.ID == instrument.CURRENCY_ID1).LABEL,
                        };
                        var log = logBusiness.UpdateLogging(sessioninfo, foundData.ID, LogEvent.INSTRUMENT_AUDIT.ToString(), LookupFactorTables.MA_INSTRUMENT, oldRecord, newRecord);
                        if (log != null) unitOfWork.DA_LOGGINGRepository.Add(log);
                    }
                    foundData.ID = instrument.ID;
                    foundData.ISACTIVE = instrument.ISACTIVE;
                    foundData.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                    foundData.LOG.MODIFYDATE = DateTime.Now;
                    foundData.CAL_METHOD = instrument.CAL_METHOD;
                    foundData.COUPON = instrument.COUPON;
                    foundData.COUPON_FREQ_TYPE_ID = instrument.COUPON_FREQ_TYPE_ID;
                    foundData.FLAG_FIXED = instrument.FLAG_FIXED;
                    foundData.INS_MKT = instrument.INS_MKT;
                    foundData.ISSUER = instrument.ISSUER;
                    foundData.LABEL = instrument.LABEL;
                    foundData.LOT_SIZE = instrument.LOT_SIZE;
                    foundData.MATURITY_DATE = instrument.MATURITY_DATE;
                    foundData.PRODUCT_ID = instrument.PRODUCT_ID;
                    foundData.CURRENCY_ID1 = instrument.CURRENCY_ID1;
                    foundData.CURRENCY_ID2 = instrument.CURRENCY_ID2;
                    unitOfWork.Commit();

                }
            }

            return instrument;
        }
    }
}
