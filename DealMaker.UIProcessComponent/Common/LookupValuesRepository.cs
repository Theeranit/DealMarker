using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using KK.DealMaker.UIProcessComponent.Common;
using KK.DealMaker.Core.Data;

namespace KK.DealMaker.UIProcessComponent.Common
{
    public enum RepositorySize
    {
        /// <summary>
        /// 8 records (2^3)
        /// </summary>
        Small,

        /// <summary>
        /// 128 records (2^7)
        /// </summary>
        Medium,

        /// <summary>
        /// 1024 records (2^10)
        /// </summary>
        Large
    }

    public static class RepositorySesssion
    {
        public static ILookupValuesRepository GetRepository(RepositorySize size = RepositorySize.Medium, string repositoryKey = "common")
        {
            var sessionKey = "Repository_" + repositoryKey + "_" + size;

            if (HttpContext.Current.Session[sessionKey] == null)
            {
                HttpContext.Current.Session[sessionKey] = CreateRepository(size);
            }

            return HttpContext.Current.Session[sessionKey] as ILookupValuesRepository;
        }

        private static ILookupValuesRepository CreateRepository(RepositorySize size)
        {
            return new MemoryRepositoryContainer(new MemoryLookupValues());
        }
    }
}
