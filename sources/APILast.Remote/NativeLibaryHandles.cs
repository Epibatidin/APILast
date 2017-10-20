using System;

namespace APILast.Remote
{
    public class NativeLibaryHandles
    {
        public IntPtr ProcessHandle { get; set; }
        public IntPtr CtoSharpLibHandle { get; set; }
        public IntPtr AssemblyResolveHandleLibHandle { get; set; }
    }

    public class NonChangingHandlesInOwnProcess
    {

    }
}
