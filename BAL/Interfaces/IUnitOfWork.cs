using DAL.models;
namespace BLLProject.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseClass;
        Task<int> CompleteAsync();
    }
}
