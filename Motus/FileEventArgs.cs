using System;

namespace Motus
{
    public class FileEventArgs : EventArgs
    {
        public ObservedFile File { get; private set; }

        public FileEventArgs(ObservedFile file)
        {
            this.File = file;
        }
    }
}