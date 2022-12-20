using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NptExplorer.Dto.Models;
using System.Collections.Generic;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using AutoMapper;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetAllPointsOfIntrest
    {
        private readonly IMapper _mapper;
        private readonly IChallengeRepository _challengeRepository;

        public GetAllPointsOfIntrest(IMapper mapper, IChallengeRepository challengeRepository)
        {
            _challengeRepository = challengeRepository;
            _mapper = mapper; 
        }
        [FunctionName("GetAllPointsOfIntrest")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var results = _challengeRepository.GetAllPointOfIntrests();
                var poiDto = this._mapper.Map<IList<PointOfInterestOverviewDto>>(results);
                return new OkObjectResult(poiDto);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
