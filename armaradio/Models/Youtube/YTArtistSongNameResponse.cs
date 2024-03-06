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
        public Thumbnail thumbnail { get; set; }
        public Title title { get; set; }
    }

    public class Thumbnail
    {
        public List<Thumbnails> thumbnails { get; set; }
    }

    public class Thumbnails
    {
        public string url { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
    }

    public class Title
    {
        public List<Runs> runs { get; set; }
    }
    
    public class Runs
    {
        public string text { get; set; }
    }
}
