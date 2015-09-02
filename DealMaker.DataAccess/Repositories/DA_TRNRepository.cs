using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	
using KK.DealMaker.Core.Constraint;

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class DA_TRNRepository : EFRepository<DA_TRN>, IDA_TRNRepository
	{
		public DA_TRNRepository(DbContext context)
            : base(context)
		{
		}

        public List<DA_TRN> GetAllByEngineDate(DateTime date)
        {
            return ObjectSet.Where(p => DateTime.Compare(p.ENGINE_DATE, date)==0)
                .Include(t => t.MA_COUTERPARTY)
                .Include(t => t.MA_INSRUMENT)
                .Include(t => t.MA_PRODUCT)
                .Include(t => t.MA_PORTFOLIO)
                .Include(t => t.MA_STATUS)
                .Include(t => t.DA_TMBA_EXTENSION)
                .ToList();
        }

        public List<DA_TRN> GetAllInternalByEngineDate(DateTime date)
        {
            return ObjectSet.Where(p => DateTime.Compare(p.ENGINE_DATE, date) == 0 && p.SOURCE.Equals("INT"))
                .Include(t => t.MA_COUTERPARTY)
                .Include(t => t.MA_INSRUMENT)
                .Include(t => t.MA_PRODUCT)
                .Include(t => t.MA_PORTFOLIO)
                .Include(t => t.MA_STATUS)
                .Include(t => t.DA_TMBA_EXTENSION)
                .Include(t => t.DA_TRN_FLOW)
                .ToList();
        }

        public List<DA_TRN> GetAllByEngineDateStatusCode(DateTime date, string statuscode)
        {
            return ObjectSet.Where(p => DateTime.Compare(p.ENGINE_DATE, date) == 0)
                .Include(t => t.MA_COUTERPARTY)
                .Include(t => t.MA_INSRUMENT)
                .Include(t => t.MA_PRODUCT)
                .Include(t => t.MA_PORTFOLIO)
                .Include(t => t.MA_STATUS)
                .Where(s => s.MA_STATUS.LABEL.Equals(statuscode)&&s.SOURCE.Equals("INT"))
                .ToList();
        }

        public List<DA_TRN> GetImportedByEngineDate(DateTime date)
        {
            return ObjectSet.Where(p => DateTime.Compare(p.ENGINE_DATE, date) == 0)
                .Include(t => t.MA_PRODUCT)
                .Where(s => s.SOURCE.Equals("EXT"))
                .ToList();
        }

        public DA_TRN GetPastImportedByExtNo(string strExtNo)
        {
            return ObjectSet.FirstOrDefault(p => p.EXT_DEAL_NO == strExtNo && p.SOURCE == "INT");
        }

        public List<DA_TRN> GetAllByStatusCode(string statuscode)
        {
            return ObjectSet
                .Include(t => t.MA_COUTERPARTY)
                .Include(t => t.MA_INSRUMENT)
                .Include(t => t.MA_PRODUCT)
                .Include(t => t.MA_PORTFOLIO)
                .Include(t => t.MA_STATUS)
                .Where(s => s.MA_STATUS.LABEL.Equals(statuscode) && s.SOURCE.Equals("INT"))
                .ToList();
        }

        public List<DA_TRN> GetAll()
        {
            return ObjectSet
                .Include(t => t.MA_COUTERPARTY)
                .Include(t => t.MA_INSRUMENT)
                .Include(t => t.MA_PRODUCT)
                .Include(t => t.MA_PORTFOLIO)
                .Include(t => t.MA_STATUS)
                .ToList();
                
        }

        public virtual DA_TRN GetById(Guid id, bool includenavigate)
        {
            return ObjectSet
                .Include(t => t.MA_COUTERPARTY)
                .Include(t => t.MA_INSRUMENT)
                .Include(t => t.MA_PRODUCT)
                .Include(t => t.MA_PORTFOLIO)
                .Include(t => t.MA_STATUS)
                .Include(t => t.DA_TMBA_EXTENSION)
                .Include(t => t.DA_TRN_FLOW)
                .FirstOrDefault(p => p.ID == id);
        }

        public virtual DA_TRN GetByDealNoProcessDate(DateTime procDate,  string dealno, SourceType source, bool includenavigate)
        {
            return ObjectSet.Where(p => DateTime.Compare(p.ENGINE_DATE, procDate) == 0)
                .Include(t => t.MA_COUTERPARTY)
                .Include(t => t.MA_INSRUMENT)
                .Include(t => t.MA_PRODUCT)
                .Include(t => t.MA_PORTFOLIO)
                .Include(t => t.MA_STATUS)
                .FirstOrDefault(p => source == SourceType.EXT ? p.EXT_DEAL_NO.Contains(dealno) : p.INT_DEAL_NO.Contains(dealno));
        }

        public List<DA_TRN> GetByExternalByInternalDealNo(DateTime procDate, string intdealno)
        {
            return ObjectSet.Where(p => DateTime.Compare(p.ENGINE_DATE, procDate) == 0)
                .Include(t => t.MA_COUTERPARTY)
                .Include(t => t.MA_INSRUMENT)
                .Include(t => t.MA_PRODUCT)
                .Include(t => t.MA_PORTFOLIO)
                .Include(t => t.MA_STATUS)
                 .Where(s => s.SOURCE.Equals("EXT") && s.INT_DEAL_NO == intdealno)
                 .ToList();
        }
	}

	public interface IDA_TRNRepository : IRepository<DA_TRN>
	{

	}
}