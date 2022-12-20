using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using AutoMapper;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Models;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetSearchedPortalLocation
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        public GetSearchedPortalLocation(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        [FunctionName("GetSearchedPortalLocation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetSearchedPortalLocation HTTP trigger function started");

            string searchPhrase = req.Query["loc"];

            if (string.IsNullOrEmpty(searchPhrase))
            {
                log.LogError("LocationRequest is null");
                return new BadRequestResult();
            }

            var locations = _locationRepository.GetSearchedPortalLocation(searchPhrase);
            var locationsDto = this._mapper.Map<IList<LocationOverviewDto>>(locations);
            return new OkObjectResult(locationsDto);
        }
    }
}
