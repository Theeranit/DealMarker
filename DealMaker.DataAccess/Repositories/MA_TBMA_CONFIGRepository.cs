using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_TBMA_CONFIGRepository : EFRepository<MA_TBMA_CONFIG>, IMA_TBMA_CONFIGRepository
	{
		public MA_TBMA_CONFIGRepository(DbContext context)
            : base(context)
		{
		}
	}

	public interface IMA_TBMA_CONFIGRepository : IRepository<MA_TBMA_CONFIG>
	{

	}
}