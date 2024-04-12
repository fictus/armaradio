using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace armaradio_ops.Models
{
    public class AlbumArtistDataItem
    {
        public int Id { get; set; }
        public string ArtistName { get; set; }
        public string ArtistName_Flat { get; set; }
        public string AlbumsUrl { get; set; }
        public string DBDesignator { get; set; }
        public DateTime EntryDateTime { get; set; }
    }
}
