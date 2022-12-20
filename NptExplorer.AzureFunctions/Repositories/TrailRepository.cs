using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NptExplorer.AzureFunctions.Context;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.AzureFunctions.Models.Transient;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Models;

namespace NptExplorer.AzureFunctions.Repositories;

public class TrailRepository : RepositoryBase<NptExplorerContext, Trail>, ITrailRepository
{
    private readonly NptExplorerContext _context;

    public TrailRepository(NptExplorerContext context) : base(context)
    {
        _context = context;
    }

    public List<Trail> GetTrailsByDistance(GeoPosition currentLocation, int? maxRecords, Dto.Models.FiltersDto filters)
    {
        var dbSet = _context.Trails.AsQueryable();
        dbSet = dbSet
            .Include(x => x.LocationTrails)
            .ThenInclude(x => x.Location)
            .ThenInclude(x => x.LocationHabitats);

        var allTrails = dbSet.ToList();
        var trails = allTrails.Select((x, i) => new TrailWithDistance()
        {
            Trail = x,
            Distance = new GeoPosition((double)x.StartLatitude, (double)x.StartLongitude)
                .DistanceTo(
                    currentLocation,
                    UnitOfLength.Miles
                )
        });

        var trailsDistance = FilterTrails(filters, trails);

        return maxRecords.HasValue ?
            trailsDistance.Where(x => x.Distance <= 5).Select(x => x.Trail).Take((int)maxRecords).ToList() :
            trailsDistance.Where(x => x.Distance <= 5).Select(x => x.Trail).ToList();
    }

    public List<Trail> GetTrailsBySearch(string searchPhrase, int? maxRecords, Dto.Models.FiltersDto filters)
    {
        var dbSet = _context.Trails.AsQueryable();
        dbSet = dbSet
            .Include(x => x.LocationTrails)
            .ThenInclude(x => x.Location)
            .ThenInclude(x => x.LocationHabitats);

        var trails =
            dbSet.Where(x => x.NameEnglish.Contains(searchPhrase) || x.NameWelsh.Contains(searchPhrase));

        trails = FilterTrails(filters, trails);

        return maxRecords.HasValue ?
            trails.Take((int)maxRecords).ToList() :
            trails.ToList();
    }

    public Trail GetTrail(int id)
    {
        var dbSet = _context.Trails.AsQueryable();
        dbSet = dbSet
            .Include(x => x.LocationTrails)
            .ThenInclude(x => x.Location)
            .Include(x => x.Badges);

        var trail = dbSet.FirstOrDefault(x => x.Id == id);
        return trail;
    }

    public List<Trail> GetDefaultTrails(int? maxRecords, Dto.Models.FiltersDto filters)
    {
        var dbSet = _context.Trails.AsQueryable();
        dbSet = dbSet
            .Include(x => x.LocationTrails)
            .ThenInclude(x => x.Location)
            .ThenInclude(x => x.LocationHabitats);

        var trails = maxRecords.HasValue ?
            dbSet.Where(x => x.DefaultSelection == true).Take((int)maxRecords) :
            dbSet.Where(x => x.DefaultSelection == true);

        return trails.ToList();
    }

    public List<Trail> GetTrailsPortal()
    {
        var dbSet = _context.Trails.AsQueryable();
        var trails = dbSet.ToList();

        return trails;
    }

    public bool UpdateDefaultTrails(int id)
    {
        var dbSet = _context.Trails.AsQueryable();
        var trail = dbSet.FirstOrDefault(x => x.Id == id);
        var result = false;
        if (trail != null)
        {
            if(!trail.DefaultSelection)
            {
                trail.DefaultSelection = true;
                _context.Trails.Update(trail);
                _context.SaveChanges();
                result = true;
            }
            else
            {
                trail.DefaultSelection = false;
                _context.Trails.Update(trail);
                _context.SaveChanges();
                result = true;
            }
           
        }

        return result;
    }

    private static IEnumerable<TrailWithDistance> FilterTrails(FiltersDto filters, IEnumerable<TrailWithDistance> trails)
    {
        if (filters == null)
        {
            return trails;
        }

        if (filters.DifficultyFilters != null && filters.DifficultyFilters.Any())
        {
            trails = trails.Where(x =>
                filters.DifficultyFilters.Contains(x.Trail.DifficultyId));
        }

        if (filters.DistanceFilters != null && filters.DistanceFilters.Any())
        {
            trails = trails.Where(x =>
                filters.DistanceFilters.Contains(x.Trail.DistanceId));
        }

        if (filters.TrailTimeFilters != null && filters.TrailTimeFilters.Any())
        {
            trails = trails.Where(x =>
                filters.TrailTimeFilters.Contains(x.Trail.TimeId));
        }

        return trails;
    }

    private static IQueryable<Trail> FilterTrails(FiltersDto filters, IQueryable<Trail> trails)
    {
        if (filters == null)
        {
            return trails;
        }

        if (filters.DifficultyFilters != null && filters.DifficultyFilters.Any())
        {
            trails = trails.Where(x =>
                x.Badges.Any(b => filters.DifficultyFilters.Contains(b.Trail.DifficultyId)));
        }

        if (filters.DistanceFilters != null && filters.DistanceFilters.Any())
        {
            trails = trails.Where(x =>
                x.Badges.Any(b => filters.DistanceFilters.Contains(b.Trail.DistanceId)));
        }

        if (filters.TrailTimeFilters != null && filters.TrailTimeFilters.Any())
        {
            trails = trails.Where(x =>
                x.Badges.Any(b => filters.TrailTimeFilters.Contains(b.Trail.TimeId)));
        }

        return trails;
    }
}