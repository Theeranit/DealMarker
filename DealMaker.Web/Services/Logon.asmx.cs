using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Web.Security;
using System.Web.Services;

using KK.DealMaker.Core.Helper;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.UIProcessComponent.Admin;
using KK.DealMaker.UIProcessComponent.External;
using log4net;

namespace KK.DealMaker.Web.Services
{

    /// <summary>
    /// Summary description for Logon
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Logon : BaseWS
    {
        private ILog log = LogManager.GetLogger(typeof(Logon));
        /// <summary>
        /// Initializes a new instance of the <see cref="loginWS"/> class.
        /// </summary>
        public Logon()
        {
            //CODEGEN: This call is required by the ASP.NET Web Services Designer
            InitializeComponent();
        }

        #region Component Designer generated code

        //Required by the Web Services Designer 
        private IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Add the current user name to the cookie to be used when displaying at the top of the screen
        /// </summary>
        /// <param name="sessioninfo">The sessioninfo.</param>
        private void AddUsernameToCookie(SessionInfo sessioninfo)
        {
            HttpCookie cookie = new HttpCookie("UserName");
            try
            {
                // Get the user data for the current user.
                //xuser userData = userUIP.GetUserLoginByID(sessioninfo, sessioninfo.CurrentUserID.ToString());
                MA_USER userData = UserUIP.GetByUserCode(sessioninfo, sessioninfo.UserLogon);
                // Build the concatenation of the first and last names.
                // Add the user name to the cookie.
                cookie.Value = userData.USERCODE;
                Context.Response.Cookies.Add(cookie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cookie = null;
            }
        }
        #endregion

        [WebMethod]
        [ScriptMethod(ResponseFormat=ResponseFormat.Xml)]
        public string LogOn(string username, string password)
        {
            ResultData rs = null;
            bool passLogon = true;
            try
            {
                LoggingHelper.Debug("Start Logon");
                //Put verify code for USER
                SessionInfo sessioninfo = null;
                sessioninfo = UserUIP.LogOn(username, password);

                if (sessioninfo == null)
                {
                    LoggingHelper.Debug(username + " unsuccessfully logged in.");
                    Tracing.WriteLine(Tracing.Category.Trace, username + " unsuccessfully logged in.", TraceLevel.Info, Guid.Empty, Guid.Empty);
                    rs = new ResultData(new Exception(username + " unsuccessfully logged in."), username + " unsuccessfully logged in.");
                    passLogon = false;
                }
                else
                {
                    // We need to keep token here.
                    FormsAuthentication.SetAuthCookie(username, false);

                    // Add a cookie here so we can retrieve later.
                    HttpCookie cookie = new HttpCookie(AppSettingName.TOKEN);
                    cookie.Value = SessionInfoSerializer.SessionInfoToString(sessioninfo);
                    Context.Response.Cookies.Add(cookie);

                    AddUsernameToCookie(sessioninfo);

                    // Check User Active
                    if (!sessioninfo.IsActive)
                    {
                        LoggingHelper.Debug(username + " has been inactive");
                        Tracing.WriteLine(Tracing.Category.Trace,username + " has been inactive", TraceLevel.Info, Guid.Empty, sessioninfo.CurrentUserId);
                        //rs = new ResultData(new Exception(username + " has been inactive"), username + " has been inactive");
                        //passLogon = false;
                    }
                }
                if (passLogon)
                {
                    LoggingHelper.Debug(username + " successfully logged in.");
                    Tracing.WriteLine(Tracing.Category.Trace, username + " successfully logged in.", TraceLevel.Info, Guid.Empty, sessioninfo.CurrentUserId);
                    rs = new ResultData("Success");
                }
                return rs.ToString();
            }
            catch (Exception ex)
            {
                rs = new ResultData(ex);
                return rs.ToString();
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public string LogOut(string username)
        {
            ResultData rs = null;
            try
            {   
                Context.Request.Cookies.Remove(AppSettingName.TOKEN);
                Context.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);

                FormsAuthentication.SignOut();
                rs = new ResultData("Success");
                return rs.ToString();
            }
            catch (Exception ex)
            {
                rs = new ResultData(ex);
                return rs.ToString();
            }
        }
    }
}
