using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motus
{
    public abstract class ObservedFile
    {
        public string WatchPath { get; private set; }

        public string FileName
        {
            get
            {
                return Path.GetFileName(this.WatchPath);
            }
        }

        public ObservedFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("The given file path has a null or empty file name.", nameof(filePath));

            this.WatchPath = filePath;
        }

        public bool Exists
        {
            get => File.Exists(this.WatchPath);
        }

        public virtual bool CausesDelay { get; }
    }
}