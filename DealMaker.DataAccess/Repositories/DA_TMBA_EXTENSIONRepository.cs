using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class DA_TMBA_EXTENSIONRepository : EFRepository<DA_TMBA_EXTENSION>, IDA_TMBA_EXTENSIONRepository
	{
		public DA_TMBA_EXTENSIONRepository(DbContext context)
            : base(context)
		{
		}
	}

	public interface IDA_TMBA_EXTENSIONRepository : IRepository<DA_TMBA_EXTENSION>
	{

	}
}