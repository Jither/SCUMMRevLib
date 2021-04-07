namespace SCUMMRevLib.Decoders.Sound
{
    public class SoundInfo
    {
        public uint Channels { get; protected set; }
        public uint SampleRate { get; protected set; }
        public uint BitsPerSample { get; protected set; }

        public SoundInfo(uint channels, uint sampleRate, uint bitsPerSample)
        {
            Channels = channels;
            SampleRate = sampleRate;
            BitsPerSample = bitsPerSample;
        }
    }
}
