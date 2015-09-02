using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_STATUSRepository : EFRepository<MA_STATUS>, IMA_STATUSRepository
	{
		public MA_STATUSRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_STATUS> GetAll()
        {
            return ObjectSet.ToList();
        }
	}

	public interface IMA_STATUSRepository : IRepository<MA_STATUS>
	{

	}
}