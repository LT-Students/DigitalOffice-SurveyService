using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json")
              .Build();

            string seqServerUrl = Environment.GetEnvironmentVariable("seqServerUrl");
            if (string.IsNullOrEmpty(seqServerUrl))
            {
              seqServerUrl = configuration["Serilog:WriteTo:1:Args:serverUrl"];
            }

            string seqApiKey = Environment.GetEnvironmentVariable("seqApiKey");
            if (string.IsNullOrEmpty(seqApiKey))
            {
              seqApiKey = configuration["Serilog:WriteTo:1:Args:apiKey"];
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
