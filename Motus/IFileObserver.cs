using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motus
{
    public interface IFileObserver : IDisposable
    {
        event EventHandler StartedObserving;
        event EventHandler StoppedObserving;
        event EventHandler CheckingForFirstFile;
        event EventHandler FirstFileObserved;
        event EventHandler WatchingForAllFiles;
        event EventHandler AllFilesObserved;

        List<ObservedFile> Files { get; }

        void AddFile(ObservedFile file);
        void AddFile(string watchPath, bool required);
        void StartObserving();
        void StopObserving();
    }
}