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
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.UIProcessComponent.Admin;
using KK.DealMaker.UIProcessComponent.External;

namespace KK.DealMaker.Web.Admin
{
    public partial class BondMarketMaster : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetByFilter(string label, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return LookupUIP.GetBondMarketByFilter(SessionInfo, label, jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object Create(MA_BOND_MARKET record)
        {
            return LookupUIP.CreateBondMarket(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object Update(MA_BOND_MARKET record)
        {
            return LookupUIP.UpdateBondMarket(SessionInfo, record);
        }
    }
}