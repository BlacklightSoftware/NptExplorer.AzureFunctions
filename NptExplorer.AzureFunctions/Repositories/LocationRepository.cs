using NptExplorer.AzureFunctions.Context;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.AzureFunctions.Models.Transient;
using System;
using NptExplorer.Dto.Models;
using DefaultLocations = NptExplorer.AzureFunctions.Models.DefaultLocation;

namespace NptExplorer.AzureFunctions.Repositories;

public class LocationRepository : RepositoryBase<NptExplorerContext, Location>, ILocationRepository
{
    private readonly NptExplorerContext _context;

    public LocationRepository(NptExplorerContext context) : base(context)
    {
        _context = context;
    }

    public List<Location> GetLocations()
    {
        var dbSet = _context.Locations.AsQueryable();
        var locations = dbSet.ToList();
        return locations;
    }

    public Location GetLocation(int Id)
    {
        var dbSet = _context.Locations.AsQueryable();
        dbSet = dbSet
            .Include(x => x.LocationFacilities)
            .Include(x => x.LocationActivities)
            .Include(x => x.LocationHabitats)
            .Include(x => x.LocationBusRoutes)
            .ThenInclude(b => b.BusRoute)
            .Include(x => x.LocationHighlights);

        var location = dbSet.FirstOrDefault(x => x.Id == Id);
        return location;
    }

    public List<Location> GetSearchedLocation(string searchPhrase, int? maxRecords, ExploreFiltersDto filters)
    {
        var dbSet = _context.Locations.AsQueryable();

        dbSet = dbSet
            .Include(x => x.LocationFacilities)
            .Include(x => x.LocationActivities);

        var locations = dbSet.Where(x => x.NameEnglish.Contains(searchPhrase) || x.NameWelsh.Contains(searchPhrase));

        locations = FilterLocations(filters, locations);

        return maxRecords.HasValue ?
            locations.Take((int)maxRecords).ToList() :
            locations.ToList();
    }

    public List<Location> GetSearchedPortalLocation(string searchPhrase)
    {
        var dbSet = _context.Locations.AsQueryable();

        var locations = dbSet.Where(x => x.NameEnglish.Contains(searchPhrase)).ToList();

        if (locations.Any())
        {
            return locations;
        }
        return new List<Location>();
    }

    public List<Location> GetLocationOverview()
    {
        var dbset = _context.Locations.AsQueryable();
        dbset = dbset.Include(x => x.LocationFacilities);

        return dbset.ToList();
    }

    public List<Location> GetLocationByDistance(GeoPosition currentLocation, int? maxRecords, Dto.Models.ExploreFiltersDto filters)
    {
        var dbSet = _context.Locations.AsQueryable();

        dbSet = dbSet
            .Include(x => x.LocationFacilities)
            .Include(x => x.LocationActivities);

        var allLocations = dbSet.ToList();
        var locations = allLocations.Select((x, i) => new LocationWithDistance()
        {
            Location = x,
            Distance = new GeoPosition((double)x.Latitude, (double)x.Longitude)
                .DistanceTo(
                    currentLocation,
                    UnitOfLength.Miles
                )
        });

        var trailsDistance = FilterLocations(filters, locations);

        return maxRecords.HasValue ?
            trailsDistance.Where(x => x.Distance <= 5).Select(x => x.Location).Take((int)maxRecords).ToList() :
            trailsDistance.Where(x => x.Distance <= 5).Select(x => x.Location).ToList();
    }

    public List<Location> GetDefaultLocations()
    {
        var dbSet = _context.Locations.AsQueryable();

        var defaultLocations = Enum.GetValues(typeof(DefaultLocation)).Cast<int>();

        var locations = dbSet.Where(l => defaultLocations.Contains(l.Id)).ToList();

        return locations;
    }

    public bool RemoveLocation(int Id)
    {
        var result = false;
        try
        {
            var dbSet = _context.Locations.AsQueryable();
            var location = dbSet.FirstOrDefault(l => l.Id == Id);
            DeleteLocation(location.Id);
        }
        catch(Exception ex)
        {
            var message = ex.Message;
        }
        return result;
  
    }

