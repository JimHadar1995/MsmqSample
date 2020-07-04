using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace MsmqSample.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = GetConfig();
            var connectionString = config.GetConnectionString("DefaultConnection");
            var receiver = new MsmqMessageReceiver(connectionString);
            receiver.StartListen().Wait();
        }

        private static IConfiguration GetConfig()
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var appSettingsFileName =
                !string.IsNullOrWhiteSpace(envName)
                ? $"appsettings.{envName}.json"
                : "appsettings.json";
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appSettingsFileName)
                .Build();

            return config;
        }
    }
}
