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
        //private readonly AppSettings _appSettings;
        private readonly string _apiKey;
        private readonly double _apiInterval;

        public WeatherWorker(IConfiguration configuration, IHttpClientFactory clientFactory, AppSettings appSettings)
        {
            _config = configuration;
            //_logger = logger;
            _clientFactory = clientFactory;
            _apiKey = appSettings.ApiKey;
            _apiInterval = appSettings.ApiInterval ?? 15; // If value not presents defualt to 15 minutes

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var client = _clientFactory.CreateClient();
                    var key = _apiKey ?? throw new Exception("Unable to obtain a vaild API Key from the App Settings.");
                    await Task.Delay(TimeSpan.FromMinutes(_apiInterval));
                }
                catch (Exception ex)
                {
                    //_logger.LogError($"Unexpected error fetching weather data: {ex.Message}");
                }
            }
        }
    }
}
