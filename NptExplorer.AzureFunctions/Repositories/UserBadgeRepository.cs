using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NptExplorer.AzureFunctions.Context;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.AzureFunctions.Repositories.Interfaces;

namespace NptExplorer.AzureFunctions.Repositories;

public class UserBadgeRepository : RepositoryBase<NptExplorerContext, UserBadge>, IUserBadgeRepository
{
    private readonly NptExplorerContext _context;

    public UserBadgeRepository(NptExplorerContext context) : base(context)
    {
        _context = context;
    }

    public void PutUserBadge(int userId, int badgeId, bool checkedIn)
    {
        var userBadge = new UserBadge { UserId = userId, BadgeId = badgeId, CheckedIn = checkedIn };
        _context.Add(userBadge);
        _context.SaveChanges();
    }

    public List<UserBadge> GetByUser(int userId)
    {
        var dbSet = _context.UserBadges.AsQueryable();
        var userBadges = dbSet.Where(x => x.UserId == userId);
        return userBadges.ToList();
    }

    public bool GetTrailBadgeByUser(int userId, int trailId)
    {
        var dbSet = _context.UserBadges.AsQueryable();
        dbSet = dbSet.Include(x => x.Badge);
        var userBadge = dbSet.FirstOrDefault(x => x.UserId == userId && x.Badge.TrailId == trailId);
        return userBadge != null;
    }

    public List<int> GetLocationCount(int userId)
    {
        var dbSet = _context.Badges.AsQueryable()
        .Include(x => x.UserBadges);
        var dbSetBadges = _context.UserBadges.AsQueryable()
            .Include(x => x.Badge);
        var dbSetLocation = _context.Locations.AsQueryable();

        var resultBadge = dbSetBadges.Where(x => x.UserId == userId).Select(x => x.Badge).ToList();
        var resultLocationList = resultBadge.Select(x => x.LocationId).ToList();

        return resultLocationList;
    }

    public List<int?> GetTrailCount(int userId)
    {
        var dbSet = _context.Badges.AsQueryable()
          .Include(x => x.UserBadges);
        var dbSetBadges = _context.UserBadges.AsQueryable()
            .Include(x => x.Badge);
        var dbSetTrail = _context.Trails.AsQueryable();

        var resultBadge = dbSetBadges.Where(x => x.UserId == userId).Select(x => x.Badge).ToList();
        var resultTrailList = resultBadge.Select(x => x.TrailId).ToList();

        return resultTrailList;
    }
}