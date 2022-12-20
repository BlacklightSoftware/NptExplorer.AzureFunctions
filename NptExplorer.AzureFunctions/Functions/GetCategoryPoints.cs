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
    public class GetCategoryPoints
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        public GetCategoryPoints(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }
        [FunctionName("GetCategoryPoints")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var result = _usersRepository.GetCategoryPoints();
            try
            {

                var categoryPointsDto = this._mapper.Map<IList<CategoryPointsDto>>(result);
                return new OkObjectResult(categoryPointsDto);
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                return new BadRequestResult();
            }

        }
    }
}
