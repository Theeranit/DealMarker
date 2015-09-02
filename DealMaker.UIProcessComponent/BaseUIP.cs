using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Core.Constraint;
using System.Web;
using KK.DealMaker.UIProcessComponent.Common;
using KK.DealMaker.Core.SystemFramework;

namespace KK.DealMaker.UIProcessComponent
{
    public abstract class BaseUIP
    {
        public static UpdateStates updateStates;

        /// <summary>
        /// Used to Decode the client side Javascript encoding of password
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>decoded string</returns>
        public static string DecodeJSPassEncoding(string password)
        {
            string result = string.Empty;
            string tempResult = System.Web.HttpUtility.UrlDecode(password);

            //if the length of the decode is the same as the original the password is not encoded
            if (password.Length == tempResult.Length)
                return password;

            //since the everyother character is becoming 3 ('+ +') grab the appropriate substr
            tempResult = tempResult.Substring(16, tempResult.Length - 32);

            for (int i = 0; i < tempResult.Length; i++)
            {

                if (i == 0 || i % 4 == 0)
                {
                    result += tempResult[i];
                }

            }

            return result;

        }

        public static UIPException CreateException(Exception ex, string message)
        {
            return String.IsNullOrEmpty(message) ? new UIPException(ex) : new UIPException(ex, message);
        }
    }
}
