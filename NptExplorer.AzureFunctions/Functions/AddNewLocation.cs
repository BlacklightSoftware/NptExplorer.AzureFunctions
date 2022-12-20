using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NptExplorer.AzureFunctions.Functions
{
    public class AddNewLocation
    {
        private readonly ILocationRepository _locationRepository;

        public AddNewLocation(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }
        [FunctionName("AddNewLocation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Location>(requestBody);

            if (data is null)
            {
                log.LogError("Location is null");
                return new BadRequestResult();
            }

            var location = data;

            try
            {
                _locationRepository.AddNewLocation(location);
                return new OkObjectResult(true);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }


        }
    }
}
