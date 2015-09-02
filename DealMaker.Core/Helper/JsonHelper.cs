using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KK.DealMaker.Core.Helper
{
    public class JsonHelper
    {
        /// <summary>
        /// Convert Object to Json String
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>Json representation of the Object in string</returns>
        public static string ToJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}
