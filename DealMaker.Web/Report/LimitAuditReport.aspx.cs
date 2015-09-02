using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using KK.DealMaker.Core.Data;
using KK.DealMaker.UIProcessComponent.Report;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.Core.Common;
namespace KK.DealMaker.Web.Report
{
    public partial class LimitAuditReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetLimitAuditReport(string strLogDatefrom, string strLogDateto, string strCtpy, string strCountry, string strEvent, int jtStartIndex, int jtPageSize)
        {
            return ReportUIP.GetLimitAuditReport(SessionInfo, strLogDatefrom, strLogDateto, strCtpy, strCountry, strEvent, jtStartIndex, jtPageSize);
        }

        [WebMethod(EnableSession = true)]
        public static object Export2CSV()
        {
            return null;
        }
    }
}