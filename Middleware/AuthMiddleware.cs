using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using portrait_forum.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Numerics;
using System.Text;

namespace portrait_forum.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public AuthMiddleware(RequestDelegate next, IConfiguration configuration) 
        { 
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, ForumContext forumContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!String.IsNullOrEmpty(token))
                await AuthenticateUser(context, token, forumContext);
            else
                await _next(context);
        }
        public async Task AuthenticateUser(HttpContext context, string token, ForumContext forumContext)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _configuration["Secret"];
            if (String.IsNullOrEmpty(secret))
                throw new Exception("secret must be provided :<");
            var key = Encoding.ASCII.GetBytes(secret);
            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out validatedToken);
            }
            catch
            {
                await Unauthorized(context);
                return; 
            }
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = long.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            context.Items["User"] = await forumContext.Users.FindAsync(userId);
            if (context.Items["User"] == null)
                await Unauthorized(context);
            else
                await _next(context);
        }
        public async Task Unauthorized(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.StartAsync();
        }
    }
    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
