using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motus
{
    public delegate void FileCopyEventHandler(object sender, FileCopyEventArgs e);
    public delegate void FileEventHandler(object sender, FileEventArgs e);
}