using YoutubeDLSharp.Options;
using YoutubeDLSharp;
using System.Runtime.InteropServices;

namespace armaradio.Repositories
{
    public class ArmaYoutubeDownloader
    {
        private readonly YoutubeDL _youtubeDl;

        public ArmaYoutubeDownloader(
            YoutubeDL youtubeDl
        )
        {
            _youtubeDl = youtubeDl;
        }

        public async Task DownloadAudioFileAsync(string url, string endFileName)
        {
            var options = new OptionSet
            {
                Format = "bestaudio[ext=m4a]/bestaudio",
                Output = endFileName,
                ExtractAudio = true,
                AudioFormat = AudioConversionFormat.M4a,
                NoPlaylist = true,
                NoCheckCertificates = true,
                NoWarnings = true,
                Downloader = "native",
                BufferSize = 16000, // Increased buffer size
                ConcurrentFragments = 4, // Download multiple fragments concurrently
                FragmentRetries = 10, // Retry failed fragments
                ForceIPv4 = true, // Force IPv4 to potentially avoid slow IPv6 connections
                SocketTimeout = 10,
                DownloaderArgs = "-4"
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
                options.NoCacheDir = true; // Disable cache directory on Linux
                options.DownloaderArgs = "-4 native:buffer_size=16k"; // Set native downloader buffer size
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
                throw new Exception((result.ErrorOutput != null && result.ErrorOutput.Length > 0 ? string.Join("; ", result.ErrorOutput) : "An error occurred"));
            }
        }
    }
}
