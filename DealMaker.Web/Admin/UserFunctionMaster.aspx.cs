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
    public partial class FunctionMaster : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetFunctionOptions()
        {
            return FunctionUIP.GetFunctionOptions(SessionInfo);
        }

        [WebMethod(EnableSession = true)]
        public static object GetFunctionByFilter(string code, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return FunctionUIP.GetFunctionByFilter(SessionInfo, code, jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object Create(MA_FUNCTIONAL record)
        {
            return FunctionUIP.CreateFunction(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object Update(MA_FUNCTIONAL record)
        {
            return FunctionUIP.UpdateFunction(SessionInfo, record);
        }
    }
}