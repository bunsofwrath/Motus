using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Motus
{
    public class TimerFileObserver : IFileObserver
    {
        #region Events
        public event EventHandler StartedObserving;
        public event EventHandler StoppedObserving;
        public event EventHandler CheckingForFirstFile;
        public event EventHandler FirstFileObserved;
        public event EventHandler WatchingForAllFiles;
        public event EventHandler AllFilesObserved;

        private void OnStartedObserving()
        {
            this.StartedObserving?.Invoke(this, EventArgs.Empty);
        }

        private void OnStoppedObserving()
        {
            this.StoppedObserving?.Invoke(this, EventArgs.Empty);
        }

        private void OnCheckForFirstFile()
        {
            this.CheckingForFirstFile?.Invoke(this, EventArgs.Empty);
        }

        private void OnFirstFileObserved()
        {
            this.FirstFileObserved?.Invoke(this, EventArgs.Empty);
        }

        private void OnWatchForAllFiles()
        {
            this.WatchingForAllFiles?.Invoke(this, EventArgs.Empty);
        }

        private void OnAllFilesObserved()
        {
            this.AllFilesObserved?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        private TimeSpan _checkInterval;
        private TimeSpan _watchInterval;
        private TimeSpan _watchTimeout;

        private DateTime _watchStart;

        private List<ObservedFile> _files;

        private Timer _checkTimer;
        private Timer _watchTimer;

        public TimerFileObserver(int checkIntervalSeconds, int watchIntervalSeconds, int watchTimeoutSeconds)
        {
            this._checkInterval = new TimeSpan(0, 0, checkIntervalSeconds);
            this._watchInterval = new TimeSpan(0, 0, watchIntervalSeconds);
            this._watchTimeout = new TimeSpan(0, 0, watchTimeoutSeconds);

            this._files = new List<ObservedFile>();
        }

        public void AddFile(string path, bool isRequired)
        {
            if (isRequired)
                this._files.Add(new RequiredFile(path));
            else
                this._files.Add(new OptionalFile(path));
        }

        public void StartObserving()
        {
            if ((this._files?.Count ?? 0) == 0)
                throw new InvalidOperationException("Cannot start observing when no files have been added to be observed.");

            this._checkTimer = new Timer(this.CheckForAnyFiles, null, new TimeSpan(), this._checkInterval);
            this.OnStartedObserving();
        }

        public void StopObserving()
        {
            this._checkTimer?.Dispose();
            this._watchTimer?.Dispose();

            this.OnStoppedObserving();
        }

        private void CheckForAnyFiles(object state)
        {
            this.OnCheckForFirstFile();

            if (!this._files.Any(f => f.Exists))
                return;

            this.OnFirstFileObserved();

            this._checkTimer?.Dispose();

            this._watchStart = DateTime.UtcNow;
            this._watchTimer = new Timer(this.CheckForAllFiles, null, new TimeSpan(), this._watchInterval);

            this.OnWatchForAllFiles();
        }

        private void CheckForAllFiles(object state)
        {
            if (DateTime.UtcNow - this._watchStart > this._watchTimeout)
                throw new TimeoutException("Files have been observed, but timed out waiting for remaining files to be observed.");

            if (this._files.Any(f => f.DelayObservation))
                return;

            this.StopObserving();
            this.OnAllFilesObserved();
        }
    }
}
