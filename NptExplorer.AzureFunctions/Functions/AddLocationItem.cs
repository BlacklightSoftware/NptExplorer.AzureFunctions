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
    public class AddLocationItem
    {
        private readonly ILocationRepository _locationRepository;

        public AddLocationItem(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }
        [FunctionName("AddLocationItem")]
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

            var result = _locationRepository.AddLocationItem(locationItem);

            if (result) return new OkObjectResult(true);
            return new BadRequestResult();
        }
    }
}
