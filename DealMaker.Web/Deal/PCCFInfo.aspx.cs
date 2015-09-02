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

namespace KK.DealMaker.Web.Deal
{
    public partial class PCCFInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region PCCF Methods

        [WebMethod(EnableSession = true)]
        public static object GetPCCFByFilter(string label, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return PCCFUIP.GetPCCFByFilter(SessionInfo, label, jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object CreatePCCF(MA_PCCF record)
        {
            return PCCFUIP.Create(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object UpdatePCCF(MA_PCCF record)
        {
            return PCCFUIP.Update(SessionInfo, record);
        }

        //[WebMethod(EnableSession = true)]
        //public static object DeleteUser(string ID)
        //{
        //    return UserUIP.Delete(SessionInfo, new Guid(ID));
        //}

        #endregion

    }
}