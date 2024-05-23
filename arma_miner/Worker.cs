using arma_miner.Data;
using arma_miner.Service;

namespace arma_miner
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public readonly IArmaMinerService _armaMinerService;
        private readonly IDapperHelper _dapper;

        public Worker(
            ILogger<Worker> logger,
            IArmaMinerService armaMinerService,
            IDapperHelper dapper
        )
        {
            _logger = logger;
            _armaMinerService = armaMinerService;
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

                // we only want to execute any valid process between the times included in the settings

                DateTime minTime = _dapper.GetFirstOrDefault<DateTime>("radioconn", "Operations_Sync_GetStartingTime"); //DateTime.Today.AddHours(20);
                DateTime maxTime = minTime.AddHours(1);
                DateTime currentTime = DateTime.Now;

                if (currentTime >= minTime && currentTime <= maxTime)
                {
                    await _armaMinerService.RunUpdateRoutine();
                }

                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
