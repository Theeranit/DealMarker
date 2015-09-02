using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_CONFIG_ATTRIBUTERepository : EFRepository<MA_CONFIG_ATTRIBUTE>, IMA_CONFIG_ATTRIBUTERepository
	{
		public MA_CONFIG_ATTRIBUTERepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_CONFIG_ATTRIBUTE> GetAll()
        {
            return ObjectSet.ToList();
        }
	}

	public interface IMA_CONFIG_ATTRIBUTERepository : IRepository<MA_CONFIG_ATTRIBUTE>
	{

	}
}