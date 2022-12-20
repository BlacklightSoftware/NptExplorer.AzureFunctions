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
using Microsoft.Graph;

namespace NptExplorer.AzureFunctions.Functions
{
    public class RemoveDefaultLocationPortal
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public RemoveDefaultLocationPortal(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }


        [FunctionName("RemoveDefaultLocationPortal")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string Id = req.Query["id"];


            if (!int.TryParse(Id, out int locationId))
            {
                log.LogError("data is null");
                return new BadRequestResult();
            }

            try
            {
                var result = _locationRepository.RemoveDefaultLocationPortal(locationId);
                return new OkObjectResult(result);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }

        }
    }
}
