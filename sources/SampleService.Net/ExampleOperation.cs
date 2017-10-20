using System;

namespace SampleService
{
    public class ExampleOperation : IOperation<string, string>
    {
        public ExampleOperation()
        {
        }

        public string Operate(string request)
        {
            Console.WriteLine($"I {nameof(ExampleOperation)} recieved your request {request}");

            return "Juten Tach";

        }
    }
}
