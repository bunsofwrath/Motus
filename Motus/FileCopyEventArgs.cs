namespace Motus
{
    public class FileCopyEventArgs : FileEventArgs
    {
        public string Target { get; private set; }

        public FileCopyEventArgs(ObservedFile file, string target) : base(file)
        {
            this.Target = target;
        }
    }
}