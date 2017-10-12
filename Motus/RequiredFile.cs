namespace Motus
{
    class RequiredFile : ObservedFile
    {
        public override bool DelayObservation
        {
            get => !this.Exists;
        }

        public RequiredFile(string path)
        {
            this._path = path;
        }
    }
}
