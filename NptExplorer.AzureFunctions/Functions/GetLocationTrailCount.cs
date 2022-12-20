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
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using NptExplorer.AzureFunctions.Models;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetLocationTrailCount
    {
        private readonly IUserBadgeRepository _userBadgeRepository;
        private readonly IMapper _mapper;
        public GetLocationTrailCount(IUserBadgeRepository userBadgeRepository, IMapper mapper)
        {
            _userBadgeRepository = userBadgeRepository;
            _mapper = mapper;
        }
        [FunctionName("GetLocationTrailCount")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetLocation HTTP trigger function processed a request.");

            string id = req.Query["id"];
            if (!int.TryParse(id, out var userId))
            {
                return new BadRequestResult();
            }

            try
            {
                var location = _userBadgeRepository.GetLocationCount(userId);

                var trail = _userBadgeRepository.GetTrailCount(userId);

                var userCountDto = new UserCountDto()
                {
                    Location = location,
                    Trail = trail

                };
                return new OkObjectResult(userCountDto);
            }
            catch(Exception ex)
            {
                var test = ex.Message;
                return new BadRequestResult();
            }
           
        }
    }
}
