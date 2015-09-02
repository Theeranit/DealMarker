using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_SPOT_RATERepository : EFRepository<MA_SPOT_RATE>, IMA_SPOT_RATERepository
	{
		public MA_SPOT_RATERepository(DbContext context)
            : base(context)
		{
		}


        public List<MA_SPOT_RATE> GetAll()
        {
            return ObjectSet
                    .Include(t => t.MA_CURRENCY)
                    .ToList();
        }

        public List<MA_SPOT_RATE> GetByProcessDate(DateTime processdate)
        {
            return ObjectSet
                    .Where(t => t.PROC_DATE == processdate)
                    .Include(t => t.MA_CURRENCY)                    
                    .ToList();
        }
	}

	public interface IMA_SPOT_RATERepository : IRepository<MA_SPOT_RATE>
	{

	}
}