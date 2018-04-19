using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Altkom.Intel.ParallelDev.ReactiveClient
{
    class TimeObservableSample
    {


        public static void FileSystemWatcherTest()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var fileSystemWatcher = new FileSystemWatcher(path);
            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.EnableRaisingEvents = true;

            var created = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                h => fileSystemWatcher.Created += h,
                h => fileSystemWatcher.Created -= h)
                .Select(x => x.EventArgs);

            var changed = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
              h => fileSystemWatcher.Changed += h,
              h => fileSystemWatcher.Changed -= h)
              .Select(x => x.EventArgs);

            var deleted = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
              h => fileSystemWatcher.Deleted += h,
              h => fileSystemWatcher.Deleted -= h)
              .Select(x => x.EventArgs);

            created.Subscribe(file => Console.WriteLine($"Create {file.Name}"));

            deleted.Subscribe(file =>
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Delete {file.Name}");
                Console.ResetColor();
            });


            var all = created.Merge(deleted)
                .Throttle(TimeSpan.FromSeconds(3));

            all.Subscribe(file => {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"ALL {file.Name}");
                Console.ResetColor();
            });




            // var all2 = Observable.Merge(created, changed, deleted);

          
            //all2.Throttle(.Subscribe(file => {
            //    Console.BackgroundColor = ConsoleColor.Blue;
            //    Console.ForegroundColor = ConsoleColor.White;
            //    Console.WriteLine($"ALL {file.Name}");
            //    Console.ResetColor();
            //});

        }

        public static void ColdSubject()
        {
            {
                var random = new Random();

                var tickSource = Observable
                    .Interval(TimeSpan.FromSeconds(5))
                    .Select(x => random.Next(1, 100));

                var publisher = new ReplaySubject<int>(NewThreadScheduler.Default);
                tickSource.Subscribe(publisher);

                publisher.Subscribe(next => Console.WriteLine($"[1] received {next} (thread:{Thread.CurrentThread.ManagedThreadId})"));
                publisher.Subscribe(next => { Thread.Sleep(2000); Console.WriteLine($"[2 slow] received {next} (thread:{Thread.CurrentThread.ManagedThreadId})"); });
                publisher.Subscribe(next => Console.WriteLine($"[3] received {next} (thread:{Thread.CurrentThread.ManagedThreadId})"));
                publisher.Subscribe(next => { Thread.Sleep(7000); Console.WriteLine($"[4 slowest] received {next} (thread:{Thread.CurrentThread.ManagedThreadId})"); });
                publisher.Subscribe(next => Console.WriteLine($"[5] received {next} (thread:{Thread.CurrentThread.ManagedThreadId})"));
                publisher.Subscribe(next => Console.WriteLine());
            }
        }

        public static void BufferTest()
        {
            var random = new Random();

            var source = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(p => random.Next(1, 100));

            // var buffered = publisher.Buffer(3);

      //      var buffered = publisher.Buffer(3, 1);


            var windowed = source.Window(3);

            source.Subscribe(item => Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Observer #1 Received {item}"));

            // buffered.Subscribe(list => Console.WriteLine(string.Join(" - ", list)));

            windowed.Subscribe(list => list.Subscribe(n => Console.WriteLine(n)));

            /// windowed.Subscribe(window => Console.WriteLine(window));

            //  filtered.Subscribe(item => Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Observer #1 Received {item}"));
        }
    }
}
