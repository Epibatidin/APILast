using APILast.Abstractions;
using APILast.Stuff.Configuration;
using Castle.Windsor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace APILast.Stuff
{
    public class Startup : IStartup
    {
        private IConfigurationRoot _config;

        public Startup(IConfigurationRoot config)
        {
            _config = config;
        }

        public static IWindsorContainer Windsor { get; set; }

        public void Configure(IApplicationBuilder app)
        {

        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //var processes = _config.GetSection("Processes").Get<ProcessConfig[]>();
            //services.AddSingleton<IProcessResolver>(new ProcessResolver(processes));
            //services.AddSingleton<IServiceConnectionFactory, ServiceConnectionFactory>();



            //var natives = _config.GetSection("Native").Get<NativeConfig>();

            //var libLoader = new LibraryLoader(natives);
            //var me = Process.GetCurrentProcess();


            //var nativeHandles = libLoader.LoadNativeLibariesInProcess(me.Id);



            //return services.BuildServiceProvider();
            return null;

        }
    }
}
