using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherService
{
    public class WeatherWorker : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public WeatherWorker(IConfiguration configuration, ILogger logger)
        {
            _config = configuration;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {

                    await Task.Delay(TimeSpan.FromMinutes(60));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unexpected error fetching weather data: {ex.Message}");
                }
            }
        }
    }
}
