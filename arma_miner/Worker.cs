using arma_miner.Service;

namespace arma_miner
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public readonly IArmaMinerService _armaMinerService;

        public Worker(
            ILogger<Worker> logger,
            IArmaMinerService armaMinerService
        )
        {
            _logger = logger;
            _armaMinerService = armaMinerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                //// we only want to execute any valid process between 8:00-9:00 PM
                //DateTime minTime = DateTime.Today.AddHours(20);
                //DateTime maxTime = DateTime.Today.AddHours(21);
                //DateTime currentTime = DateTime.Now;

                //if (currentTime >= minTime && currentTime <= maxTime)
                //{
                    _armaMinerService.RunUpdateRoutine();
                //}

                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
