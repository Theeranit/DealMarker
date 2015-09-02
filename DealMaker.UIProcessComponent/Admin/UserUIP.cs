using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Business.Master;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.Core.Helper;

namespace KK.DealMaker.UIProcessComponent.Admin
{
    public class UserUIP : BaseUIP
    {
        #region Logon and Logout
        //public static SessionInfo LogOn(string username)
        //{
        //    SessionInfo _session = new SessionInfo();
        //    if (username.ToUpper().Equals("ADMINISTRATOR"))
        //    { 
        //        UserBusiness _userBusiness = new UserBusiness();
        //        _session = _userBusiness.LogOn(username);
        //    }

        //    return _session;
        //}

        public static SessionInfo LogOn(string username, string password, string userIP, int intADLogin)
        {
            UserBusiness _userBusiness = new UserBusiness();
            SessionInfo _session = null;
            //string currentpassword = DecodeJSPassEncoding(password);
            string currentpassword = password.Trim();
            try
            {

                if (username.ToUpper().Equals("ADMINISTRATOR"))
                {
                    string systempassword = DecodeJSPassEncoding((string)ConfigurationSettings.AppSettings[AppSettingName.SYSTEM_KEY]);
                    if (systempassword.Equals(currentpassword))
                        _session = _userBusiness.LogOn(username, userIP);
                    else
                        throw new Exception("That password is incorrect. Be sure you're using the password for your Deal Maker account.");

                    _session.IsSystemUser = true;
                }
                else
                {
                    //log on via AD user
                    bool validUser = false;
                    if (intADLogin == 1)
                        validUser = LDAPHelper.ValidateUser(username, currentpassword);
                    else
                        validUser = _userBusiness.GetAll().FirstOrDefault(p => p.USERCODE.ToLower().Equals(username.ToLower())) != null ? true : false;
                    
                    if (validUser)
                    {
                        _session = _userBusiness.LogOn(username, userIP);
                        _session.IsSystemUser = false;
                    }
                    else
                        throw new Exception("That username is not authorized. Be sure you're using the username and password for your Windows account.");

                }

            }
            catch (Exception ex)
            {
                throw new UIPException(ex);
            }

            return _session;
        }

        public static void LogOut(SessionInfo sessioninfo)
        {
            AuditBusiness _auditBusiness = new AuditBusiness();
            _auditBusiness.TraceAuditLogoutUser(sessioninfo);
        }

        public static DA_LOGIN_AUDIT GetLogUser(SessionInfo sessioninfo)
        {
            AuditBusiness _auditBusiness = new AuditBusiness();
            return _auditBusiness.GetUserLogged(sessioninfo);
        }
        #endregion

        public static MA_USER GetByUserCode(SessionInfo sessioninfo, string usercode)
        {
            try
            {
                UserBusiness _userBusiness = new UserBusiness();
                //Get data from database
                MA_USER user = _userBusiness.GetByUserCode(sessioninfo, usercode);

                //Return result to jTable
                return user;
            }
            catch (Exception ex)
            {
                throw new UIPException(ex);
            }
        }

        public static MA_USER ValidateUser(SessionInfo sessioninfo, string strUsername, string strPassword, int intADLogin)
        {
            UserBusiness _userBusiness = new UserBusiness();

            try
            {
                bool validUser = true;
                if (intADLogin == 1)
                    validUser = LDAPHelper.ValidateUser(strUsername, strPassword);

                if (validUser)
                {
                    MA_USER user = _userBusiness.GetByUserCode(sessioninfo, strUsername);

                    return user;
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                throw new UIPException(ex);
            }
            

            //if (strUsername == "Admin")
            //{
            //    return new { Result = "OK", Message = "" };
            //}
            //else
            //{
            //    return new { Result = "ERROR", Message = "User has no right to approve limit." };
            //}
        }

        public static object GetUserOptions(SessionInfo sessioninfo)
        {
            try
            {
                UserBusiness _userBusiness = new UserBusiness();
                //Get data from database
                var users = _userBusiness.GetAll().Select(c => new { DisplayText = c.USERCODE, Value = c.ID });

                //Return result to jTable
                return new { Result = "OK", Options = users };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetByFilter(SessionInfo sessioninfo, string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                UserBusiness _userBusiness = new UserBusiness();
                //Get data from database
                List<MA_USER> users = _userBusiness.GetByFilter(sessioninfo, name, jtSorting);

                //Return result to jTable
                return new { Result = "OK", 
                             Records = jtPageSize > 0 ? users.Skip(jtStartIndex).Take(jtPageSize).ToList() : users, 
                             TotalRecordCount = users.Count };
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

        public static object Create(SessionInfo sessioninfo, MA_USER record, int intCheckAD)
        {
            try
            {
                UserBusiness _userBusiness = new UserBusiness();
                if (intCheckAD == 1)
                {
                    bool validUser = LDAPHelper.UserExists(record.USERCODE);
                    if (!validUser)
                        return new { Result = "ERROR", Message = "User is not exist."};
                }
                
                record.ID = Guid.NewGuid();
                record.DEPARTMENT = record.DEPARTMENT.ToUpper();
                record.NAME = record.NAME.ToUpper();
                record.USER_OPICS = record.USER_OPICS.ToUpper();
                record.USERCODE = record.USERCODE.ToUpper();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.HasValue ? false : true;
                record.ISLOCKED = record.ISLOCKED == null || !record.ISACTIVE.HasValue ? false : true;
                record.LOG.INSERTDATE = DateTime.Now;
                record.LOG.INSERTBYUSERID = sessioninfo.CurrentUserId;
                var addedStudent = _userBusiness.CreateUser(sessioninfo, record);
                return new { Result = "OK", Record = addedStudent };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object Update(SessionInfo sessioninfo, MA_USER record, int intCheckAD)
        {
            try
            {
                UserBusiness _userBusiness = new UserBusiness();
                if (intCheckAD == 1)
                {
                    bool validUser = LDAPHelper.UserExists(record.USERCODE);
                    if (!validUser)
                        return new { Result = "ERROR", Message = "User is not exist." };
                }

                record.DEPARTMENT = record.DEPARTMENT.ToUpper();
                record.NAME = record.NAME.ToUpper();
                record.USER_OPICS = record.USER_OPICS.ToUpper();
                record.USERCODE = record.USERCODE.ToUpper();
                record.ISACTIVE = record.ISACTIVE == null || !record.ISACTIVE.Value ? false : true;
                record.ISLOCKED = record.ISLOCKED == null || !record.ISACTIVE.Value ? false : true;
                record.LOG.MODIFYBYUSERID = sessioninfo.CurrentUserId;
                record.LOG.MODIFYDATE = DateTime.Now;
                var addedStudent = _userBusiness.UpdateUser(sessioninfo, record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object Delete(SessionInfo sessioninfo, Guid ID)
        {
            try
            {
                UserBusiness _userBusiness = new UserBusiness();
                _userBusiness.DeleteUser(sessioninfo, ID);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
    }
}
