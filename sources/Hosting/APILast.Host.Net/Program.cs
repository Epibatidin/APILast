using APILast.Remote;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

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
                var cmdPath = @"D:\Develop\APILast\sources\Hosting\APILast.Host.Net\cmd.json";
                using (var fs = new FileStream(cmdPath, FileMode.Open, FileAccess.Read))
                    jsonContent = new StreamReader(fs).ReadToEnd();
            }
            var configuration = JsonConvert.DeserializeObject<NetHostConfig>(jsonContent);

            var libaryLoader = new LibraryLoader(configuration.Native);

            var me = Process.GetCurrentProcess();

            var handlesInOwnProcess = libaryLoader.LoadNativeLibariesIntoProcess(me.Id);

            var remote = RemoteFunction.Locate(handlesInOwnProcess.ProcessHandle, 
                handlesInOwnProcess.AssemblyResolveHandleLibHandle, "_RegisterHandler@4");

            var invoker = RemoteFunction.Locate(handlesInOwnProcess.ProcessHandle,
                handlesInOwnProcess.CtoSharpLibHandle, "_InitAssembly@4");

            var offsetforResolve = IntPtr.Subtract(remote.FunctionPointer, handlesInOwnProcess.AssemblyResolveHandleLibHandle.ToInt32());
            var offsetforInvoker = IntPtr.Subtract(invoker.FunctionPointer, handlesInOwnProcess.CtoSharpLibHandle.ToInt32());


            List<NativeLibaryHandles> handles = new List<NativeLibaryHandles>();

            foreach (var item in configuration.Services)
            {
                var handleInRemoteProcess = libaryLoader.LoadNativeLibariesIntoProcess(item.Value);
                handles.Add(handleInRemoteProcess);
            }

            var remoteHandle = handles[0];

            var functionPosInRemoteProcess = IntPtr.Add(remoteHandle.AssemblyResolveHandleLibHandle, offsetforResolve.ToInt32());

            var moreRemote = new RemoteFunction(remoteHandle.ProcessHandle, functionPosInRemoteProcess, Encoding.ASCII);
            moreRemote.Execute(@"D:\Develop\APILast\sources\Debug\");

            var functionPosInRemoteProcess2 = IntPtr.Add(remoteHandle.CtoSharpLibHandle, offsetforInvoker.ToInt32());

            var moreRemote2 = new RemoteFunction(remoteHandle.ProcessHandle, functionPosInRemoteProcess2, Encoding.ASCII);
            moreRemote2.Execute("SampleService;APILast.Adapter.Sample");


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
