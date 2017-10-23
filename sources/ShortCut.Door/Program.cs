using APILast.Adapter.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShortCut.Door
{
    class Program
    {
        static void Main(string[] args)
        {
            DoorOpener.Inject("Foo;APILast.Adapters.Sample");
        }
    }
}
