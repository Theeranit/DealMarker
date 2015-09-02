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
    public partial class LimitProduct : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetLimitProductByFilter(string strproduct, string strlimit, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return LimitProductUIP.GetLimitproductByFilter(SessionInfo, strproduct, strlimit, jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object Create(MA_LIMIT_PRODUCT record)
        {
            return LimitProductUIP.Create(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object Update(MA_LIMIT_PRODUCT record)
        {
            return LimitProductUIP.Update(SessionInfo, record);
        }
    }
}