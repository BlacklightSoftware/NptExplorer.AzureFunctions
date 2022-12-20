using System.Collections.Generic;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.Dto.Models;

namespace NptExplorer.AzureFunctions.Repositories.Interfaces;

public interface ILocationRepository : IRepository
{
    List<Location> GetLocations();
    List<Location>GetLocationOverview();
    List<Location> GetSearchedLocation(string searchPhrase, int? maxRecords, ExploreFiltersDto filters);
    List<Location> GetSearchedPortalLocation(string searchPhrase);
    Location GetLocation(int id);
    List<Location> GetLocationByDistance(GeoPosition currentLocation, int? maxRecords, ExploreFiltersDto filters);
    List<Location> GetDefaultLocations();
    bool RemoveLocation(int id);
    bool UpdateLocation(Location location);
    bool RemoveLocationItem(LocationItemSumRequest locationItem);
    bool AddLocationItem(LocationItemSumRequest locationItemSumRequest);
    bool AddNewLocation(Location location);
    List<Location> GetDefaultLocationsPortal();
    bool AddDefaultLocationsPortal(int locationId);
    bool RemoveDefaultLocationPortal(int locationId);
    bool UpdateDefaultChallenges(int id);
    List<Location> GetLocationsBySearch(string? searchPhrase, int? maxRecords, ExploreFiltersDto filters);
}