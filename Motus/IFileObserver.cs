using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motus
{
    public interface IFileObserver
    {
        event EventHandler StartedObserving;
        event EventHandler StoppedObserving;
        event EventHandler CheckingForFirstFile;
        event EventHandler FirstFileObserved;
        event EventHandler WatchingForAllFiles;
        event EventHandler AllFilesObserved;

        void AddFile(string path, bool isRequired);
        void StartObserving();
        void StopObserving();
    }
}