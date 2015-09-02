using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;	

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_PCCFRepository : EFRepository<MA_PCCF>, IMA_PCCFRepository
	{
		public MA_PCCFRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_PCCF> GetAll()
        {
            return ObjectSet.ToList();
        }

        public MA_PCCF GetByLabel(string label)
        {
            return ObjectSet.FirstOrDefault(p => p.LABEL.Contains(label));
        }

        public MA_PCCF GetByID(Guid ID)
        {
            return ObjectSet.FirstOrDefault(p => p.ID == ID);
        }

        public MA_PCCF GetByLabelProduct(string label, Guid product)
        {
            return ObjectSet.FirstOrDefault(p => p.LABEL.Contains(label));
        }
	}

	public interface IMA_PCCFRepository : IRepository<MA_PCCF>
	{

	}
}