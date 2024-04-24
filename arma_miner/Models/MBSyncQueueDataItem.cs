using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arma_miner.Models
{
    public class MBSyncQueueDataItem
    {
        public bool HasBeenProcessed { get; set; }
        public bool FirstTimeProcess { get; set; }
    }
}
