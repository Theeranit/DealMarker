using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using KK.DealMaker.Core.Data;
using KK.DealMaker.UIProcessComponent.View;
using KK.DealMaker.Core.Helper;
using KK.DealMaker.Core.Common;
using KK.DealMaker.UIProcessComponent.Admin;

namespace KK.DealMaker.Web.View
{
    public partial class DealView : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetDealInquiryByFilter(string strDMKNo
                                                    , string strOPICNo
                                                    , string strProduct
                                                    , string strCtpy
                                                    , string strPortfolio
                                                    , string strTradeDate
                                                    , string strEffDate
                                                    , string strMatDate
                                                    , string strInstrument
                                                    , string strUser
                                                    , string strStatus
                                                    , string strOverStatus
                                                    , string strProcDate
                                                    , string strSettleStatus
                                                    , int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return DealViewUIP.GetDealInquiryByFilter(SessionInfo
                                                        , strDMKNo
                                                        , strOPICNo
                                                        , strProduct
                                                        , strCtpy
                                                        , strPortfolio
                                                        , strTradeDate
                                                        , strEffDate
                                                        , strMatDate
                                                        , strInstrument
                                                        , strUser
                                                        , strStatus
                                                        , strOverStatus
                                                        , strProcDate
                                                        , strSettleStatus
                                                        , jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object GetStatusOptions()
        {
            return LookupUIP.GetStatusOptions(SessionInfo);
        }

        [WebMethod(EnableSession = true)]
        public static object CancelDeal(DA_TRN record)
        {
            return DealViewUIP.CancelDeal(SessionInfo, record);
        }

        //[WebMethod(EnableSession = true)]
        //public static object GetSwapLegDetailByID(string ID)
        //{
        //    return DealViewUIP.GetSwapLegDetailByID(SessionInfo, Guid.Parse(ID));
        //}
    }
}