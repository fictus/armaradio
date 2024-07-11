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
        bool Singin(string Email, string Password);
        List<ArmaUserPlaylistDataItem> GetUserPlaylists();
    }
}
