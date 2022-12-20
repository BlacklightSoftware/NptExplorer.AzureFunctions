using Microsoft.EntityFrameworkCore;

namespace NptExplorer.AzureFunctions.Repositories
{
    public abstract class RepositoryBase<TContext, T>
        where T : class
        where TContext : DbContext
    {

        private readonly TContext _baseContext;

        protected RepositoryBase(TContext context)
        {
            _baseContext = context;
        }
    }
}