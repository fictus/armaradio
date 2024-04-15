using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace armaradio_ops.Models
{
    public class ArmaArtistAlbumsResponse
    {
        public List<ArmaAlbumDataItem> Albums { get; set; }
        public List<ArmaAlbumDataItem> Singles { get; set; }
    }
}
