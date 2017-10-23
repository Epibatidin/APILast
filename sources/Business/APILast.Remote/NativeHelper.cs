using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace APILast.Remote
{
    public static class NativeHelper
    {
        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms681383(v=vs.85).aspx

        // 203 ERROR_ENVVAR_NOT_FOUND  Das System konnte die eingegebene Umgebungsoption nicht finden
        // this just means netcore does not run the CLR C++ stuff

        // 1008 ERROR_NO_TOKEN 


        // 1150 ERROR_OLD_WIN_VERSION Für das angegebene Programm ist eine neuere Version von Windows erforderlich
        // in OpenProcess 
        
        public static void ThrowIfRequired(IntPtr pointer)
        {
            var errorCode = Marshal.GetLastWin32Error();
            if (pointer != IntPtr.Zero) return;
            if (errorCode != 0)
                throw new Win32Exception(errorCode);
        }

        public static void ThrowIfRequired(UIntPtr pointer)
        {
            var errorCode = Marshal.GetLastWin32Error();
            if (pointer != UIntPtr.Zero) return;
            if (errorCode != 0)
                throw new Win32Exception(errorCode);
        }

        //public static void ThrowIfRequired(IntPtr pointer, Func<Exception> exceptionFactory)
        //{
        //    //var errorCode = Marshal.GetLastWin32Error();
        //    if (pointer != IntPtr.Zero) return;

        //    var wrapperException = exceptionFactory();
        //    wrapperException.InnerException = ""

        //    ThrowIfRequired();
        //}

        public static void ThrowIfRequired()
        {
            var errorCode = Marshal.GetLastWin32Error();
            if (errorCode != 0)
                throw new Win32Exception(errorCode);            
        }

        public static Exception GetExecptionIfPossible()
        {
            var errorCode = Marshal.GetLastWin32Error();
            if (errorCode == 0) return null;
            return new Win32Exception(errorCode);
        }
    }
}
