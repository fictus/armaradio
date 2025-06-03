namespace armaradio.Repositories
{
    public interface IArmaAudioDownloader
    {
        Task<string> DownloadAudioAsync(string youtubeUrl, string outputFileName);
    }
}
