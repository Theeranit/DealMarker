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
    public partial class ReconcileInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Web Method
        [WebMethod(EnableSession = true)]
        public static object GetDealToday(string processdate, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return ReconcileUIP.GetDealByProcessDateStatusCode(SessionInfo,
                                    DateTime.ParseExact(processdate, FormatTemplate.DATE_DMY_LABEL, null),
                                    StatusCode.OPEN,
                                    jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object GetDealOpics(string processdate, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return ReconcileUIP.GetDealExternalByProcessDate(SessionInfo, 
                                    DateTime.ParseExact(processdate, FormatTemplate.DATE_DMY_LABEL, null),
                                    jtStartIndex, jtPageSize, jtSorting);
        }

        [WebMethod(EnableSession = true)]
        public static object GetDealMatchToday(string processdate, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return ReconcileUIP.GetDealByProcessDateStatusCode(SessionInfo,
                                    DateTime.ParseExact(processdate, FormatTemplate.DATE_DMY_LABEL, null),
                                    StatusCode.MATCHED,
                                    jtStartIndex, jtPageSize, jtSorting);
        }
        [WebMethod(EnableSession = true)]
        public static object MatchingDeal(string processdate, string dmkid, string opicsno)
        {
            return ReconcileUIP.MatchingDeal(SessionInfo,
                            DateTime.ParseExact(processdate, FormatTemplate.DATE_DMY_LABEL, null),
                            new Guid(dmkid),
                            opicsno);
        }

        [WebMethod(EnableSession = true)]
        public static object CancelDeal(string processdate, string dmkid)
        {
            return ReconcileUIP.CancellingDeal(SessionInfo, new Guid(dmkid));
        }

        [WebMethod(EnableSession = true)]
        public static object CheckFlagReconcile()
        {
            return ReconcileUIP.CheckFlagReconcile(SessionInfo);
        }
        
        #endregion
    }
}