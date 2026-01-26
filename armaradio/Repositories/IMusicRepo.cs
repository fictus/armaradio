using armaradio.Models;
using armaradio.Models.Odysee;
using armaradio.Models.Request;
using armaradio.Models.Response;
using armaradio.Models.Youtube;

namespace armaradio.Repositories
{
    public interface IMusicRepo
    {
        Guid? GetSiteApiToken();
        bool UseSiteApiToken(Guid token);
        Task<List<TrackDataItem>> GetCurrentTop100();
        List<ArmaArtistDataItem> Artist_FindArtists(string search);
        ArmaArtistAlbumsResponse Albums_GetArtistsAlbums(int artistId);
        byte[] CompressJsonString(string data);
        List<ArmaAlbumSongDataItem> Albums_GetAlbumSongs(int artistId, int albumId);
        List<ArmaRandomSongDataItem> Songs_GetRandomFromPlaylists(string userIdentity);
        string GetApiToken();
        RadioSessionSongsResponse GetRadioPlalistSongsFromArtist(string artistName);
        List<ArmaRecommendationDataItem> GetRadioSessionRecommendedSongsFromArtist(string artistName, string songName);
        List<ArmaApiSimilarArtistIdDataItem> SiteApiGetSimilarArtistIds(string artistid);
        Task<ArmaAISongsResponse> GetAIRecommendedSongs(string artistName, string songName, int limit = 10);
        ArtistPlaylistsResponse GetArtistPlaylists(string artistName, string songName);
        List<ArtistDataItem> Artist_GetArtistList(string search);
        List<TrackDataItem> Tracks_GetTop50Songs();
        Guid? Tracks_CacheTop100LastFMTrending();
        List<TrackDataItem> Tracks_GetTop100LastFMTrending(Guid requestId);
        Task<List<TrackDataItem>> GetCurrentTop40DanceSingles();
        Task<List<TrackDataItem>> GetCurrentTranceTop100();
        Task<List<TrackDataItem>> GetCurrentTranceHype100();
        Task<List<TrackDataItem>> GetCurrentLatinTop50();
        Task<List<TrackDataItem>> GetCurrentTopDanceElectronic();
        Task<List<TrackDataItem>> GetCurrentTopRockAlternative();
        Task<List<TrackDataItem>> GetTopEmergingArtists();
        Task<List<TrackDataItem>> GetCurrentTopCountrySongs();
        Task<List<TrackDataItem>> GetCurrentTopRegionalMexicanoSongs();
        List<TrackDataItem> GetTopUserRankedArtists5stars();
        List<TrackDataItem> GetTopUserRankedArtists4stars();
        Task<List<ProxySocks4DataItem>> GetSocks4ProxyList();
        Task<List<AdaptiveFormatDataItem>> GetAudioStreams(string VideoId);
        Task DownloadMp4File(string url, string endFileName);
        void FlagFileForDeletion(string FullDirFIleName);
        Task<YTVideoIdsDataItem> Youtube_GetUrlByArtistNameSongName(string artistName, string songName);
        Task<List<YTGeneralSearchDataItem>> Youtube_PerformGeneralSearch(string searchText);
        Task<List<OdyseeSearchResult>> SearchOdyseeAsync(string searchQuery, int pageSize = 20);
        Task<List<YTGeneralSearchDataItem>> DuckDuckGo_PerformGeneralSearch(string searchText);
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
