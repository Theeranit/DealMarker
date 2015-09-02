using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_CURRENCYRepository : EFRepository<MA_CURRENCY>, IMA_CURRENCYRepository
	{
		public MA_CURRENCYRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_CURRENCY> GetAll()
        {
            return ObjectSet.ToList();
        }

        public MA_CURRENCY GetByID(Guid ID)
        {
            return ObjectSet.FirstOrDefault(p => p.ID.Equals(ID));
        }
	}

	public interface IMA_CURRENCYRepository : IRepository<MA_CURRENCY>
	{

	}
}