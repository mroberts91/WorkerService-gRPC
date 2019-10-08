using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherService.Config;

namespace WeatherService
{
    public class WeatherWorker : BackgroundService
    {
        private readonly IConfiguration _config;
        //private readonly ILogger _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly AppSettings _appSettings;

        public WeatherWorker(IConfiguration configuration, IHttpClientFactory clientFactory, AppSettings appSettings)
        {
            _config = configuration;
            //_logger = logger;
            _clientFactory = clientFactory;
            _appSettings = appSettings;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var client = _clientFactory.CreateClient();
                    var key = _appSettings.ApiKey ?? throw new Exception("Unable to obtain a vaild API Key from the App Settings.");
                    await Task.Delay(TimeSpan.FromMinutes(60));
                }
                catch (Exception ex)
                {
                    //_logger.LogError($"Unexpected error fetching weather data: {ex.Message}");
                }
            }
        }
    }
}
