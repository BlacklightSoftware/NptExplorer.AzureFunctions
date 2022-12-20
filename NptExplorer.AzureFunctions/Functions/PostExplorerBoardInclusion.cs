using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Requests;

namespace NptExplorer.AzureFunctions.Functions
{
    public class PostExplorerBoardInclusion
    {
        private readonly IUsersRepository _userRepository;

        public PostExplorerBoardInclusion(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [FunctionName("PostExplorerBoardInclusion")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("PostExplorerBoardInclusion HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<ExplorerBoardRequest>(requestBody);

            if (data == null || data.UserId == 0)
            {
                return new BadRequestResult();
            }

            _userRepository.UpdateExplorerBoard(data.UserId, data.Include);
            return new OkObjectResult(true);
        }
    }
}
