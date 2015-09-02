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
    public partial class PCEDetailReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetPCEDetailReport(string strReportDate, string strReportType, string strCtpy, string strProduct, int jtStartIndex, int jtPageSize)
        {
            return ReportUIP.GetPCEDetailReport(SessionInfo, strReportDate, strCtpy, strProduct, strReportType, jtStartIndex, jtPageSize);
        }
    }
}