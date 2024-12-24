using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arma_genregenerator.Services
{
    public interface IArmaGenresService
    {
        Task PopulateArtistGenres();
    }
}
