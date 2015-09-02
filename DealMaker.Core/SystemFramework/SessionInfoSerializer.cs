using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using KK.DealMaker.Core.Common;
using KK.DealMaker.Core.Constraint;
using KK.DealMaker.Core.SystemFramework;

namespace KK.DealMaker.Core.SystemFramework
{
    public class SessionInfoSerializer
    {
        #region Constants

        #endregion Constants

        #region Enums

        #endregion Enums

        #region Fields

        private static string _key = "2343fj4544sd";

        #endregion Fields

        #region Delegates

        #endregion Delegates

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionInfoSerializer"/> class.
        /// </summary>
        public SessionInfoSerializer()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Strings to session info.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static SessionInfo StringToSessionInfo(string src)
        {
            if (src == null)
                return null;

            CryptoString cry = new CryptoString(_key, CryptoString.Method.Decrypt, src);
            StringReader sr = new StringReader(cry.Execute());
            XmlSerializer xml = new XmlSerializer(typeof(SessionInfo));
            SessionInfo sessionInfo = (SessionInfo)xml.Deserialize(sr);
            sr.Close();

            return sessionInfo;
        }

        /// <summary>
        /// Sessions the info to string.
        /// </summary>
        /// <param name="sessionInfo">The session info.</param>
        /// <returns></returns>
        public static string SessionInfoToString(SessionInfo sessionInfo)
        {
            if (sessionInfo == null)
                return null;

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            XmlSerializer xml = new XmlSerializer(typeof(SessionInfo));
            xml.Serialize(sw, sessionInfo);
            sw.Flush();

            CryptoString cry = new CryptoString(_key, CryptoString.Method.Encrypt, sb.ToString());
            string ret = cry.Execute();
            sb.Remove(0, sb.Length);
            if (Regex.IsMatch(ret, @".*[Oo][Nn][a-zA-Z]*=*$"))
            {
                sessionInfo.ConnectionString += ';';
                xml.Serialize(sw, sessionInfo);
                sw.Flush();
                cry = new CryptoString(_key, CryptoString.Method.Encrypt, sb.ToString());
                ret = cry.Execute();
            }

            sw.Close();
            return ret;
        }

        /// <summary>
        /// Sessions the info from current HTTP context.
        /// </summary>
        /// <returns></returns>
        public static SessionInfo SessionInfoFromCurrentHttpContext()
        {
            if (HttpContext.Current == null)
                return null;

            SessionInfo sessionInfo = null;
            HttpCookie tokenCookie = null;
            tokenCookie = HttpContext.Current.Request.Cookies[AppSettingName.TOKEN];
            if (tokenCookie != null)
                sessionInfo = SessionInfoSerializer.StringToSessionInfo(tokenCookie.Value);

            return sessionInfo;
        }

        #endregion Functions
    }
}
