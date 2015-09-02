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
using KK.DealMaker.UIProcessComponent.Common;

namespace KK.DealMaker.Web.Deal
{
    public partial class BondInstrumentInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        public static object GetByFilter(string label, int jtStartIndex, int jtPageSize, string jtSorting)
        {
            return InstrumentUIP.GetBondsByFilter(SessionInfo, label, jtStartIndex, jtPageSize, jtSorting);
        }

        //[WebMethod(EnableSession = true)]
        //public static object GetBondProductOptions()
        //{
        //    return LookupUIP.GetProductOptions(SessionInfo);
        //}

        [WebMethod(EnableSession = true)]
        public static object Create(MA_INSTRUMENT record)
        {
            ILookupValuesRepository _lookupvaluesRepository = RepositorySesssion.GetRepository();

            record.PRODUCT_ID = _lookupvaluesRepository.ProductRepository.GetByLabel(ProductCode.BOND.ToString()).ID;

            return InstrumentUIP.Create(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object Update(MA_INSTRUMENT record)
        {
            return InstrumentUIP.Update(SessionInfo, record);
        }

        [WebMethod(EnableSession = true)]
        public static object GetOPICSInstrumentByLabel(string name_startsWith)
        {
            return OpicsUIP.GetOPICSInstrumentByLabel(name_startsWith.ToUpper());
        }
    }
}