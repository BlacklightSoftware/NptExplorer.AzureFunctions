using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NptExplorer.AzureFunctions.Helpers;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Requests;
using NptExplorer.Dto.Models;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetChallenges
    {
        private readonly IChallengeRepository _challengeRepository;
        private readonly IUserBadgeRepository _userBadgeRepository;
        private readonly IUsersRepository _userRepository;

        public GetChallenges(IChallengeRepository challengeRepository,
            IUserBadgeRepository userBadgeRepository,
            IUsersRepository userRepository)
        {
            _challengeRepository = challengeRepository ?? throw new ArgumentNullException(nameof(challengeRepository));
            _userBadgeRepository = userBadgeRepository ?? throw new ArgumentNullException(nameof(userBadgeRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [FunctionName("GetChallenges")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetChallenges HTTP trigger function processed a request.");

            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<ChallengeRequest>(requestBody);

                if (data is null)
                {
                    log.LogError("ChallengeRequest is null");
                    return new BadRequestResult();
                }

                log.LogInformation($"Data.LocationServicesEnabled: {data.LocationServicesEnabled}");
                log.LogInformation($"Data.MaxRecords: {data.MaxRecords}");
                log.LogInformation($"Data.CurrentLocation: {data.CurrentLocation}");
                log.LogInformation($"Data.SearchPhrase: {data.SearchPhrase}");

                List<Location> locations;
                if (!string.IsNullOrEmpty(data.SearchPhrase) || data.Filters != new FiltersDto())
                {
                    locations = _challengeRepository.GetChallengesBySearch(data.SearchPhrase, data.MaxRecords, data.Filters);
                }
                else if (data.LocationServicesEnabled && data.CurrentLocation != null)
                {
                    locations = _challengeRepository.GetChallengesByDistance(new GeoPosition(data.CurrentLocation.Latitude, data.CurrentLocation.Longitude), data.MaxRecords, data.Filters);
                    if (!locations.Any())
                    {
                        locations = _challengeRepository.GetDefaultChallenges(data.MaxRecords, data.Filters);
                    }
                }
                else
                {
                    locations = _challengeRepository.GetDefaultChallenges(data.MaxRecords, data.Filters);
                }

                var user = _userRepository.GetByUserId(data.UserId);
                var userBadges = user == null ? new List<UserBadge>() : _userBadgeRepository.GetByUser(user.Id);

                var dto = ChallengeHelper.BuildChallengeOverviewDtoList(locations, userBadges);
                return new OkObjectResult(dto);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
