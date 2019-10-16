using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherService.Config;
using WeatherService.Data;
using WeatherService.Dto;
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
        private readonly int _maxApiCallsPerMinute;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public WeatherWorker(IConfiguration configuration, IHttpClientFactory clientFactory, AppSettings appSettings, IServiceScopeFactory serviceScopeFactory)
        {
            _config = configuration;
            //_logger = logger;
            _clientFactory = clientFactory;
            _apiKey = appSettings.ApiKey;
            _apiInterval = appSettings.ApiInterval ?? 15; // If value not presents defualt to 15 minutes
            _maxApiCallsPerMinute = appSettings.MaxCalls ?? 59;
            _serviceScopeFactory = serviceScopeFactory;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var _context = scope.ServiceProvider.GetRequiredServiceInstance<WeatherContext>();
                    var client = _clientFactory.CreateClient();
                    var key = _apiKey ?? throw new Exception("Unable to obtain a vaild API Key from the App Settings.");
                    for (int i = 0; i < _maxApiCallsPerMinute; i++)
                    {
                        var city = _context.City.FirstOrDefault(c => !c.IsUpdated);
                        if (city is null) break;
                        var url = $"https://api.openweathermap.org/data/2.5/weather?zip={city.ZipCode},us&appid={_apiKey}";
                        var data = await client.GetStringAsync(url);  
                        var foo = Forecast.FromJson(data);
                    }
                    await Task.Delay(TimeSpan.FromSeconds(_apiInterval));
                }
                catch (Exception ex)
                {
                    //_logger.LogError($"Unexpected error fetching weather data: {ex.Message}");
                }
            }
        }
    }
}
