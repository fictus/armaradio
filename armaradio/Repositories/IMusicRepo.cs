using armaradio.Models;
using armaradio.Models.Request;
using armaradio.Models.Response;
using armaradio.Models.Youtube;

namespace armaradio.Repositories
{
    public interface IMusicRepo
    {
        List<TrackDataItem> GetCurrentTop100();
        List<ArmaArtistDataItem> Artist_FindArtists(string search);
        ArmaArtistAlbumsResponse Albums_GetArtistsAlbums(int artistId);
        List<ArmaAlbumSongDataItem> Albums_GetAlbumSongs(int artistId, int albumId);
        List<ArmaRandomSongDataItem> Songs_GetRandomFromPlaylists(string userIdentity);
        string GetApiToken();
        RadioSessionSongsResponse GetRadioPlalistSongsFromArtist(string artistName);
        RadioSessionRecommendedResponse GetRadioSessionRecommendedSongsFromArtist(string artistName, string songName);
        Task<ArmaAISongsResponse> GetAIRecommendedSongs(string artistName, string songName, int limit = 10);
        ArtistPlaylistsResponse GetArtistPlaylists(string artistName, string songName);
        List<ArtistDataItem> Artist_GetArtistList(string search);
        List<TrackDataItem> Tracks_GetTop50Songs();
        List<TrackDataItem> GetCurrentTop40DanceSingles();
        List<TrackDataItem> GetCurrentTranceTop100();
        List<TrackDataItem> GetCurrentTranceHype100();
        List<TrackDataItem> GetCurrentLatinTop50();
        List<TrackDataItem> GetCurrentTopDanceElectronic();
        List<TrackDataItem> GetCurrentTopRockAlternative();
        List<TrackDataItem> GetTopEmergingArtists();
        List<TrackDataItem> GetCurrentTopCountrySongs();
        List<TrackDataItem> GetTopUserRankedArtists5stars();
        List<TrackDataItem> GetTopUserRankedArtists4stars();
        List<ProxySocks4DataItem> GetSocks4ProxyList();
        List<AdaptiveFormatDataItem> GetAudioStreams(string VideoId);
        void DownloadMp4File(string url, string endFileName);
        YTVideoIdsDataItem Youtube_GetUrlByArtistNameSongName(string artistName, string songName);
        List<YTGeneralSearchDataItem> Youtube_PerformGeneralSearch(string searchText);
        List<YTGeneralSearchDataItem> DuckDuckGo_PerformGeneralSearch(string searchText);
        bool CheckIfPlaylistExists(string PlaylistName, string UserId);
        int? InsertPlaylistName(string PlaylistName, string UserId);
        void InsertSongToPlaylist(int PlaylistId, string Artist, string Song);
        List<ArmaPlaylistDataItem> GetPlaylistByName(string PlaylistName, string UserId);
        string GetSharedPlaylistToken(int PlaylistId, string UserId);
        ArmaSharedPlaylistDataItem GetSharedPlaylist(string Token);
        List<ArmaPlaylistDataItem> GetPlaylistById(int PlaylistId, string UserId);
        List<ArmaUserPlaylistDataItem> GetUserPlaylists(string UserId);
        void DeleteSongFromPlaylist(int SongId, string UserId);
        void DeleteUserPlaylistAndData(int PlaylistId, string UserId);
        void AddSongToPlaylist(int PlaylistId, string Artist, string Song, string VideoId);
    }
}
