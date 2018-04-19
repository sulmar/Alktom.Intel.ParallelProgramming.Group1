using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alktom.Intel.ParallelDev.ConsoleClient
{
    class ConcurentQueueSample
    {

        public static void ConcurentQueueTest()
        {
            IProducerConsumerCollection<int> queue = new ConcurrentQueue<int>();

            Task producentTask1 = Task.Run(() => Producent(queue));
            Task producentTask2 = Task.Run(() => Producent(queue));

            Task cosumentTask1 = Task.Run(() => Consume(queue));
            Task cosumentTask2 = Task.Run(() => Consume(queue));

            Task.WaitAll(producentTask1,  producentTask2, cosumentTask1, cosumentTask2);

        }

        public static void Producent(IProducerConsumerCollection<int> queue)
        {
            for (int i = 0; i < 100; i++)
            {
                if (queue.TryAdd(i))
                {
                    Thread.Sleep(50);
                }
                else
                {
                    Console.WriteLine($"{i} skipped");
                }
            
            }
        }

        public static void Consume(IProducerConsumerCollection<int> queue)
        {
            while (true)
            {
                while (queue.TryTake(out int item))
                {
                    Console.WriteLine($"received {item}");

                    Thread.Sleep(100);
                }

                 Console.WriteLine("not received");
            }
        }
    }
}
