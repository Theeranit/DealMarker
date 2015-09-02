using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_PRODUCTRepository : EFRepository<MA_PRODUCT>, IMA_PRODUCTRepository
	{
		public MA_PRODUCTRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_PRODUCT> GetAll()
        {
            return ObjectSet
                .ToList();
        }
	}

	public interface IMA_PRODUCTRepository : IRepository<MA_PRODUCT>
	{

	}
}