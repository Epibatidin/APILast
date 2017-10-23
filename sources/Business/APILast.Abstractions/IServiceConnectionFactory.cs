using APILast.DomainObjects;

namespace APILast.Abstractions
{
    public interface IServiceConnectionFactory
    {
        ServiceConnection CreateConnection(string serviceName);
    }
}