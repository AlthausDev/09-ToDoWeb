using Microsoft.AspNetCore.Mvc;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Controllers.Impl
{
    [ApiController]
    [Route("[controller]")]
    public class UserController
    {
        private readonly ILogger<UserController> Logger;
        private readonly IUserService service;
        private readonly IConfiguration Configuration;

        public UserController(ILogger<UserController> logger, IUserService userService, IConfiguration configuration)
        {
            Logger = logger;
            service = userService;
            Configuration = configuration;
        }

        [HttpPost("GetAll")]
        public async Task<IEnumerable<User>>? GetAll([FromBody] GetRequest<User>? request = null)
        {
            return await service.GetAll(request);
        }

        [HttpGet("{id:int}")]                     
        public ActionResult<User> Get(int id)
        {
            return service.GetById(id);
        }

        [HttpGet("{username}")]        
        public ActionResult<User> Get(string username)
        {
            return service.GetByUserName(username);
        }


        [HttpPost]
        public async Task<User> Post(User entity)
        {
            return await service.Add(entity);
        }

        [HttpPut]
        public async Task<User>? Put(User entity)
        {
            return await service.Update(entity);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.Delete(id);
        }
    }
}

