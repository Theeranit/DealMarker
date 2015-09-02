using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_COUNTRY_LIMITRepository : EFRepository<MA_COUNTRY_LIMIT>, IMA_COUNTRY_LIMITRepository
	{
		public MA_COUNTRY_LIMITRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_COUNTRY_LIMIT> GetAll()
        {
            return ObjectSet.Include(p => p.MA_COUNTRY).ToList();
        }
	}

	public interface IMA_COUNTRY_LIMITRepository : IRepository<MA_COUNTRY_LIMIT>
	{

	}
}