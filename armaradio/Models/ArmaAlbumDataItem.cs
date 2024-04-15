namespace armaradio.Models
{
    public class ArmaAlbumDataItem
    {
        public int Id { get; set; }
        public int ArtistId { get; set; }
        public string AlbumName { get; set; }
        public string AlbumName_Flat { get; set; }
        public string AlbumDetails { get; set; }
        public string ReleaseDate { get; set; }
        public string Label { get; set; }
        public bool IsSingle { get; set; }
        public DateTime AddedDateTime { get; set; }
    }
}
