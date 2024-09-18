using armaradio.Models.ArmaAuth;
using armaradio.Models.Home;
using armaradio.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace armaradio.Controllers
{
    [DisableCors]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArmaAuth _authControl;
        private readonly IMusicRepo _musicRepo;

        public HomeController(
            ILogger<HomeController> logger,
            IArmaAuth authControl,
            IMusicRepo musicRepo
        )
        {
            _logger = logger;
            _authControl = authControl;
            _musicRepo = musicRepo;
        }

        public IActionResult Index()
        {
            //_musicRepo.DuckDuckGo_PerformGeneralSearch("depeche mode - somebody");
            return View();
        }

        [HttpGet]
        public IActionResult SessionState()
        {
            bool userIsLoggedIn = false;

            try
            {
                userIsLoggedIn = _authControl.UserIsLoggedIn();
            }
            catch (Exception ex)
            {
            }

            return new JsonResult(new
            {
                web_static = userIsLoggedIn
            });
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest value)
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

                AuthLoginResponse loginResults = _authControl.Login(value.UserName, value.Password);



                return new JsonResult(loginResults);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.ToString() == "Unauthorized" ? "Incorrect Email or Password" : ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
        }

        [HttpPost]
        public IActionResult Register([FromBody] RegisterRequest value)
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
                if (!_authControl.IsValidEmailAddress(value.UserName))
                {
                    throw new Exception("'Email' is incorrect");
                }
                if (string.IsNullOrEmpty(value.Password) || string.IsNullOrEmpty(value.ConfirmPassword))
                {
                    throw new Exception("'Password' is required");
                }
                if (value.Password != value.ConfirmPassword)
                {
                    throw new Exception("'Confirm Password' does not match Password");
                }

                AuthRegisterResponse registerResults = _authControl.Register(value.UserName, value.Password);

                return new JsonResult(registerResults);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        //public async Task<IActionResult> Logout()
        //{
        //    await this.HttpContext.SignOutAsync("oauth2");
        //    //Response.Cookies.Delete(".AspNetCore.Identity.Application", new CookieOptions { HttpOnly = true });

        //    return RedirectToAction("Index", "Home");
        //}


        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
