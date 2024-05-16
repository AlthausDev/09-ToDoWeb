//using Microsoft.AspNetCore.Mvc;
//using TODO_V2.Server.Controllers.Interfaces;
//using TODO_V2.Server.Services.Interfaces;
//using TODO_V2.Shared.Models;

//namespace TODO_V2.Server.Controllers.Impl
//{
//    [Route("[controller]")]
//    [ApiController]
//    public abstract class CrudGenericController<T> : ControllerBase where T : BaseModel
//    {
//        private readonly ILogger<CrudGenericController<T>> Logger;
//        private readonly IConfiguration Configuration;
//        //private readonly IGenericService<T> service;
//        private readonly IUserService service;

//        protected CrudGenericController(ILogger<CrudGenericController<T>> logger, IConfiguration configuration, IUserService service)
//        {
//            Logger = logger;
//            Configuration = configuration;
//            this.service = service;
//        }

//        //public CrudGenericController(IGenericService<T> service)
//        //{
//        //    this.service = service;
//        //}

//        [HttpPost("GetAll")]
//        public async Task<IEnumerable<T>>? GetAll([FromBody] GetRequest<T>? request = null)
//        {
//            //return await service.GetAll(request);
//            return null;
//        }

//        [HttpGet("{id}")]
//        public ActionResult<T> Get(int id) 
//        {
//            //return service.GetById(id);
//            return null;
//        }

//        [HttpPost]
//        public async Task<T> Post(T entity)
//        {
//            //return await service.Add(entity);
//            return null;
//        }

//        [HttpPut]
//        public async Task<T>? Put(T entity)
//        {
//            return await service.Update(entity);
//        }

//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//            service.Delete(id);
//        }
//    }
//}
