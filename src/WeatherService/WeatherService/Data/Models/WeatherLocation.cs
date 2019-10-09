using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WeatherService.Data.Models
{
    public class City
    {
        [Key]
        public int ZipCode { get; set; }
        public int CityId { get; set; }
        public string? CityName { get; set; }
        public double? Longitude { get; set; }
        public double? Lattitude { get; set; }
        public double? Visibility { get; set; }
        public Weather? Weather { get; set; }
        public Conditions? Conditions { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now;
    }

    public class Weather
    {
        public long WeatherID { get; set; }
        public double? Temperature { get; set; }
        public double? Pressure { get; set; }
        public double? Humidity { get; set; }
    }

    public class Conditions
    {
        public long ConditionsID { get; set; }
        public Wind? Wind { get; set; }
        public Rain? Rain { get; set; }
        public Snow? Snow { get; set; }
    }

    public class Rain
    {
        public long RainID { get; set; }
        public double? OneHourRainfall { get; set; }
        public double? ThreeHourRainfall { get; set; }
    }

    public class Wind
    {
        public long WindID { get; set; }
        public double? Speed { get; set; }
        public int? Direction { get; set; }
    }

    public class Snow
    {
        public long SnowID { get; set; }
        public double? OneHourSnowfall { get; set; }
        public double? ThreeHourSnowfall { get; set; }
    }

}
