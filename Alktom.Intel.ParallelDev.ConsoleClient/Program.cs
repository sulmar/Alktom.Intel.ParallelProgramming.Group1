using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alktom.Intel.ParallelDev.ConsoleClient
{
    class Program
    {
        delegate void Send(string message);

        static private Send send;

        // delegate void DoIt();

        delegate bool CanDoIt();

        delegate decimal Calculate(decimal amount);

        private static void ActionTest()
        {
            Action doit = () => Console.WriteLine("Do It!");

            Func<bool> canDoIt = () => true;
            
            Predicate<int> overLimit = input => input > 10;
            
            Func<decimal, decimal> calculate = amount => amount * 10;


        }

        private static void SendSms(string message)
        {
            Console.WriteLine($"{message} via sms");
        }

        private static void SendTweet(string tweet)
        {
            Console.WriteLine($"{tweet} published");
        }


        public void Test() => Console.WriteLine("Hello");

        static void Main(string[] args)
        {

           Task.Run(() => ExceptionTest());
            
            
            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();

            return;



            int x = 10;

            // Ręczne tworzenie taska i uruchomienie
            Task task1 = new Task(()=>DoSecondWork(x));
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


            // DelegatesTest();

            // SequentialTest();

            // ThreadTest();

            // ThreadPool.QueueUserWorkItem(DoSecondWork);

            //for (int i = 0; i < 1000; i++)
            //{
            //    Thread t = new Thread(DoWork);
            //    t.Start();
            //}

            // ThreadPoolTest();

            Console.WriteLine($"Main thread {Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();
        }

        private static void ThreadPoolTest()
        {
            ThreadPool.SetMaxThreads(2, 0);

            for (int i = 0; i < 100; i++)
            {
                ThreadPool.QueueUserWorkItem(DoSecondWork, i);
            }
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

        private static void DelegatesTest()
        {
            send += SendSms;
            send += SendTweet;

            // metoda anonimowa
            send += delegate (string message)
            {
                Console.WriteLine(message);
            };

            // wyrażenie lambda
            send += message => Console.WriteLine(message);

            var delegates = send.GetInvocationList().ToList();

            send -= SendTweet;

            send?.Invoke("Hello World!");
        }

        private static void SequentialTest()
        {
            try
            {
                DoWork();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            DoWork();

            DoWork();
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

        private static void ContinueWithTest()
        {
            var task5 = Task.Run<int>(() => GetLength("Hello World!"));

            task5
                .ContinueWith(t => Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Wynik: {t.Result}"), TaskContinuationOptions.AttachedToParent)
                    .ContinueWith(t => Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} thx"), TaskContinuationOptions.AttachedToParent);
        }

        private static async Task ExceptionTest()
        {
            try
            {
                AsyncAwaitTest();
            }
            catch(ApplicationException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static async Task AsyncAwaitTest()
        {
            
            int length = await Task.Run<int>(() => GetLength("Hello World!"));

            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Wynik: {length}");

                    
        }
    }
}
