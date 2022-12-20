using NptExplorer.AzureFunctions.Models;
using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Repositories.Interfaces;

public interface IChallengeRepository : IRepository
{
    List<Location> GetChallengesByDistance(GeoPosition currentLocation, int? maxRecords, Dto.Models.FiltersDto filters);
    List<Location> GetChallengesBySearch(string searchPhrase, int? maxRecords, Dto.Models.FiltersDto filters);
    Location GetChallenge(int id);
    List<Location> GetDefaultChallenges(int? maxRecords, Dto.Models.FiltersDto filters);
    List<PointOfInterest> GetAllPointOfIntrests();
    void UpdatePointOfInterest(PointOfInterest pointOfInterest);
}