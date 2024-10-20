namespace armaradio.Models.Response
{
    public class ArmaAIRecommendationResponse
    {
        public List<ArmaAIRecommendationResponse_Results> results { get; set; }
    }

    public class ArmaAIRecommendationResponse_Results
    {
        public string text { get; set; }
        public string finish_reason { get; set; }
    }
}
