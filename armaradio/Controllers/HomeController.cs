using armaradio.Models;
using armaradio.Models.ArmaAuth;
using armaradio.Models.Home;
using armaradio.Repositories;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using YoutubeExplode.Channels;

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
        private readonly IDapperHelper _dapper;
        private readonly ArmaSmtp.IArmaEmail _email;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(
            ILogger<HomeController> logger,
            IArmaAuth authControl,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IMusicRepo musicRepo,
            Operations.ArmaUserOperation operation,
            IDapperHelper dapper,
            ArmaSmtp.IArmaEmail email,
            IWebHostEnvironment hostEnvironment
        )
        {
            _logger = logger;
            _authControl = authControl;
            _userManager = userManager;
            _musicRepo = musicRepo;
            _operation = operation;
            _signInManager = signInManager;
            _dapper = dapper;
            _email = email;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (_operation)
            {
                //await _musicRepo.GetAIRecommendedSongs("Rank1", "Airwaves");
                //_musicRepo.DuckDuckGo_PerformGeneralSearch("depeche mode - somebody");

                ArmaAdminControlsDataItem adminControls = new ArmaAdminControlsDataItem();

                if (User.Identity.IsAuthenticated)
                {
                    adminControls.ShowAdminControls = _authControl.IsAdminUser();
                }

                return View(adminControls);
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

        [Authorize]
        [HttpGet]
        public IActionResult GetSessionCookie()
        {
            try
            {
                string returnItem = "";

                if (_authControl.IsAdminUser())
                {
                    returnItem = _dapper.GetFirstOrDefault<string>("radioconn", "ArmaAdmin_GetSessionCookie");
                }

                return new JsonResult(new
                {
                    cookie = returnItem
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult SaveSessionCookie([FromBody] ArmaCookieRequest value)
        {
            try
            {
                if (!_authControl.IsAdminUser())
                {
                    throw new Exception("invalid request");
                }
                if (value == null || string.IsNullOrWhiteSpace(value.Cookie))
                {
                    throw new Exception("invalid request");
                }

                if (_authControl.IsAdminUser())
                {
                    int result = _dapper.ExecuteNonQuery("radioconn", "ArmaAdmin_SetSessionCookie", new
                    {
                        cookie = value.Cookie
                    });

                    if (result != 0)
                    {
                        var rootPath = _hostEnvironment.WebRootPath.TrimEnd('/').TrimEnd('\\');
                        var cookiesDir = Path.Combine(rootPath, "wwwroot", "cookies");
                        var cookiesFile = Path.Combine(cookiesDir, "file.txt");

                        if (!Directory.Exists(cookiesDir))
                        {
                            Directory.CreateDirectory(cookiesDir);
                        }

                        if (System.IO.File.Exists(cookiesFile))
                        {
                            System.IO.File.Delete(cookiesFile);
                        }

                        System.IO.File.WriteAllText(cookiesFile, value.Cookie);

                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                            RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                        {
                            try
                            {
                                System.IO.File.SetUnixFileMode(cookiesFile,
                                    UnixFileMode.UserRead | UnixFileMode.UserWrite |
                                    UnixFileMode.GroupRead | UnixFileMode.OtherRead);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }

                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
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

                    bool? emailIsConfirmed = _dapper.GetFirstOrDefault<bool?>("radioconn", "ArmaUsers_GetEmailConfirmedStatus", new
                    {
                        email = value.UserName
                    });

                    if (emailIsConfirmed.HasValue && !emailIsConfirmed.Value)
                    {
                        throw new Exception("Email has not been confirmed. Please follow the instructions sent to you via email.");
                    }

                    var result = _signInManager.PasswordSignInAsync(value.UserName, value.Password, true, lockoutOnFailure: false).Result;

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
                        throw new Exception("'Password' is required");
                    }

                    if (value.Password != value.ConfirmPassword)
                    {
                        throw new Exception("'Confirm Password' does not match Password");
                    }

                    var user = new IdentityUser
                    {
                        UserName = value.UserName,
                        Email = value.UserName
                    };

                    var newUser = _userManager.Users.Where(usr => usr.Email.Equals(value.UserName)).FirstOrDefault();

                    if (newUser != null)
                    {
                        throw new Exception("Account already exists for this email");
                    }

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

                    //var loginResult = _signInManager.PasswordSignInAsync(value.UserName, value.Password, false, lockoutOnFailure: false).Result;

                    string emailToken = _dapper.GetFirstOrDefault<string>("radioconn", "ArmaUsers_CreateEmailConfirmationRequest", new
                    {
                        email = value.UserName.Trim()
                    });

                    if (string.IsNullOrWhiteSpace(emailToken))
                    {
                        throw new Exception("Registration failed");
                    }

                    string confirmUrl = $"https://armarad.com/registration/confirmemail/?t={emailToken}";
                    string emailBody = @$"
                        Welcome to ArmaRad!
                        <br><br>
                        To complete registration please confirm your email:
                        <br>
                        <br>
                        <a href=""{confirmUrl}"">{confirmUrl}</a>
                    ";
                    List<string> toEmails = new List<string>();

                    toEmails.Add(value.UserName);

                    _email.SendEmail("noreply@armarad.com", "Confirm Your Email", toEmails, emailBody, emailToken);

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
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] RegisterRequest value)
        {
            try
            {
                _operation.SetRequestBody(value);
                using (_operation)
                {
                    ArmaUser currentUser = _authControl.GetCurrentUser();

                    if (value == null || currentUser == null)
                    {
                        throw new Exception("Invalid request");
                    }
                    if (string.IsNullOrWhiteSpace(value.Password) || string.IsNullOrWhiteSpace(value.ConfirmPassword))
                    {
                        throw new Exception("'Password' is required");
                    }
                    if (value.Password != value.ConfirmPassword)
                    {
                        throw new Exception("'Confirm Password' does not match Password");
                    }

                    var armaUser = _userManager.Users.Where(usr => usr.Email.Equals(currentUser.UserName)).FirstOrDefault();
                    string token = await _userManager.GeneratePasswordResetTokenAsync(armaUser);

                    var passwordResult = await _userManager.ResetPasswordAsync(armaUser, token, value.Password);

                    if (!passwordResult.Succeeded)
                    {
                        List<string> errors = passwordResult.Errors.Select(err =>
                        {
                            return err.Description;
                        }).ToList();

                        throw new Exception(string.Join("; ", errors.ToArray()));
                    }

                    string emailBody = @$"
                        Hi,
                        <br><br>
                        This is a confirmation that your password has been reset.
                    ";
                    List<string> toEmails = new List<string>();

                    toEmails.Add(value.UserName);

                    string emailToken = _dapper.GetFirstOrDefault<string>("radioconn", "ArmaUsers_GetEmailConfirmationTokenByEmail", new
                    {
                        email = value.UserName.Trim()
                    });

                    _email.SendEmail("noreply@armarad.com", "Your password has been reset", toEmails, emailBody, emailToken);

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
        [Authorize]
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
