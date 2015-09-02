using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_CSA_TYPERepository : EFRepository<MA_CSA_TYPE>, IMA_CSA_TYPERepository
	{
		public MA_CSA_TYPERepository(DbContext context)
            : base(context)
		{
		}
	}

	public interface IMA_CSA_TYPERepository : IRepository<MA_CSA_TYPE>
	{

	}
}