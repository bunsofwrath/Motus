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
            var copyTargets = new List<CopyTarget>()
            {
                new CopyTarget("C:\\Temp\\Input\\Motus"),
                new ArchiveTarget("C:\\Temp\\Archive\\Motus")
            };

            using (IFileObserver observer = new TimerFileObserver(5, 1, 300))
            {
                IFileMover mover = new FileMover(observer, copyTargets);

                var lockObject = new object();

                observer.AddFile("C:\\Temp\\Motus\\required1.txt", true);
                observer.AddFile("C:\\Temp\\Motus\\required2.txt", true);
                observer.AddFile("C:\\Temp\\Motus\\optional1.txt", false);
                observer.AddFile("C:\\Temp\\Motus\\optional2.txt", false);

                observer.CheckingForFirstFile += (sender, e) => Console.WriteLine("Checking for first file...");
                observer.WatchingForAllFiles += (sender, e) => Console.WriteLine("First file observed. Watching for remaining files.");
                observer.StoppedObserving += (sender, e) => Console.WriteLine("Stopped observing files.");
                observer.AllFilesObserved += (sender, e) => Console.WriteLine("All expected files observed.");

                mover.OneFileCopied += (sender, e) => Console.WriteLine($"Source: {e.File.FileName} | Target: {e.Target}");
                mover.OneFileDeleted += (sender, e) => Console.WriteLine($"Source: {e.File.FileName}");
                mover.AllFilesCopied += (sender, e) => Console.WriteLine($"All files copied.");
                mover.AllFilesDeleted += (sender, e) => Console.WriteLine($"All files deleted.");
                mover.AllFilesMoved += (sender, e) => Console.WriteLine($"All files moved.");

                observer.AllFilesObserved += (sender, e) =>
                {
                    mover.Move();
                    observer.StartObserving();
                };

                observer.StartObserving();

                Console.ReadLine();
            }
        }
    }
}