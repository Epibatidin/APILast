using APILast.DomainObjects;
using System.Runtime.InteropServices;

namespace APILast.Stuff
{
    public interface INativeFunctionInvoker
    {
        //RemoteFunction FindFunction(SafeHandle processHandle, SafeHandle libaryHandle, string functionName);



        void Invoke();

    }
}