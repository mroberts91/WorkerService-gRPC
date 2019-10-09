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
        public DbSet<City>? City { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=weather.db");

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
            
        //}
        
    }

    public class ZipCodeContext: DbContext
    {

    }
}
