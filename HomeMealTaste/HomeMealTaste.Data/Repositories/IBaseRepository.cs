
using System.Linq.Expressions;


namespace HomeMealTaste.Data.Repositories
{
    public interface IBaseRepository<T>
    {
        public Task<T> Create(T entity, bool completeSingle = false);
        public Task<T> Update(int id, bool completeSingle = false);
        public Task Delete(int id, bool completeSingle = false);
        public Task<IEnumerable<T>> GetAll(bool completeSingle = false);
        public Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression,bool completeSingle = false);
        public Task<T> GetFirstOrDefault (Expression<Func<T, bool>> expression,bool completeSingle = false);
    }
}
