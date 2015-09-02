using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using KK.DealMaker.Core.Data;
using KK.DealMaker.UIProcessComponent.Deal;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.Core.Common;
using KK.DealMaker.UIProcessComponent.Admin;
using KK.DealMaker.UIProcessComponent.External;
using KK.DealMaker.Core.Constraint;

namespace KK.DealMaker.Web.Deal
{
    public partial class ImportOpicsInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object ImportOPICSDeals(string processdate)
        {
            return ReconcileUIP.ImportExternalByProcessDate(SessionInfo);
        }
    }
}