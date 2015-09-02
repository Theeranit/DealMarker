using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_FUNCTIONALRepository : EFRepository<MA_FUNCTIONAL>, IMA_FUNCTIONALRepository
	{
		public MA_FUNCTIONALRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_FUNCTIONAL> GetAll()
        {
            return ObjectSet
                .ToList();
        }
	}

	public interface IMA_FUNCTIONALRepository : IRepository<MA_FUNCTIONAL>
	{

	}
}