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
    public partial class RepoReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetRepoReport(string strReportDate, string strReportType, string strCtpy, int jtStartIndex, int jtPageSize)
        {
            return ReportUIP.GetRepoReport(SessionInfo, strReportDate, strReportType, strCtpy, jtStartIndex, jtPageSize);
        }

    }
}