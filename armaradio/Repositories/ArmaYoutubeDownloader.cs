using AngleSharp.Dom;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;

namespace armaradio.Repositories
{
    public class ArmaYoutubeDownloader
    {
        private readonly bool IsWindows = false;
        private readonly YoutubeDL _youtubeDl;
        private readonly ILogger<ArmaAudioDownloader> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ArmaYoutubeDownloader(
            YoutubeDL youtubeDl,
            ILogger<ArmaAudioDownloader> logger,
            IWebHostEnvironment hostEnvironment
        )
        {
            _youtubeDl = youtubeDl;
            _logger = logger;
            _hostEnvironment = hostEnvironment;

            IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public async Task DownloadAudioFileAsync(string url, string endFileName)
        {
            string tempCookiesFile = "";

            try
            {
                string rootPath = _hostEnvironment.WebRootPath.TrimEnd('/').TrimEnd('\\');
                string cookiesFile = (!IsWindows ? $"{rootPath}/cookies/file.txt" : $"{rootPath}\\cookies\\file.txt");

                tempCookiesFile = Path.Combine(Path.GetTempPath(), $"yt-cookies-{Guid.NewGuid()}.txt");
                File.Copy(cookiesFile, tempCookiesFile, true);

                var retrySleep = new MultiValue<string>();

                retrySleep.Values.Add("http:exp=2:60");
                retrySleep.Values.Add("fragment:linear=2:10");
                retrySleep.Values.Add("extractor:exp=1:30");

                var options = new OptionSet
                {
                    //Format = "bestaudio/best",//"bestaudio[ext=m4a]/bestaudio",
                    //Format = "ba*[ext=m4a]/ba*/b*[ext=m4a]/b*",
                    Format = "best",
                    Output = endFileName,
                    ExtractAudio = true,
                    AudioFormat = AudioConversionFormat.M4a,
                    NoPlaylist = true,
                    NoCheckCertificates = true,
                    NoWarnings = true,
                    Downloader = "native",
                    BufferSize = 1048576, // Increased buffer size
                    ConcurrentFragments = 2, // Download multiple fragments concurrently
                    Retries = 10,
                    ThrottledRate = 500000,
                    //RetrySleep = retrySleep,
                    FragmentRetries = 15, // Retry failed fragments
                    ForceIPv4 = true, // Force IPv4 to potentially avoid slow IPv6 connections
                    SocketTimeout = 30,
                    KeepFragments = false,
                    WriteInfoJson = false,
                    //DownloaderArgs = "-4"
                    Verbose = true,
                    SleepInterval = 5,
                    MaxSleepInterval = 10,
                    Cookies = tempCookiesFile,
                    CustomOptions = new IOption[]
                    {
                        new Option<string>("--js-runtimes", "deno")
                    }
                };
                //var options = new OptionSet
                //{
                //    Format = "bestaudio[ext=m4a]/bestaudio",
                //    Output = endFileName,
                //    ExtractAudio = true,
                //    AudioFormat = AudioConversionFormat.M4a,
                //    NoPlaylist = true,
                //    NoCheckCertificates = true,
                //    NoWarnings = true,
                //    Downloader = "native"
                //};

                options.ExtractorArgs = "youtube:player_client=android";

                //options.ExtractorArgs = "--no-cookies-update"; // "youtube:player_client=android,ios";
                options.AddHeaders = new MultiValue<string>();

                options.AddHeaders.Values.Clear();
                options.AddHeaders.Values.Add("User-Agent:com.google.android.youtube/19.09.37 (Linux; U; Android 13) gzip");
                options.AddHeaders.Values.Add("Accept-Language:en-US,en;q=0.9");

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    //options.NoCacheDir = true; // Disable cache directory on Linux
                    options.DownloaderArgs = "-4 --buffer-size 1M"; // Set native downloader buffer size
                }

                var result = await _youtubeDl.RunAudioDownload(
                    url,
                    AudioConversionFormat.M4a,
                    progress: null,
                    output: null,
                    overrideOptions: options
                );

                if (!result.Success)
                {
                    string errorMsg = (result.ErrorOutput != null && result.ErrorOutput.Length > 0 ? string.Join("; ", result.ErrorOutput) : "An error occurred");
                    Console.WriteLine($"yt-dlp failed: {errorMsg}");

                    throw new Exception(errorMsg);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message.ToString() + "; Stack-Trace: " + ex.StackTrace.ToString());

                throw;
            }
            finally
            {
                if (File.Exists(tempCookiesFile))
                {
                    try
                    {
                        File.Delete(tempCookiesFile);
                    }
                    catch (Exception ex)
                    {
                        // Log but don't throw - cleanup failure shouldn't break the download
                        Console.WriteLine($"Warning: Could not delete temp cookie file: {ex.Message}");
                    }
                }
            }
        }
    }
}
