using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Models;
using System.Threading.Tasks;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetLocation
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public GetLocation(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }
        [FunctionName("GetLocation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetLocation HTTP trigger function processed a request.");

            string id = req.Query["id"];
            if (!int.TryParse(id, out var locationId))
            {
                return new BadRequestResult();
            }

            var location = _locationRepository.GetLocation(locationId);
            var locationDto = _mapper.Map<LocationDto>(location);
            return new OkObjectResult(locationDto);

        }
    }
}
