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

namespace NptExplorer.AzureFunctions.Functions
{
    public class UpdateBadgeType
    {

        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public UpdateBadgeType(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [FunctionName("UpdateBadgeType")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"put", Route = null)] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<BadgeType>(requestBody);

            if(data is null)
            {
                log.LogError("data is null");
                return new BadRequestResult();
            }

            try
            {
                _usersRepository.UpdateBadgeType(data);
                return new OkObjectResult(true);
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
