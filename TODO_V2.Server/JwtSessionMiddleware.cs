
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TODO_V2.Server
{
    public class JwtSessionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtSessionMiddleware> _logger;

        public JwtSessionMiddleware(RequestDelegate next, ILogger<JwtSessionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader) && authHeader.ToString().StartsWith("Bearer "))
            {

               

                var token = authHeader.ToString().Substring("Bearer ".Length).Trim();                
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);

                if (jwtSecurityToken != null)
                {
                    var userId = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    var userType = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                    var sessionData = $"UserId:{userId}, UserType:{userType}";
                    context.Items["SessionData"] = sessionData;

                    _logger.LogInformation($"SessionData stored: {sessionData}");
                }
                else
                {
                    _logger.LogWarning("JWT token is invalid or cannot be read.");
                }
            }
            else
            {
               
                _logger.LogInformation("No Authorization header found or it does not start with Bearer.");
            }

            await _next(context);
        }
    }
}


