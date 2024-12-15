using arma_historycompiler.Services;

namespace arma_historycompiler
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IArmaHistoryService _historyService;
        private DateTime? _runDatetime;

        public Worker(
            ILogger<Worker> logger,
            IArmaHistoryService historyService
        )
        {
            _logger = logger;
            _historyService = historyService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!_runDatetime.HasValue || _runDatetime.Value.Date != DateTime.Today)
                {
                    _runDatetime = DateTime.Now;

                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    }

                    //await _historyService.RunUpdateRoutine();
                    await _historyService.RunQueueList();
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
