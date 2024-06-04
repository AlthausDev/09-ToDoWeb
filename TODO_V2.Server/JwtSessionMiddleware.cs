using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TODO_V2.Server
{
    public class JwtSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtSessionMiddleware(RequestDelegate next)
        {
            _next = next;
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
                }
            }

            await _next(context);
        }
    }
}
