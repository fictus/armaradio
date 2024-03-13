namespace armaradio.Models.Request
{
    public class MusicAddSongToPlaylistRequest
    {
        public int PlaylistId { get; set; }
        public string Artist { get; set; }
        public string Song { get; set; }
        public string VideoId { get; set; }
    }
}
