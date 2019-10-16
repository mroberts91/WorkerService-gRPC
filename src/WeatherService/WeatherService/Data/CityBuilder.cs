using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using WeatherService.Data.Models;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace WeatherService.Data
{
    public class CityBuilder : IDisposable
    {
        private readonly string _csvPath;
        public CityBuilder(string csvPath)
        {
            _csvPath = csvPath;
        }

        public List<City> BuildCitiesAsync()
        {
            var cities = new List<City>();
            using var reader = File.OpenText(_csvPath);
            using var csv = new CsvReader(reader);
            csv.Configuration.Delimiter = ",";
            csv.Configuration.MissingFieldFound = null;
            while (csv.Read())
            {
                var dto = csv.GetRecord<CsvCityDto>();
                var city = new City()
                {
                    ZipCode = dto.ZipCode,
                    CityName = dto.CityName,
                    StateAbbr = dto.StateAbbr,
                    StateName = dto.StateName,
                    CountyName = dto.CountyName,
                    Longitude = dto.Longitude,
                    Lattitude = dto.Lattitude
                };
                cities.Add(city);
            }

            return cities;
        }

        #region IDisposable Support
        private bool disposedValue = false;
        private readonly SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    handle.Dispose();
                }
                disposedValue = true;
            }
            else
            {
                return;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    internal class CsvCityDto
    {
        public string? ZipCode { get; set; }
        public string? CityName { get; set; }
        public string? StateName { get; set; }
        public string? StateAbbr { get; set; }
        public string? CountyName { get; set; }
        public double? Longitude { get; set; }
        public double? Lattitude { get; set; }
    }
}
