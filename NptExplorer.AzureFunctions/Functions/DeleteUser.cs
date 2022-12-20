using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.AzureFunctions.Services.Abstract;
using System.Threading.Tasks;

namespace NptExplorer.AzureFunctions.Functions
{
    public class DeleteUser
    {
        private readonly IGraphService _graphService;
        private readonly IUsersRepository _usersRepository;

        public DeleteUser(IGraphService graphService, IUsersRepository usersRepository)
        {
            _graphService = graphService;
            _usersRepository = usersRepository;
        }

        [FunctionName("DeleteUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("DeleteUser HTTP trigger function processed a request.");

            var userId = req.Query["userId"];

            if (string.IsNullOrEmpty(userId))
            {
                return new BadRequestResult();
            }

            var result = await _graphService.DeleteAdUser(userId);

            if (result == false)
            {
                log.LogInformation("User does not exist.");
            }

            // clear up database data just in case
            _usersRepository.DeleteUser(userId);
            return new OkObjectResult("OK");
        }
    }
}
