using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Models;
using AutoMapper;
using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Functions
{
    public class ExplorerLevel
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public ExplorerLevel(IUsersRepository usersRepository,IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [FunctionName("ExplorerLevel")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {

            var users = _usersRepository.ExplorerLevel();
            var userDto = this._mapper.Map<IList<UserDto>>(users);
            return new OkObjectResult(userDto);
        }
    }
}
