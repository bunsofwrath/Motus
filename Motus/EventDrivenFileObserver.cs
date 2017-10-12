using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motus
{
    class EventDrivenFileObserver : IFileObserver
    {
        public event EventHandler StartedObserving;
        public event EventHandler StoppedObserving;
        public event EventHandler CheckingForFirstFile;
        public event EventHandler FirstFileObserved;
        public event EventHandler WatchingForAllFiles;
        public event EventHandler AllFilesObserved;

        private List<ObservedFile> _files;

        private TimeSpan _resetInterval;
        private TimeSpan _watchTimeout;

        private List<FileSystemWatcher> _watchers;

        public EventDrivenFileObserver()
        {
            this._files = new List<ObservedFile>();
            this._watchers = new List<FileSystemWatcher>();
        }

        public void AddFile(string path, bool isRequired)
        {
            if (isRequired)
                this._files.Add(new RequiredFile(path));
            else
                this._files.Add(new OptionalFile(path));

            var newWatcher = new FileSystemWatcher(path);

            // newWatcher.Changed += ;
            // newWatcher.Created += ;
            // newWatcher.Deleted += ;
            // newWatcher.Renamed += ;

            this._watchers.Add(newWatcher);
        }

        public void StartObserving()
        {
            foreach (var watcher in this._watchers)
                watcher.EnableRaisingEvents = true;
        }

        public void StopObserving()
        {
            foreach (var watcher in this._watchers)
                watcher.EnableRaisingEvents = false;
        }
    }
}
