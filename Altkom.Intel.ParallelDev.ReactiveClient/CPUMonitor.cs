using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altkom.Intel.ParallelDev.ReactiveClient
{
    class CPUMonitor
    {
        public static void CpuTest()
        {
            const float cpuLimit = 10;

            Performance performance = new Performance();

            performance.CurrentCpuUsage
                .Subscribe(cpu => Console.WriteLine($"CPU {cpu} %"));


            performance.CurrentCpuUsage
                .Where(cpu => cpu > cpuLimit)
                .Subscribe(cpu =>
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ALERT CPU {cpu} %");
                    Console.ResetColor();
                }
                );

        }
    }
}
