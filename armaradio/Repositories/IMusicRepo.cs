using armaradio.Models;

namespace armaradio.Repositories
{
    public interface IMusicRepo
    {
        List<TrackDataItem> Tracks_GetTop50Songs();
    }
}
