
using System.Linq.Expressions;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Services.Helper;


namespace HomeMealTaste.Data.Repositories
{
    public interface IBaseRepository<T>
    {
        public Task<T> Create(T entity, bool completeSingle = false);
        public Task<T> Update(T entity, bool completeSingle = false);
        public Task Delete(int id, bool completeSingle = false);
        public IQueryable<T> GetAll(bool completeSingle = false);
        public Task<PagedList<T>> GetWithPaging(PagingParams pagingParams);
        public Task<PagedList<TResponse>> GetWithPaging<TResponse>(PagingParams pagingParams, Expression<Func<T, bool>> conditionExpression, Expression<Func<T, TResponse>> selectExpression, params Expression<Func<T, object>>[] includes);
        public Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression,bool completeSingle = false);
        public Task<T> GetFirstOrDefault(Expression<Func<T, bool>> expression,
            Expression<Func<T, object>> include = null, bool completeSingle = false);
    }
}
