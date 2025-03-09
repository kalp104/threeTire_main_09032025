using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public TokenMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.User.Identity != null &&
            !context.User.Identity.IsAuthenticated &&
            context.Request.Cookies.ContainsKey("auth_token"))
        {
            var token = context.Request.Cookies["auth_token"];

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out _);
                context.User = principal;

                var emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value;
                var roleClaim = principal.FindFirst(ClaimTypes.Role)?.Value;

                if (!string.IsNullOrEmpty(emailClaim) && !string.IsNullOrEmpty(roleClaim))
                {
                    context.Items["UserEmail"] = emailClaim;
                    context.Items["UserRole"] = roleClaim;
                    
                }
                else
                {
                    Console.WriteLine("Email not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                context.Response.Redirect("/Home/Index");
                return;
            }

            if (context.User.Identity != null && !context.User.Identity.IsAuthenticated)
            {
                context.Response.Redirect("/Home/Index");
                return;
            }
        }

        await _next(context);
    }
}

// Extension method for easy middleware registration
public static class TokenMiddlewareExtensions
{
    public static IApplicationBuilder UseTokenMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TokenMiddleware>();
    }
}
