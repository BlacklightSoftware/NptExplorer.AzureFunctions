using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetLocations
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public GetLocations(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        [FunctionName("GetLocations")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetLocations HTTP trigger function started");

            var locations = _locationRepository.GetLocations();
            var locationsDto = this._mapper.Map<IList<LocationDto>>(locations);

            return new OkObjectResult(locationsDto);

        }
    }
}