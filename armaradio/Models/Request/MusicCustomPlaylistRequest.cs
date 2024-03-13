namespace armaradio.Models.Request
{
    public class MusicCustomPlaylistRequest
    {
        public string PlayList { get; set; }
        public string PlaylistName { get; set; }
        public bool CreateNewPlaylist { get; set; }
    }
}
