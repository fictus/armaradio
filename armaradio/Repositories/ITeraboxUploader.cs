using static armaradio.Repositories.TeraboxUploader;

namespace armaradio.Repositories
{
    public interface ITeraboxUploader
    {
        Task<PreCreateResponse> PreCreateUpload(string filePath, long fileSize, DateTime localMtime);
        Task<string> UploadFile(string filePath, PreCreateResponse preCreateResponse);
    }
}
