namespace armaradio.Models
{
    public class ArmaSharedPlaylistDataItem
    {
        public int PlaylistId { get; set; }
        public string PlaylistName { get; set; }
        public List<ArmaPlaylistDataItem> PlaylistData { get; set; }
    }
}
