using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Models;
using AutoMapper;
using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetTrailsPortal
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public GetTrailsPortal(ITrailRepository trailRepository,IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }
        [FunctionName("GetTrailsPortal")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var result = _trailRepository.GetTrailsPortal();
            try
            {

                var trailsDto = _mapper.Map<IList<TrailOverviewDto>>(result);
                return new OkObjectResult(trailsDto);
            }
            catch(Exception ex)
            {
                var test = ex.Message;
                return new BadRequestResult();
            }
        }
    }
}
