using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NptExplorer.Dto.Requests;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.AzureFunctions.Repositories;
using NptExplorer.Dto.Models;
using AutoMapper;

namespace NptExplorer.AzureFunctions.Functions
{
    public class AddUser
    {
        private readonly IUsersRepository _usersRepository;

        public AddUser(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [FunctionName("AddUser")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<UserRequest>(requestBody);

                if (data == null || string.IsNullOrEmpty(data.UserId) || string.IsNullOrEmpty(data.Name))
                {
                    log.LogError("UserRequest is null");
                    return new BadRequestResult();
                }

                var userData = new User()
                {
                    UserId = data.UserId,
                    Name = data.Name
                };
                
                var userName = _usersRepository.CheckUserExists(userData.UserId);

                if (!userName)
                {
                    var user = _usersRepository.AddUser(userData);
                    return new OkObjectResult(true);
                }
      
                return new OkObjectResult(true);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestResult();
            }
        }
    }
}
