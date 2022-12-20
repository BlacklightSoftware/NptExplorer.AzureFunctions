using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NptExplorer.Dto.Requests;
using NptExplorer.AzureFunctions.Models;
using System.Collections.Generic;
using System.Linq;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Models;
using AutoMapper;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetLocationByDistance
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        public GetLocationByDistance(ILocationRepository locationRepository,IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }
        [FunctionName("GetLocationByDistance")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<LocationRequest>(requestBody);
            try
            {
                List<Location> locations;
                if (data is null)
                {
                    log.LogError("TrailRequest is null");
                    return new BadRequestResult();
                }
                
                if (data.LocationServicesEnabled && data.CurrentLocation != null)
                {
                    locations = _locationRepository.GetLocationByDistance(new GeoPosition(data.CurrentLocation.Latitude, data.CurrentLocation.Longitude), data.MaxRecords, data.Filters);
                    if (!locations.Any())
                    {
                        locations = _locationRepository.GetDefaultLocations();
                    }
                    var locationsDto = this._mapper.Map<IList<LocationOverviewDto>>(locations);
                    return new OkObjectResult(locationsDto);
                }
                else
                {
                    locations = _locationRepository.GetDefaultLocations();
                    var locationsDto = this._mapper.Map<IList<LocationOverviewDto>>(locations);
                    return new OkObjectResult(locationsDto);

                }
            } catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestResult();
            }
            return new BadRequestResult();
        }
    }
}
