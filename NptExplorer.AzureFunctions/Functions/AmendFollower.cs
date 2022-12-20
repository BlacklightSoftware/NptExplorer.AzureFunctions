using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.Dto.Requests;
using NptExplorer.AzureFunctions.Repositories.Interfaces;

namespace NptExplorer.AzureFunctions.Functions
{
    public class AmendFollower
    {
        private readonly IUsersRepository _usersRepository;

        public AmendFollower(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [FunctionName("AmendFollower")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<UserRequest>(requestBody);

                if (data == null && string.IsNullOrEmpty(data.UserId) || string.IsNullOrEmpty(data.FriendId))
                {
                    log.LogError("UserRequest is null");
                    return new BadRequestResult();
                }

                var userData = new UserRequest()
                {
                    UserId = data.UserId,
                    FriendId = data.FriendId
                };

               
                _usersRepository.AmendFollower(userData);
                return new OkObjectResult(true);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
