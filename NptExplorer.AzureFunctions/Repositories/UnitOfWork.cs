using Microsoft.EntityFrameworkCore;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace NptExplorer.AzureFunctions.Repositories;

public class UnitOfWork<T> : IUnitOfWork
        where T : DbContext
{
    private readonly T _context;

    private readonly IServiceProvider _serviceProvider;

    public UnitOfWork(T context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public TRepository Repository<TRepository>()
        where TRepository : IRepository
    {
        return _serviceProvider.GetRequiredService<TRepository>();
    }

    public void Complete()
    {
        _context.SaveChanges();
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task CompleteAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}