using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Diagnostics;

namespace SampleService
{
    public class Program
    {
        public static IWindsorContainer Container { get; set; }



        static void Main(string[] args)
        {
            Process me = Process.GetCurrentProcess();
            Console.WriteLine($"Hello my Name is {me.ProcessName} i am unique with my ID {me.Id}");

            Container = new WindsorContainer();

            Container.Register(Component.For<IOperation<string, string>>().ImplementedBy<ExampleOperation>());


            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;

            Console.WriteLine("Hit enter for Process Shutdown");
            Console.ReadLine();
            Console.WriteLine("enter recieved shutting down");
        }
        
        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            Console.WriteLine("CurrentDomain_AssemblyLoad: " + args.LoadedAssembly.Location);
        }


    }
}