    public bool UpdateLocation(Location location)
    {
        var result = false;

        try
        {
            var dbSet = _context.Locations.AsQueryable();
            var foundLocation = dbSet.First(l => l.Id == location.Id);

            foundLocation.NameEnglish = location.NameEnglish;
            foundLocation.NameWelsh = location.NameWelsh;
            foundLocation.DescriptionEnglish = location.DescriptionEnglish;
            foundLocation.DescriptionWelsh = location.DescriptionWelsh;
            foundLocation.MapImage = location.MapImage;
            foundLocation.Type = location.Type;
            foundLocation.Address = location.Address;
            foundLocation.Website = location.Website;
            foundLocation.NearestBusStop = location.NearestBusStop;
            foundLocation.ResourceLink = location.ResourceLink;
            foundLocation.PrimaryImage = location.PrimaryImage;

            _context.Locations.Update(foundLocation);
            _context.SaveChanges();
            result = true;
        }
        catch (Exception ex)
        {
            var message = ex.Message;
        }
        return result;
    }

    public bool RemoveLocationItem(LocationItemSumRequest locationItem)
    {
        var result = false;
        if(locationItem.Area == "Facilities")
        {
            try
            {
                var dbSet = _context.LocationFacilities.AsQueryable();
                var hasLocation = dbSet.Where(l => l.FacilityId == int.Parse(locationItem.SelectedValue) && l.LocationId == locationItem.Id).Any();

                if (hasLocation)
                {
                    var locationToRemove = dbSet.First(l => l.FacilityId == int.Parse(locationItem.SelectedValue) && l.LocationId == locationItem.Id);
                    _context.LocationFacilities.Remove(locationToRemove);
                    _context.SaveChanges();
                    result = true;
                }

            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }

        if (locationItem.Area == "Habitats")
        {
            try
            {
                var dbSet = _context.LocationHabitats.AsQueryable();
                var hasLocation = dbSet.Where(l => l.HabitatId == int.Parse(locationItem.SelectedValue) && l.LocationId == locationItem.Id).Any();

                if (hasLocation)
                {
                    var locationToRemove = dbSet.First(l => l.HabitatId == int.Parse(locationItem.SelectedValue) && l.LocationId == locationItem.Id);
                    _context.LocationHabitats.Remove(locationToRemove);
                    _context.SaveChanges();
                    result = true;
                }

            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }

        if (locationItem.Area == "Activities")
        {
            try
            {
                var dbSet = _context.LocationActivities.AsQueryable();
                var hasLocation = dbSet.Where(l => l.ActivityId == int.Parse(locationItem.SelectedValue) && l.LocationId == locationItem.Id).Any();

                if (hasLocation)
                {
                    var locationToRemove = dbSet.First(l => l.ActivityId == int.Parse(locationItem.SelectedValue) && l.LocationId == locationItem.Id);
                    _context.LocationActivities.Remove(locationToRemove);
                    _context.SaveChanges();
                    result = true;
                }

            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }

        return result;
    }
    
    public bool AddLocationItem(LocationItemSumRequest locationItem)
    {
        var result = false;
        if (locationItem.Area == "Facilities")
        {
            try
            {
                var dbSet = _context.LocationFacilities.AsQueryable();
                var hasLocation = dbSet.Where(l => l.FacilityId == int.Parse(locationItem.SelectedValue) && l.LocationId == locationItem.Id).Any();

                if (!hasLocation)
                {
                    var locationFacility = new LocationFacility()
                    {
                        FacilityId = int.Parse(locationItem.SelectedValue),
                        LocationId = locationItem.Id,
                    };
                    _context.LocationFacilities.Add(locationFacility);
                    _context.SaveChanges();
                }
                else
                {
                    result = false;
                }

                result = true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return result;
        }

        if (locationItem.Area == "Habitats")
        {
            try
            {
                var dbSet = _context.LocationHabitats.AsQueryable();
                var hasLocation = dbSet.Where(l => l.HabitatId == int.Parse(locationItem.SelectedValue) && l.LocationId == locationItem.Id).Any();

                if (!hasLocation)
                {
                    var locationHabitat = new LocationHabitat()
                    {
                        HabitatId = int.Parse(locationItem.SelectedValue),
                        LocationId = locationItem.Id,
                    };
                    _context.LocationHabitats.Add(locationHabitat);
                    _context.SaveChanges();
                }
                else
                {
                    result = false;
                }

                result = true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return result;
        }

        if (locationItem.Area == "Activities")
        {
            try
            {
                var dbSet = _context.LocationActivities.AsQueryable();
                var hasLocation = dbSet.Where(l => l.ActivityId == int.Parse(locationItem.SelectedValue) && l.LocationId == locationItem.Id).Any();

                if (!hasLocation)
                {
                    var locationActivity = new LocationActivity()
                    {
                        ActivityId = int.Parse(locationItem.SelectedValue),
                        LocationId = locationItem.Id,
                    };
                    _context.LocationActivities.Add(locationActivity);
                    _context.SaveChanges();
                }
                else
                {
                    result = false;
                }

                result = true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return result;
        }
        return result;
    }

    public bool AddNewLocation(Location location)
    {
        var result = false;
        try
        {
            var dbSet = _context.Locations.AsQueryable();
            var existingLocation = dbSet.Where(l => l.NameEnglish == location.NameEnglish);

            if (existingLocation.Any())
            {
                result = false;
            }
            else
            {
                _context.Locations.Add(location);
                _context.SaveChanges();
            }
            result = true;
        }
        catch (Exception ex)
        {
            var message = ex.Message;
        }
        return result;
    }

    public List<Location> GetLocationsBySearch(string searchPhrase, int? maxRecords, ExploreFiltersDto filters)
    {
        var dbSet = _context.Locations.AsQueryable();

        dbSet = dbSet
            .Include(x => x.LocationFacilities)
            .Include(x => x.LocationActivities);

        var locations = string.IsNullOrEmpty(searchPhrase) ? dbSet : dbSet.Where(x => x.NameEnglish.Contains(searchPhrase) || x.NameWelsh.Contains(searchPhrase));

        locations = FilterLocations(filters, locations);

        return maxRecords.HasValue ?
            locations.Take((int)maxRecords).ToList() :
            locations.ToList();
    }

    public enum DefaultLocation
    {
        CraigCuliadus = 4,
        PantYSais = 8,
        BritonFerryWoods = 10,
        NeathAndTenantCanals = 13,
        NeathAbbeyIronWorks = 47,
    }

    private static IEnumerable<LocationWithDistance> FilterLocations(ExploreFiltersDto filters, IEnumerable<LocationWithDistance> locations)
    {
        if (filters == null)
        {
            return locations;
        }

        if (filters.ActivitiesFilters != null && filters.ActivitiesFilters.Any())
        {
            locations = locations.Where(x => x.Location.LocationActivities.Any(
                l => filters.ActivitiesFilters.Contains(l.ActivityId)));
        }

        if (filters.FacilitiesFilters != null && filters.FacilitiesFilters.Any())
        {
            locations = locations.Where(x => x.Location.LocationFacilities.Any(
                l => filters.FacilitiesFilters.Contains(l.FacilityId)));
        }

        return locations;
    }

    private static IQueryable<Location> FilterLocations(ExploreFiltersDto filters, IQueryable<Location> locations)
    {
        if (filters == null)
        {
            return locations;
        }

        if (filters.ActivitiesFilters != null && filters.ActivitiesFilters.Any())
        {
            locations = locations.Where(x => x.LocationActivities.Any(
                lt => lt.Location.LocationActivities.Any(
                    l => filters.ActivitiesFilters.Contains(l.ActivityId))));
        }

        if (filters.FacilitiesFilters != null && filters.FacilitiesFilters.Any())
        {
            locations = locations.Where(x => x.LocationFacilities.Any(
                lt => lt.Location.LocationFacilities.Any(
                    l => filters.FacilitiesFilters.Contains(l.FacilityId))));
        }

        return locations;
    }

    public List<Location> GetDefaultLocationsPortal() 
    {
        var defaultLocations = _context.DefaultLocations.AsQueryable().ToList();
        var allLocations = _context.Locations.AsQueryable().ToList();

        var locations = new List<Location>();

        foreach (var location in allLocations)
        {
            foreach (var defaultLocation in defaultLocations)
            {
                if (defaultLocation.LocationId == location.Id)
                {
                    locations.Add(location);
                }
            }
        }

        return locations;
    }

    public bool AddDefaultLocationsPortal(int locationId)
    {
        var location = new DefaultLocations()
        {
            LocationId = locationId,
        };
        var result = false;

        var defaultLocations = _context.DefaultLocations.AsQueryable().ToList();

        var existingLocation = defaultLocations.Where(l => l.LocationId == locationId);

        if (existingLocation.Any()) 
        {
            result = false;
        }
        else
        {
            _context.DefaultLocations.Add(location);
            _context.SaveChanges();
            result = true;
        }

        return result;
    }

    public bool RemoveDefaultLocationPortal(int locationId)
    {
        var location = new DefaultLocations()
        {
            LocationId = locationId,
        };
        var result = false;

        var defaultLocations = _context.DefaultLocations.AsQueryable().ToList();

        var existingLocation = defaultLocations.Where(l => l.LocationId == locationId);

        if (existingLocation.Any())
        {
            var locationToRemove = defaultLocations.FirstOrDefault(l => l.LocationId == locationId);
            _context.DefaultLocations.Remove(locationToRemove);
            _context.SaveChanges();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    public bool UpdateDefaultChallenges(int id)
    {
        var dbSet = _context.Locations.AsQueryable();
        var location = dbSet.FirstOrDefault(x => x.Id == id);
        var result = false;
        if (location != null)
        {
            if (!location.ChallengeDefaultSelection)
            {
                location.ChallengeDefaultSelection = true;
                _context.Locations.Update(location);
                _context.SaveChanges();
                result = true;
            }
            else
            {
                location.ChallengeDefaultSelection = false;
                _context.Locations.Update(location);
                _context.SaveChanges();
                result = true;
            }

        }

        return result;
    }
    public void DeleteLocation(int locationId)
    {
        var location = GetLocation(locationId);
        if(location == null)
        {
            return;
        }


        var dbActivitySet = _context.LocationActivities.AsQueryable();
        var activities = dbActivitySet.Where(a => a.LocationId == locationId);
        if (activities.Any())
        {
            _context.Set<LocationActivity>().RemoveRange(activities);
        }

        var dbBusRoutes = _context.LocationBusRoutes.AsQueryable();
        var busRoutes = dbBusRoutes.Where(a => a.LocationId == locationId);
        if (busRoutes.Any())
        {
            _context.Set<LocationBusRoute>().RemoveRange(busRoutes);
        }

        var dbFacility = _context.LocationFacilities.AsQueryable();
        var facilities = dbFacility.Where(a => a.LocationId == locationId);
        if (facilities.Any())
        {
            _context.Set<LocationFacility>().RemoveRange(facilities);
        }

        var dbHabitat = _context.LocationHabitats.AsQueryable();
        var habitat = dbHabitat.Where(a => a.LocationId == locationId);
        if (habitat.Any())
        {
            _context.Set<LocationHabitat>().RemoveRange(habitat);
        }

        var dbPointOfInterest = _context.LocationPointOfInterests.AsQueryable();
        var pointsOfInterest = dbPointOfInterest.Where(dbPointOfInterest => dbPointOfInterest.LocationId == locationId);
        if (pointsOfInterest.Any())
        {
            _context.Set<LocationPointOfInterest>().RemoveRange(pointsOfInterest);
        }

        var dbRatings = _context.LocationRatings.AsQueryable();
        var ratings = dbRatings.Where(r => r.LocationId == locationId);
        if (ratings.Any())
        {
            _context.Set<LocationRating>().RemoveRange(ratings);
        }

        var dbTrails = _context.LocationTrails.AsQueryable();
        var trails = dbTrails.Where(r => r.LocationId == locationId);
        if (trails.Any())
        {
            _context.Set<LocationTrail>().RemoveRange(trails);
        }

        _context.Set<Location>().Remove(location);
        _context.SaveChanges();
    }

}