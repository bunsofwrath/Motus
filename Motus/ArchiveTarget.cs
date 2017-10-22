using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motus
{
    public class ArchiveTarget : CopyTarget
    {
        public ArchiveTarget(string directoryPath) : base(directoryPath)
        { }

        public override string DirectoryPath
        {
            get
            {
                var dateTimeString = DateTime.Now.ToString("yyyyMMddhhmmss");
                return Path.Combine(this._basePath, dateTimeString);
            }
        }
    }
}
