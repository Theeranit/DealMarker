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
    public partial class CountryReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetCountryReport(string strReportDate, string strReportType, string strCountry, string strStatus, int jtStartIndex, int jtPageSize)
        {
            return ReportUIP.GetCountryReport(SessionInfo, strReportDate, strCountry, strReportType, strStatus, jtStartIndex, jtPageSize);
        }
    }
}