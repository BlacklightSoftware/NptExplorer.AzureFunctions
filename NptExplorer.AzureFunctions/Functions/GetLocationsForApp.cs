using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.Dto.Models;
using NptExplorer.Dto.Requests;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetLocationsForApp
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public GetLocationsForApp(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        [FunctionName("GetLocationsForApp")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetLocationsForApp HTTP trigger function started");

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
                if (!string.IsNullOrEmpty(data.SearchPhrase) || data.Filters != new ExploreFiltersDto())
                {
                    locations = _locationRepository.GetLocationsBySearch(data.SearchPhrase, data.MaxRecords, data.Filters);
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


                var locationsDto = this._mapper.Map<IList<LocationOverviewDto>>(locations);
                return new OkObjectResult(locationsDto);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestResult();
            }
        }
    }
}