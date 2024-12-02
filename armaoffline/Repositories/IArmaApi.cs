using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using armaoffline.Models;

namespace armaoffline.Repositories
{
    public interface IArmaApi
    {
        bool Signin(string Email, string Password);
        List<ArmaUserPlaylistDataItem> GetUserPlaylists();
        List<ArmaPlaylistDataItem> GetPlaylistById(int playlistId);
        void GetAudioFile(string VideoId);
    }
}
