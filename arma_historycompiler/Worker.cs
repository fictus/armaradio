using arma_historycompiler.Data;
using arma_historycompiler.Models;
using arma_historycompiler.Services;

namespace arma_historycompiler
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IArmaHistoryService _historyService;
        private readonly IDapperHelper _dapper;

        public Worker(
            ILogger<Worker> logger,
            IArmaHistoryService historyService,
            IDapperHelper dapper
        )
        {
            _logger = logger;
            _historyService = historyService;
            _dapper = dapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                await _historyService.GetPendingLinks();

                QueueDataItem queueItem = _dapper.GetList<QueueDataItem>("radioconn", "queue_get_pending_list").FirstOrDefault();

                while (queueItem != null)
                {
                    await _historyService.RunQueueItem(queueItem);

                    queueItem = _dapper.GetList<QueueDataItem>("radioconn", "queue_get_pending_list").FirstOrDefault();
                }

                await Task.Delay((1000 * 60), stoppingToken);
            }
        }
    }
}
