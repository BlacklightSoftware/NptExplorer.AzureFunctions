using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using System.Threading.Tasks;

namespace NptExplorer.AzureFunctions.Functions
{
    public class RemoveLocation
    {
        private readonly ILocationRepository _locationRepository;
        public RemoveLocation(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [FunctionName("RemoveLocation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string id = req.Query["id"];

            if (!int.TryParse(id, out var locationid)) return new BadRequestResult();

            var locationResult = _locationRepository.RemoveLocation(locationid);

            if (!locationResult)
            {
                return new OkObjectResult(false);
            }

            return new OkObjectResult(true);

        }
    }
}
