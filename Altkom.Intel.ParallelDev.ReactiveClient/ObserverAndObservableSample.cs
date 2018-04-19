using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altkom.Intel.ParallelDev.ReactiveClient
{
    class ObserverAndObservableSample
    {

        public static void ColdSourceTest()
        {
            var source = new MySource();

            var observer1 = new MyObserver();

            using (var subscription = source.Subscribe(observer1))
            {

            }
   
        }
    }
}
