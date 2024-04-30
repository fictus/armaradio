namespace armaradioSongsAlike.Models
{
    public class SongsAlikeResponse
    {
        public List<SongsAlikeResponse_Track> tracks { get; set; }
    }

    public class SongsAlikeResponse_Track
    {
        public int id { get; set; }
        public string artist { get; set; }
        public int? duration { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public string spotifyId { get; set; }
    }
}
