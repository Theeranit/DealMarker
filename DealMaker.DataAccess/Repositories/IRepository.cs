

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace KK.DealMaker.DataAccess.Repositories
{ 
	public interface IRepository<T> 
	{
		//IUnitOfWork UnitOfWork { get; set; }
		IQueryable<T> All();
		IQueryable<T> Where(Expression<Func<T, bool>> expression);
		void Add(T entity);
		void Delete(T entity);
		T GetById(long id);
        T GetById(string id);
        T GetById(Guid id);

	}
}

