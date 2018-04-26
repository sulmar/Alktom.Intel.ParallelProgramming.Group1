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


            Console.WriteLine($"Main thread {Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();
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




       

      
    }
}
