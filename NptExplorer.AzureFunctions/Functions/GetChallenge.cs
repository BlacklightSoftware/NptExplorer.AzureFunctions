using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NptExplorer.AzureFunctions.Helpers;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.AzureFunctions.Repositories.Interfaces;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetChallenge
    {
        private readonly IChallengeRepository _challengeRepository;
        private readonly IUserBadgeRepository _userBadgeRepository;
        private readonly IUsersRepository _userRepository;

        public GetChallenge(IChallengeRepository challengeRepository, 
            IUserBadgeRepository userBadgeRepository,
            IUsersRepository userRepository)
        {
            _challengeRepository = challengeRepository ?? throw new ArgumentNullException(nameof(challengeRepository));
            _userBadgeRepository = userBadgeRepository ?? throw new ArgumentNullException(nameof(userBadgeRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [FunctionName("GetChallenge")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetChallenge HTTP trigger function processed a request.");

            string cid = req.Query["challengeId"];
            string uid = req.Query["userId"];
            if (!int.TryParse(cid, out var challengeId)) return new BadRequestResult();

            var challenge = _challengeRepository.GetChallenge(challengeId);
            var user = _userRepository.GetByUserId(uid);
            var userBadges = user == null ? new List<UserBadge>() : _userBadgeRepository.GetByUser(user.Id);

            return new OkObjectResult(ChallengeHelper.BuildChallengeDto(challenge, userBadges));
        }
    }
}
