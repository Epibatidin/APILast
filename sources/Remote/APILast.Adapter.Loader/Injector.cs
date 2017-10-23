using System;
using System.Reflection;

namespace APILast.Adapter.Loader
{
    public class DoorOpener
    {
        public static void Inject(string serviceNameAndAssambylHook)
        {
            var parts = serviceNameAndAssambylHook.Split(';');

            var serviceName = parts[0];

            var assembly = Assembly.Load(new AssemblyName(parts[1]));
            
            //Console.WriteLine("Guten Tag Now Managed again: " + serviceName);
        }
    }
}
