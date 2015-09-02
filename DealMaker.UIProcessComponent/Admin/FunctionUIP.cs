using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Business.Master;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.Core.Helper;

namespace KK.DealMaker.UIProcessComponent.Admin
{
    public class FunctionUIP : BaseUIP 
    {
        public static object GetFunctionOptions(SessionInfo sessioninfo)
        {
            try
            {
                FunctionBusiness _functionBusiness = new FunctionBusiness();
                //Get data from database
                var functions = _functionBusiness.GetFunctionOptions().OrderBy(p => p.LABEL).Select(c => new { DisplayText = c.LABEL, Value = c.ID });
        
                //Return result to jTable
                return new { Result = "OK", Options = functions };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetFunctionByFilter(SessionInfo sessioninfo, string code, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                //Return result to jTable
                FunctionBusiness _functionBusiness = new FunctionBusiness();
                //Get data from database
                List<MA_FUNCTIONAL> function = _functionBusiness.GetFunctionByFilter(sessioninfo, code, jtSorting);

                //Return result to jTable
                return new { Result = "OK"
                            , Records = jtPageSize > 0 ? function.Skip(jtStartIndex).Take(jtPageSize).ToList() : function
                            , TotalRecordCount = function.Count };
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

        public static object CreateFunction(SessionInfo sessioninfo, MA_FUNCTIONAL record)
        {
            try
            {
                FunctionBusiness _functionbusiness = new FunctionBusiness();
                record.ID = Guid.NewGuid();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE ? false : true;
                record.LABEL = record.LABEL;
                record.USERCODE = record.USERCODE;
                var added = _functionbusiness.CreateFunction(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object UpdateFunction(SessionInfo sessioninfo, MA_FUNCTIONAL record)
        {
            try
            {
                FunctionBusiness _functionBusiness = new FunctionBusiness();
                record.ID = record.ID;
                record.LABEL = record.LABEL;
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE ? false : true;
                record.USERCODE = record.USERCODE;
                var addedStudent = _functionBusiness.UpdateFunction(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        //public static object CreateFunction(MA_USER_PROFILE record)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
