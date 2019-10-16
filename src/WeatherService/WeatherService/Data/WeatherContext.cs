using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherService.Data.Models;

namespace WeatherService.Data
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> options):base(options){}
        public DbSet<City>? City { get; set; }
    }
}
