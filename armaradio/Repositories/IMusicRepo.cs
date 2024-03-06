using armaradio.Models;
using armaradio.Models.Youtube;

namespace armaradio.Repositories
{
    public interface IMusicRepo
    {
        List<ArtistDataItem> Artist_GetArtistList(string search);
        List<TrackDataItem> Tracks_GetTop50Songs();
        string Youtube_GetUrlByArtistNameSongName(string artistName, string songName);
        List<YTGeneralSearchDataItem> Youtube_PerformGeneralSearch(string searchText);
    }
}
