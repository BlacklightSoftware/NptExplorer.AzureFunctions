using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Models;
using NptExplorer.Dto.Enums;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetPrerequisiteData
    {
        private readonly IBadgeRepository _badgeRepository;

        public GetPrerequisiteData(IBadgeRepository badgeRepository)
        {
            _badgeRepository = badgeRepository;
        }

        [FunctionName("GetPrerequisiteData")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetPrerequisiteData HTTP trigger function processed a request.");

            //hard coded points for now until the portal is set up
            // Levels: Explorer > Adventurer > Hero > Champion
            var data = new PrerequisiteDataDto
            {
                NatureBadgePoints = 20,
                HeritageBadgePoints = 20,
                WellbeingBadgePoints = 10,
                TrailBadgePoints = 15,
                RatingBadgePoints = 10,
                NatureAdventureLevel = 140,
                NatureHeroLevel = 280,
                NatureChampionLevel = 500,
                HeritageAdventureLevel = 60,
                HeritageHeroLevel = 100,
                HeritageChampionLevel = 200,
                WellbeingAdventureLevel = 80,
                WellbeingHeroLevel = 100,
                WellbeingChampionLevel = 150,
                TrailAdventureLevel = 45,
                TrailHeroLevel = 75,
                TrailChampionLevel = 150,
                OverallAdventureLevel = 605,
                OverallHeroLevel = 1015,
                OverallChampionLevel = 1865
            };

            var badges = _badgeRepository.GetBadges();
            data.TotalNatureBadges = badges.Count(x => (BadgeType)x.BadgeTypeId == BadgeType.Nature);
            data.TotalHeritageBadges = badges.Count(x => (BadgeType)x.BadgeTypeId == BadgeType.Heritage);
            data.TotalWellbeingBadges = badges.Count(x => (BadgeType)x.BadgeTypeId == BadgeType.Wellbeing);
            data.TotalTrailBadges = badges.Count(x => (BadgeType)x.BadgeTypeId == BadgeType.Trail);

            return new OkObjectResult(data);
        }
    }
}
