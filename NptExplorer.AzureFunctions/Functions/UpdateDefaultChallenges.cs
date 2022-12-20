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
    public class UpdateDefaultChallenges
    {
        private readonly ILocationRepository _locationRepository;

        public UpdateDefaultChallenges(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [FunctionName("UpdateDefaultChallenges")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string id = req.Query["id"];
            try
            {
                if (!int.TryParse(id, out var locationId)) return new BadRequestResult();

                _locationRepository.UpdateDefaultChallenges(locationId);
                return new OkObjectResult(true);
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }
    }
}
