using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_USERRepository : EFRepository<MA_USER>, IMA_USERRepository
	{
		public MA_USERRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_USER> GetAll()
        {
            return ObjectSet
                .Include(t => t.MA_USER_PROFILE)
                .ToList();
        }

        public MA_USER GetByUserCode(string usercode)
        {
            return ObjectSet
                    .Include(t => t.MA_USER_PROFILE)
                    .FirstOrDefault(p => p.USERCODE.ToLower().Equals(usercode.ToLower()));
        }
	}

	public interface IMA_USERRepository : IRepository<MA_USER>
	{

	}
}