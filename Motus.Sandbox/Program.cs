using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Motus.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            IFileObserver observer = new TimerFileObserver(15, 5, 300);

            observer.AddFile("C:\\Temp\\required.txt", true);
            observer.AddFile("C:\\Temp\\optional.txt", false);

            observer.CheckingForFirstFile += (sender, e) => Console.WriteLine("Checking for first file...");
            observer.WatchingForAllFiles += (sender, e) => Console.WriteLine("First file observed. Watching for remaining files.");
            observer.StoppedObserving += (sender, e) => Console.WriteLine("Stopped observing files.");
            observer.AllFilesObserved += (sender, e) => Console.WriteLine("All expected files observed.");

            observer.StartObserving();
            Thread.Sleep(5000);
            observer.StopObserving();
        }
    }
}
