using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Alktom.Intel.ParallelDev.TaskConsoleClient.DownloadDemo
{
    public class TaskDownload
    {
        public static async Task<string> DownloadStringAsync(string uri)
        {
            using (var client = new WebClient())
            {
                var content = await client.DownloadStringTaskAsync(uri);

                return content;
            }
        }
    }
}
