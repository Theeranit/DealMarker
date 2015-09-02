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
    public class ProfileUIP : BaseUIP 
    {
        public static object GetProfileOptions(SessionInfo sessioninfo)
        {
            try
            {
                ProfileBusiness _profileBusiness = new ProfileBusiness();
                //Get data from database
                var profiles = _profileBusiness.GetProfileOptions().Select(c => new { DisplayText = c.LABEL, Value = c.ID });
        
                //Return result to jTable
                return new { Result = "OK", Options = profiles };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetProfileByFilter(SessionInfo sessioninfo, string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                //Return result to jTable
                ProfileBusiness _profilebusiness = new ProfileBusiness();
                //Get data from database
                List<MA_USER_PROFILE> status = _profilebusiness.GetProfileByFilter(sessioninfo, name, jtSorting);

                //Return result to jTable
                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? status.Skip(jtStartIndex).Take(jtPageSize).ToList() : status, 
                             TotalRecordCount = status.Count };
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

        public static object CreateProfile(SessionInfo sessioninfo, MA_USER_PROFILE record)
        {
            try
            {
                ProfileBusiness _profileBusiness = new ProfileBusiness();
                record.ID = Guid.NewGuid();
                record.LABEL = record.LABEL;
                record.ISACTIVE = record.ISACTIVE;
                var addedStudent = _profileBusiness.CreateUserProfile(sessioninfo, record);
                return new { Result = "OK", Record = addedStudent };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object UpdateProfile(SessionInfo sessioninfo, MA_USER_PROFILE record)
        {
            try
            {
                ProfileBusiness _profileBusiness = new ProfileBusiness();
                record.ID = record.ID;
                record.LABEL = record.LABEL;
                record.ISACTIVE = record.ISACTIVE;
                var addedStudent = _profileBusiness.UpdateUserProfile(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
    }
}
