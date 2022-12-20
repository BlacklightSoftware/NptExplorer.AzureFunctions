using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Cryptography;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using Microsoft.Graph;
using NptExplorer.AzureFunctions.Models;

namespace NptExplorer.AzureFunctions.Functions
{
    public class UpdateDefaultTrails
    {
        private readonly ITrailRepository _trailRepository;

        public UpdateDefaultTrails(ITrailRepository trailRepository)
        {
            _trailRepository = trailRepository;
        }

        [FunctionName("UpdateDefaultTrails")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Admin, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string id = req.Query["id"];
            try
            {
                if (!int.TryParse(id, out var trailId)) return new BadRequestResult();

                var trails = _trailRepository.UpdateDefaultTrails(trailId);
                return new OkObjectResult(true);
            }
            catch(Exception ex)
            {
                return new BadRequestResult();
            }
       
        }
    }
}
