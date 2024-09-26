using YoutubeDLSharp.Options;
using YoutubeDLSharp;

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
                Downloader = "native"
            };

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
