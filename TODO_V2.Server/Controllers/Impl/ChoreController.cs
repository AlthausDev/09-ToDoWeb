using Microsoft.AspNetCore.Mvc;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Controllers.Impl
{
    [ApiController]
    [Route("[controller]")]
    public class ChoreController
    {
        private readonly ILogger<ChoreController> Logger;
        private readonly IChoreService service;
        private readonly IConfiguration Configuration;

        public ChoreController(ILogger<ChoreController> logger, IChoreService choreService, IConfiguration configuration)
        {
            Logger = logger;
            service = choreService;
            Configuration = configuration;
        }

        [HttpPost("GetAll")]
        public async Task<IEnumerable<Chore>>? GetAll([FromBody] GetRequest<Chore>? request = null)
        {
            return await service.GetAll(request);
        }

        [HttpGet("{id}")]
        public ActionResult<Chore> Get(int id)
        {
            return service.GetById(id);
        }

        [HttpPost]
        public async Task<bool> Post(Chore entity)
        {
            return await service.Add(entity);
        }

        [HttpPut]
        public async Task<Chore>? Put(Chore entity)
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
