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
using NptExplorer.AzureFunctions.Repositories.Interfaces;

namespace NptExplorer.AzureFunctions.Functions
{
    public class UpdatePointOfInterest
    {
        private readonly IChallengeRepository _challengeRepository;
        private readonly ILogger _logger;
        public UpdatePointOfInterest(IChallengeRepository challengeRepository)
        {
            _challengeRepository = challengeRepository;
        }
        [FunctionName("UpdatePointOfInterest")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"put", Route = null)] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<PointOfInterest>(requestBody);

            if (data is null)
            {
                log.LogError("data is null");
                return new BadRequestResult();
            }

            try
            {
                _challengeRepository.UpdatePointOfInterest(data);
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
