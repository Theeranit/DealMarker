using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Common;

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class DA_LOGIN_AUDITRepository : EFRepository<DA_LOGIN_AUDIT>, IDA_LOGIN_AUDITRepository
	{
		public DA_LOGIN_AUDITRepository(DbContext context)
            : base(context)
		{
		}

        public DA_LOGIN_AUDIT GetBySessionInfo(SessionInfo sessioninfo)
        {
            return ObjectSet.FirstOrDefault(p => p.USER_ID.Equals(sessioninfo.CurrentUserId) && p.TERMINAL.Contains(sessioninfo.IPAddress) && !p.LOGOUT_DATE.HasValue);
        }

        public List<DA_LOGIN_AUDIT> GetByUserID(Guid userID)
        {
            return ObjectSet.Where(p => p.USER_ID.Equals(userID)).ToList();
        }

        public List<DA_LOGIN_AUDIT> GetLoggedByUserID(Guid userID)
        {
            return ObjectSet.Where(p => p.USER_ID.Equals(userID) && !p.LOGOUT_DATE.HasValue).ToList();
        }

        public DA_LOGIN_AUDIT GetByID(Guid sessionID)
        {
            return ObjectSet.FirstOrDefault(p => p.ID.Equals(sessionID));
        }
	}

	public interface IDA_LOGIN_AUDITRepository : IRepository<DA_LOGIN_AUDIT>
	{

	}
}