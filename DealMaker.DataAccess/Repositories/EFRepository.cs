using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;


namespace KK.DealMaker.DataAccess.Repositories
{
	public abstract class EFRepository<T> : IRepository<T> where T : class
	{
		private DbContext _context;

		public EFRepository(DbContext context)
		{
			_context = context;
		}
		
		private IDbSet<T> _objectset;
		public IDbSet<T> ObjectSet
		{
			get
			{
				if (_objectset == null)
				{
					_objectset = _context.Set<T>();
				}
				return _objectset;
			}
		}

		public virtual IQueryable<T> All()
		{
			return ObjectSet.AsQueryable();
		}

		public IQueryable<T> Where(Expression<Func<T, bool>> expression)
		{
			return ObjectSet.Where(expression);
		}

		public void Add(T entity)
		{
			ObjectSet.Add(entity);
		}

		public void Delete(T entity)
		{
			ObjectSet.Remove(entity);
		}

		public virtual T GetById(long id)
        {
            return ObjectSet.Find(id);
        }

        public virtual T GetById(string id)
        {
            return ObjectSet.Find(id);
        }

        public virtual T GetById(Guid id)
        {
            return ObjectSet.Find(id);
        }

	}
}