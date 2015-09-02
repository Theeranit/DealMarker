using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.Common
{
    public class LimitAuditReportModel
    {
        public string ENTITY { get; set; }
        public Guid ENTITY_ID { get; set; }
        public string LIMIT { get; set; }
        public string USER { get; set; }
        public DateTime LOG_DATE { get; set; }
        public string LOG_DATE_STR { get; set; }
        public string DETAIL { get; set; }
    }
}
