using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Business.Master;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.Core.Helper;
namespace KK.DealMaker.UIProcessComponent.Admin
{
    public class LimitProductUIP : BaseUIP
    {
        public static object GetLimitproductByFilter(SessionInfo sessioninfo, string strproduct, string strlimit, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                //Return result to jTable
                LimitProductBusiness _limitproductbusiness = new LimitProductBusiness();

                //Get data from database
                List<MA_LIMIT_PRODUCT> limitproduct = _limitproductbusiness.GetLimitProductByFilter(sessioninfo, strproduct, strlimit, jtSorting);

                //Return result to jTable
                return new { Result = "OK",
                             Records = jtPageSize > 0 ? limitproduct.Skip(jtStartIndex).Take(jtPageSize).ToList() : limitproduct, 
                             TotalRecordCount = limitproduct.Count };
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
        public static object Create(SessionInfo sessioninfo, MA_LIMIT_PRODUCT record)
        {
            try
            {
                LimitProductBusiness _limitproductbusiness = new LimitProductBusiness();
                record.ID = Guid.NewGuid();
                record.LIMIT_ID = record.LIMIT_ID;
                record.PRODUCT_ID = record.PRODUCT_ID;
                var added = _limitproductbusiness.CreateLimitProduct(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object Update(SessionInfo sessioninfo, MA_LIMIT_PRODUCT record)
        {
            try
            {
                LimitProductBusiness _limitproductbusiness = new LimitProductBusiness();
                record.LIMIT_ID = record.LIMIT_ID;
                record.PRODUCT_ID = record.PRODUCT_ID;
                var added = _limitproductbusiness.UpdateLimitProduct(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

    }
}
