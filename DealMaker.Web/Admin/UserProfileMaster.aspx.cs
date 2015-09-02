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


namespace KK.DealMaker.Web.Admin
{
    public partial class UserProfileMaster : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        [WebMethod(EnableSession = true)]
        public static object GetProfileOptions()
        {
            return ProfileUIP.GetProfileOptions(SessionInfo);
        }
        
        [WebMethod(EnableSession = true)]
        public static object GetProfileByFilter(string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return ProfileUIP.GetProfileByFilter(SessionInfo, name, jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object Create(MA_USER_PROFILE record)
        {
            return ProfileUIP.CreateProfile(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object Update(MA_USER_PROFILE record)
        {
            return ProfileUIP.UpdateProfile(SessionInfo, record);
        }
	}
}