namespace APILast.Abstractions
{
    public interface ICommunicationManager
    {
        object PushMessage(string serviceName, string operationId, object value);
    }
}
