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
    public class ProfileFunctionalUIP : BaseUIP
    {
        public static object GetProfileFunctionByFilter(SessionInfo sessioninfo, string strprofile, string strfunction, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                //Return result to jTable
                ProfileFunctionBusiness _profilefunctionbusiness = new ProfileFunctionBusiness();

                //Get data from database
                List<MA_PROFILE_FUNCTIONAL> PF = _profilefunctionbusiness.GetProfileFunctionByFilter(sessioninfo,strprofile ,strfunction, jtSorting);

                //Return result to jTable
                return new { Result = "OK",
                             Records = jtPageSize > 0 ? PF.Skip(jtStartIndex).Take(jtPageSize).ToList() : PF, 
                             TotalRecordCount = PF.Count };
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
        public static List<PermisionModel> GetPermissionByProfileID(Guid guProfileID)
        {
            ProfileFunctionBusiness _profilefunctionbusiness = new ProfileFunctionBusiness();
            return _profilefunctionbusiness.GetPermissionByProfileID(guProfileID);
        }

        public static object Create(SessionInfo sessioninfo, MA_PROFILE_FUNCTIONAL record)
        {
            try
            {
                ProfileFunctionBusiness _profilefunctionbusiness = new ProfileFunctionBusiness();
                record.ID = Guid.NewGuid();
                //record.ISREADABLE = record.ISREADABLE == null ? false : true;
                //record.ISWRITABLE = record.ISWRITABLE == null ? false : true;
                //record.ISAPPROVABLE = record.ISAPPROVABLE == null ? false : true;
                var added = _profilefunctionbusiness.CreateProfileFunction(sessioninfo, record);
                return new { Result = "OK", Record = added };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        public static object Update(SessionInfo sessioninfo, MA_PROFILE_FUNCTIONAL record)
        {
            try
            {
                ProfileFunctionBusiness _profilefunctionbusiness = new ProfileFunctionBusiness();
                //record.ISREADABLE = record.ISREADABLE == null ? false : true;
                //record.ISWRITABLE = record.ISWRITABLE == null ? false : true;
                //record.ISAPPROVABLE = record.ISAPPROVABLE == null ? false : true;
                var updated = _profilefunctionbusiness.UpdateProfileFunction(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object Delete(SessionInfo sessioninfo, Guid guID)
        {
            try
            {
                ProfileFunctionBusiness _profilefunctionbusiness = new ProfileFunctionBusiness();

                _profilefunctionbusiness.DeleteProfileFunction(sessioninfo, guID);

                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
    }
}
