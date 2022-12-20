using System.Threading.Tasks;

namespace NptExplorer.AzureFunctions.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        void Complete();
        Task CompleteAsync();
        TRepository Repository<TRepository>() where TRepository : IRepository;
    }
}