using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;

namespace armaradio_ops.Models
{
    public class MBArtistParseDataItem
    {
        public string name { get; set; }
        [Newtonsoft.Json.JsonProperty("sort-name")]
        public string sortname { get; set; }
        public string country { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        [Newtonsoft.Json.JsonProperty("type-id")]
        public string typeid { get; set; }
        public List<MBArtistParseDataItem_Genres> genres { get; set; }
        public MBArtistParseDataItem_Rating rating { get; set; }
        [Newtonsoft.Json.JsonProperty("life-span")]
        public MBArtistParseDataItem_LifeSpan lifespan { get; set; }
        //public string dbsource { get; set; } 
    }
    
    public class MBArtistParseDataItem_Genres
    {
        public int? count { get; set; }
        public string disambiguation { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class MBArtistParseDataItem_Rating
    {
        public float? value { get; set; }
        [Newtonsoft.Json.JsonProperty("votes-count")]
        public int? votescount { get; set; }
    }

    public class MBArtistParseDataItem_LifeSpan
    {
        public string begin { get; set; }
        public string end { get; set; }
        public bool ended { get; set; }
    }
}
