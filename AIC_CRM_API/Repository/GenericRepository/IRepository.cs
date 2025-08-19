
namespace Repository.GenericRepository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> AsQueryable();

        Task<T> AddAsync(T entity);
        Task<List<T>> AddRangeAsync(List<T> entity);

        T Update(T entity);
        List<T> UpdateRange(List<T> entity);

        Task<T> GetAsync(T id);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        void Remove(T entity);
        Task RemoveAsync(Expression<Func<T, bool>> predicate);
        void RemoveRange(List<T> entities);

        Task<long> CountAsync(Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<int> MaxAsync(Expression<Func<T, int>> predicate);
        Task<string> MaxAsync(Expression<Func<T, string>> predicate);
        Task<int> MaxAsync(Expression<Func<T, bool>> where, Expression<Func<T, int>> predicate);
        Task<string> MaxAsync(Expression<Func<T, bool>> where, Expression<Func<T, string>> predicate);
    }
}
