using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_COUTERPARTYRepository : EFRepository<MA_COUTERPARTY>, IMA_COUTERPARTYRepository
	{
		public MA_COUTERPARTYRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_COUTERPARTY> GetAll()
        {
            return ObjectSet
                .Include(p => p.MA_CSA_AGREEMENT)
                .ToList();
        }

        public MA_COUTERPARTY GetByUsercode(int usercode)
        {
            return ObjectSet.FirstOrDefault(p => p.USERCODE.Equals(usercode));

        }

        public MA_COUTERPARTY GetByShortName(string shortname)
        {
            return ObjectSet.FirstOrDefault(p => p.SNAME.Equals(shortname));

        }
	}

	public interface IMA_COUTERPARTYRepository : IRepository<MA_COUTERPARTY>
	{

	}
}