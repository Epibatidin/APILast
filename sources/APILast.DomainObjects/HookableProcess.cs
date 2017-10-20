using System;

namespace APILast.DomainObjects
{
    public class HookableProcess
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int ProcessId { get; set; }

        public IntPtr ProcessHandle { get; set; }

    }
}
