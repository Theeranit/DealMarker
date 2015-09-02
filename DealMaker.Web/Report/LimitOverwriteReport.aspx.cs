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
    public partial class LimitOverwriteReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod(EnableSession = true)]
        public static object GetLimitOverwriteReport(string strReportDate, string strCtpy, int jtStartIndex, int jtPageSize)
        {
            return ReportUIP.GetLimitOverwriteReport(SessionInfo, strReportDate, strCtpy, jtStartIndex, jtPageSize);
        }

        [WebMethod(EnableSession = true)]
        public static object Export2CSV()
        {
            return null;
        }
    }
}