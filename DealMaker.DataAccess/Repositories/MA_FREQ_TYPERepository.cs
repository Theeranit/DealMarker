using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_FREQ_TYPERepository : EFRepository<MA_FREQ_TYPE>, IMA_FREQ_TYPERepository
	{
		public MA_FREQ_TYPERepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_FREQ_TYPE> GetAll()
        {
            return ObjectSet
                //.Include(t => t.MA_CTPY_LIMIT)
                //.Include(t => t.MA_LIMIT_PRODUCT)
                .ToList();
        }
	}

	public interface IMA_FREQ_TYPERepository : IRepository<MA_FREQ_TYPE>
	{

	}
}