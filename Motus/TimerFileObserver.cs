using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Motus
{
    public class TimerFileObserver : IFileObserver, IDisposable
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

        public List<ObservedFile> Files { get; private set; }

        private Timer _checkTimer;
        private Timer _watchTimer;

        public TimerFileObserver(int checkIntervalSeconds, int watchIntervalSeconds, int watchTimeoutSeconds)
        {
            this._checkInterval = new TimeSpan(0, 0, checkIntervalSeconds);
            this._watchInterval = new TimeSpan(0, 0, watchIntervalSeconds);
            this._watchTimeout = new TimeSpan(0, 0, watchTimeoutSeconds);

            this.Files = new List<ObservedFile>();
        }

        public void AddFile(string watchPath, bool isRequired)
        {
            if (isRequired)
                this.AddFile(new RequiredFile(watchPath));
            else
                this.AddFile(new OptionalFile(watchPath));
        }

        public void AddFile(ObservedFile file)
        {
            this.Files.Add(file);
        }

        public void StartObserving()
        {
            if ((this.Files?.Count ?? 0) == 0)
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

            if (!this.Files.Any(f => f.Exists))
                return;

            this.OnFirstFileObserved();

            this._checkTimer?.Dispose();

            this._watchStart = DateTime.UtcNow;
            this._watchTimer = new Timer(this.CheckForAllFiles, null, new TimeSpan(), this._watchInterval);
        }

        private void CheckForAllFiles(object state)
        {
            this.OnWatchForAllFiles();

            if (DateTime.UtcNow - this._watchStart > this._watchTimeout)
                throw new TimeoutException("Files have been observed, but timed out waiting for remaining files to be observed.");

            if (this.Files.Any(f => f.CausesDelay))
                return;

            this.StopObserving();
            this.OnAllFilesObserved();
        }

        #region IDisposable Support
        private bool _disposed;

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._checkTimer.Dispose();
                    this._watchTimer.Dispose();
                }
            }

            this._disposed = true;
        }
        #endregion
    }
}