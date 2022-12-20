using System.Collections.Generic;
using NptExplorer.AzureFunctions.Models;

namespace NptExplorer.AzureFunctions.Repositories.Interfaces;

public interface ITrailRepository : IRepository
{
    List<Trail> GetTrailsByDistance(GeoPosition currentLocation, int? maxRecords, Dto.Models.FiltersDto filters);
    List<Trail> GetTrailsBySearch(string searchPhrase, int? maxRecords, Dto.Models.FiltersDto filters);
    Trail GetTrail(int id);
    List<Trail> GetDefaultTrails(int? maxRecords, Dto.Models.FiltersDto filters);
    List<Trail> GetTrailsPortal();
    bool UpdateDefaultTrails(int id);
}