using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_LIMITRepository : EFRepository<MA_LIMIT>, IMA_LIMITRepository
	{
		public MA_LIMITRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_LIMIT> GetAll()
        {
            return ObjectSet
                .ToList();
        }
	}

	public interface IMA_LIMITRepository : IRepository<MA_LIMIT>
	{

	}
}