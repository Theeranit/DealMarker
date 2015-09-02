using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_PORTFOLIORepository : EFRepository<MA_PORTFOLIO>, IMA_PORTFOLIORepository
	{
		public MA_PORTFOLIORepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_PORTFOLIO> GetAll()
        {
            return ObjectSet
                .ToList();
        }
	}

	public interface IMA_PORTFOLIORepository : IRepository<MA_PORTFOLIO>
	{

	}
}