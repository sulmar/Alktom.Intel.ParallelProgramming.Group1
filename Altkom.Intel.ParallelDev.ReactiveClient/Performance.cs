using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Altkom.Intel.ParallelDev.ReactiveClient
{
    public class Performance
    {

        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        PerformanceCounter perfCounter;


        public Performance()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public IObservable<float> CurrentCpuUsage =>
            Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(_ => cpuCounter.NextValue());


    }
}
