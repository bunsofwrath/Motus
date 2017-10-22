namespace Motus
{
    public class OptionalFile : ObservedFile
    {
        public override bool CausesDelay => false;

        public OptionalFile(string watchPath) : base(watchPath)
        { }
    }
}
