using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_PROCESS_DATERepository : EFRepository<MA_PROCESS_DATE>, IMA_PROCESS_DATERepository
	{
		public MA_PROCESS_DATERepository(DbContext context)
            : base(context)
		{
		}
	}

	public interface IMA_PROCESS_DATERepository : IRepository<MA_PROCESS_DATE>
	{

	}
}