using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

using KK.DealMaker.Core.Data;
using KK.DealMaker.UIProcessComponent.Admin;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.Core.Common;

namespace KK.DealMaker.Web.Admin
{
    public partial class FactorConfigMaster : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region User Methods
        #region Child
        [WebMethod(EnableSession = true)]
        public static object GetTableOptions()
        {
            return PCCFConfigUIP.GetTables(SessionInfo);
        }

        [WebMethod(EnableSession = true)]
        public static object GetColumnOptions(string tablename)
        {
            return PCCFConfigUIP.GetColumns(SessionInfo, tablename);
        }

        [WebMethod(EnableSession = true)]
        public static object GetFactorByFilter(string ID)
        {
            return PCCFConfigUIP.GetFactorByFilter(SessionInfo, new Guid(ID));
        }
        [WebMethod(EnableSession = true)]
        public static object CreateFactor(MA_CONFIG_ATTRIBUTE record)
        {
            return PCCFConfigUIP.CreateFactor(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object UpdateFactor(MA_CONFIG_ATTRIBUTE record)
        {
            return PCCFConfigUIP.UpdateFactor(SessionInfo, record);
        }
        #endregion

        #region Main
        [WebMethod(EnableSession = true)]
        public static object GetConfigByFilter(string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return PCCFConfigUIP.GetConfigByFilter(SessionInfo, name, jtStartIndex, jtPageSize, jtSorting);
        }
        [WebMethod(EnableSession = true)]
        public static object CreateConfig(MA_PCCF_CONFIG record)
        {
            return PCCFConfigUIP.CreateConfig(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object UpdateConfig(MA_PCCF_CONFIG record)
        {
            return PCCFConfigUIP.UpdateConfig(SessionInfo, record);
        }
        #endregion

        #endregion
    }
}