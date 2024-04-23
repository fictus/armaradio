using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arma_miner.Models
{
    public class MBAlbumParseDataItem
    {
        [Newtonsoft.Json.JsonProperty("artist-credit")]
        public List<MBAlbumParseDataItem_ArtistCredit> artistcredit { get; set; }
        [Newtonsoft.Json.JsonProperty("text-representation")]
        public MBAlbumParseDataItem_TextRepresentation textrepresentation { get; set; }
        public string title { get; set; }
        public string id { get; set; }
        public List<MBAlbumParseDataItem_Media> media { get; set; }
    }

    public class MBAlbumParseDataItem_ArtistCredit
    {
        public MBAlbumParseDataItem_Artist artist { get; set; }
    }

    public class MBAlbumParseDataItem_Artist
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class MBAlbumParseDataItem_TextRepresentation
    {
        public string language { get; set; }
        public string script { get; set; }
    }

    public class MBAlbumParseDataItem_Media
    {
        public string title { get; set; }
        public string format { get; set; }
        [Newtonsoft.Json.JsonProperty("track-count")]
        public int? trackcount { get; set; }
        public List<MBAlbumParseDataItem_Tracks> tracks { get; set; }
    }

    public class MBAlbumParseDataItem_Tracks
    {
        public int? length { get; set; }
        public int? position { get; set; }
        public string number { get; set; }
        public string title { get; set; }
        public string id { get; set; }
    }
}
