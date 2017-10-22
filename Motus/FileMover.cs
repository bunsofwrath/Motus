using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Motus
{
    public class FileMover : IFileMover
    {
        #region Events
        public event FileCopyEventHandler OneFileCopied;
        public event FileEventHandler OneFileDeleted;
        public event EventHandler AllFilesCopied;
        public event EventHandler AllFilesDeleted;
        public event EventHandler AllFilesMoved;

        private void OnOneFileCopied(ObservedFile file, string target)
        {
            this.OneFileCopied?.Invoke(this, new FileCopyEventArgs(file, target));
        }

        private void OnOneFileDeleted(ObservedFile file)
        {
            this.OneFileDeleted?.Invoke(this, new FileEventArgs(file));
        }

        private void OnAllFilesCopied()
        {
            this.AllFilesCopied?.Invoke(this, EventArgs.Empty);
        }

        private void OnAllFilesDeleted()
        {
            this.AllFilesDeleted?.Invoke(this, EventArgs.Empty);
        }

        private void OnAllFilesMoved()
        {
            this.AllFilesMoved?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        private List<ObservedFile> _files;
        private bool _createMissingTargets;
        private bool _overwriteExistingTargets;

        protected IEnumerable<CopyTarget> _targets;

        public FileMover(IFileObserver observer, IEnumerable<CopyTarget> targets, bool createMissingTargets = true, bool overwriteExistingTargets = true)
        {
            this._files = observer.Files;
            this._createMissingTargets = createMissingTargets;
            this._overwriteExistingTargets = overwriteExistingTargets;

            this._targets = targets;
        }

        public void Copy()
        {
            foreach (var file in this._files)
                foreach (var target in this._targets)
                    this.CopyOne(file, target);

            this.OnAllFilesCopied();
        }

        public void Delete()
        {
            foreach (var file in this._files)
            {
                File.Delete(file.WatchPath);
                this.OnOneFileDeleted(file);
            }

            this.OnAllFilesDeleted();
        }

        public void Move()
        {
            this.Copy();
            this.Delete();

            this.OnAllFilesMoved();
        }

        private void CopyOne(ObservedFile file, CopyTarget target)
        {
            if (!Directory.Exists(target.DirectoryPath)
                && this._createMissingTargets)
                Directory.CreateDirectory(target.DirectoryPath);

            var filePath = Path.Combine(target.DirectoryPath, file.FileName);

            File.Copy(file.WatchPath, filePath, this._overwriteExistingTargets);

            this.OnOneFileCopied(file, target.DirectoryPath);
        }
    }
}