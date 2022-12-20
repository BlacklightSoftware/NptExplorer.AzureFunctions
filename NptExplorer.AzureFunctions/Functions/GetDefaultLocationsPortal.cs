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
using NptExplorer.Dto.Models;
using System.Collections.Generic;
using NptExplorer.AzureFunctions.Models;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetDefaultLocationsPortal
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public GetDefaultLocationsPortal(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }
        [FunctionName("GetDefaultLocationsPortal")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            List<Location> locations = null;
            try 
            {
                locations = _locationRepository.GetDefaultLocationsPortal();
                var locationDto = this._mapper.Map<IList<LocationDto>>(locations);
                return new OkObjectResult(locationDto);

            }
            catch (Exception) 
            {
                return new BadRequestResult();
            }
        }
    }
}
