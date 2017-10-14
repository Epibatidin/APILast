using APILast.Abstractions;
using APILast.DomainObjects;
using APILast.Stuff.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace APILast.Stuff
{
    public class ProcessResolver : IProcessResolver
    {
        private IDictionary<string, HookableProcess> _activeProcesses;
        
        public ProcessResolver(ProcessConfig[] processes)
        {
            _activeProcesses = processes.ToDictionary(c => c.Name, c => new HookableProcess()
            {
                Name = c.Name
            });
        }

        public void StartWatching()
        {
            Process process = new Process();
            var startInfo = process.StartInfo;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/C tasklist /FO CSV /nh /m";
            process.StartInfo.UseShellExecute = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardOutput = true;
            //* Set your output and error (asynchronous) handlers
            process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            //* Start process and handlers
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
        }

        void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (outLine.Data == null) return;
            if (!outLine.Data.Contains(".dll")) return;

            foreach (var item in _activeProcesses)
            {
                if(outLine.Data.Contains(item.Key))
                {
                    var parts = outLine.Data.Split(',');

                    item.Value.IsActive = true;
                    item.Value.ProcessId = int.Parse(parts[1].Trim('"'));

                }
            }
        }        
    }
}
