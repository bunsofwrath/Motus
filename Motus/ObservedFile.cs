using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motus
{
    abstract class ObservedFile
    {
        protected string _path;

        public bool Exists
        {
            get => File.Exists(this._path);
        }

        public virtual bool DelayObservation { get; }
    }
}
