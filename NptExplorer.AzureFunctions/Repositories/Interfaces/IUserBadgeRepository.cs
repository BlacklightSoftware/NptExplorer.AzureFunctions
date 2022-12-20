using System.Collections.Generic;
using NptExplorer.AzureFunctions.Models;

namespace NptExplorer.AzureFunctions.Repositories.Interfaces;

public interface IUserBadgeRepository : IRepository
{
    void PutUserBadge(int userId, int badgeId, bool checkedIn);
    List<UserBadge> GetByUser(int userId);
    bool GetTrailBadgeByUser(int userId, int trailId);
    List<int?> GetTrailCount(int userId);
    List<int> GetLocationCount(int userId);
}