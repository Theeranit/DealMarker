using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.UIProcessComponent.Deal;

namespace KK.DealMaker.Web.Deal
{
    public partial class TempLimitInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetByFilter(string strCtpy, string strLimit, string strEffDateFrom, string strEffDateTo
                                            , string strExpDateFrom, string strExpDateTo, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return CounterpartyUIP.GetTempLimitByFilter(SessionInfo, strCtpy, strLimit, strEffDateFrom, strEffDateTo
                                                        , strExpDateFrom, strExpDateTo, jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object Create(MA_TEMP_CTPY_LIMIT record)
        {
            return CounterpartyUIP.CreateTempLimit(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object Update(MA_TEMP_CTPY_LIMIT record)
        {
            return CounterpartyUIP.UpdateTempLimit(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object GetCtpyLimitOptions()
        {
            return CounterpartyUIP.GetCtpyLimitOptions(SessionInfo);
        }

        [WebMethod(EnableSession = true)]
        public static object GetTempCountryByFilter(string strCountry, string strEffDateFrom, string strEffDateTo
                                                , string strExpDateFrom, string strExpDateTo, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return CountryUIP.GetTempLimitByFilter(SessionInfo, strCountry, strEffDateFrom, strEffDateTo
                                                        , strExpDateFrom, strExpDateTo, jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object CreateTempCountry(MA_COUNTRY_LIMIT record)
        {
            return CountryUIP.CreateTempLimit(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object UpdateTempCountry(MA_COUNTRY_LIMIT record)
        {
            return CountryUIP.UpdateTempLimit(SessionInfo, record);
        }
    }
}