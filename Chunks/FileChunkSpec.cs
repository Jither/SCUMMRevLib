namespace SCUMMRevLib.Chunks
{
    public class FileChunkSpec
    {
        public string Extension { get; protected set; }
        public string Description { get; protected set; }
        public ImageIndex ImageIndex { get; protected set; }

        public FileChunkSpec(string extension, string description, ImageIndex imageIndex)
        {
            Extension = extension;
            Description = description;
            ImageIndex = imageIndex;
        }
    }
}
