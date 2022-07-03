using CarCounter.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace CarCounter.Services.Services
{
    public class GenericCrud<T> : IGenericCrud<T> where T : class
    {
        private DbContext _db;
        private DbSet<T> _table;

        public GenericCrud(DbContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }

        public async Task Create(T entity)
        {
            await _table.AddAsync(entity);
        }

        public async Task CreateRange(IEnumerable<T> entities)
        {
            await _table.AddRangeAsync(entities);
        }

        public async Task Delete(object id)
        {
            var entity = await _table.FindAsync(id);
            _table.Remove(entity);
        }

        public async Task DeleteRange(IEnumerable<T> entities)
        {
            _table.RemoveRange(entities);
        }

        public async Task<T?> Get(Expression<Func<T, bool>> expression, List<string>? includes = null)
        {
            IQueryable<T> query = _table;
            if (includes is not null)
            {
                foreach (var includeProp in includes)
                {
                    query = query.Include(includeProp);
                }
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>>? expression = null,
            Func<IQueryable<T>,
            IOrderedQueryable<T>>? orderBy = null,
            List<string>? includes = null)
        {
            IQueryable<T> query = _table;

            if (expression is not null)
            {
                query = query.Where(expression);
            }

            if (includes is not null)
            {
                foreach (var includeProp in includes)
                {
                    query = query.Include(includeProp);
                }
            }

            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task Update(T entity)
        {
            _table.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
        }

        public async Task<bool> Save()
        {
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DataExist(Expression<Func<T, bool>> expression)
        {
            return _table.Any(expression);
        }

        public async Task<object> GetMax(Expression<Func<T, object>> selector)
        {
            return await _table.MaxAsync(selector);
        }
    }
}
