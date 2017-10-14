using APILast.Abstractions;
using APILast.Stuff;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace APILast.FasterConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder();
            Directory.GetCurrentDirectory();

            configBuilder.AddJsonFile("appSettings.json", false);
            var config = configBuilder.Build();


            var startup = new Startup(config);
            
            var serviceCollection = new ServiceCollection();

            var provider = startup.ConfigureServices(serviceCollection);

            var processResolver = provider.GetRequiredService<IProcessResolver>();
            processResolver.StartWatching();




            Console.WriteLine("Hello World!");
        }
    }
}
