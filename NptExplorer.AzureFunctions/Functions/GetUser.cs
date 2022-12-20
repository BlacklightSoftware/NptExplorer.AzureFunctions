using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using AutoMapper;
using NptExplorer.Dto.Models;

namespace NptExplorer.AzureFunctions.Functions
{
    public  class GetUser
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        public GetUser(IUsersRepository usersRepository,IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [FunctionName("GetUser")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string userId = req.Query["UserId"];

            if (string.IsNullOrEmpty(userId))
            {
                return new BadRequestResult();
            }


            var userDetails = _usersRepository.GetUser(userId);
            var userDetailsDto = _mapper.Map<UserDto>(userDetails);
            return new OkObjectResult(userDetailsDto);
        }
    }
}
