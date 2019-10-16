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
using WeatherService.Data.Models;

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
            _apiInterval = appSettings.ApiInterval ?? 70; // If value not presents defualt to 15 minutes
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
                    using var transaction = _context.Database.BeginTransaction();
                    for (int i = 0; i < _maxApiCallsPerMinute; i++)
                    {
                        var city = _context.City.FirstOrDefault(c => !c.IsUpdated);
                        if (city is null)
                        { 
                            _context.City
                                    .Where(c => c.IsUpdated)
                                    .ToList()
                                    .ForEach(c => c.IsUpdated = false);
                            _context.SaveChanges();
                            break;
                        }
                        var url = $"https://api.openweathermap.org/data/2.5/weather?zip={city.ZipCode},us&appid={_apiKey}";
                        var data = await client.GetStringAsync(url);  
                        city = MapResponseToCity(city, Forecast.FromJson(data));
                        _context.SaveChanges();
                    }
                    transaction.Commit();
                    await Task.Delay(TimeSpan.FromSeconds(_apiInterval));
                }
                catch (Exception ex)
                {
                    //_logger.LogError($"Unexpected error fetching weather data: {ex.Message}");
                }
            }
        }

        private City MapResponseToCity(City city, Forecast forecast)
        {
            var oneHourSnowfall = forecast?.Snow?.OneHourSnowfall;
            var threeHourSnowfall = forecast?.Snow?.ThreeHourSnowfall;
            var oneHourRainfall = forecast?.Rain?.OneHourRainfall;
            var threeHourRainfall = forecast?.Rain?.ThreeHourRainfall;
            var windDirection = forecast?.Wind?.Direction;
            var windSpeed = forecast?.Wind?.Speed;
            var humidity = forecast?.Main?.Humidity;
            var pressure = forecast?.Main?.Pressure;
            var temperature = forecast?.Main?.Temperature;
            var visibility = forecast?.Visibility;

            if (oneHourSnowfall.HasValue)
                oneHourSnowfall = Utilities.MillimetersToInches(oneHourSnowfall.Value);
            if (threeHourSnowfall.HasValue)
                threeHourSnowfall = Utilities.MillimetersToInches(threeHourSnowfall.Value);
            if (oneHourRainfall.HasValue)
                oneHourRainfall = Utilities.MillimetersToInches(oneHourRainfall.Value);
            if (threeHourRainfall.HasValue)
                threeHourRainfall = Utilities.MillimetersToInches(threeHourRainfall.Value);
            if (windSpeed.HasValue)
                windSpeed = Utilities.MetersPerSecondToMilesPerHour(windSpeed.Value);
            if (pressure.HasValue)
                pressure = Utilities.HectopasacalToInchesOfMercury(pressure.Value);
            if (temperature.HasValue)
                temperature = Utilities.KelvinToFahrenheit(temperature);
            if (visibility.HasValue)
                visibility = Utilities.MetersToMiles(visibility.Value);

            return new City()
            {
                ZipCode = city.ZipCode,
                CityName = city.CityName,
                StateName = city.StateName,
                StateAbbr = city.StateAbbr,
                CountyName = city.CountyName,
                Lattitude = city.Lattitude,
                Longitude = city.Longitude,
                IsUpdated = true,
                Updated = DateTime.Now,
                Conditions = new Data.Models.Conditions()
                {
                    //Conditon = forecast?.Conditon,
                    //ConditionDescription = forecast?.ConditionDescription,
                    //ConditionIcon = forecast?.ConditionIcon,
                    Rain = new Data.Models.Rain()
                    {
                        OneHourRainfall = oneHourRainfall,
                        ThreeHourRainfall = threeHourRainfall
                    },
                    Snow = new Data.Models.Snow()
                    {
                        OneHourSnowfall = oneHourSnowfall,
                        ThreeHourSnowfall = threeHourSnowfall
                        
                    },
                    Wind = new Data.Models.Wind()
                    {
                        Direction = windDirection,
                        Speed = windSpeed
                    }
                },
                Weather = new Data.Models.Weather()
                {
                    Humidity = humidity,
                    Pressure = pressure,
                    Temperature = temperature,
                    Visibility = visibility
                }
            };
        }
    }
}
