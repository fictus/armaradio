using arma_historycompiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arma_historycompiler.Services
{
    public interface IArmaHistoryService
    {
        Task RunUpdateRoutine();
        Task RunQueueList(List<QueueDataItem> queueItems);
    }
}
