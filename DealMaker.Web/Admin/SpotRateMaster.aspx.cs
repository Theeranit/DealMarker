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
    public partial class SpotRateMaster : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetByFilter(string processdate, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return LookupUIP.GetSpotRateByFilter(SessionInfo, processdate, jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object Create(MA_SPOT_RATE record)
        {
            return LookupUIP.CreateSpotRate(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object Update(MA_SPOT_RATE record)
        {
            return LookupUIP.UpdateSpotRate(SessionInfo, record);
        }
    }
}