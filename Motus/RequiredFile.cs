namespace Motus
{
    public class RequiredFile : ObservedFile
    {
        public override bool CausesDelay
        {
            get => !this.Exists;
        }

        public RequiredFile(string watchPath) : base(watchPath)
        { }
    }
}
