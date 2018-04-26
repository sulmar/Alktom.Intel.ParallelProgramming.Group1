using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alktom.Intel.ParallelDev.ConsoleClient
{
    class TasksSample
    {
        public static void Test()
        {
            int x = 10;
            // Ręczne tworzenie taska i uruchomienie
            Task task1 = new Task(() => DoSecondWork(x));
            task1.Start();

            // Fabryka zadań
            Task task3 = Task.Factory.StartNew(DoWork);

            // Uproszczony zapis fabryki
            Task task4 = Task.Run(() => DoSecondWork(x));

            Task task2 = new Task(() => DoWork());

            var task5 = Task.Run<int>(() => GetLength("Hello World!"));

            // blokujemy UI
            //var wynik = task5.Result;

            //task5
            //    .ContinueWith(t => Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Wynik: {t.Result}"), TaskContinuationOptions.AttachedToParent)
            //        .ContinueWith(t => Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} thx"), TaskContinuationOptions.AttachedToParent);




            // synchroniczny

            //var length = GetLength("Hello");

            //Console.WriteLine(length);


            // task1.Wait();

            //             task5.Wait();

            //  task2.Start();

            // czekamy na zakonczenie wszystkich
            //  Task.WaitAll(task1, task2);

            // wyscig
            //            Task.WaitAny(task1, task2);


            //for (int i = 0; i < 100; i++)
            //{
            //    Task task = new Task(DoWork);
            //    task.Start();
            //}


            Task.Run(() => ExceptionTest());

        }

        static void DoWork()
        {
            Console.WriteLine($"working... {Thread.CurrentThread.ManagedThreadId}");

            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Główny wątek aplikacji
            Thread mainThread = Thread.CurrentThread;

            //   Console.WriteLine($"Thread {mainThread.ManagedThreadId}");

            if (mainThread.ManagedThreadId == 1)
            {
                throw new ApplicationException("My exception");
            }



        }

        static int GetLength(string message)
        {
            Console.WriteLine($"GetLength... {message} Thread: {Thread.CurrentThread.ManagedThreadId}");

            Thread.Sleep(TimeSpan.FromSeconds(1));

            throw new ApplicationException("Test");

            return message.Length;
        }

        static void DoSecondWork(object obj)
        {
            Console.WriteLine($"working...{obj} Thread: {Thread.CurrentThread.ManagedThreadId}");

        }

        private static void ContinueWithTest()
        {
            var task5 = Task.Run<int>(() => GetLength("Hello World!"));

            task5
                .ContinueWith(t => Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Wynik: {t.Result}"), TaskContinuationOptions.AttachedToParent)
                    .ContinueWith(t => Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} thx"), TaskContinuationOptions.AttachedToParent);
        }

        private static async Task AsyncAwaitTest()
        {

            int length = await Task.Run<int>(() => GetLength("Hello World!"));

            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Wynik: {length}");

        }


        private static async Task ExceptionTest()
        {
            try
            {
                AsyncAwaitTest();
            }
            catch (ApplicationException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
