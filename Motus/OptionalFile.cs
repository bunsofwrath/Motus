namespace Motus
{
    class OptionalFile : ObservedFile
    {
        public override bool DelayObservation => false;

        public OptionalFile(string path)
        {
            this._path = path;
        }
    }
}
