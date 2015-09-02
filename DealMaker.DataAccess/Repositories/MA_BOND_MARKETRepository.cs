using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_BOND_MARKETRepository : EFRepository<MA_BOND_MARKET>, IMA_BOND_MARKETRepository
	{
		public MA_BOND_MARKETRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_BOND_MARKET> GetAll()
        {
            return ObjectSet.ToList();
        }
	}

	public interface IMA_BOND_MARKETRepository : IRepository<MA_BOND_MARKET>
	{

	}
}