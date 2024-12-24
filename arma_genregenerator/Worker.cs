using arma_genregenerator.Services;

namespace arma_genregenerator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IArmaGenresService _armaGenresService;

        public Worker(
            ILogger<Worker> logger,
            IArmaGenresService armaGenresService
        )
        {
            _logger = logger;
            _armaGenresService = armaGenresService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await _armaGenresService.PopulateArtistGenres();

            //await Task.Delay(1000, stoppingToken);
            //}
        }
    }
}
