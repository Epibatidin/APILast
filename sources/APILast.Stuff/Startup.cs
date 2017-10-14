using APILast.Abstractions;
using APILast.DomainObjects.Configuration;
using APILast.Stuff.Configuration;
using Castle.Windsor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
            var processes = _config.GetSection("Processes").Get<ProcessConfig[]>();
            services.AddSingleton<IProcessResolver>(new ProcessResolver(processes));

            var natives = _config.GetSection("Natives").Get<NativeConfig>();

            var libLoader = new LibaryLoader(natives);
            libLoader.LoadLibariesIntoProcess(Process.GetCurrentProcess().Id);



            return services.BuildServiceProvider();

        }
    }
}
