namespace armaradio.Models.BeatPort
{
    public class TranceTopSearchDataItem
    {
        public TranceTopProps props { get; set; }
    }

    public class TranceTopProps
    {
        public TranceTopPageProps pageProps { get; set; }
    }

    public class TranceTopPageProps
    {
        public TranceTopDehydratedState dehydratedState { get; set; }
    }

    public class TranceTopDehydratedState
    {
        public List<TranceTopQueries> queries { get; set; }
    }

    public class TranceTopQueries
    {
        public TranceTopState state { get; set; }
    }

    public class TranceTopState
    {
        public TranceTopData data { get; set; }
    }

    public class TranceTopData
    {
        public List<TranceTopResults> results { get; set; }
    }

    public class TranceTopResults
    {
        public List<TranceTopResultsArtists> artists { get; set; }
        public string name { get; set; }
        public string mix_name { get; set; }
    }

    public class TranceTopResultsArtists
    {
        public string name { get; set; }
    }
}
