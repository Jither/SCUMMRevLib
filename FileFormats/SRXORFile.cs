using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Encryption;

namespace SCUMMRevLib.FileFormats
{
    public abstract class SRXORFile : SRFile
    {
        protected readonly XorStream xorStream;

        public byte Encryption
        {
            get { return xorStream.Encryption; }
            set { xorStream.Encryption = value; }
        }

        protected SRXORFile(string path, XorStream stream) : base(path, stream)
        {
            this.xorStream = stream;
        }
    }
}
