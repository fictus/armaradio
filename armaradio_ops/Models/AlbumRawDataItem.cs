using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace armaradio_ops.Models
{
    public class AlbumRawDataItem
    {
        public int ArtistId { get; set; }
        public string AlbumName { get; set; }
        public string AlbumDetails{ get; set; }
        public string ReleaseDate { get; set; }
        public string Label { get; set; }
        public string DBDesignator { get; set; }
        public bool IsSingle { get; set; }
    }
}
