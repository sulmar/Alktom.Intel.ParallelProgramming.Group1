using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alktom.Intel.ParallelDev.ConsoleClient
{
    public class CancellationTokenSourceSample
    {
        public static async void CancelDownloadTest()
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(0.1)))
            {
                var token = cts.Token;

                string content = await DownloadStringAsync("http://www.intel.pl", token);

                if (!string.IsNullOrEmpty(content))
                    Console.WriteLine($"{content.Length}");
                else
                    Console.WriteLine("Nie działa");
            }
        }



        public static async Task<string> DownloadStringAsync(string uri)
        {
            using (var client = new WebClient())
            {
                var content = await client.DownloadStringTaskAsync(uri);

                return content;
            }
        }

        public static async Task<string> DownloadStringAsync(string uri, CancellationToken cancellationToken)
        {
            try
            {
                using (var client = new WebClient())
                using (var registration = cancellationToken.Register(() => client.CancelAsync()))
                {
                    var content = await client.DownloadStringTaskAsync(uri);

                    return content;
                }
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.RequestCanceled)
            {                
                return null;
            }

        }
    }
}
