using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altkom.Intel.ParallelDev.ReactiveClient
{
    public class MyObserver : IObserver<string>
    {
        public void OnCompleted()
        {
            Console.WriteLine("Finished.");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine($"Error: {error.Message}");
        }

        public void OnNext(string value)
        {
            Console.WriteLine($"Received {value}");
        }
    }
}
