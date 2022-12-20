using System.Collections.Generic;
using System.Linq;
using NptExplorer.AzureFunctions.Context;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.AzureFunctions.Repositories.Interfaces;

namespace NptExplorer.AzureFunctions.Repositories;

public class BadgeRepository : RepositoryBase<NptExplorerContext, Badge>, IBadgeRepository
{
    private readonly NptExplorerContext _context;

    public BadgeRepository(NptExplorerContext context) : base(context)
    {
        _context = context;
    }

    public List<Badge> GetBadges()
    {
        var dbSet = _context.Badges.AsQueryable();
        return dbSet.ToList();
    }
}