using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Decoders;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Viewers
{
    public abstract class Saver : IViewer
    {
        public abstract DecoderFormat DecoderFormat { get; }

        public abstract string ActionFormat { get; }

        public abstract string GetActionText(BaseDecoder decoder, Chunk chunk);
        public abstract List<FileTypeInfo> GetFileTypes(BaseDecoder decoder, Chunk chunk);
        public abstract void Execute(BaseDecoder decoder, Chunk chunk, string filename);
    }

    public abstract class Saver<T> : Saver where T : BaseDecoder
    {
        private static List<FileTypeInfo> defaultFileTypeDescription = new List<FileTypeInfo>
                                                                                  {
                                                                                      new FileTypeInfo("All files", "*.*")
                                                                                  };

        public abstract void Execute(T decoder, Chunk chunk, string filename);

        public string GetIndexedFileName(string filename, uint index)
        {
            string dir = Path.GetDirectoryName(filename);
            string filenameNoExt = Path.GetFileNameWithoutExtension(filename);
            string pathNoExt = Path.Combine(dir, filenameNoExt);
            string ext = Path.GetExtension(filename);

            return String.Format("{0}_{1:000}{2}", pathNoExt, index, ext);
        }

        public override string GetActionText(BaseDecoder decoder, Chunk chunk)
        {
            return String.Format(ActionFormat, decoder.GetOutputDescription(chunk));
        }

        public virtual List<FileTypeInfo> GetFileTypes(T decoder, Chunk chunk)
        {
            return defaultFileTypeDescription;
        }

        public override List<FileTypeInfo> GetFileTypes(BaseDecoder decoder, Chunk chunk)
        {
            T typedDecoder = decoder as T;
            if (typedDecoder == null)
            {
                throw new DecodingException("Expected {0}", typeof(T));
            }

            return GetFileTypes(typedDecoder, chunk);
        }

        public override void Execute(BaseDecoder decoder, Chunk chunk, string filename)
        {
            T typedDecoder = decoder as T;
            if (typedDecoder == null)
            {
                throw new DecodingException("Expected {0}", typeof(T));
            }

            Execute(typedDecoder, chunk, filename);
        }
    }
}
