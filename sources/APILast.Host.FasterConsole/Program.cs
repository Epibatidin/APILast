using APILast.Abstractions;
using APILast.Remote;
using APILast.Stuff;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;

namespace APILast.FasterConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var me = Process.GetCurrentProcess();

            Console.WriteLine("I am Process " + me.Id);
            
            var configBuilder = new ConfigurationBuilder();
            Directory.GetCurrentDirectory();

            configBuilder.AddJsonFile("appSettings.json", false);



            var config = configBuilder.Build();
            
            var startup = new Startup(config); 
            var natives = config.GetSection("Native").Get<NativeConfig>();
            var libLoader = new LibraryLoader(natives);

            var libHandles = libLoader.LoadNativeLibariesInOwnProcess(me.Id);

            

        }


        //private void blubber()
        //{
        //    var serviceCollection = new ServiceCollection();


        //    var provider = startup.ConfigureServices(serviceCollection);

        //    var serviceConnectionFactory = provider.GetRequiredService<IServiceConnectionFactory>();
        //    var response = serviceConnectionFactory.CreateConnection("SampleService"); //.PushMessage("SampleService", "ExampleOperation", "Greetings");

        //    Console.WriteLine("Hello World!");
        //}

        private static void PrintEnvironmentVariables()
        {
            //Environment.SetEnvironmentVariable("PROCESSOR_ARCHITEW6432", "AMD64", EnvironmentVariableTarget.Machine);
            //Environment.SetEnvironmentVariable("_NO_DEBUG_HEAP", "1", EnvironmentVariableTarget.Machine);
            //var writer = File.OpenWrite(@"D:\Develop\APILastEnv.txt");
            //var streamwriter = new StreamWriter(writer);            
            ////foreach (DictionaryEntry item in Environment.GetEnvironmentVariables().OfType<DictionaryEntry>().OrderBy(c => c.Key))
            ////{
            ////    streamwriter.WriteLine($"{item.Key} => {item.Value}");
            ////}
            //streamwriter.Flush();
            //writer.Flush();
            //streamwriter.Close();
            //writer.Close();
        }
    }
}
