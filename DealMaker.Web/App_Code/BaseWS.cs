using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.SystemFramework;

namespace KK.DealMaker.Web
{
    public class BaseWS : System.Web.Services.WebService
    {
        public BaseWS()
        { }

        private static SessionInfo _sessionInfo;

        protected static SessionInfo SessionInfo
        {
            get
            {
                if (_sessionInfo == null)
                {
                    _sessionInfo = SessionInfoSerializer.SessionInfoFromCurrentHttpContext();
                }
                return _sessionInfo;
            }
        }

        [WebMethod(EnableSession=true)]
        public string GetSystemDatetime()
        {
            return DateTime.Now.ToString();
        }
    }
}