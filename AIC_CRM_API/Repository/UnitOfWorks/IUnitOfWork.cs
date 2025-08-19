
namespace Repository.UnitOfWorks
{
    public interface IUnitOfWork
    {
        //IRepository<User> UserRepo { get; }
        Task<object> SaveAsync();
    }
}