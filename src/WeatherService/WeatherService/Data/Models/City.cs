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
        public string? ZipCode { get; set; }
        public string? CityName { get; set; }
        public string? StateName { get; set; }
        public string? StateAbbr { get; set; }
        public string? CountyName { get; set; }
        public double? Longitude { get; set; }
        public double? Lattitude { get; set; }
        public Weather? Weather { get; set; }
        public Conditions? Conditions { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now;
        public bool IsUpdated { get; set; } = false;
    }

    public class Weather
    {
        public long WeatherID { get; set; }
        public double? Temperature { get; set; }
        public double? Pressure { get; set; }
        public double? Humidity { get; set; }
        public double? Visibility { get; set; }

    }

    public class Conditions
    {
        public long ConditionsID { get; set; }
        public Wind? Wind { get; set; }
        public Rain? Rain { get; set; }
        public Snow? Snow { get; set; }
        public string? Conditon { get; set; }
        public string? ConditionDescription { get; set; }
        public string? ConditionIcon
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConditionIcon)) return null;
                return $"http://openweathermap.org/img/wn/{ConditionIcon}@2x.png";
            }
            set
            {
                ConditionIcon = value;
            }
        }
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
