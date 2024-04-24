namespace armaradio.Models
{
    public class ArmaAlbumSongDataItem
    {
        public int Id { get; set; }
        public string MBSongId { get; set; }
        public int AlbumId { get; set; }
        public int CDNumber { get; set; }
        public int SongNumber { get; set; }
        public string SongTitle { get; set; }
        public string SongTitleFlat { get; set; }
        public int ArtistId { get; set; }
        public string NameSearch { get; set; }
        public DateTime AddedDateTime { get; set; }
    }
}
