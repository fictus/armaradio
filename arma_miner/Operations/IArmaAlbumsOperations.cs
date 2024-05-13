using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arma_miner.Operations
{
    public interface IArmaAlbumsOperations
    {
        Task<bool> ProcessAlbumsFile(string Url, string tempFilesDir, string queueKey, bool fistTimeProcess);
    }
}
