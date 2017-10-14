using System;
using System.Collections.Generic;
using System.Text;
using APILast.DomainObjects.Configuration;

namespace APILast.Stuff
{
    public class LibaryLoader
    {
        private NativeConfig natives;

        public LibaryLoader(NativeConfig natives)
        {
            this.natives = natives;
        }

        public void LoadLibariesIntoProcess(int processId)
        {



        }

    }
}
