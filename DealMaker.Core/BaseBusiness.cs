using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.SystemFramework;

namespace KK.DealMaker.Business
{
    public class BaseBusiness
    {
        public string GetCurrentDateTime()
        {
            return DateTime.Now.ToString(StringFormat.THAI_FORMAT_DATE);
        }

        public BusinessWorkflowsException CreateException(Exception ex, string message)
        {
            return String.IsNullOrEmpty(message) ? new BusinessWorkflowsException(ex) : new BusinessWorkflowsException(ex, message);
        }
    }
}
