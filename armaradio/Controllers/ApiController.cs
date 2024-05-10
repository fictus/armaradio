using armaradio.Attributes;
using armaradio.Models.Home;
using armaradio.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace armaradio.Controllers
{
    [EnableCors("ApiPolicy")]
    public class ApiController : Controller
    {
        private readonly IArmaAuth _authControl;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public ApiController(
            IArmaAuth authControl,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager
        )
        {
            _authControl = authControl;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult ExternalLogin([FromBody] LoginRequest value)
        {
            try
            {
                if (value == null)
                {
                    throw new Exception("Invalid request");
                }
                if (string.IsNullOrEmpty(value.UserName))
                {
                    throw new Exception("'Email' is required");
                }
                if (string.IsNullOrEmpty(value.Password))
                {
                    throw new Exception("'Password' is required");
                }

                var result = _signInManager.PasswordSignInAsync(value.UserName.Trim(), value.Password, true, true).Result;

                if (!result.Succeeded)
                {
                    throw new Exception("Invalid Email or Password");
                }

                string userToken = _authControl.GenerateJwtToken(value.UserName.Trim());

                return new JsonResult(new
                {
                    apiToken = userToken
                });
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.ToString() == "Unauthorized" ? "Incorrect Email or Password" : ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
        }

        [SessionTokenAttribute]
        [HttpGet]
        public IActionResult TestJTokenCall()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var user = _userManager.FindByIdAsync(userId).Result;

            if (user == null)
            {
                // Handle the case where the user is not found in the user store
                return NotFound();
            }

            return new JsonResult(Ok());
        }
    }
}
