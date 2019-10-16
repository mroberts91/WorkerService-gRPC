using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherService.Config
{
    public static class Utilities
    {
        #region Extensions
        /// <summary>
        /// Performs a IEnumerable<T>.Any() check but also returns false
        /// if the IEnumerable<T> is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns>True/False</returns>
        public static bool HasItems<T>(this IEnumerable<T> source)
        {
            return (source?.Any() ?? false);
        }

        /// <summary>
        /// Performs a List<T>.Any() check but also returns false
        /// if the List<T> is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns>True/False</returns>
        public static bool HasItems<T>(this List<T> source)
        {
            return (source?.Any() ?? false);
        }

        public static T GetServiceInstance<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }

        public static T GetRequiredServiceInstance<T>(this IServiceProvider provider)
        {
            return (T)provider.GetRequiredService(typeof(T));
        }
        #endregion

        #region Converters
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        public static double? KelvinToFahrenheit(double? kelvin) =>
            ((kelvin.Value - 273.15) * 9) / 5 + 32;

        public static DateTime UnixToDateTimeUTC(long unix) => epoch.AddSeconds(unix);

        public static DateTime UnixToDateTimeOffset(long unix, long shiftSeconds) => epoch.AddSeconds(unix + shiftSeconds);

        public static double HectopasacalToInchesOfMercury(long hpa) => hpa / 33.864;

        public static double HectopasacalToInchesOfMercury(double hpa) => hpa / 33.864;

        public static double MetersPerSecondToMilesPerHour(double mps) => mps * 2.237;

        public static double MillimetersToInches(double mil) => mil / 25.40;

        public static double MetersToMiles(double meter) => meter / 1609.344;
        #endregion
    }
}
