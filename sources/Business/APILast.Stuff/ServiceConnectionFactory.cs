using APILast.Abstractions;
using APILast.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace APILast.Stuff
{
    public class ServiceConnectionFactory : IServiceConnectionFactory
    {
        private Dictionary<string, ServiceConnection> _connectionCache;

        public ServiceConnectionFactory()
        {
            _connectionCache = new Dictionary<string, ServiceConnection>();
        }



        public ServiceConnection CreateConnection(string serviceName)
        {
            ServiceConnection connection;

            //if (_connectionCache.TryGetValue(serviceName, out connection))
              //  return connection;

            connection = new ServiceConnection();
            
            

            // 0. 


            // 1. make sure our libaries are hook up 

            // 2. hook up our libaries 

            // 3. 



            return connection;
        }

    }
}
