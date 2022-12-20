using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.AzureFunctions.Helpers;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.Dto.Models;
using NptExplorer.Dto.Requests;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetTrails
    {
        private readonly ITrailRepository _trailRepository;

        public GetTrails(ITrailRepository trailRepository)
        {
            _trailRepository = trailRepository;
        }

        [FunctionName("GetTrails")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetTrails HTTP trigger function processed a request.");

            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<TrailRequest>(requestBody);

                if (data is null)
                {
                    log.LogError("TrailRequest is null");
                    return new BadRequestResult();
                }

                List<Trail> trails;
                if (!string.IsNullOrEmpty(data.SearchPhrase) || (data.Filters != null && data.Filters != new FiltersDto()))
                {
                    trails = _trailRepository.GetTrailsBySearch(data.SearchPhrase, data.MaxRecords, data.Filters);
                }
                else if (data.LocationServicesEnabled && data.CurrentLocation != null)
                {
                    trails = _trailRepository.GetTrailsByDistance(new GeoPosition(data.CurrentLocation.Latitude, data.CurrentLocation.Longitude), data.MaxRecords, data.Filters);
                    if (!trails.Any())
                    {
                        trails = _trailRepository.GetDefaultTrails(data.MaxRecords, data.Filters);
                    }
                }
                else
                {
                    trails = _trailRepository.GetDefaultTrails(data.MaxRecords, data.Filters);
                }

                var dto = TrailHelper.BuildTrailDtoList(trails);
                return new OkObjectResult(dto);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
