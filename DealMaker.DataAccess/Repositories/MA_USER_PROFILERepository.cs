using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_USER_PROFILERepository : EFRepository<MA_USER_PROFILE>, IMA_USER_PROFILERepository
	{
		public MA_USER_PROFILERepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_USER_PROFILE> GetAll()
        {
            return ObjectSet.ToList();
        }
	}

	public interface IMA_USER_PROFILERepository : IRepository<MA_USER_PROFILE>
	{

	}
}