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
    public partial class LimitMaster : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region User Methods
        [WebMethod(EnableSession = true)]
        public static object GetByFilter(string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return LookupUIP.GetLimitByFilter(SessionInfo, name, jtStartIndex, jtPageSize, jtSorting);
        }
        [WebMethod(EnableSession = true)]
        public static object Create(MA_LIMIT record)
        {
            return LookupUIP.CreateLimit(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object Update(MA_LIMIT record)
        {
            return LookupUIP.UpdateLimit(SessionInfo, record);
        }
        #endregion

    }
}