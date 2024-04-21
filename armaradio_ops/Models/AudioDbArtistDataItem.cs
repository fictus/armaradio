using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace armaradio_ops.Models
{
    public class AudioDbArtistDataItem
    {
        public int idArtist { get; set; }
        public string strArtist { get; set; }
        public string strArtistStripped { get; set; }
        public string strArtistAlternate { get; set; }
        public string strLabel { get; set; }
        public int? idLabel { get; set; }
        public int? intFormedYear { get; set; }
        public int? intDiedYear { get; set; }
        public string strStyle { get; set; }
        public string strGenre { get; set; }
        public string strMood { get; set; }
        public string strGender { get; set; }
        public string strCountry { get; set; }
        public string strCountryCode { get; set; }
        public string strArtistThumb { get; set; }
        public string strArtistLogo { get; set; }
        public string strArtistCutout { get; set; }
        public string strArtistClearart { get; set; }
        public string strArtistWideThumb { get; set; }
        public string strArtistFanart { get; set; }
        public string strArtistFanart2 { get; set; }
        public string strArtistFanart3 { get; set; }
        public string strArtistFanart4 { get; set; }
        public string strArtistBanner { get; set; }
        public string strMusicBrainzID { get; set; }
        public int? intCharted { get; set; }
    }
}
