using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace armaradio.Repositories
{
    public class ArmaAudioDownloader : IArmaAudioDownloader
    {
        public ArmaAudioDownloader()
        {
        }

        public async Task<string> DownloadAudioAsync(string youtubeUrl, string outputFileName)
        {
            try
            {
                if (File.Exists(outputFileName))
                {
                    File.Delete(outputFileName);
                }

                // Determine yt-dlp executable name based on OS
                var ytDlpExecutable = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "C:\\YTDL\\yt-dlp.exe"
                    : "/home/fictus/.local/bin/yt-dlp"; // "/usr/local/bin/yt-dlp";

                var arguments = new StringBuilder()
                    .Append("--extract-audio ")
                    .Append("--audio-format m4a ")
                    .Append("--audio-quality 0 ")  // Best quality
                    .Append("--concurrent-fragments 8 ")
                    //.Append("--postprocessor-args \"-strict -2 -b:a 96k -ac 2 -ar 44100\" ")
                    .Append("--format \"bestaudio[ext=m4a]/bestaudio/best\" ")  // Ensures we get audio streams only
                    .Append("--concurrent-fragments 8 ")
                    .Append("--no-playlist ")      // Avoid accidental playlist downloads
                    .Append("--retries 10 ")       // Increased retry attempts
                    .Append("--fragment-retries 10 ")
                    .Append("--buffer-size 1024K ")  // Better network handling
                    .Append("--no-check-certificates ")
                    .Append("--throttled-rate 500K ") // Slow down if throttled
                    .Append($"--user-agent \"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36\" ")
                    //.Append($"--cookies-from-browser chrome ")  // Use your browser cookies
                    //.Append($"--no-check-certificates ")
                    .Append($"--output \"{outputFileName}\" ")
                    .Append($"\"{youtubeUrl}\"")
                    .ToString();
                //var arguments = $"-x --audio-format m4a --audio-quality 0 -o \"{outputFileName}\" \"{youtubeUrl}\"";

                var processInfo = new ProcessStartInfo
                {
                    FileName = ytDlpExecutable,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = new Process { StartInfo = processInfo };
                process.Start();

                // Read output streams (important to prevent deadlocks)
                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"yt-dlp failed: {error}");
                }

                return outputFileName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
