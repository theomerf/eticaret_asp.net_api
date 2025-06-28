using System.Linq.Expressions;

namespace Repositories.Contracts
{
    public interface IRepositoryBase<T>{
        IQueryable<T> FindAll(bool trackChanges);

        IQueryable<T?> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);

        IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> expression, bool trackChanges);

        void Create(T entity);

        void Remove(T entity);

        void Update(T entity);

        Task<int> Count(bool trackChanges);
    }
}