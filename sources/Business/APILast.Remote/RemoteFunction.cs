using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace APILast.Remote
{
    public class RemoteFunction
    {
        // used for memory allocation
        const uint MEM_COMMIT = 0x00001000;
        const uint MEM_RESERVE = 0x00002000;
        const uint PAGE_READWRITE = 4;

        private IntPtr _processHandle;
        private Encoding _encoding;

        public IntPtr FunctionPointer { get; private set; }

        public static RemoteFunction Locate(IntPtr processHandle, IntPtr libaryReference, string functionName)
        {
            return Locate(processHandle, libaryReference, functionName, Encoding.ASCII);
        }

        public static RemoteFunction Locate(IntPtr processHandle, IntPtr libaryReference, string functionName,Encoding encoding)
        {
            var functionReference = GetProcAddress(libaryReference, functionName);
            NativeHelper.ThrowIfRequired(functionReference);

            var remote = new RemoteFunction(processHandle, functionReference, encoding);
            remote.Name = functionName;
            return remote;
        }


        public RemoteFunction(IntPtr processHandle, IntPtr functionPointer, Encoding encoding)
        {
            _processHandle = processHandle;
            FunctionPointer = functionPointer;
            _encoding = encoding;
        }

        public string Name { get; set; }

        public int Execute(string argument)
        {
            var bytesOfArgument = _encoding.GetBytes(argument);
            return ExecuteFunctionInProcess(_processHandle, FunctionPointer, bytesOfArgument);
        }
        
        private static int ExecuteFunctionInProcess(IntPtr processHandle, IntPtr functionPointer, byte[] bytesOfArgument)
        {  
            // alocating some memory on the target process - enough to store the name of the dll
            IntPtr argumentMemory = VirtualAllocEx(processHandle, IntPtr.Zero, (uint)bytesOfArgument.Length +1, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
            NativeHelper.ThrowIfRequired(argumentMemory);

            // writing the name of the dll there
            UIntPtr bytesWritten;
            var faulted = WriteProcessMemory(processHandle, argumentMemory, bytesOfArgument, (uint)bytesOfArgument.Length + 1, out bytesWritten);
            if(faulted == 0)
                NativeHelper.ThrowIfRequired();
            
            // creating a thread that will call LoadLibraryW with allocMemAddress as argument
            var remoteThread = CreateRemoteThread(processHandle, IntPtr.Zero, 0, functionPointer, argumentMemory, 0, IntPtr.Zero);
            NativeHelper.ThrowIfRequired(remoteThread);

            var threadWaitResult = WaitForSingleObject(remoteThread, 30000);
            if(threadWaitResult == -1)
                NativeHelper.ThrowIfRequired();

            if (threadWaitResult != 0)
                throw new Win32Exception(threadWaitResult);                
                        
            int libaryHandle = 0;
            Exception e = null;
            if (!GetExitCodeThread(remoteThread, ref libaryHandle))
                e = new Win32Exception(libaryHandle);

            var dealloc = VirtualFreeEx(processHandle, argumentMemory, 0, FreeType.Release);
            var closeHandleResult = CloseHandle(remoteThread);
            if(closeHandleResult == 0)
                NativeHelper.ThrowIfRequired();

            if (e != null)
                throw e;

            return libaryHandle;
        }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess,
            IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern Int32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, FreeType dwFreeType);

        [Flags]
        public enum FreeType
        {
            Decommit = 0x4000,
            Release = 0x8000,
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetExitCodeThread(IntPtr hThread, ref int lpExitCode);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int CloseHandle(IntPtr hThread);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    }
}
