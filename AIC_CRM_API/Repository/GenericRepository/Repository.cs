
namespace Repository.GenericRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MainContext _context;
        private DbSet<T> _dbSet;

        public Repository(MainContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity is null) throw new ArgumentNullException("entity");
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<List<T>> AddRangeAsync(List<T> entity)
        {
            if (entity is null) throw new ArgumentNullException("entity");
            await _dbSet.AddRangeAsync(entity);
            return entity;
        }

        public T Update(T entity)
        {
            if (entity is null) throw new ArgumentNullException("entity");
            _dbSet.Update(entity);
            return entity;
        }

        public List<T> UpdateRange(List<T> entity)
        {
            if (entity is null) throw new ArgumentNullException("entity");
            _dbSet.UpdateRange(entity);
            return entity;
        }

        public async Task<T> GetAsync(T id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public void Remove(T entity)
        {
            if (entity is null) throw new ArgumentNullException("entity");
            _dbSet.Remove(entity);
        }

        public async Task RemoveAsync(Expression<Func<T, bool>> expression)
        {
            await _dbSet.Where(expression).ExecuteDeleteAsync();
        }

        public void RemoveRange(List<T> entities)
        {
            if (entities.Any())
            {
                _dbSet.RemoveRange(entities);
            }
            else
                throw new ArgumentNullException("entities");
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).LongCountAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> MaxAsync(Expression<Func<T, int>> predicate)
        {
            return await _dbSet
            .Select(predicate)
            .DefaultIfEmpty()
            .MaxAsync();
        }

        public async Task<string> MaxAsync(Expression<Func<T, string>> predicate)
        {
            return await _dbSet
            .Select(predicate)
            .DefaultIfEmpty()
            .MaxAsync();
        }

        public async Task<int> MaxAsync(Expression<Func<T, bool>> where, Expression<Func<T, int>> predicate)
        {
            return await _dbSet
            .Where(where)
            .Select(predicate)
            .DefaultIfEmpty()
            .MaxAsync();
        }

        public async Task<string> MaxAsync(Expression<Func<T, bool>> where, Expression<Func<T, string>> predicate)
        {
            return await _dbSet
            .Where(where)
            .Select(predicate)
            .DefaultIfEmpty()
            .MaxAsync();
        }
    }

}
