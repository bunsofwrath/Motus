namespace Motus
{
    public class CopyTarget
    {
        protected string _basePath;

        public virtual string DirectoryPath
        {
            get
            {
                return this._basePath;
            }
        }

        public CopyTarget(string directoryPath)
        {
            this._basePath = directoryPath;
        }
    }
}