using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace NptExplorer.AzureFunctions.Functions
{
    public class RemoveLocationItem
    {
        private readonly ILocationRepository _locationRepository;

        public RemoveLocationItem(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }
        [FunctionName("RemoveLocationItem")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<LocationItemSumRequest>(requestBody);

            if (data is null)
            {
                log.LogError("Location is null");
                return new BadRequestResult();
            }

            var locationItem = data;

            var result = _locationRepository.RemoveLocationItem(locationItem);

            if (result) return new OkObjectResult(true);
            return new BadRequestResult();
        }
    }
}
