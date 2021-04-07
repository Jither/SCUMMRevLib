using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.FileFormats.Factories
{
    public abstract class FileFactory
    {
        public SRFile Create(string path)
        {
            var stream = CreateStream(path);
            if (stream == null)
            {
                return null;
            }
            var result = CreateFromStream(path, stream);
            return result;
        }

        protected virtual Stream CreateStream(string path)
        {
            return File.OpenRead(path);
        }

        protected abstract SRFile CreateFromStream(string path, Stream stream);
    }
}
