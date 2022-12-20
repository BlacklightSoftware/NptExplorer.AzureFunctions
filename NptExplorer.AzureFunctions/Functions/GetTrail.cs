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
    public class GetTrail
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IUserBadgeRepository _userBadgeRepository;
        private readonly IUsersRepository _userRepository;

        public GetTrail(ITrailRepository trailRepository, IUserBadgeRepository userBadgeRepository, IUsersRepository userRepository)
        {
            _trailRepository = trailRepository;
            _userBadgeRepository = userBadgeRepository ?? throw new ArgumentNullException(nameof(userBadgeRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [FunctionName("GetTrail")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetTrail HTTP trigger function processed a request.");

            string tid = req.Query["trailId"];
            string uid = req.Query["userId"];
            if (!int.TryParse(tid, out var trailId)) return new BadRequestResult();

            var trail = _trailRepository.GetTrail(trailId);
            var user = _userRepository.GetByUserId(uid);
            var badgeCollected = user != null && _userBadgeRepository.GetTrailBadgeByUser(user.Id, trailId);
            var dto = TrailHelper.BuildTrailDto(trail, badgeCollected);

            return new OkObjectResult(dto);
        }
    }
}
