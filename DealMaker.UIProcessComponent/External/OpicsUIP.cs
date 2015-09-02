using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.OpicsData;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Business.External;
using KK.DealMaker.Core.SystemFramework;

namespace KK.DealMaker.UIProcessComponent.External
{
    public class OpicsUIP : BaseUIP 
    {
        public static object GetOPICSCustomerByName(string name)
        { 
            OpicsBusiness opicsBusiness = new OpicsBusiness();
            try
            {
                List<CUSTModel> cust = opicsBusiness.GetOPICSCustomerByName(name);

                //Return result to jTable
                return new { Result = "OK", Records = cust, TotalRecordCount = cust.Count };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetOPICSCountryByLabel(string label)
        {
            OpicsBusiness opicsBusiness = new OpicsBusiness();
            try
            {
                List<COUNModel> country = opicsBusiness.GetOPICSCountryByLabel(label);

                //Return result to jTable
                return new { Result = "OK", Records = country, TotalRecordCount = country.Count };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        public static object GetOPICSInstrumentByLabel(string label)
        {
            OpicsBusiness opicsBusiness = new OpicsBusiness();
            try
            {
                List<SECMModel> secm = opicsBusiness.GetOPICSInstrumentByLabel(label);

                //Return result to jTable
                return new { Result = "OK", Records = secm, TotalRecordCount = secm.Count };
            }
            catch (BusinessWorkflowsException bex)
            {
                return new { Result = "ERROR", Message = bex.Message };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
    }
}
