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
        private DateTime? _runDatetime;

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

                //await _historyService.RunUpdateRoutine();
                List<QueueDataItem> queueItems = _dapper.GetList<QueueDataItem>("radioconn", "queue_get_pending_list");

                while (queueItems.Count > 0)
                {
                    await _historyService.RunQueueList(queueItems);

                    queueItems = _dapper.GetList<QueueDataItem>("radioconn", "queue_get_pending_list");
                }

                await Task.Delay((1000 * 60), stoppingToken);
            }
        }
    }
}
