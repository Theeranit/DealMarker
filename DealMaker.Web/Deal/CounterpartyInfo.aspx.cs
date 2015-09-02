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
using KK.DealMaker.UIProcessComponent.External;
using KK.DealMaker.UIProcessComponent.Admin;

namespace KK.DealMaker.Web.Deal
{
    public partial class CounterpartyInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //base.OnLoad(e);

        }

        #region User Methods
        [WebMethod(EnableSession = true)]
        public static object GetByFilter(string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return CounterpartyUIP.GetByFilter(SessionInfo, name, jtStartIndex, jtPageSize, jtSorting);
        }
        [WebMethod(EnableSession = true)]
        public static object GetGroupViewByFilter(string name, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return CounterpartyUIP.GetGroupViewByFilter(SessionInfo, name, jtStartIndex, jtPageSize, jtSorting);
        }
        [WebMethod(EnableSession = true)]
        public static object GetCtpyLimitGroupViewByCtpyID(string ID)
        {
            return CounterpartyUIP.GetCtpyLimitGroupViewByCtpyID(SessionInfo, new Guid(ID));
        }
        [WebMethod(EnableSession = true)]
        public static object Create(MA_COUTERPARTY record)
        {
            return CounterpartyUIP.Create(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object Update(MA_COUTERPARTY record)
        {
            return CounterpartyUIP.Update(SessionInfo, record);
        }
        [WebMethod(EnableSession = true)]
        public static object GetOPICNameByFilter(string name_startsWith)
        {
            return OpicsUIP.GetOPICSCustomerByName(name_startsWith.ToUpper());
        }

        [WebMethod(EnableSession = true)]
        public static object GetCtpyLimitByCtpyID(string ID)
        {
            return CounterpartyUIP.GetCtpyLimitByCtpyID(SessionInfo, new Guid(ID));
        }
        [WebMethod(EnableSession = true)]
        public static object DeleteCtpyLimit(string ID)
        {
            return CounterpartyUIP.DeleteCtpyLimitByID(SessionInfo, new Guid(ID));
        }
        [WebMethod(EnableSession = true)]
        public static object UpdateCtpyLimit(MA_CTPY_LIMIT record)
        {
            return CounterpartyUIP.UpdateCtpyLimit(SessionInfo, record);
        }
        [WebMethod(EnableSession = true)]
        public static object CreateCtpyLimit(MA_CTPY_LIMIT record)
        {
            return CounterpartyUIP.CreateCtpyLimit(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object GetCSAByCtpyID(string ID)
        {
            return CounterpartyUIP.GetCSAByCtpyID(SessionInfo, new Guid(ID));
        }

        [WebMethod(EnableSession = true)]
        public static object GetCSATypeOptions()
        {
            return LookupUIP.GetCSATypeOptions(SessionInfo);
        }

        [WebMethod(EnableSession = true)]
        public static object CreateCSA(MA_CSA_AGREEMENT record)
        {
            return CounterpartyUIP.CreateCSA(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object UpdateCSA(MA_CSA_AGREEMENT record)
        {
            return CounterpartyUIP.UpdateCSA(SessionInfo, record);
        }
        
        [WebMethod(EnableSession = true)]
        public static object GetCSAProducts(string ID)
        {
            return CounterpartyUIP.GetCSAProducts(SessionInfo, new Guid(ID));
        }

        [WebMethod(EnableSession = true)]
        public static object CreateCSAProduct(MA_CSA_PRODUCT record)
        {
            return CounterpartyUIP.CreateCSAProduct(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object DeleteCSAProduct(string CSA_AGREEMENT_ID, string PRODUCT_ID)
        {
            return CounterpartyUIP.DeleteCSAProduct(SessionInfo, new Guid(CSA_AGREEMENT_ID), new Guid(PRODUCT_ID));
        }
        #endregion
    }
}