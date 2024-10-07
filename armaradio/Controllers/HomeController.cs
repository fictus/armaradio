using armaradio.Models.ArmaAuth;
using armaradio.Models.Home;
using armaradio.Repositories;
using Azure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace armaradio.Controllers
{
    [DisableCors]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArmaAuth _authControl;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMusicRepo _musicRepo;
        private readonly Operations.ArmaUserOperation _operation;

        public HomeController(
            ILogger<HomeController> logger,
            IArmaAuth authControl,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IMusicRepo musicRepo,
            Operations.ArmaUserOperation operation
        )
        {
            _logger = logger;
            _authControl = authControl;
            _userManager = userManager;
            _musicRepo = musicRepo;
            _operation = operation;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            using (_operation)
            {
                //_musicRepo.DuckDuckGo_PerformGeneralSearch("depeche mode - somebody");
                return View();
            }
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
        public IActionResult UserLogin([FromBody] LoginRequest value)
        {
            try
            {
                _operation.SetRequestBody(value);
                using (_operation)
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

                    //AuthLoginResponse loginResults = _authControl.Login(value.UserName, value.Password);

                    var result = _signInManager.PasswordSignInAsync(value.UserName, value.Password, false, lockoutOnFailure: false).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception("Incorrect Username or Password");
                    }

                    return new JsonResult(Ok());
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.ToString() == "Unauthorized" ? "Incorrect Email or Password" : ex.Message.ToString();
                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAccount([FromBody] RegisterRequest value)
        {
            try
            {
                _operation.SetRequestBody(value);
                using (_operation)
                {
                    if (value == null)
                    {
                        throw new Exception("Invalid request");
                    }
                    if (string.IsNullOrWhiteSpace(value.UserName))
                    {
                        throw new Exception("'Email' is required");
                    }
                    if (!_authControl.IsValidEmailAddress(value.UserName))
                    {
                        throw new Exception("'Email' is incorrect");
                    }
                    if (string.IsNullOrWhiteSpace(value.Password) || string.IsNullOrWhiteSpace(value.ConfirmPassword))
                    {
                        throw new Exception("Contraseña es requerida");
                    }

                    if (value.Password != value.ConfirmPassword)
                    {
                        throw new Exception("Contraseñas no coinciden");
                    }

                    var user = new IdentityUser
                    {
                        UserName = value.UserName,
                        Email = value.UserName
                    };

                    var validators = _userManager.PasswordValidators;

                    foreach (var validator in validators)
                    {
                        var result = await validator.ValidateAsync(_userManager, user, value.Password);

                        if (!result.Succeeded)
                        {
                            List<string> allErrors = new List<string>();

                            foreach (var error in result.Errors)
                            {
                                allErrors.Add(error.Description);
                            }

                            throw new Exception(string.Join("\n", allErrors.ToArray()));
                        }
                    }

                    AuthRegisterResponse registerResults = _authControl.Register(value.UserName, value.Password);

                    if (registerResults.status != 0)
                    {
                        throw new Exception(registerResults.detail);
                    }

                    if (registerResults.errors != null)
                    {
                        if (registerResults.errors.additionalProp1?.Any() == true)
                        {
                            throw new Exception(string.Join("\n", registerResults.errors.additionalProp1.ToArray()));
                        }
                        if (registerResults.errors.additionalProp2?.Any() == true)
                        {
                            throw new Exception(string.Join("\n", registerResults.errors.additionalProp2.ToArray()));
                        }
                        if (registerResults.errors.additionalProp3?.Any() == true)
                        {
                            throw new Exception(string.Join("\n", registerResults.errors.additionalProp3.ToArray()));
                        }
                    }

                    var loginResult = _signInManager.PasswordSignInAsync(value.UserName, value.Password, false, lockoutOnFailure: false).Result;

                    return new JsonResult(Ok());
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content(ex.Message.ToString());
            }
        }

        [HttpPost]
        public IActionResult Logout([FromBody] object empty)
        {
            if (empty != null)
            {
                _signInManager.SignOutAsync().Wait();
            }

            return new JsonResult(Ok());
        }

        //[HttpPost]
        //public IActionResult Register([FromBody] RegisterRequest value)
        //{
        //    try
        //    {
        //        _operation.SetRequestBody(value);
        //        using (_operation)
        //        {
        //            if (value == null)
        //            {
        //                throw new Exception("Invalid request");
        //            }
        //            if (string.IsNullOrEmpty(value.UserName))
        //            {
        //                throw new Exception("'Email' is required");
        //            }
        //            if (!_authControl.IsValidEmailAddress(value.UserName))
        //            {
        //                throw new Exception("'Email' is incorrect");
        //            }
        //            if (string.IsNullOrEmpty(value.Password) || string.IsNullOrEmpty(value.ConfirmPassword))
        //            {
        //                throw new Exception("'Password' is required");
        //            }
        //            if (value.Password != value.ConfirmPassword)
        //            {
        //                throw new Exception("'Confirm Password' does not match Password");
        //            }

        //            AuthRegisterResponse registerResults = _authControl.Register(value.UserName, value.Password);

        //            return new JsonResult(registerResults);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
        //    }
        //}

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
