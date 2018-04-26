using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alktom.Intel.ParallelDev.ConsoleClient
{
    class ConcurentQueueLockSample
    {

        public static void Test()
        {
            QueueTest();
        }


        private static int Qty;

        public static void QueueTest()
        {
            Queue<int> queue = new Queue<int>();

            for (int i = 0; i < 10; i++)
            {
                queue.Enqueue(i);
            };
            

            //Task taskProducer1 = Task.Run(() => Producer(queue));
            //Task taskProducer2 = Task.Run(() => Producer(queue));

            Task taskConsumer1 = Task.Run(() => Consume2(queue));
            Task taskConsumer2 = Task.Run(() => Consume2(queue));

            Task.WaitAll(taskConsumer1, taskConsumer2);


        }

        public static void Producer(Queue<int> queue)
        {
            for (int i = 0; i < 10; i++)
            {
                queue.Enqueue(i);
                Thread.Sleep(200);
            }
        }


        private static object lockObject = new object();

        public static void Consume2(Queue<int> queue)
        {
            while (true)
            {
                lock (lockObject)
                {

                    if (queue.Count <= 0)
                    {
                        break;
                    }

                    Qty += queue.Peek();


                    Thread.Sleep(200);

                    var item = queue.Dequeue();
                }

            }

            Console.WriteLine($"Licznik {Qty}");
        }

        public static void Consume(Queue<int> queue)
        {
            Thread.Sleep(500);

            while (true)
            {
                Qty = queue.Count;

                if (queue.Count > 0)
                {
                    var item = queue.Dequeue();

                    Console.WriteLine($"Received: {item}");
                }
            }

            //foreach (var item in queue)
            //{
            //    Console.WriteLine(item);
            //    Thread.Sleep(200);
            //}
        }
    }
}
