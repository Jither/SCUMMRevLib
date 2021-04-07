using SCUMMRevLib.Chunks;

namespace SCUMMRevLib.Decoders.Images
{
    public abstract class BaseImageDecoder : BaseDecoder
    {
        public override DecoderFormat Format
        {
            get { return DecoderFormat.Image; }
        }

        public override string  GetOutputDescription(Chunk chunk)
        {
 	        return "Image";
        }

        public abstract uint GetCount(Chunk chunk);
        public abstract ImageInfo GetInfo(Chunk chunk, uint index);
        public abstract byte[] Decode(Chunk chunk, uint index);
    }
}
