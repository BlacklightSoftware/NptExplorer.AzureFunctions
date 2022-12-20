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
using NptExplorer.Dto.Models;
using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetBadgeLevels
    {

        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public GetBadgeLevels(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [FunctionName("GetBadgeLevels")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var results = _usersRepository.GetBadgeLevels();
                var badgePointDto = _mapper.Map<IList<BadgePointDto>>(results);
                return new OkObjectResult(badgePointDto);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
