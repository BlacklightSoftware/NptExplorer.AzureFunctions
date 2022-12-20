using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Requests;
using Microsoft.AspNetCore.Http;

namespace NptExplorer.AzureFunctions.Functions
{
    public class PutUserBadge
    {
        private readonly IUserBadgeRepository _badgeRepository;
        private readonly IUsersRepository _userRepository;

        public PutUserBadge(IUsersRepository userRepository, IUserBadgeRepository badgeRepository)
        {
            _userRepository = userRepository;
            _badgeRepository = badgeRepository;
        }

        [FunctionName("PutUserBadge")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("PutUserBadge HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UserBadgeRequest>(requestBody);

            if (data == null || string.IsNullOrEmpty(data.UserId) || data.BadgeId == 0)
            {
                return new BadRequestResult();
            }

            var user = _userRepository.GetByUserId(data.UserId);
            if (user == null)
            {
                return new NotFoundResult();
            }

            _badgeRepository.PutUserBadge(user.Id, data.BadgeId, data.CheckedIn);
            return new OkObjectResult(true);
        }
    }
}
