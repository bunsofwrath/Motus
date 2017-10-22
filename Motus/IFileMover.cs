using System;

namespace Motus
{
    public interface IFileMover
    {
        event FileCopyEventHandler OneFileCopied;
        event FileEventHandler OneFileDeleted;
        event EventHandler AllFilesCopied;
        event EventHandler AllFilesDeleted;
        event EventHandler AllFilesMoved;

        void Copy();
        void Delete();
        void Move();
    }
}