using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_PCCF_CONFIGRepository : EFRepository<MA_PCCF_CONFIG>, IMA_PCCF_CONFIGRepository
	{
		public MA_PCCF_CONFIGRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_PCCF_CONFIG> GetAll()
        {
            return ObjectSet
                .Include(p => p.MA_CONFIG_ATTRIBUTE)
                .Include(p => p.MA_PCCF)
                .Where(p => p.ISACTIVE.Value.Equals(true)).ToList();
        }
	}

	public interface IMA_PCCF_CONFIGRepository : IRepository<MA_PCCF_CONFIG>
	{

	}
}