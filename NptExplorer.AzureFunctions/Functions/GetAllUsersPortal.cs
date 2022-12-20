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
using AutoMapper;
using NptExplorer.AzureFunctions.Repositories.Interfaces;

namespace NptExplorer.AzureFunctions.Functions
{
    public class GetAllUsersPortal
    {
        public readonly IUsersRepository _userRepository;
        public readonly IMapper _mapper;

        public GetAllUsersPortal(IUsersRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        [FunctionName("GetAllUsersPortal")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetAllUsers HTTP trigger function started");

            var users = _userRepository.GetAllUsersPortal();
            var userDto = this._mapper.Map<IList<UserDto>>(users);

            return new OkObjectResult(userDto);
        }
    }
}
