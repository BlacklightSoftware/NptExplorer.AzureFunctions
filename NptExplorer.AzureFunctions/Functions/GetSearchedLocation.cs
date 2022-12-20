using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NptExplorer.Dto.Requests;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetSearchedLocation
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        public GetSearchedLocation(ILocationRepository locationRepository, IMapper mapper)
        {
             _locationRepository = locationRepository;
            _mapper = mapper;
        }

        [FunctionName("GetSearchedLocation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetLocations HTTP trigger function started");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<LocationRequest>(requestBody);

            if (data is null)
            {
                log.LogError("LocationRequest is null");
                return new BadRequestResult();
            }

            var locations = _locationRepository.GetSearchedLocation(data.SearchPhrase, data.MaxRecords, data.Filters);
            var locationsDto = this._mapper.Map<IList<LocationOverviewDto>>(locations);
            return new OkObjectResult(locationsDto);
        }
    }
}
