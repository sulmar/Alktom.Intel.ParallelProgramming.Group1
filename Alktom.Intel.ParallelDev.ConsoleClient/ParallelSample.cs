using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alktom.Intel.ParallelDev.ConsoleClient
{
    class ParallelSample
    {

        public static void ParallelForEachTest()
        {
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

            // zla praktyka
            // Parallel.ForEach(urls, Download);

            // nie dziala
            //using (WebClient client = new WebClient())
            //{
            //    Parallel.ForEach(urls, url => Download(client, url));
            //}

            int numberOfInit = 0;
            int numberOfDisposed = 0;

            Parallel.ForEach(urls,
                () => {
                    Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Create");
                    numberOfInit++;
                    return new WebClient();
                },
                (url, loopstate, index, client) =>
                {
                    Download(client, url);
                    return client;
                },
                client =>
                {
                    client.Dispose();
                    Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} Disposed");
                    numberOfDisposed++;
                }

                );

            Console.WriteLine($"numberOfInit={numberOfInit} numberOfDisposed ={numberOfDisposed} urls={urls.Count}");

        }

        private static void Download(string url)
        {
            Console.WriteLine($"thread #{Thread.CurrentThread.ManagedThreadId} downloading {url} ...");

            using (var client = new WebClient())
            {
                var content = client.DownloadString(url);

                Console.WriteLine($"thread #{Thread.CurrentThread.ManagedThreadId} downloaded {content.Length}");
            }
        }

        private static void Download(WebClient client, string url)
        {
            Console.WriteLine($"thread #{Thread.CurrentThread.ManagedThreadId} downloading {url} ...");
            
            var content = client.DownloadString(url);

            Console.WriteLine($"thread #{Thread.CurrentThread.ManagedThreadId} downloaded {content.Length}");
            
        }
    }
}
