using armaradio.Models;
using armaradio.Models.Youtube;

namespace armaradio.Repositories
{
    public interface IMusicRepo
    {
        List<TrackDataItem> GetCurrentTop100();
        List<ArtistDataItem> Artist_GetArtistList(string search);
        List<TrackDataItem> Tracks_GetTop50Songs();
        List<TrackDataItem> GetCurrentTop40DanceSingles();
        List<TrackDataItem> GetCurrentTranceTop100();
        public List<TrackDataItem> GetCurrentTranceHype100();
        string Youtube_GetUrlByArtistNameSongName(string artistName, string songName);
        List<YTGeneralSearchDataItem> Youtube_PerformGeneralSearch(string searchText);
        bool CheckIfPlaylistExists(string PlaylistName, string UserId);
        int? InsertPlaylistName(string PlaylistName, string UserId);
        void InsertSongToPlaylist(int PlaylistId, string Artist, string Song);
        List<ArmaPlaylistDataItem> GetPlaylistByName(string PlaylistName, string UserId);
        List<ArmaPlaylistDataItem> GetPlaylistById(int PlaylistId, string UserId);
        List<ArmaUserPlaylistDataItem> GetUserPlaylists(string UserId);
        void DeleteSongFromPlaylist(int SongId, string UserId);
        void DeleteUserPlaylistAndData(int PlaylistId, string UserId);
        void AddSongToPlaylist(int PlaylistId, string Artist, string Song, string VideoId);
    }
}
