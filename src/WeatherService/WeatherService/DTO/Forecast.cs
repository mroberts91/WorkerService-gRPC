using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Newtonsoft.Json.NullValueHandling;

namespace WeatherService.Dto
{
    public class Forecast
    {
        public static Forecast FromJson(string json) => 
            JsonConvert.DeserializeObject<Forecast>(json, Converter.Settings);

        //[JsonProperty("coord", NullValueHandling = Ignore)]
        //public Coord? Coord { get; set; }

        //[JsonProperty("weather", NullValueHandling = Ignore)]
        //public List<Conditions>? Weather { get; set; }
        [JsonProperty("weather/main", NullValueHandling = Ignore)]
        public string? Conditon { get; set; }

        [JsonProperty("weather/description", NullValueHandling = Ignore)]
        public string? ConditionDescription { get; set; }

        [JsonProperty("weather/icon", NullValueHandling = Ignore)]
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

        //[JsonProperty("base", NullValueHandling = Ignore)]
        //public string? Base { get; set; }

        [JsonProperty("main", NullValueHandling = Ignore)]
        public Main? Main { get; set; }

        [JsonProperty("visibility", NullValueHandling = Ignore)]
        public long? Visibility { get; set; }

        [JsonProperty("wind", NullValueHandling = Ignore)]
        public Wind? Wind { get; set; }

        [JsonProperty("rain", NullValueHandling = Ignore)]
        public Rain? Rain { get; set; }

        [JsonProperty("snow", NullValueHandling = Ignore)]
        public Snow? Snow { get; set; }

        [JsonProperty("clouds", NullValueHandling = Ignore)]
        public Clouds? Clouds { get; set; }

        //[JsonProperty("dt", NullValueHandling = Ignore)]
        //public long? LastUpdate { get; set; }

        [JsonProperty("sys", NullValueHandling = Ignore)]
        public Sys? Sys { get; set; }

        //[JsonProperty("timezone", NullValueHandling = Ignore)]
        //public long? TimezoneShift { get; set; }
    }

    public class Clouds
    {
        [JsonProperty("all", NullValueHandling = Ignore)]
        public long? All { get; set; }
    }

    public class Coord
    {
        [JsonProperty("lon", NullValueHandling = Ignore)]
        public double? Longitude { get; set; }

        [JsonProperty("lat", NullValueHandling = Ignore)]
        public double? Lattitude { get; set; }
    }

    public class Main
    {
        [JsonProperty("temp", NullValueHandling = Ignore)]
        public double? Temperature { get; set; }

        [JsonProperty("pressure", NullValueHandling = Ignore)]
        public long? Pressure { get; set; }

        [JsonProperty("humidity", NullValueHandling = Ignore)]
        public long? Humidity { get; set; }

        [JsonProperty("temp_min", NullValueHandling = Ignore)]
        public double? TempMin { get; set; }

        [JsonProperty("temp_max", NullValueHandling = Ignore)]
        public double? TempMax { get; set; }

        [JsonProperty("sea_level", NullValueHandling = Ignore)]
        public long? SeaLevelPressure { get; set; }

        [JsonProperty("grnd_level", NullValueHandling = Ignore)]
        public long? GroundLevelPressure { get; set; }

    }

    public class Rain
    {
        [JsonProperty("1h", NullValueHandling = Ignore)]
        public double? OneHourRainfall { get; set; }
        [JsonProperty("3h", NullValueHandling = Ignore)]
        public double? ThreeHourRainfall { get; set; }
    }

    public class Snow
    {

        [JsonProperty("1h", NullValueHandling = Ignore)]
        public double? OneHourSnowfall { get; set; }
        [JsonProperty("3h", NullValueHandling = Ignore)]
        public double? ThreeHourSnowfall { get; set; }
    }

    public class Sys
    {
        [JsonProperty("type", NullValueHandling = Ignore)]
        public long? Type { get; set; }

        [JsonProperty("id", NullValueHandling = Ignore)]
        public long? Id { get; set; }

        [JsonProperty("message", NullValueHandling = Ignore)]
        public double? Message { get; set; }

        [JsonProperty("country", NullValueHandling = Ignore)]
        public string? Country { get; set; }

        [JsonProperty("sunrise", NullValueHandling = Ignore)]
        public long? SunriseUTC { get; set; }

        [JsonProperty("sunset", NullValueHandling = Ignore)]
        public long? SunsetUTC { get; set; }
    }

    public class Conditions
    {
        //[JsonProperty("id", NullValueHandling = Ignore)]
        //public long? Id { get; set; }

        [JsonProperty("main", NullValueHandling = Ignore)]
        public string? Conditon { get; set; }

        [JsonProperty("description", NullValueHandling = Ignore)]
        public string? Description { get; set; }

        [JsonProperty("icon", NullValueHandling = Ignore)]
        public string? Icon 
        { 
            get
            {
                if (string.IsNullOrWhiteSpace(Icon)) return null;
                return $"http://openweathermap.org/img/wn/{Icon}@2x.png";
            }
            set
            {
                Icon = value;
            } 
        }
    }

    public class Wind
    {
        [JsonProperty("speed", NullValueHandling = Ignore)]
        public double? Speed { get; set; }

        [JsonProperty("deg", NullValueHandling = Ignore)]
        public long? Direction { get; set; }
    }

    public static class Serialize
    {
        public static string ToJson(this Forecast self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
