using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arma_genregenerator.Models
{
    public class ArtistPlaylistsResponse
    {
        public ArtistPlaylistsResponse_Artist artists { get; set; }
        public ArtistPlaylistsResponse_Playlist playlists { get; set; }
        public ArtistPlaylistsResponse_Tracks tracks { get; set; }
    }

    public class ArtistPlaylistsResponse_Artist
    {
        public List<ArtistPlaylistsResponse_ArtistEntry> items { get; set; }
    }

    public class ArtistPlaylistsResponse_ArtistEntry
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> genres { get; set; }
        public int? popularity { get; set; }
    }

    public class ArtistPlaylistsResponse_Playlist
    {
        public List<ArtistPlaylistsResponse_PlaylistEntry> items { get; set; }
    }

    public class ArtistPlaylistsResponse_PlaylistEntry
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string uri { get; set; }
        public ArtistPlaylistsResponse_PlaylistEntry_Tracks tracks { get; set; }
    }

    public class ArtistPlaylistsResponse_PlaylistEntry_Tracks
    {
        public string href { get; set; }
        public int? total { get; set; }
    }

    public class ArtistPlaylistsResponse_Tracks
    {
        public List<ArtistPlaylistsResponse_Tracks_Item> items { get; set; }
    }

    public class ArtistPlaylistsResponse_Tracks_Item
    {
        public string id { get; set; }
        public string name { get; set; }
        public int? popularity { get; set; }
        public List<ArtistPlaylistsResponse_Tracks_Item_Artist> artists { get; set; }
    }

    public class ArtistPlaylistsResponse_Tracks_Item_Artist
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
