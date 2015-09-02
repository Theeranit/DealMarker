using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using KK.DealMaker.UIProcessComponent.Admin;
using KK.DealMaker.Core.Data;

namespace KK.DealMaker.Web.Admin
{
    public partial class CSATypeMaster : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetByFilter(string label, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return LookupUIP.GetCSATypeByFilter(SessionInfo, label, jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object Create(MA_CSA_TYPE record)
        {
            return LookupUIP.CreateCSAType(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object Update(MA_CSA_TYPE record)
        {
            return LookupUIP.UpdateCSAType(SessionInfo, record);
        }
    }
}