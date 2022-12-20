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
using NptExplorer.AzureFunctions.Models;
using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Functions
{
    public class UpdateLocation
    {
        private readonly ILocationRepository _locationRepository;

        public UpdateLocation(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [FunctionName("UpdateLocation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Location>(requestBody);

            try
            {

                if (data is null)
                {
                    log.LogError("location is null");
                    return new BadRequestResult();
                }

                var location = _locationRepository.UpdateLocation(data);

            }
            catch(Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestResult();
            }

            return new OkObjectResult(true);
        }
    }
}
