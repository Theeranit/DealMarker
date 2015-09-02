using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_CTPY_LIMITRepository : EFRepository<MA_CTPY_LIMIT>, IMA_CTPY_LIMITRepository
	{
		public MA_CTPY_LIMITRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_CTPY_LIMIT> GetAll()
        {
            return ObjectSet
                .Include(t => t.MA_LIMIT)
                .ToList();
        }
	}

	public interface IMA_CTPY_LIMITRepository : IRepository<MA_CTPY_LIMIT>
	{

	}
}