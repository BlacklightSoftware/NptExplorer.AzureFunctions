using NptExplorer.AzureFunctions.Models;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NptExplorer.AzureFunctions.Models.Transient;
using NptExplorer.AzureFunctions.Context;
using System.Linq;
using NptExplorer.Dto.Models;

namespace NptExplorer.AzureFunctions.Repositories;

public class ChallengeRepository : RepositoryBase<NptExplorerContext, Location>, IChallengeRepository
{
    private readonly NptExplorerContext _context;

    public ChallengeRepository(NptExplorerContext context) : base(context)
    {
        _context = context;
    }

    public List<Location> GetChallengesByDistance(GeoPosition currentLocation, int? maxRecords, FiltersDto filters)
    {
        var trailIds = GetTrailsByDistance(currentLocation).Select(x => x.TrailId).ToList();
        var poiIds = GetPoisByDistance(currentLocation).Select(x => x.PointOfInterestId).ToList();

        var dbSet = _context.Badges.AsQueryable();
        var badgesWithinFiveMiles =
            dbSet.Where(x => poiIds.Contains((int)x.PointOfInterestId) || trailIds.Contains((int)x.TrailId)).Select(x => x.Id);

        var locationDbSet = _context.Locations.AsQueryable();
        var locations = locationDbSet
            .Include(x => x.Badges)
            .ThenInclude(x => x.PointOfInterest)
            .Include(x => x.Badges)
            .ThenInclude(x => x.Trail)
            .Include (x => x.LocationHabitats)
            .Where(x => x.Badges.Any(b => badgesWithinFiveMiles.Contains(b.Id)));

        var locationsDistance = FilterChallenges(filters, locations);

        return maxRecords.HasValue ?
            locationsDistance.Take((int)maxRecords).ToList() :
            locationsDistance.ToList();
    }

    public List<Location> GetChallengesBySearch(string searchPhrase, int? maxRecords, FiltersDto filters)
    {
        var dbSet = _context.Locations.AsQueryable();
        dbSet = dbSet
            .Include(x => x.Badges)
            .ThenInclude(x => x.PointOfInterest)
            .Include(x => x.Badges)
            .ThenInclude(x => x.Trail)
            .Include (x => x.LocationHabitats);

        var challenges = string.IsNullOrEmpty(searchPhrase) ? dbSet : dbSet.Where(x => x.NameEnglish.Contains(searchPhrase) || x.NameWelsh.Contains(searchPhrase));

        challenges = FilterChallenges(filters, challenges);

        return maxRecords.HasValue ?
            challenges.Take((int)maxRecords).ToList() :
            challenges.ToList();
    }

    public Location GetChallenge(int id)
    {
        var dbSet = _context.Locations.AsQueryable();
        dbSet = dbSet
            .Include(x => x.Badges)
            .ThenInclude(x => x.PointOfInterest);
        var challenge = dbSet.FirstOrDefault(x => x.Id == id);
        return challenge;
    }

    public List<Location> GetDefaultChallenges(int? maxRecords, FiltersDto filters)
    {
        var dbSet = _context.Locations.AsQueryable();
        dbSet = dbSet
            .Include(x => x.Badges)
            .ThenInclude(x => x.PointOfInterest)
            .Include(x => x.Badges)
            .ThenInclude(x => x.Trail)
            .Include (x => x.LocationHabitats);

        return maxRecords.HasValue ? 
            dbSet.Take((int)maxRecords).ToList() : 
            dbSet.ToList();
    }
    
    private static IQueryable<Location> FilterChallenges(FiltersDto filters, IQueryable<Location> challenges)
    {
        if (filters == null)
        {
            return challenges;
        }

        if (filters.BadgeFilters != null && filters.BadgeFilters.Any())
        {
            challenges = challenges.Where(x => x.Badges.Any(b => filters.BadgeFilters.Contains(b.BadgeTypeId)));
        }

        if (filters.HabitatFilters != null && filters.HabitatFilters.Any())
        {
            challenges = challenges.Where(x => x.LocationHabitats.Any(h => filters.HabitatFilters.Contains(h.HabitatId)));
        }

        return challenges;
    }

    private IEnumerable<ChallengeDistance> GetTrailsByDistance(GeoPosition currentLocation)
    {
        var dbSet = _context.Trails.AsQueryable();
        dbSet = dbSet
            .Include(x => x.LocationTrails)
            .ThenInclude(x => x.Location);

        var allTrails = dbSet.ToList();
        var trails = allTrails.Select(x => new ChallengeDistance()
        {
            TrailId = x.Id,
            Distance = new GeoPosition((double)x.StartLatitude, (double)x.StartLongitude)
                .DistanceTo(
                    currentLocation,
                    UnitOfLength.Miles
                )
        }).ToList();

        return trails.Where(x => x.Distance <= 5).ToList();
    }

    private IEnumerable<ChallengeDistance> GetPoisByDistance(GeoPosition currentLocation)
    {
        var dbSet = _context.PointOfInterests.AsQueryable();
        dbSet = dbSet
            .Include(x => x.LocationPointOfInterests)
            .ThenInclude(x => x.Location);

        var allPois = dbSet.ToList();
        var pois = allPois.Select(x => new ChallengeDistance()
        {
            PointOfInterestId = x.Id,
            Distance = new GeoPosition((double)x.Latitude, (double)x.Longitude)
                .DistanceTo(
                    currentLocation,
                    UnitOfLength.Miles
                )
        }).ToList();

        return pois.Where(x => x.Distance <= 5).ToList();
    }

    public List<PointOfInterest> GetAllPointOfIntrests()
    {
        var dbSet = _context.PointOfInterests.AsQueryable();
        var allPoi = dbSet.ToList();

        return allPoi;
    }

    public void UpdatePointOfInterest(PointOfInterest pointOfInterest)
    {
        var dbSet = _context.PointOfInterests.AsQueryable();

        var foundPoi = dbSet.FirstOrDefault(b => b.Id == pointOfInterest.Id);

        if (foundPoi != null)
        {
            foundPoi.DescriptionEnglish = pointOfInterest.DescriptionEnglish;
            foundPoi.DescriptionWelsh = pointOfInterest.DescriptionWelsh;
            foundPoi.Image = pointOfInterest.Image;
            _context.PointOfInterests.Update(foundPoi);
            _context.SaveChanges();
        }
    }
}