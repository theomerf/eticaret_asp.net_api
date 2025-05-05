using Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T>
    where T : class, new()
    {
        protected readonly RepositoryContext _context;

        protected RepositoryBase(RepositoryContext context)
        {
            _context = context;
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return trackChanges
            ? _context.Set<T>()
            : _context.Set<T>().AsNoTracking();
        }

        public int Count(bool trackChanges)
        {
            return trackChanges
                ? _context.Set<T>().Count()
                : _context.Set<T>().AsNoTracking().Count();
        }

        public T? FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return trackChanges
                ? _context.Set<T>().Where(expression).SingleOrDefault()
                : _context.Set<T>().Where(expression).AsNoTracking().SingleOrDefault();
        }

        public IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return trackChanges
                ? _context.Set<T>().Where(expression)
                : _context.Set<T>().Where(expression).AsNoTracking();
        }


        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
