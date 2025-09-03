using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;

namespace armaradio.Repositories
{
    public class ArmaYoutubeDownloader
    {
        private readonly YoutubeDL _youtubeDl;
        private readonly ILogger<ArmaAudioDownloader> _logger;

        public ArmaYoutubeDownloader(
            YoutubeDL youtubeDl,
            ILogger<ArmaAudioDownloader> logger
        )
        {
            _youtubeDl = youtubeDl;
            _logger = logger;
        }

        public async Task DownloadAudioFileAsync(string url, string endFileName)
        {
            try
            {
                var retrySleep = new MultiValue<string>();

                retrySleep.Values.Add("http:exp=1:30");
                retrySleep.Values.Add("fragment:linear=1:10:2");
                retrySleep.Values.Add("5");

                var options = new OptionSet
                {
                    Format = "bestaudio[ext=m4a]/bestaudio[abr<=128]/bestaudio",
                    Output = endFileName,
                    ExtractAudio = true,
                    AudioFormat = AudioConversionFormat.M4a,
                    NoPlaylist = true,
                    NoCheckCertificates = true,
                    NoWarnings = true,
                    Downloader = "native",
                    BufferSize = 1048576, // Increased buffer size
                    ConcurrentFragments = 8, // Download multiple fragments concurrently
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
                    AddHeaders = "--add-header \"User-Agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36\" " +
                        "--add-header \"Accept-Language:en-US,en;q=0.9\" " +
                        "--add-header \"Accept-Encoding:gzip, deflate\" " +
                        "--add-header \"Accept:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\" " +
                        "--sleep-interval 5"
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
        }
    }
}
