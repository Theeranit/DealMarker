using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using KK.DealMaker.UIProcessComponent.Report;

namespace KK.DealMaker.Web.Report
{
    public partial class SCEReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetSCEReport(string strReportDate, string strReportType, string strCtpy, string strStatus, int jtStartIndex, int jtPageSize)
        {
            return ReportUIP.GetSCEReport(SessionInfo, strReportDate, strCtpy, strReportType, strStatus, jtStartIndex, jtPageSize);
        }
    }
}