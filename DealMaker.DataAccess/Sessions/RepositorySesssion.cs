using System.Web;
using KK.DealMaker.DataAccess.Repositories;
using KK.DealMaker.JTable.Sessions;

namespace KK.DealMaker.JTable.Sessions
{
    public static class RepositorySesssion
    {
        public static IRepository<T> GetRepository<T>(RepositorySize size = RepositorySize.Medium, string repositoryKey = "common")
        {
            var sessionKey = "Repository_" + repositoryKey + "_" + size;

            return HttpContext.Current.Session[sessionKey] as IRepository<T>;
        }

    }
}
