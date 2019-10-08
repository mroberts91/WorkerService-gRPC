using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherService
{
    public static class ValueConverter
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        public static double? KelvinToFahrenheit(double? kelvin) => 
            ((kelvin.Value - 273.15) * 9) / 5 + 32;

        public static DateTime UnixToDateTimeUTC(long unix) => epoch.AddSeconds(unix);

        public static DateTime UnixToDateTimeOffset(long unix, long shiftSeconds) => epoch.AddSeconds(unix + shiftSeconds);

        public static double HectopasacalToInchesOfMercury(long hpa) => hpa/33.864;

        public static double MetersPerSecondToMilesPerHour(double mps) => mps*2.237;

        public static double MillimetersToInches(double mil) => mil/25.40;
    }
}
