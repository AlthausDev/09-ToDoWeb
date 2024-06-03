using Azure.Core;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NLog;
using System.Diagnostics;
using TODO_V2.Client.DTO;
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
            LoggerN.Info("dasfas");
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
            if (result == null)
            {
                return NotFound();
            }
            return result;
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
            if (credentials == null)
            {
                return NotFound();
            }
            return credentials;
        }


        //[HttpDelete("{id}")]
        //public ActionResult Delete(int id)
        //{
        //    _userService.Delete(id);
        //    return NoContent();
        //}
    }
}
