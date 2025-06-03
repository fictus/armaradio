using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using armaoffline.Models;

namespace armaoffline.Repositories
{
    public interface IArmaApi
    {
        bool Signin(string Email, string Password);
        List<ArmaUserDataItem> Offline_GetListOfOfflineUsers();
        List<ArmaUserPlaylistDataItem> GetUserPlaylists();
        List<ArmaPlaylistDataItem> GetPlaylistById(int playlistId);
        List<ArmaPlaylistDataItem> Offline_GetPlaylistById(int playlistId);
        List<ArmaPlaylistDataItem> Offline_GetRandomSongs(int UserId);
        List<ArmaPlaylistDataItem> Offline_SearchSongs(int UserId, string SearchText);
        List<ArmaUserPlaylistDataItem> Offline_GetAvailablePlaylists(int UserId);
        void DownloadPlaylistSongsForOffline(int? PlaylistId);
        Task<bool?> GetAudioFile(string VideoId);
        void MarkPlaylistSongsAsDownloaded(int PlaylistId);
        public bool CheckIfAudioFileExists(string FileName);
        string GetFileWithPath(string FileName);
    }
}
