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
    public class GetBadgeTypes
    {

        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public GetBadgeTypes(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }
        [FunctionName("GetBadgeTypes")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var results = _usersRepository.GetBadgeTypes();
                var badgeTypeDto = _mapper.Map<IList<BadgeTypeDto>>(results);
                return new OkObjectResult(badgeTypeDto);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
