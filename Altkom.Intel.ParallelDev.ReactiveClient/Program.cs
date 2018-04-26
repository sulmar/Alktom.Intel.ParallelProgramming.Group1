using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altkom.Intel.ParallelDev.ReactiveClient
{
    class Program
    {
        static void Main(string[] args)
        {
            CPUMonitor.CpuTest();

            TimeObservableSample.FileSystemWatcherTest();

            // TimeObservableSample.ColdSubject();

            // TimeObservableSample.BufferTest();

            // TimeObservableSample.WindowTest();

            // SubjectSample.SubjectTest();

            //   SubjectSample.ReplaySubjectTest();


            // ObserverAndObservableSample.ColdSourceTest();

            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();
        }
    }

    
}
