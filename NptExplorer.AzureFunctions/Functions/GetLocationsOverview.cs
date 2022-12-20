using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.Dto.Requests;
using NptExplorer.AzureFunctions.Repositories;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetLocationsOverview
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        public GetLocationsOverview(ILocationRepository locationRepository,IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        [FunctionName("GetLocationsOverview")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetLocations HTTP trigger function processed a request.");
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<LocationRequest>(requestBody);

                if (data is null)
                {
                    log.LogError("LocationRequest is null");
                    return new BadRequestResult();
                }

                List<Location> locations;
                if (!string.IsNullOrEmpty(data.SearchPhrase))
                {
                    locations = _locationRepository.GetSearchedLocation(data.SearchPhrase, data.MaxRecords, data.Filters);
                }
                else if (data.LocationServicesEnabled && data.CurrentLocation != null)
                {
                    locations = _locationRepository.GetLocationByDistance(new GeoPosition(data.CurrentLocation.Latitude, data.CurrentLocation.Longitude), data.MaxRecords, data.Filters);
                    if (!locations.Any())
                    {
                        locations = _locationRepository.GetDefaultLocations();
                    }
                }
                else
                {
                    locations = _locationRepository.GetDefaultLocations();
                }

                var locationsFacilityDto = _mapper.Map<IList<LocationOverviewDto>>(locations);
                return new OkObjectResult(locationsFacilityDto);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return new BadRequestResult();
            }
        }
    }
}
