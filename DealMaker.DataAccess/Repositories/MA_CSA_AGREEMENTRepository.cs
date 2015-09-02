using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_CSA_AGREEMENTRepository : EFRepository<MA_CSA_AGREEMENT>, IMA_CSA_AGREEMENTRepository
	{
		public MA_CSA_AGREEMENTRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_CSA_AGREEMENT> GetAll()
        {
            return ObjectSet.Include(p => p.MA_CSA_PRODUCT)
                            .Include(p => p.MA_CSA_TYPE)
                            .ToList();
        }
	}

	public interface IMA_CSA_AGREEMENTRepository : IRepository<MA_CSA_AGREEMENT>
	{

	}
}