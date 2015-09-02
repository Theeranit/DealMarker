using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_PROFILE_FUNCTIONALRepository : EFRepository<MA_PROFILE_FUNCTIONAL>, IMA_PROFILE_FUNCTIONALRepository
	{
		public MA_PROFILE_FUNCTIONALRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_PROFILE_FUNCTIONAL> GetAll()
        {
            return ObjectSet
                .Include(t => t.MA_FUNCTIONAL)
                .Include(t => t.MA_USER_PROFILE)
                .ToList();
        }

        public List<MA_PROFILE_FUNCTIONAL> GetByUserProfileId(Guid userprofileID)
        {
            return ObjectSet
                .Include(t => t.MA_FUNCTIONAL)
                .Include(t => t.MA_USER_PROFILE)
                .Where(p => p.USER_PROFILE_ID.Equals(userprofileID)&&p.MA_FUNCTIONAL.ISACTIVE==true&&p.MA_USER_PROFILE.ISACTIVE==true)
                .ToList();
        }
	}

	public interface IMA_PROFILE_FUNCTIONALRepository : IRepository<MA_PROFILE_FUNCTIONAL>
	{

	}
}