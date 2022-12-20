using NptExplorer.AzureFunctions.Models;
using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Repositories.Interfaces;

public interface IBadgeRepository : IRepository
{
    List<Badge> GetBadges();
}