namespace armaradio.Models.Youtube
{
    public class YTArtistSongNameResponse
    {
        public ResponseContents contents { get; set; }
    }

    public class ResponseContents
    {
        public TwoColumnSearchResultsRenderer twoColumnSearchResultsRenderer { get; set; }
    }

    public class TwoColumnSearchResultsRenderer
    {
        public PrimaryContents primaryContents { get; set; }
    }

    public class PrimaryContents
    {
        public SectionListRenderer sectionListRenderer { get; set; }
    }

    public class SectionListRenderer
    {
        public List<SectionContents> contents { get; set; }
    }

    public class SectionContents
    {
        public ItemSectionRenderer itemSectionRenderer { get; set; }
    }

    public class ItemSectionRenderer
    {
        public List<ItemSectionContents> contents { get; set; }
    }

    public class ItemSectionContents
    {
        public VideoRenderer videoRenderer { get; set; }
    }

    public class VideoRenderer
    {
        public string videoId { get; set; }
    }
}
