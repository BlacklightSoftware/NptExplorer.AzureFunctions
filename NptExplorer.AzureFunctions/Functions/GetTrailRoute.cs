using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using NptExplorer.AzureFunctions.Helpers;
using NptExplorer.AzureFunctions.Models.Transient;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.AzureFunctions.Services.Abstract;
using System.Threading.Tasks;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetTrailRoute
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IRequestProviderService _requestProviderService;

        public GetTrailRoute(ITrailRepository trailRepository, IRequestProviderService requestProviderService)
        {
            _trailRepository = trailRepository;
            _requestProviderService = requestProviderService;
        }

        [FunctionName("GetTrailRoute")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetTrailRoute HTTP trigger function processed a request.");
            string tid = req.Query["trailId"];
            if (!int.TryParse(tid, out var trailId)) return new BadRequestResult();

            var trail = _trailRepository.GetTrail(trailId);
            var route = await _requestProviderService.Get<TrailRouteResponse>(trail.TrailRouteApi);
            var dto = TrailHelper.BuildTrailRouteDto(trail, route);

            return new OkObjectResult(dto);
        }
    }
}
