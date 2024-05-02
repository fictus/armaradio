namespace armaradio.Models.Response
{
    public class RadioSessionRecommendedResponse
    {
        public List<RadioSessionRecommendedResponse_Track> tracks { get; set; }
    }

    public class RadioSessionRecommendedResponse_Track
    {
        public string id { get; set; }
        public string name { get; set; }
        public int? popularity { get; set; }
        public List<RadioSessionRecommendedResponse_Track_Artist> artists { get; set; }
    }

    public class RadioSessionRecommendedResponse_Track_Artist
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
