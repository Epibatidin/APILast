namespace SampleService
{
    public interface IOperation<TRequest, TResponse>
    {
        TResponse Operate(TRequest request);

    }
}
