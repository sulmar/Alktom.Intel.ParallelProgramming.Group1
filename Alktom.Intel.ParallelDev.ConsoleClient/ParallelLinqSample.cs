using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alktom.Intel.ParallelDev.ConsoleClient
{
    class ParallelLinqSample
    {

        public static void DownloadTest()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

            var urls = new List<string>
            {
                "http://www.altkom.pl",
                "http://www.intel.com",
                "http://www.altkom.pl",
                "http://www.intel.com",
                "http://www.altkom.pl",
                "http://www.intel.com",
                "http://www.altkom.pl",
                "http://www.microsoft.com",
                "http://www.microsoft.com",
                "http://www.microsoft.com",
                "http://www.microsoft.com",
            };

            var sites = urls
                .AsParallel()
                .WithCancellation(cts.Token)
                .WithDegreeOfParallelism(4);

            var q = from url in sites
                    let result = Download(url)
                    select new
                    {
                        site = url,
                        length = result.Length
                    };

            foreach (var item in q)
            {
                Console.WriteLine($"Site {item.site} Size {item.length}");
            }
        }

        private static string Download(string url)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }

        private static bool IsExists(int number)
        {
            
            return number == 555;
        }

        public static void FindTest()
        {
            var numbers = Enumerable.Range(1, 1000)
                .AsParallel();

            var number = numbers
                .Single(n => n == 555);

            Console.WriteLine(number);
        }

        public static void AsOrderedNumbersTest()
        {
            var numbers = Enumerable.Range(1, 10000)
                .AsParallel()
                .AsOrdered()
                .Select(e => e * e)
               // .AsSequential()
                .Take(5);

            var source = Enumerable.Range(1, 100);

            // .ForAll(e => Console.WriteLine(e));

            foreach (var item in numbers)
            {
                Console.WriteLine(item);
            }



        }

        public static void AsParallelTest()
        {
            Console.WriteLine("AsParallelTest");

            for (int i = 0; i < 10; i++)
            {
                var q = "abcdefghijklmn"
                .AsParallel()                
                .Select(c => $"{char.ToUpper(c)} - #{Thread.CurrentThread.ManagedThreadId}");

                foreach (var item in q)
                {
                    Console.WriteLine(item);
                }

                //q.ForAll(c => Console.Write(c));

                Console.WriteLine("-----");
            }
                
        }

        public static void AsOrderedTest()
        {
            Console.WriteLine("AsOrderedTest");

            for (int i = 0; i < 10; i++)
            {
                var q = "abcdefghijklmn"
                .AsParallel()
                .AsOrdered()
                .Select(c => $"{char.ToUpper(c)} - #{Thread.CurrentThread.ManagedThreadId}");

                foreach (var item in q)
                {
                    Console.WriteLine(item);
                }

                //q.ForAll(c => Console.Write(c));

                Console.WriteLine("-----");
            }

        }
    }
}
