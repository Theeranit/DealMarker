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
    public partial class UserProfileFunction : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetProfileFunctionByFilter(string strprofile, string strfunction, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return ProfileFunctionalUIP.GetProfileFunctionByFilter(SessionInfo,strprofile, strfunction, jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object Create(MA_PROFILE_FUNCTIONAL record)
        {
            return ProfileFunctionalUIP.Create(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object Update(MA_PROFILE_FUNCTIONAL record)
        {
            return ProfileFunctionalUIP.Update(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object Delete(Guid ID)
        {
            return ProfileFunctionalUIP.Delete(SessionInfo, ID);
        }
    }
}