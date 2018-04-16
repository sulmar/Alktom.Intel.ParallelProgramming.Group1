using Alktom.Intel.ParallelDev.TaskConsoleClient.DownloadDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alktom.Intel.ParallelDev.TaskConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started");

            CancellationTokenSource cts = new CancellationTokenSource();

            CancellationToken token = cts.Token;

            Task task = DoWorkAsync(token);

            Console.WriteLine("Press key to cancel.");

            Console.ReadKey();

            cts.Cancel();

            try
            {
                task.Wait();

                Console.WriteLine($"IsCanceled: {task.IsCompleted}");

            }
            catch(AggregateException e)
            {
                Console.WriteLine($"IsCanceled: {task.IsCanceled}");
            }

            

            //var task1 = TaskDownload.DownloadStringAsync("http://www.altkom.pl");

            //var task2 = TaskDownload.DownloadStringAsync("http://www.intel.com");

            //Console.WriteLine(task1.Result.Length);

            // Task.WaitAll(task1, task2);

            // Task.Run(()=>Console.WriteLine(task1.Result.Length + task2.Result.Length));


            // DownloadTest();

            // CheckedTest();

            // var task = Task.Run(() => ExceptionTest());

            Console.WriteLine("Press any  key to exit.");

            Console.ReadKey();

        }


        private static Task DoWorkAsync(CancellationToken token)
        {
            return Task.Run(() => DoWork(token));
        }

        private static void DoWork(CancellationToken token)
        {
           while(true)
            {
                
                 token.ThrowIfCancellationRequested();
                


                Console.Write(".");
                Thread.Sleep(TimeSpan.FromSeconds(0.1));
            }
        }

        private static async void DownloadTest()
        {
            var content = await TaskDownload.DownloadStringAsync("http://www.altkom.pl");

            var content2 = await TaskDownload.DownloadStringAsync("http://www.intel.com");

            Console.WriteLine(content);
        }

        private static void CheckedTest()
        {
            checked
            {
                for (byte i = 0; i < 300; i++)
                {
                    Console.WriteLine(i);

                    Thread.Sleep(50);
                }
            }
        }

        private static async Task ExceptionTest()
        {
            try
            {
                // await Task.Run(() => Download("http://www.altkom.pl"));

                Task t1 = DownloadAsync("http://www.altkom.pl");

                Task t2 = DownloadAsync("http://www.altkom.pl");

                Task t3 = DownloadAsync("http://www.altkom.pl");


                var tasks = new Task<string>[]
                {
                    DownloadStringAsync("http://www.altkom.pl"),
                    DownloadStringAsync("http://www.altkom.pl"),
                    DownloadStringAsync("http://www.altkom.pl")
                };

                Task.WaitAll(tasks);

                // 
                Task.WaitAll(t1, t2, t3);

                var contents = Task.WhenAll(tasks);

                await Task.WhenAll(t1, t2, t3);

                await AsyncAwaitTest();                
            }
            catch (ApplicationException e)
            {
                Console.WriteLine($"Błąd: {e.Message}");
            }
        }


        private static Task DownloadAsync(string uri)
        {
            return Task.Run(() => Download(uri));
        }

        private static Task<string> DownloadStringAsync(string uri)
        {
            return Task.Run<string>(() => DownloadString(uri));
        }

        private static string DownloadString(string uri)
        {
            Console.WriteLine("downloading...");

            Thread.Sleep(TimeSpan.FromSeconds(2));

            Console.WriteLine("downloaded.");

            return "<html><b>Hello World</b></html>";
        }

        private static void Download(string uri)
        {
            Console.WriteLine("downloading...");

            Thread.Sleep(TimeSpan.FromSeconds(2));

            Console.WriteLine("downloaded.");
        }

        private static async Task AsyncAwaitTest()
        {
            int length = await Task.Run<int>(() => GetLength("Hello World!"));

            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Wynik: {length}");
        }

        static int GetLength(string message)
        {
            Console.WriteLine($"GetLength... {message} Thread: {Thread.CurrentThread.ManagedThreadId}");

            Thread.Sleep(TimeSpan.FromSeconds(1));

            throw new ApplicationException("Wystąpił wyjątek");

            return message.Length;
        }
    }
}
