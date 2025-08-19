
namespace Repository.UnitOfWorks
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private bool disposed = false;
        private readonly MainContext _context;
        public UnitOfWork(MainContext context)
        {
            _context = context;
        }

        //private IRepository<User> _userRepo;
        
        //public IRepository<User> UserRepo
        //{
        //    get
        //    {
        //        return _userRepo ??
        //            (_userRepo = new Repository<User>(_context));
        //    }
        //}
      

        public async Task<object> SaveAsync()
        {
            var res = await _context.SaveChangesAsync();
            return res;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}
