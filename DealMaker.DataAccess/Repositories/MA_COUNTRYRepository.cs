using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_COUNTRYRepository : EFRepository<MA_COUNTRY>, IMA_COUNTRYRepository
	{
		public MA_COUNTRYRepository(DbContext context)
            : base(context)
		{
		}
	}

	public interface IMA_COUNTRYRepository : IRepository<MA_COUNTRY>
	{

	}
}