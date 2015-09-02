using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using KK.DealMaker.UIProcessComponent.Deal;
using KK.DealMaker.Core.Data;
using KK.DealMaker.UIProcessComponent.External;

namespace KK.DealMaker.Web.Deal
{
    public partial class CountryInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region CountryInfo
        [WebMethod(EnableSession = true)]
        public static object GetByFilter(string label, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return CountryUIP.GetByFilter(SessionInfo, label, jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object Create(MA_COUNTRY record)
        {
            return CountryUIP.Create(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object Update(MA_COUNTRY record)
        {
            return CountryUIP.Update(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object GetOPICSCountryByLabel(string label_startsWith)
        {
            return OpicsUIP.GetOPICSCountryByLabel(label_startsWith.ToUpper());
        }
        #endregion

        #region Country Limit
        [WebMethod(EnableSession = true)]
        public static object GetCountryLimitByCountryID(string ID)
        {
            return CountryUIP.GetCountryLimitByCountryID(SessionInfo, new Guid(ID));
        }

        [WebMethod(EnableSession = true)]
        public static object UpdateCountryLimit(MA_COUNTRY_LIMIT record)
        {
            return CountryUIP.UpdateCountryLimit(SessionInfo, record);
        }
        #endregion
    }
}