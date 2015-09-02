using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Web.Security;
using System.Web.Services;
using System.Text;

using KK.DealMaker.Core.Helper;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.UIProcessComponent.Admin;
using KK.DealMaker.UIProcessComponent.External;
using log4net;

namespace KK.DealMaker.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblInfo.Visible = true;
                lblErrorMessage.Visible = false;
                lblInfo.Text = "<div id='info-label' class='information-box round'>Just click on the ''LOG IN'' button to continue, no login information required.</div>";
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            LoggingHelper.Info("Begin Login.....");
            string result = LogOn(UserName.Text, Password.Text);

            LoggingHelper.Info("End Login.");
        }

        #region Private
        private void AddUsernameToCookie(SessionInfo sessioninfo)
        {
            HttpCookie cookie = new HttpCookie("UserName");
            try
            {
                // Get the user data for the current user.
                //xuser userData = userUIP.GetUserLoginByID(sessioninfo, sessioninfo.CurrentUserID.ToString());
                //MA_USER userData = UserUIP.GetByUserCode(sessioninfo, sessioninfo.UserLogon);
                // Build the concatenation of the first and last names.
                // Add the user name to the cookie.
                cookie.Value = sessioninfo.UserLogon;
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

        public string LogOn(string username, string password)
        {
            ResultData rs = null;
            bool passLogon = true;
            try
            {
                LoggingHelper.Debug("Start Logon");
                //Put verify code for USER
                SessionInfo sessioninfo = null;
                sessioninfo = UserUIP.LogOn(username, password
                                            , Context.Request.UserHostAddress
                                            , Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[AppSettingName.AD_LOGIN]));

                if (sessioninfo == null)
                {
                    LoggingHelper.Debug(username + " unsuccessfully logged in.");
                    Tracing.WriteLine(Tracing.Category.Trace, username + " unsuccessfully logged in.", TraceLevel.Info, Guid.Empty, Guid.Empty);
                    rs = new ResultData(new Exception(username + " unsuccessfully logged in."), username + " unsuccessfully logged in.");
                    passLogon = false;
                    ShowErrorMessage(username + " unsuccessfully logged in.");
                }
                else
                {
                    //Additional information for overwrite limit
                    sessioninfo.PCEOverwrite = ConfigurationManager.AppSettings[AppSettingName.PCE_OVERWRITE].ToString() == "1" ? true : false;
                    sessioninfo.SETOverwrite = ConfigurationManager.AppSettings[AppSettingName.SET_OVERWRITE].ToString() == "1" ? true : false;
                    sessioninfo.CountryOverwrite = ConfigurationManager.AppSettings[AppSettingName.COUNTRY_OVERWRITE].ToString() == "1" ? true : false;


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
                        Tracing.WriteLine(Tracing.Category.Trace, username + " has been inactive", TraceLevel.Info, Guid.Empty, sessioninfo.CurrentUserId);
                        rs = new ResultData(new Exception(username + " has been inactive"), username + " has been inactive");
                        //passLogon = false;
                        ShowErrorMessage(username + " has been inactive");
                    }
                }
                if (passLogon)
                {
                    LoggingHelper.Debug(username + " successfully logged in.");
                    Tracing.WriteLine(Tracing.Category.Trace, username + " successfully logged in.", TraceLevel.Info, Guid.Empty, sessioninfo.CurrentUserId);
                    rs = new ResultData("Success");

                    Response.Redirect("Main.aspx?type=logon");
                }
                
            }
            catch (Exception ex)
            {
                rs = new ResultData(ex);
                //return rs.ToString();
                ShowErrorMessage("Error::" + ex.Message);
            }
            return rs.ToString();
        }

        private void ShowErrorMessage(string message)
        { 
            StringBuilder sp = new StringBuilder();
            sp.Append("<div id='error-label' class='error-box round'>");
            sp.Append(message);
            sp.Append("</div>");

            lblErrorMessage.Text = sp.ToString();
            lblErrorMessage.Visible = true;
            lblInfo.Visible = false;
        }
        #endregion

    }
}