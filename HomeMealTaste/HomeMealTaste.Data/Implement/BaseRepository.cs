using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


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

        public async Task<IEnumerable<T>> GetAll(bool completeSingle = false)
        {
            return await _context.Set<T>().ToListAsync();
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

        public async Task<T> Update(int id, bool completeSingle = false)
        {
            var entity =await _context.Set<T>().FindAsync(id);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
