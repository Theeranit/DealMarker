using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class DA_LOGGINGRepository : EFRepository<DA_LOGGING>, IDA_LOGGINGRepository
	{
		public DA_LOGGINGRepository(DbContext context)
            : base(context)
		{
		}
	}

	public interface IDA_LOGGINGRepository : IRepository<DA_LOGGING>
	{

	}
}