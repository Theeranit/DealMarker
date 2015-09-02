using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_CSA_PRODUCTRepository : EFRepository<MA_CSA_PRODUCT>, IMA_CSA_PRODUCTRepository
	{
		public MA_CSA_PRODUCTRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_CSA_PRODUCT> GetAll()
        {
            return ObjectSet.Include(p => p.MA_PRODUCT)
                            .ToList();
        }
	}

	public interface IMA_CSA_PRODUCTRepository : IRepository<MA_CSA_PRODUCT>
	{

	}
}