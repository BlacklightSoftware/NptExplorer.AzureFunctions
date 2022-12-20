using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AutoMapper;
using NptExplorer.AzureFunctions.Repositories.Interfaces;

namespace NptExplorer.AzureFunctions.Functions
{
    public class AddDefaultLocationsPortal
    {
        private readonly ILocationRepository _locationRepository;

        public AddDefaultLocationsPortal(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
        }

        [FunctionName("AddDefaultLocationsPortal")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<string>(requestBody);

            if (!int.TryParse(data, out int locationId))
            {
                log.LogError("data is null");
                return new BadRequestResult();
            }

            try
            {
                var result = _locationRepository.AddDefaultLocationsPortal(locationId);
                return new OkObjectResult(result);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }
    }
}
