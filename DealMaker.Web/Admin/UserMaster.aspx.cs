using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using KK.DealMaker.Core.Data;
using KK.DealMaker.UIProcessComponent.Admin;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;

namespace KK.DealMaker.Web.Admin
{
    public partial class UserMaster : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region User Methods
        [WebMethod(EnableSession = true)]
        public static object GetByFilter(string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return UserUIP.GetByFilter(SessionInfo, name, jtStartIndex, jtPageSize, jtSorting);
        }
        [WebMethod(EnableSession = true)]
        public static object CreateUser(MA_USER record)
        {
            return UserUIP.Create(SessionInfo, record
                                , Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[AppSettingName.CHECK_AD_USER]));
        }

        [WebMethod(EnableSession = true)]
        public static object UpdateUser(MA_USER record)
        {
            return UserUIP.Update(SessionInfo, record
                                , Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[AppSettingName.CHECK_AD_USER]));
        }

        [WebMethod(EnableSession = true)]
        public static object DeleteUser(string ID)
        {
            return UserUIP.Delete(SessionInfo, new Guid(ID));
        }
        #endregion

        #region Profile methods

        [WebMethod(EnableSession = true)]
        public static object GetProfileOptions()
        {
            return ProfileUIP.GetProfileOptions(SessionInfo);
        }

        #endregion
    }
}