using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Services.Helper;


namespace HomeMealTaste.Data.Implement
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly HomeMealTasteContext _context;

        public BaseRepository(HomeMealTasteContext context)
        {
            _context = context;
        }

        public async Task<T> Create(T entity, bool completeSingle = false)
        {
            var created = await _context.Set<T>().AddAsync(entity);
            if (completeSingle)
            {
                await _context.SaveChangesAsync();
            }
            return created.Entity;
        }

        public async Task Delete(int id, bool completeSingle = false)
        {
           var entity = await _context.Set<T>().FindAsync(id);

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();

        }

        public IQueryable<T> GetAll(bool completeSingle = false)
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<PagedList<T>> GetWithPaging(PagingParams pagingParams)
        {
            var dataSource = GetAll();
            return await PagedList<T>.ToPagedList(dataSource, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<PagedList<TResponse>> GetWithPaging<TResponse>(PagingParams pagingParams, Expression<Func<T, bool>> conditionExpression, Expression<Func<T, TResponse>> selectExpression, params Expression<Func<T, object>>[] includes)
        {
            var queryable = _context.Set<T>().Where(conditionExpression).AsNoTracking();
            
            if (includes != null && includes.Any())
            {
                queryable = includes.Aggregate(queryable, (current, include) => current.Include(include));
            }
            
            return await PagedList<TResponse>.ToPagedList(queryable.Select(selectExpression), pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression, bool completeSingle = false)
        {
            var result =await _context.Set<T>().Where(expression).ToListAsync();
            return result;
        }

        public Task<T> GetFirstOrDefault(Expression<Func<T, bool>> expression, Expression<Func<T, object>> include = null, bool completeSingle = false)
        {
            var query = _context.Set<T>().AsQueryable();
            if (include != null)
            {
                query = query.Include(include);
            }
            return Task.FromResult(query.FirstOrDefault(expression));
        }

        public async Task<T> Update(T entity, bool completeSingle = false)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
