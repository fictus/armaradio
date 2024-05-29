using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace armaradio.Attributes
{
    public class SessionTokenAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
            //var tokenService = context.HttpContext.RequestServices.GetRequiredService<TokenService>();
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            byte[] jwtBytes = Convert.FromBase64String(Uri.UnescapeDataString(token));
            var jwtToken = Encoding.UTF8.GetString(jwtBytes);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(jwtToken);

            if (securityToken.ValidTo < DateTime.UtcNow)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = securityToken.Subject;

            IdentityUser tokenUser = userManager.FindByIdAsync(userId).Result;

            if (tokenUser == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // You can add additional checks here if needed, such as checking user roles/permissions

            // Set user id as a claim for further use if needed
            context.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(new[] {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, userId)
            }, "Custom"));

            // Authorization successful
        }
    }
}
