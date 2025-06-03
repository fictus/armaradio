using armaradio.Attributes;
using armaradio.Models;
using armaradio.Models.Home;
using armaradio.Models.Request;
using armaradio.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;

namespace armaradio.Controllers
{
    [EnableCors]
    public class ApiController : Controller
    {
        private readonly bool IsWindows = false;
        private readonly IArmaAuth _authControl;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMusicRepo _musicRepo;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IDapperHelper _dapper;
        private readonly ArmaYoutubeDownloader _armaYTDownloader;
        public ApiController(
            IArmaAuth authControl,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IMusicRepo musicRepo,
            IWebHostEnvironment hostEnvironment,
            IDapperHelper dapper,
            ArmaYoutubeDownloader armaYTDownloader
        )
        {
            _authControl = authControl;
            _signInManager = signInManager;
            _userManager = userManager;
            _musicRepo = musicRepo;
            _hostEnvironment = hostEnvironment;
            _dapper = dapper;
            _armaYTDownloader = armaYTDownloader;

            IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
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

        [ApiTokenAttribute]
        [HttpPost]
        public IActionResult GetUserPlaylists()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.FindByIdAsync(userId).Result;

            if (user == null)
            {
                // Handle the case where the user is not found in the user store
                return NotFound();
            }

            List<ArmaUserPlaylistDataItem> returnItem = _musicRepo.GetUserPlaylists(userId);

            return new JsonResult(returnItem);
        }

        [ApiTokenAttribute]
        [HttpPost]
        public IActionResult GetPlaylistById([FromBody] ApiGetPlaylistByIdRequest value)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.FindByIdAsync(userId).Result;

            if (user == null)
            {
                // Handle the case where the user is not found in the user store
                return NotFound();
            }

            List<ArmaPlaylistDataItem> returnItem = _musicRepo.GetPlaylistById(value.PlaylistId, userId);

            return new JsonResult(returnItem);
        }

        [ApiTokenAttribute]
        [HttpGet]
        public async Task<IActionResult> GetAudioFileDetails(string VideoId)
        {
            try
            {
                var youtube = new YoutubeExplode.YoutubeClient();
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"https://www.youtube.com/watch?v={(VideoId ?? "").Trim()}");
                var allStreams = streamManifest.GetAudioOnlyStreams();
                var streamInfo = allStreams.GetWithHighestBitrate();
                var fileType = MimeTypes.GetMimeType($"tmpFileName.{streamInfo.Container.Name}");

                return new JsonResult(new
                {
                    FileExtension = streamInfo.Container.Name,
                    MimeType = fileType
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [ApiTokenAttribute]
        [HttpGet]
        public IActionResult GetAudioFile(string VideoId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(VideoId))
                {
                    string rootPath = _hostEnvironment.WebRootPath.TrimEnd('/').TrimEnd('\\');
                    string downloadFolder = (!IsWindows ? $"{rootPath}/AudioFiles/" : $"{rootPath}\\AudioFiles\\");
                    string endFileName = endFileName = $"{downloadFolder}{VideoId.Trim()}.m4a";
                    string fileType = "audio/mp4";

                    if (!System.IO.Directory.Exists(downloadFolder))
                    {
                        System.IO.Directory.CreateDirectory(downloadFolder);
                    }

                    if (!System.IO.File.Exists(endFileName))
                    {
                        _musicRepo.DownloadMp4File($"https://www.youtube.com/watch?v={(VideoId ?? "").Trim()}", endFileName);
                    }

                    _musicRepo.FlagFileForDeletion(endFileName);

                    return new FileStreamResult(new FileStream(endFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), fileType)
                    {
                        EnableRangeProcessing = true
                    };
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                _dapper.ExecuteNonQuery("radioconn", "ArmaError_LogError", new
                {
                    ErrorController = "ArmaRadio_Api",
                    ErrorMethod = "GetAudioFile",
                    ErrorMessage = ex.Message.ToString()
                });

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRelatedArtistIDs(string artistid, string tk)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(artistid) || string.IsNullOrWhiteSpace(tk))
                {
                    throw new Exception("Invalid request");
                }

                List<ArmaApiSimilarArtistIdDataItem> returnItems = new List<ArmaApiSimilarArtistIdDataItem>();

                if (Guid.TryParse(tk, out Guid _token))
                {
                    bool isTokenValid = _musicRepo.UseSiteApiToken(_token);

                    if (isTokenValid)
                    {
                        returnItems = _musicRepo.SiteApiGetSimilarArtistIds(artistid) ?? new List<ArmaApiSimilarArtistIdDataItem>();
                    }
                }

                return new JsonResult(returnItems);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        //private static void ConvertToMp3(string inputFilePath, string outputFilePath)
        //{
        //    FFMpegArguments
        //        .FromFileInput(inputFilePath)
        //        .OutputToFile(outputFilePath, true, options => options
        //            .WithAudioCodec("libmp3lame")
        //            .WithAudioBitrate(192)
        //        )
        //        .ProcessSynchronously();
        //}

        public static string SanitizeFileName(string fileName)
        {
            // Define a regular expression pattern to match harmful characters
            string pattern = @"[/\\?%*:|""<>]";

            // Replace harmful characters with an empty string
            return Regex.Replace(fileName, pattern, "");
        }
    }
}
