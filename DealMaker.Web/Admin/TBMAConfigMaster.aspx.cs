using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using KK.DealMaker.Core.Data;
using KK.DealMaker.UIProcessComponent.Admin;

namespace KK.DealMaker.Web.Admin
{
    public partial class TBMAConfigMaster : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetAll()
        {
            return LookupUIP.GetTBMAConfigAll(SessionInfo);
        }

        [WebMethod(EnableSession = true)]
        public static object Update(MA_TBMA_CONFIG record)
        {
            return LookupUIP.UpdateTBMAConfig(SessionInfo, record);
        }
    }
}