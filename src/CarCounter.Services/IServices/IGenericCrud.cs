using System.Linq.Expressions;

namespace CarCounter.Services.IServices
{
    public interface IGenericCrud<T> where T : class
    {
        public Task Create(T entity);
        public Task CreateRange(IEnumerable<T> entities);

        /// <summary>
        /// get single object
        /// </summary>
        /// <param name="expression">lambda expression</param>
        /// <param name="includes">include nested object</param>
        /// <returns></returns>
        public Task<T?> Get(Expression<Func<T, bool>> expression,
            List<string>? includes = null);

        /// <summary>
        /// get multiple object
        /// </summary>
        /// <param name="expression">lambda expression</param>
        /// <param name="orderBy">order by expression</param>
        /// <param name="includes">include nested object</param>
        /// <returns></returns>
        public Task<IList<T>?> GetAll(Expression<Func<T, bool>>? expression = null,
            Func<IQueryable<T>,
            IOrderedQueryable<T>>? orderBy = null,
            List<string>? includes = null);

        public Task Update(T entity);
        public Task Delete(object id);
        public Task DeleteRange(IEnumerable<T> entities);
        public Task<bool> DataExist(Expression<Func<T, bool>> expression);
        public Task<object> GetMax(Expression<Func<T, object>> selector);
        public Task<bool> Save();
    }
}
