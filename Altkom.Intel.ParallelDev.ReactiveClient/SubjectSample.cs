using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Altkom.Intel.ParallelDev.ReactiveClient
{
    public class SubjectSample
    {
        // Gorące źródło danych
        public static void SubjectTest()
        {
            var source = new Subject<string>();

            source.OnNext("Hello");
            source.OnNext("World");

            source
                .Subscribe(
                    item => Console.WriteLine($"Observer #1 Received {item}"),
                    e => Console.WriteLine($"Observer #1 Error {e.Message}"),
                    () => Console.WriteLine("Observer #1 Completed")
                    );

            source.Subscribe(item => Console.WriteLine($"Observer #2 Received {item}"));

            source.OnNext("Hello Poland");

            source.OnCompleted();
        }


        // Zimne źródło 
        public static void ReplaySubjectTest()
        {

            // jeden watek
            // var source = new ReplaySubject<string>();

            // wiele watkow
            var source = new ReplaySubject<string>(NewThreadScheduler.Default);

            source.OnNext("Hello");
            source.OnNext("World");

            var sub = source
                .Subscribe(
                    item => Console.WriteLine($"Received {item}"),
                       e => Console.WriteLine($"Error {e.Message}"),
                      () => Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Completed")
                    );

            source.Subscribe(item => Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Observer #2 Received {item}"));
            source.Subscribe(item => Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Observer #3 Received {item}"));
            source.Subscribe(item => Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Observer #4 Received {item}"));


            source.OnNext("Hello Poland");

            source.OnCompleted();
        }
    }
}
