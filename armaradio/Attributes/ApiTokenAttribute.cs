using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace armaradio.Attributes
{
    public class ApiTokenAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!IsUserAuthorized(context.HttpContext))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }

        private bool IsUserAuthorized(HttpContext httpContext)
        {
            bool returnItem = false;

            try
            {
                var token = httpContext.Request.Headers.ContainsKey("Authorization")
                            ? httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                            : "";

                if (!string.IsNullOrEmpty(token))
                {
                    var userManager = httpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();

                    byte[] jwtBytes = Convert.FromBase64String(Uri.UnescapeDataString(token));
                    var jwtToken = Encoding.UTF8.GetString(jwtBytes);

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.ReadJwtToken(jwtToken);

                    if (!(securityToken.ValidTo < DateTime.UtcNow))
                    {
                        var userId = securityToken.Subject;

                        IdentityUser tokenUser = userManager.FindByIdAsync(userId).Result;

                        if (tokenUser != null)
                        {
                            // Set user id as a claim for further use if needed
                            httpContext.User = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(new[] {
                                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, userId)
                            }, "Custom"));

                            returnItem = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var _dapper = httpContext.RequestServices.GetRequiredService<Repositories.IDapperHelper>();

                _dapper.ExecuteNonQuery("radioconn", "ArmaError_LogError", new
                {
                    ErrorController = "ApiTokenAttribute",
                    ErrorMethod = "IsUserAuthorized",
                    ErrorMessage = ex.Message.ToString()
                });
            }

            return returnItem;
        }
    }
}
