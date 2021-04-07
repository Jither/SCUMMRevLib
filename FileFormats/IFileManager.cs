using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.FileFormats
{
    public interface IFileManager
    {
        FileTypeInfos FileTypeInfos { get; }
        SRFile OpenFile(string path);
    }

}
