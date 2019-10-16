using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherService.Config;
using WeatherService.Data;

namespace WeatherService
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = new AppSettings();
            _config.GetSection("AppSettings").Bind(appSettings);

            services.AddGrpc();
            //services.AddDbContext<WeatherContext>();
            services.AddDbContextPool<WeatherContext>(optionsBuilder => optionsBuilder.UseSqlite("Data Source=weather.db"));
            services.AddSingleton(appSettings);
            services.AddHostedService<WeatherWorker>();
            services.AddHttpClient();
            services.AddMemoryCache();
            services.AddLogging();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WeatherContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });

            
            try
            {
                if (context.City != null && !context.City.HasItems())
                {
                    var cityCsv = Path.GetFullPath(@".\Data\us_postal_codes.csv");
                    if (!File.Exists(cityCsv)) throw new Exception("Unable to build cities database, because the source data file was not found.");
                    using var cityBuilder = new CityBuilder(cityCsv);
                    var cities = cityBuilder.BuildCitiesAsync();
                    using var trans = context.Database.BeginTransaction();
                    context.City.AddRange(cities);
                    context.SaveChanges();
                    trans.CommitAsync();
                }
                if (context.City is null)
                {
                    throw new Exception("Unable to build cities database, because the City collection is null.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
