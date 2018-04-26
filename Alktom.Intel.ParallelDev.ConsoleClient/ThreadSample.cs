using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alktom.Intel.ParallelDev.ConsoleClient
{
    class ThreadSample
    {
        public static void Test()
        {
            ThreadTest();

            ThreadPool.QueueUserWorkItem(DoSecondWork);

            ThreadPoolTest();
        }

        private static void ThreadTest()
        {
            Thread t1 = new Thread(DoWork);
            // t1.IsBackground = true;
            t1.Start();

            Thread t2 = new Thread(() => DoWork());
            // t2.IsBackground = true;
            t2.Start();

            // t1.Join();
        }

        private static void ThreadPoolTest()
        {
            ThreadPool.SetMaxThreads(2, 0);

            for (int i = 0; i < 100; i++)
            {
                ThreadPool.QueueUserWorkItem(DoSecondWork, i);
            }
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

        static void DoSecondWork(object obj)
        {
            Console.WriteLine($"working...{obj} Thread: {Thread.CurrentThread.ManagedThreadId}");

        }
    }
}
