using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Data;

namespace KK.DealMaker.Core.Common
{
    [Serializable]
    public class SessionInfo
    {
        public SessionInfo()
        {
            Process = new ProcessDate();
        }

        public Guid ID { get; set; }
        public string ConnectionString { get; set; }
        public string Local { get; set; }
        public string UserLogon { get; set; }
        public string UserFullName { get; set; }
        public Guid CurrentUserId { get; set; }
        public string ProxyName { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public DateTime StartSession { get; set; }
        public Guid ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string IPAddress { get; set; }
        public bool IsSystemUser { get; set; }
        public bool PCEOverwrite { get; set; }
        public bool SETOverwrite { get; set; }
        public bool CountryOverwrite { get; set; }

        //Navigate Properties
        public ProcessDate Process { get; set; }
    }

    public class ProcessDate
    {
        public DateTime CurrentDate { get; set; }
        public DateTime PreviousDate { get; set; }
        public DateTime NextDate { get; set; }
    }
}
