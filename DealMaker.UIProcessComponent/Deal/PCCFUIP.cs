using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Business.Deal;
using KK.DealMaker.Business.Master;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.SystemFramework;

namespace KK.DealMaker.UIProcessComponent.Deal
{
    public class PCCFUIP
    {
        public static object GetPCCFOptions(SessionInfo sessioninfo)
        {
            try
            {
                StaticDataBusiness _staticBusiness = new StaticDataBusiness();
                //Get data from database
                var pccfs = _staticBusiness.GetPCCFAll().Where(t => t.ISACTIVE == true);
                
                //Return result to jTable
                return new { Result = "OK", Options = pccfs.OrderBy(t => t.LABEL).Select(c => new { DisplayText = c.LABEL, Value = c.ID }) };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetPCCFByFilter(SessionInfo sessioninfo, string label, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                StaticDataBusiness _staticBusiness = new StaticDataBusiness();
                
                //Get data from database
                List<MA_PCCF> pccfs = _staticBusiness.GetPCCFByFilter(sessioninfo, label, jtSorting);

                //Return result to jTable
                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? pccfs.Skip(jtStartIndex).Take(jtPageSize).ToList() : pccfs,
                             TotalRecordCount = pccfs.Count };
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

        public static object Create(SessionInfo sessioninfo, MA_PCCF record)
        {
            try
            {
                StaticDataBusiness _staticBusiness = new StaticDataBusiness();
                record.ID = Guid.NewGuid();

                //record.FLAG_MULTIPLY = record.FLAG_MULTIPLY == null || !record.FLAG_MULTIPLY.Value ? false : true;
                record.LABEL = record.LABEL.ToUpper();
                record.LOG.INSERTDATE = DateTime.Now;
                record.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
                var added = _staticBusiness.CreatePCCF(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object Update(SessionInfo sessioninfo, MA_PCCF record)
        {
            try
            {
                StaticDataBusiness _staticBusiness = new StaticDataBusiness();

                //record.FLAG_MULTIPLY = record.FLAG_MULTIPLY == null || !record.FLAG_MULTIPLY.Value ? false : true;
                record.LABEL = record.LABEL.ToUpper();
                record.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.MODIFYDATE = DateTime.Now;
                _staticBusiness.UpdatePCCF(sessioninfo, record);

                return new { Result = "OK", Record = record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        #region FX SPOT Product
        public static object GetPCCFFXSpotByLabel(SessionInfo sessioninfo, string CCYPairLabel)
        {
            StaticDataBusiness _staticBusiness = new StaticDataBusiness();
            LookupBusiness _lookupBusiness = new LookupBusiness();
            var productID = _lookupBusiness.GetProductAll().FirstOrDefault(p => p.LABEL.Replace(" ", string.Empty) == ProductCode.FXSPOT.ToString()).ID;
            MA_PCCF pccf = _staticBusiness.GetPCCFByLabelProduct(sessioninfo, CCYPairLabel, productID);
            if (pccf != null)
            {
                return null;// new { CURRENCY1 = pccf.MA_CURRENCY1.LABEL, CURRENCY2 = pccf.MA_CURRENCY2.LABEL, CURRENCYID1 = pccf.MA_CURRENCY1.ID, CURRENCYID2 = pccf.MA_CURRENCY2.ID, FLAG_MULTIPLY = pccf.FLAG_MULTIPLY };
            }
            else {
                throw new Exception("Invalid currency pair.");
            }
        }
        #endregion

        #region FX FORWARD Product
        public static object GetPCCFFXForwardByLabel(SessionInfo sessioninfo, string CCYPairLabel)
        {
            StaticDataBusiness _staticBusiness = new StaticDataBusiness();
            LookupBusiness _lookupBusiness = new LookupBusiness();
            var productID = _lookupBusiness.GetProductAll().FirstOrDefault(p => p.LABEL.Replace(" ", string.Empty) == ProductCode.FXFORWARD.ToString()).ID;
            MA_PCCF pccf = _staticBusiness.GetPCCFByLabelProduct(sessioninfo, CCYPairLabel, productID);
            if (pccf != null)
            {
                return null;// new { CURRENCY1 = pccf.MA_CURRENCY1.LABEL, CURRENCY2 = pccf.MA_CURRENCY2.LABEL, CURRENCYID1 = pccf.MA_CURRENCY1.ID, CURRENCYID2 = pccf.MA_CURRENCY2.ID, FLAG_MULTIPLY = pccf.FLAG_MULTIPLY };
            }
            else
            {
                throw new Exception("Invalid currency pair.");
            }
        }
        #endregion

        #region FX SWAP Product
        public static object GetPCCFFXSwapByLabel(SessionInfo sessioninfo, string CCYPairLabel)
        {
            StaticDataBusiness _staticBusiness = new StaticDataBusiness();
            LookupBusiness _lookupBusiness = new LookupBusiness();
            var productID = _lookupBusiness.GetProductAll().FirstOrDefault(p => p.LABEL.Replace(" ", string.Empty) == ProductCode.FXSWAP.ToString()).ID;
            MA_PCCF pccf = _staticBusiness.GetPCCFByLabelProduct(sessioninfo, CCYPairLabel, productID);
            if (pccf != null)
            {
                return null;// new { CURRENCY1 = pccf.MA_CURRENCY1.LABEL, CURRENCY2 = pccf.MA_CURRENCY2.LABEL, CURRENCYID1 = pccf.MA_CURRENCY1.ID, CURRENCYID2 = pccf.MA_CURRENCY2.ID, FLAG_MULTIPLY = pccf.FLAG_MULTIPLY };
            }
            else
            {
                throw new Exception("Invalid currency pair.");
            }
        }
        #endregion
    }
}
