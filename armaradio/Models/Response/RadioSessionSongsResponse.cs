namespace armaradio.Models.Response
{
    public class RadioSessionSongsResponse
    {
        public int? total { get; set; }
        public List<RadioSessionSongsResponse_Items> items { get; set; }
    }

    public class RadioSessionSongsResponse_Items
    {
        public RadioSessionSongsResponse_ItemTrack track { get; set; }
    }

    public class RadioSessionSongsResponse_ItemTrack
    {
        public string id { get; set; }
        public string name { get; set; }
        public int? popularity { get; set; }
        public List<RadioSessionSongsResponse_ItemArtist> artists { get; set; }
    }

    public class RadioSessionSongsResponse_ItemArtist
    {
        public string id { get; set; }
        public string name { get; set; }
        public int? popularity { get; set; }
    }
}
