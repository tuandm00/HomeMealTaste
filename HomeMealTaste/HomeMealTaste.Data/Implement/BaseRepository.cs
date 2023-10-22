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

        public async Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression, bool completeSingle = false)
        {
            var result =await _context.Set<T>().Where(expression).ToListAsync();
            return result;
        }

        public  Task<T> GetFirstOrDefault(Expression<Func<T, bool>> expression, bool completeSingle = false)
        {
            return  Task.FromResult((_context.Set<T>().FirstOrDefault(expression)));
        }

        public async Task<T> Update(T entity, bool completeSingle = false)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
