
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
        public Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression,bool completeSingle = false);
        public Task<T> GetFirstOrDefault (Expression<Func<T, bool>> expression,bool completeSingle = false);
    }
}
