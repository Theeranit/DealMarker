using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class DA_TRN_MATCHRepository : EFRepository<DA_TRN_MATCH>, IDA_TRN_MATCHRepository
	{
		public DA_TRN_MATCHRepository(DbContext context)
            : base(context)
		{
		}
	}

	public interface IDA_TRN_MATCHRepository : IRepository<DA_TRN_MATCH>
	{

	}
}