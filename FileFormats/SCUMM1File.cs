using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Encryption;
using SCUMMRevLib.FileFormats.Factories;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.FileFormats
{
    public enum SCUMM1ResourceType
    {
        Costume,
        Script,
        Sound
    }

    public class SCUMM1ResourceMap : Dictionary<ulong, SCUMM1ResourceType>
    {
        
    }

    public class SCUMM1File : SRXORFile
    {
        public int FileVersion { get; set; }

        public bool IsDirectory { get; set; }

        private SCUMM1ResourceMap resourceMap;

        private int RoomNumber
        {
            get
            {
                int result;
                if (Int32.TryParse(System.IO.Path.GetFileNameWithoutExtension(Path), out result))
                {
                    return result;
                }
                return -1;
            }
        }

        public SCUMM1File(string path, XorStream stream) : base(path, stream)
        {
        }

        public override ChunkList GetRootChunks()
        {
            return IsDirectory ? GetDirectoryRootChunks() : GetResourceRootChunks();
        }

        private ChunkList GetDirectoryRootChunks()
        {
            var result = new ChunkList();
            result.Add(new UnknownChunk(this, RootChunk, 0, Size));
            return result;
        }

        private ChunkList GetResourceRootChunks()
        {
            var map = GetResourceMap();
            var result = new ChunkList();

            Position = 0;
            ulong offset = Position;
            uint chunkSize = ReadU16LE();
            var chunk = new SCUMM1Chunk(this, RootChunk, "ROOMv1", offset, chunkSize);
            result.Add(chunk);

            Position += chunkSize - 2;

            string chunkName;
            while (Position < Size)
            {
                offset = Position;
                chunkSize = ReadU16LE();

                if (offset + chunkSize > Size)
                {
                    chunkSize = (uint) (Size - offset);
                    chunkName = "????v1";
                }
                else
                {
                    SCUMM1ResourceType type;
                    if (!map.TryGetValue(offset, out type))
                    {
                        chunkName = "????v1";
                    }
                    else
                    {
                        switch (type)
                        {
                            case SCUMM1ResourceType.Costume: chunkName = "COSTv1"; break;
                            case SCUMM1ResourceType.Script: chunkName = "SCRPv1"; break;
                            case SCUMM1ResourceType.Sound: chunkName = "SOUNv1"; break;
                            default:
                                throw new FileFormatException("Unknown resource type: {0}", type);
                        }
                    }
                }
                result.Add(new SCUMM1Chunk(this, RootChunk, chunkName, offset, chunkSize));
                Position += chunkSize - 2;
            }

            return result;
        }

        public SCUMM1ResourceMap GetResourceMap()
        {
            if (resourceMap != null)
            {
                return resourceMap;
            }
            int room = RoomNumber;

            ushort objectCount = 0;
            byte roomCount = 0;
            byte costCount = 0;
            byte scriptCount = 0;
            byte soundCount = 0;

            switch (FileVersion)
            {
                case 1:
                    // Maniac Mansion
                    // TODO: Zak
                    objectCount = 0x320;
                    roomCount = 0x37;
                    costCount = 0x23;
                    scriptCount = 0xC8;
                    soundCount = 0x64;
                    break;
                case 2:
                    break;
            }

            SCUMM1ResourceMap map = new SCUMM1ResourceMap();

            using (SCUMM1File file = OpenDirectory(Path))
            {
                file.Encryption = Encryption;
                // Skip magic number
                file.Position = 2;

                // Skip objects - 1 byte per object
                if (objectCount == 0)
                {
                    objectCount = file.ReadU16LE();
                }
                file.Position += objectCount;

                // Skip rooms - 3 bytes per room
                if (roomCount == 0)
                {
                    roomCount = file.ReadU8();
                }
                file.Position += (ulong)roomCount*3;

                ReadResourceMap(SCUMM1ResourceType.Costume, map, file, room, costCount);
                ReadResourceMap(SCUMM1ResourceType.Script, map, file, room, scriptCount);
                ReadResourceMap(SCUMM1ResourceType.Sound, map, file, room, soundCount);

                resourceMap = map;
                return map;
            }
        }

        private void ReadResourceMap(SCUMM1ResourceType type, SCUMM1ResourceMap map, SCUMM1File file, int room, uint count)
        {
            if (count == 0)
            {
                count = file.ReadU8();
            }
            byte[] roomNos = new byte[count];
            ushort[] offsets = new ushort[count];
            file.Read(roomNos, 0, count);
            for (int i = 0; i < count; i++)
            {
                offsets[i] = file.ReadU16LE();
            }

            for (int i = 0; i < count; i++)
            {
                if (room == roomNos[i])
                {
                    map.Add(offsets[i], type);
                }
            }
        }

        public static SCUMM1File OpenDirectory(string path)
        {
            string directoryPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "00.LFL");
            if (!File.Exists(directoryPath))
            {
                return null;
            }
            // FIXME: Bit error-prone to create the file this way...
            XorStream stream = new XorStream(directoryPath);
            SCUMM1File directoryFile = new SCUMM1File(directoryPath, stream);
            return directoryFile;
        }
    }
}
