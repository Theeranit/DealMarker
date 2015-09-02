using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_TEMP_CTPY_LIMITRepository : EFRepository<MA_TEMP_CTPY_LIMIT>, IMA_TEMP_CTPY_LIMITRepository
	{
		public MA_TEMP_CTPY_LIMITRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_TEMP_CTPY_LIMIT> GetAll()
        {
            return ObjectSet.ToList();
        }
	}

	public interface IMA_TEMP_CTPY_LIMITRepository : IRepository<MA_TEMP_CTPY_LIMIT>
	{

	}
}