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

using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.UIProcessComponent.Admin;

namespace KK.DealMaker.Web
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //TRACE LOGOUT
            //UserUIP.LogOut(SessionInfo);
            Context.Request.Cookies.Remove("UserName");
            Context.Request.Cookies.Remove(AppSettingName.TOKEN);
            Context.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);

            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now);
            HttpContext.Current.Response.Cache.SetNoServerCaching();
            HttpContext.Current.Response.Cache.SetNoStore();
            Session.Abandon();
            Session.Clear();
            Response.Redirect("Login.aspx");
        }
    }
}