using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Business.Deal;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.UIProcessComponent.Common;

namespace KK.DealMaker.UIProcessComponent.Deal
{
    public class InstrumentUIP : BaseUIP
    {
        public static object GetNoBondsByFilter(SessionInfo sessioninfo, string label, string product, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();
                //Get data from database
                List<MA_INSTRUMENT> ins = _instrumentBusiness.GetNoBondsByFilter(sessioninfo, label, product, jtSorting);

                //Return result to jTable
                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? ins.Skip(jtStartIndex).Take(jtPageSize).ToList() : ins, 
                             TotalRecordCount = ins.Count };
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

        public static object GetBondsByFilter(SessionInfo sessioninfo, string label, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();
                //Get data from database
                List<MA_INSTRUMENT> ins = _instrumentBusiness.GetBondsByFilter(sessioninfo, label, jtSorting);

                //Return result to jTable
                return new
                {
                    Result = "OK",
                    Records = jtPageSize > 0 ? ins.Skip(jtStartIndex).Take(jtPageSize).ToList() : ins,
                    TotalRecordCount = ins.Count
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
        
        public static object GetOptionsByProduct(SessionInfo sessioninfo, ProductCode productcode)
        {
            try
            {
                InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();
                //Get data from database
                var ins = _instrumentBusiness.GetByProduct(sessioninfo, productcode)
                                .OrderBy(p => p.LABEL)
                                .Select(c => new { DisplayText = c.LABEL, Value = c.ID });

                //Return result to jTable
                return new { Result = "OK", Options = ins };
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
        public static object GetInstrumentByName(SessionInfo sessioninfo, ProductCode productcode,string name)
        {
            try
            {
                InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();
                //Get data from database
                var ins = _instrumentBusiness.GetByProduct(sessioninfo, productcode).Where(c => c.ISACTIVE == true && c.LABEL.StartsWith(name)).OrderBy(c => c.LABEL);

                //Return result to jTable
                return new { Result = "OK", Records = ins };
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

        public static object GetInstrumentOptions(SessionInfo sessioninfo)
        {
            try
            {
                InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();
                //Get data from database
                var instruments = _instrumentBusiness.GetInstrumentAll()
                                        .Where(t => t.ISACTIVE == true)
                                        .OrderBy(p => p.LABEL)
                                        .Select(c => new { DisplayText = c.LABEL, Value = c.ID });

                //Return result to jTable
                return new { Result = "OK", Options = instruments };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }


        public static MA_INSTRUMENT GetByID(SessionInfo sessioninfo, Guid id )
        {
                InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();
                var ins = _instrumentBusiness.GetByID(sessioninfo, id);
                return ins;
        }


        public static object Create(SessionInfo sessioninfo, MA_INSTRUMENT record)
        {
            try
            {
                InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();
                ILookupValuesRepository _lookupvaluesRepository = RepositorySesssion.GetRepository();
                MA_PRODUCT product = _lookupvaluesRepository.ProductRepository.GetByID(record.PRODUCT_ID);

                ProductCode eProduct = (ProductCode)Enum.Parse(typeof(ProductCode), product.LABEL.Replace(" ", string.Empty));

                record.ID = Guid.NewGuid();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE ? false : true;
                record.LABEL = record.LABEL.ToUpper();

                if (eProduct != ProductCode.BOND)
                {
                    record.INS_MKT = null;
                    record.ISSUER = null;
                    record.LOT_SIZE = null;
                    record.COUPON = null;
                    record.MATURITY_DATE = null;
                    record.CAL_METHOD = null;
                    record.FLAG_FIXED = null;
                    record.COUPON_FREQ_TYPE_ID = null;
                }

                record.LOG.INSERTDATE = DateTime.Now;
                record.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
                var addedRecord = _instrumentBusiness.Create(sessioninfo, record, eProduct);
                return new { Result = "OK", Record = addedRecord };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        //public static object CreateBondInstruments(SessionInfo sessioninfo, MA_INSTRUMENT record)
        //{
        //    try
        //    {
        //        InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();
        //        ILookupValuesRepository _lookupvaluesRepository = RepositorySesssion.GetRepository();

        //        record.ID = Guid.NewGuid();
        //        record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE ? false : true;
        //        record.PRODUCT_ID = _lookupvaluesRepository.ProductRepository.GetByLabel(ProductCode.BOND.ToString()).ID;
        //        record.LOG.INSERTDATE = DateTime.Now;
        //        record.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;

        //        var addedRecord = _instrumentBusiness.Create(sessioninfo, record);
        //        return new { Result = "OK", Record = addedRecord };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new { Result = "ERROR", Message = ex.Message };
        //    }
        //}

        public static object Update(SessionInfo sessioninfo, MA_INSTRUMENT record)
        {
            try
            {
                InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();
                ILookupValuesRepository _lookupvaluesRepository = RepositorySesssion.GetRepository();
                MA_PRODUCT product = _lookupvaluesRepository.ProductRepository.GetByID(record.PRODUCT_ID);

                ProductCode eProduct = (ProductCode)Enum.Parse(typeof(ProductCode), product.LABEL.Replace(" ", string.Empty));

                record.LABEL = record.LABEL.ToUpper();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE ? false : true;
                record.FLAG_FIXED = record.FLAG_FIXED == null || !record.FLAG_FIXED.Value ? false : true;

                if (eProduct != ProductCode.BOND)
                {
                    record.INS_MKT = null;
                    record.ISSUER = null;
                    record.LOT_SIZE = null;
                    record.COUPON = null;
                    record.MATURITY_DATE = null;
                    record.CAL_METHOD = null;
                    record.FLAG_FIXED = null;
                    record.COUPON_FREQ_TYPE_ID = null;
                }

                record.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.MODIFYDATE = DateTime.Now;
                var addedRecord = _instrumentBusiness.Update(sessioninfo, record, eProduct);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        //public static object UpdateBondInstruments(SessionInfo sessioninfo, MA_INSTRUMENT record)
        //{
        //    try
        //    {
        //        InstrumentBusiness _instrumentBusiness = new InstrumentBusiness();

        //        record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE ? false : true;
                
        //        record.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
        //        record.LOG.MODIFYDATE = DateTime.Now;
        //        var addedRecord = _instrumentBusiness.Update(sessioninfo, record);
        //        return new { Result = "OK" };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new { Result = "ERROR", Message = ex.Message };
        //    }
        //}
    }
}
