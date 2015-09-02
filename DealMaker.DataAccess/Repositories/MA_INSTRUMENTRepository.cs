using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using KK.DealMaker.Core.Data;
using KK.DealMaker.Core.Constraint;

namespace KK.DealMaker.DataAccess.Repositories
{   
	public class MA_INSTRUMENTRepository : EFRepository<MA_INSTRUMENT>, IMA_INSTRUMENTRepository
	{
		public MA_INSTRUMENTRepository(DbContext context)
            : base(context)
		{
		}

        public List<MA_INSTRUMENT> GetAll()
        {
            return ObjectSet
                .Include(t => t.MA_PRODUCT)
                .Include(t => t.MA_FREQ_TYPE)
                .Include(t => t.MA_BOND_MARKET)
                .Include(t => t.MA_CURRENCY)
                .Include(t => t.MA_CURRENCY2)
                .ToList();
        }


        public List<MA_INSTRUMENT> GetAllByProductCode(string productcode)
        {
            return ObjectSet
                .Include(t => t.MA_PRODUCT)
                //.Include(t => t.DA_TRN)
                //.Include(t => t.MA_FREQ_TYPE)
                .Where(p => p.ISACTIVE == true && p.MA_PRODUCT.LABEL.Replace(" ", string.Empty).Equals(productcode))
                .ToList();
        }

        public MA_INSTRUMENT GetByLabel(string label)
        {
            return ObjectSet
                .Include(t => t.MA_PRODUCT)
                .Include(t => t.DA_TRN)
                .Include(t => t.MA_FREQ_TYPE)
                .FirstOrDefault(p => p.ISACTIVE == true && p.LABEL.Equals(label));
        }


        public MA_INSTRUMENT GetByID(Guid ID)
        {
            return ObjectSet
                .Include(t => t.MA_PRODUCT)
                .Include(t => t.MA_FREQ_TYPE)
                .Include(t => t.MA_BOND_MARKET)
                .Include(t => t.MA_CURRENCY)
                .Include(t => t.MA_CURRENCY2)
                .FirstOrDefault(p => p.ISACTIVE == true && p.ID.Equals(ID));
        }

        public MA_INSTRUMENT GetByIDWithOutInclude(Guid ID)
        {
            return ObjectSet
                .FirstOrDefault(p => p.ISACTIVE == true && p.ID.Equals(ID));
        }

        public MA_INSTRUMENT GetByIDWithInsMarket(Guid ID)
        {
            return ObjectSet
                .Include(t => t.MA_BOND_MARKET)
                .FirstOrDefault(p => p.ISACTIVE == true && p.ID.Equals(ID));
        }
	}

	public interface IMA_INSTRUMENTRepository : IRepository<MA_INSTRUMENT>
	{

	}
}