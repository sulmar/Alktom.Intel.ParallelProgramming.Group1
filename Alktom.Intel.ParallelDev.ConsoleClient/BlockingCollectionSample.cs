using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alktom.Intel.ParallelDev.ConsoleClient
{
    class BlockingCollectionSample
    {
        public static void BlockingCollectionTest()
        {
            BlockingCollection<int> collection = new BlockingCollection<int>();

            Task productTask1 = Task.Run(() => Producent(collection));
            Task productTask2 = Task.Run(() => Producent(collection));
            Task consumerTask = Task.Run(() => Consumer(collection));

            Task.WaitAll(productTask1, productTask2);

      //       collection.CompleteAdding();

            Task.WaitAll(consumerTask);

            collection.Add(100);
        }

        private static void Consumer(BlockingCollection<int> collection)
        {
            foreach (var item in collection.GetConsumingEnumerable())
            {
                Console.WriteLine(item);
            }
        }

        private static void Producent(BlockingCollection<int> collection)
        {
            for (int i = 0; i < 10; i++)
            {
                collection.Add(i);
                Thread.Sleep(1000);
            }
        }
    }
}
