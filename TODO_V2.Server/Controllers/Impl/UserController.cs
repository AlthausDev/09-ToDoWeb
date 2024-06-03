using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NLog;
using TODO_V2.Client.ClientModels;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Models.Request;
using TODO_V2.Shared.Utils;

namespace TODO_V2.Server.Controllers.Impl
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly Logger LoggerN = LogManager.GetCurrentClassLogger();
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAll([FromBody] GetRequest<User>? request = null)
        {
            return await _userService.GetAll(request);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _userService.GetById(id);
            return user == null ? (ActionResult<User>)NotFound() : (ActionResult<User>)user;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<User>> Get(string username)
        {
            var user = await _userService.GetByUserName(username);
            return user == null ? (ActionResult<User>)NotFound() : (ActionResult<User>)user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginCredentials credentials)
        {
            try
            {
                var result = await _userService.Login(credentials);
                LoggerN.Info(result.Value);
                return result.Value != null ? Ok(result.Value) : Unauthorized("Invalid credentials");
            }
            catch (Exception ex)
            {
                LoggerN.Error($"Error logging in: {ex.Message}");
                return StatusCode(500, "An error occurred while logging in");
            }
        }


        [AllowAnonymous]
        [HttpGet("count")]
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
                    _ = HttpContext.Response.Headers.Remove("Authorization");
                    return Ok();
                }
                else
                {
                    return StatusCode(500, "Failed to logout");
                }
            }
            catch (Exception ex)
            {
                LoggerN.Error($"Error during logout: {ex.Message}");
                return StatusCode(500, "An error occurred while logging out");
            }
        }


        [HttpPost]
        public async Task<ActionResult<bool>> Post(UserCredentialsRequest request)
        {
            User user = request.user;
            LoginCredentials credentials = request.Credentials;

            return await _userService.Add(user, credentials);
        }

        [HttpPut]
        public async Task<ActionResult<User>> Put(UserCredentialsRequest request)
        {
            User user = request.user;
            LoginCredentials credentials = request.Credentials;

            var result = await _userService.Update(user, credentials);
            return result == null ? (ActionResult<User>)NotFound() : (ActionResult<User>)result;
        }

        [HttpPut("toggleIsActive/{id}")]
        public async Task<ActionResult<User>> ToggleIsActive(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = !user.IsActive;
            var result = await _userService.Update(user, null);

            return result == null ? (ActionResult<User>)NotFound() : (ActionResult<User>)result;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            _userService.Delete(id);
            return NoContent();
        }


        [HttpGet("credentials/{id}")]
        public async Task<ActionResult<LoginCredentials>> GetCredentials(int id)
        {
            var credentials = await _userService.GetCredentialsByUserId(id);
            return credentials == null ? (ActionResult<LoginCredentials>)NotFound() : (ActionResult<LoginCredentials>)credentials;
        }


    }
}
