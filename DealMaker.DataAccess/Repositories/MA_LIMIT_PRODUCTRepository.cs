using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_LIMIT_PRODUCTRepository : EFRepository<MA_LIMIT_PRODUCT>, IMA_LIMIT_PRODUCTRepository
	{
		public MA_LIMIT_PRODUCTRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_LIMIT_PRODUCT> GetAll()
        {
            return ObjectSet
                //.Include(t => t.MA_LIMIT)
                //.Include(t => t.MA_PRODUCT)
                .ToList();
        }
	}

	public interface IMA_LIMIT_PRODUCTRepository : IRepository<MA_LIMIT_PRODUCT>
	{

	}
}