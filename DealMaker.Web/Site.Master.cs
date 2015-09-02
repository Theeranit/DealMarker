using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.SystemFramework;
using KK.DealMaker.UIProcessComponent.Admin;
using KK.DealMaker.UIProcessComponent.Deal;

namespace DealMaker.Web
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected string LogonUserFullName
        {
            get
            {
                return SessionInfo.UserFullName;
            }
        }

        protected string CurrentProcessDate
        {
            get
            {
                return "Process on: " + SessionInfo.Process.CurrentDate.ToString(FormatTemplate.DATE_DMY_LABEL);
            }
        }

        private SessionInfo _sessionInfo = null;

        protected SessionInfo SessionInfo
        {
            get
            {
                if (_sessionInfo == null)
                {
                    _sessionInfo = SessionInfoSerializer.SessionInfoFromCurrentHttpContext();
                }
                return _sessionInfo;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }
    }
}
