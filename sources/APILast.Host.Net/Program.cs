using APILast.Remote;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APILast.NetHost
{
    public class Program
    {
        public class NetHostConfig
        {
            public Dictionary<string, int> Services { get; set; }

            public NativeConfig Native { get; set; } 
        }


        public static void Main(string[] args)
        {
            string jsonContent = "";
            if (args.Length == 1)
            {
                jsonContent = args[0];
            }
            else
            {
                var cmdPath = @"D:\Develop\APILast\sources\APILast.Host.Net\cmd.json";
                using (var fs = new FileStream(cmdPath, FileMode.Open, FileAccess.Read))
                    jsonContent = new StreamReader(fs).ReadToEnd();
            }
            var configuration = JsonConvert.DeserializeObject<NetHostConfig>(jsonContent);

            var libaryLoader = new LibraryLoader(configuration.Native);

            var me = Process.GetCurrentProcess();

            var handlesInOwnProcess = libaryLoader.LoadNativeLibariesIntoProcess(me.Id);

            var remote = RemoteFunction.Locate(handlesInOwnProcess.ProcessHandle, 
                handlesInOwnProcess.AssemblyResolveHandleLibHandle, "_RegisterHandler@0");


            List<NativeLibaryHandles> handles = new List<NativeLibaryHandles>();

            foreach (var item in configuration.Services)
            {
                var handleInRemoteProcess = libaryLoader.LoadNativeLibariesIntoProcess(item.Value);
                handles.Add(handleInRemoteProcess);
            }

            libaryLoader.CalculateFunctionOffsets();

            // what next ?             
            // find methods 
            // calculate offsets 
            // free libaries in own process
            
            // foobazen



            var r = configuration;

        }
    }
}
