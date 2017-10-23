using System;
using System.Runtime.InteropServices;
using System.Text;

namespace APILast.Remote
{
    public class LibraryLoader
    {
        public LibraryLoader(NativeConfig nativeConfig)
        {
            _nativeConfig = nativeConfig;
        }


        const int PROCESS_CREATE_THREAD = 0x0002;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_READ = 0x0010;
        

        private readonly NativeConfig _nativeConfig;

        public NativeLibaryHandles LoadLibrariesForNetCore(int processId)
        {
            return null;
        }
           

        public NativeLibaryHandles LoadNativeLibariesIntoProcess(int processId)
        {
            var nativeHandles = new NativeLibaryHandles();
            
            // geting the handle of the process - with required privileges
            nativeHandles.ProcessHandle = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, processId);
            NativeHelper.ThrowIfRequired(nativeHandles.ProcessHandle);

            // searching for the address of LoadLibraryW and storing it in a pointer
            var kernel32Handle = GetModuleHandle("kernel32.dll");
            NativeHelper.ThrowIfRequired(kernel32Handle);

            var functionExecutor = RemoteFunction.Locate(nativeHandles.ProcessHandle, kernel32Handle, nameof(LoadLibraryA));
                      
            var assemblyResolverPath = _nativeConfig.NativeLibDirectory + "\\" + _nativeConfig.AssemblyResolveHandle;
            
            nativeHandles.AssemblyResolveHandleLibHandle = new IntPtr(functionExecutor.Execute(assemblyResolverPath));
            if (nativeHandles.AssemblyResolveHandleLibHandle == IntPtr.Zero)
                throw new NullReferenceException($"Cant load Library from {assemblyResolverPath}");

            var ctosharp = _nativeConfig.NativeLibDirectory + "\\" + _nativeConfig.CtoSharpLib;
            nativeHandles.CtoSharpLibHandle = new IntPtr(functionExecutor.Execute(ctosharp));
            if (nativeHandles.CtoSharpLibHandle == IntPtr.Zero)
                throw new NullReferenceException($"Cant load Library from {ctosharp}");

            return nativeHandles;
        }

        public void CalculateFunctionOffsets()
        {
    

        }


        //[DllImport("kernel32", SetLastError = true)]
        //static extern IntPtr LoadLibraryW([MarshalAs(UnmanagedType.LPWStr)]string lpFileName);

        // just for spelling 
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        static extern IntPtr LoadLibraryA(string lpFileName);

        

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    }
}
