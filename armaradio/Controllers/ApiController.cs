using armaradio.Attributes;
using armaradio.Models;
using armaradio.Models.Home;
using armaradio.Models.Request;
using armaradio.Repositories;
using FFMpegCore;
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
        public ApiController(
            IArmaAuth authControl,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IMusicRepo musicRepo,
            IWebHostEnvironment hostEnvironment
        )
        {
            _authControl = authControl;
            _signInManager = signInManager;
            _userManager = userManager;
            _musicRepo = musicRepo;
            _hostEnvironment = hostEnvironment;

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
        public async Task<IActionResult> GetAudioFile(string VideoId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(VideoId))
                {
                    List<string> fileNameParts = new List<string>();
                    string rootPath = _hostEnvironment.WebRootPath.TrimEnd('/').TrimEnd('\\');
                    string downloadFolder = (!IsWindows ? $"{rootPath}/tempMp3/" : $"{rootPath}\\tempMp3\\");
                    string fileHandle = $"{SanitizeFileName(VideoId).ToLower()}.mp3";
                    string fileHandleTemp = $"{Guid.NewGuid().ToString().ToLower()}";
                    string endFile = $"{downloadFolder}{fileHandle}";
                    string endTempFile = $"{downloadFolder}{fileHandleTemp}";

                    if (!System.IO.Directory.Exists(downloadFolder))
                    {
                        System.IO.Directory.CreateDirectory(downloadFolder);
                    }

                    var youtube = new YoutubeExplode.YoutubeClient();
                    var streamManifest = await youtube.Videos.Streams.GetManifestAsync($"https://www.youtube.com/watch?v={VideoId}");
                    var allStreams = streamManifest.GetAudioOnlyStreams();
                    var streamInfo = allStreams.GetWithHighestBitrate();
                    endTempFile = $"{endTempFile}.{streamInfo.Container.Name}";

                    await youtube.Videos.Streams.DownloadAsync(streamInfo, endTempFile);

                    ConvertToMp3(endTempFile, endFile);

                    System.IO.File.Delete(endTempFile);

                    MemoryStream memoryStream = new MemoryStream();
                    using (FileStream fileStream = new FileStream(endFile, FileMode.Open, FileAccess.Read))
                    {
                        fileStream.CopyTo(memoryStream);
                    }

                    System.IO.File.Delete(endFile);

                    memoryStream.Position = 0;

                    var returnItem = new FileStreamResult(memoryStream, "audio/mpeg")
                    {
                        FileDownloadName = fileHandle,
                    };

                    Response.RegisterForDispose(memoryStream);

                    return returnItem;
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        private static void ConvertToMp3(string inputFilePath, string outputFilePath)
        {
            FFMpegArguments
                .FromFileInput(inputFilePath)
                .OutputToFile(outputFilePath, true, options => options
                    .WithAudioCodec("libmp3lame")
                    .WithAudioBitrate(192)
                )
                .ProcessSynchronously();
        }

        public static string SanitizeFileName(string fileName)
        {
            // Define a regular expression pattern to match harmful characters
            string pattern = @"[/\\?%*:|""<>]";

            // Replace harmful characters with an empty string
            return Regex.Replace(fileName, pattern, "");
        }
    }
}
