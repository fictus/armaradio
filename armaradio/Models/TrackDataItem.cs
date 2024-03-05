namespace armaradio.Models
{
    public class TrackDataItem
    {
        public int tid { get; set; }
        public string artist_name { get; set; }
        public string track_uri { get; set; }
        public string artist_uri { get; set; }
        public string track_name { get; set; }
        public int usability_score { get; set; }
    }
}
