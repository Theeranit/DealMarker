using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class DA_TRN_CASHFLOWRepository : EFRepository<DA_TRN_CASHFLOW>, IDA_TRN_CASHFLOWRepository
	{
		public DA_TRN_CASHFLOWRepository(DbContext context)
            : base(context)
		{
		}

        public List<DA_TRN_CASHFLOW> GetAllByEngineDate(DateTime dteProcessingDate)
        {
            return ObjectSet
                .Include(t => t.DA_TRN)
                .Include(t => t.DA_TRN.MA_STATUS)
                .Include(t => t.DA_TRN.MA_COUTERPARTY)
                .Include(t => t.DA_TRN.MA_PRODUCT)
                .Include(t => t.DA_TRN.MA_INSRUMENT)
                .Where(t => t.DA_TRN.ENGINE_DATE == dteProcessingDate)
                //.Include(t => t.MA_COUTERPARTY)
                //.Include(t => t.MA_INSRUMENT)
                //.Include(t => t.MA_PRODUCT)
                //.Include(t => t.MA_PORTFOLIO)
                //.Include(t => t.MA_STATUS)
                .ToList();

        }
	}

	public interface IDA_TRN_CASHFLOWRepository : IRepository<DA_TRN_CASHFLOW>
	{

	}
}