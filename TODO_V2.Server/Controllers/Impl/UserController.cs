using Microsoft.AspNetCore.Mvc;
using TODO_V2.Client.DTO;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Controllers.Impl
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(ILogger<UserController> logger, IUserService userService, IConfiguration configuration)
        {
            _logger = logger;
            _userService = userService;
            _configuration = configuration;
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<User>> GetAll([FromBody] GetRequest<User>? request = null)
        {
            return await _userService.GetAll(request);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<User>> Get(string username)
        {
            var user = await _userService.GetByUserName(username);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(LoginCredentials credentials)
        {
            try
            {
                var result = await _userService.Login(credentials.Username, credentials.Password);
                return result.Value != null ? Ok(result.Value) : Unauthorized("Invalid credentials");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error logging in: {ex.Message}");
                return StatusCode(500, "An error occurred while logging in");
            }
        }



        [HttpGet("Count")]
        public Task<int> Count()
        {
            return _userService.Count();
        }


        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var success = await _userService.Logout();
                if (success)
                {
                    HttpContext.Response.Headers.Remove("Authorization");
                    return Ok();
                }
                else
                {
                    return StatusCode(500, "Failed to logout");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during logout: {ex.Message}");
                return StatusCode(500, "An error occurred while logging out");
            }
        }




        [HttpGet("CheckToken")]
        public IActionResult CheckToken()
        {
            if (HttpContext.Request.Headers.ContainsKey("Authorization"))
            {    
                return Redirect("/");
            }  
            return Ok();
        }


        [HttpPost]
        public async Task<ActionResult<bool>> Post(User entity)
        {
            return await _userService.Add(entity);
        }

        [HttpPut]
        public async Task<ActionResult<User>> Put(User entity)
        {
            var result = await _userService.Update(entity);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _userService.Delete(id);
            return NoContent();
        }
    }
}
