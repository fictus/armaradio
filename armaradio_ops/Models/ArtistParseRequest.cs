using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace armaradio_ops.Models
{
    public class ArtistParseRequest
    {
        public List<ArtistRawDataItem> artists { get; set; }
        public string NextPageUrl { get; set; }
    }
}
