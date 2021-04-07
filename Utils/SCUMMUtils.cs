using System;
using Katana.IO;
using SCUMMRevLib.Chunks;
using SCUMMRevLib.Decoders;
using SCUMMRevLib.FileFormats;

namespace SCUMMRevLib.Utils
{
    public static class SCUMMUtils
    {
        /// <summary>
        /// Determines version of SCUMM engine used for a particular file.
        /// The revision is rarely, if ever, relevant or determinable by file (without checksums).
        /// For now, we're also leaving out minor version
        /// 
        /// Versions by game:
        /// - Maniac Mansion                        0.0
        /// - Zak McKracken                         1.0
        /// - Maniac Mansion (NES)                  1.5
        /// - Maniac Mansion (enhanced)             2.0
        /// - Zak McKracken (enhanced)              2.0
        /// - Last Crusade                          3.0
        /// - Zak McKracken (FM-TOWNS)              3.0
        /// - Loom                                  3.5
        /// - Secret of Monkey Island (EGA)         4.0
        /// - Secret of Monkey Island (VGA)         5.0
        /// - Loom (CD)                             5.1
        /// - Monkey Island 2                       5.2
        /// - Fate of Atlantis                      5.2
        /// - Secret of Monkey Island (CD)          5.3
        /// - Fate of Atlantis (TALKIE)             5.5
        /// - Day of the Tentacle                   6.4
        /// - Sam & Max Hit the Road                6.5
        /// - Sam & Max Hit the Road (CD)           7.0
        /// - Full Throttle                         7.3
        /// - The Dig                               7.5
        /// - Curse of Monkey Island                8.1
        /// </summary>
        public static int DetermineSCUMMVersion(SRFile file)
        {
            if (file is SCUMM3File)
            {
                // TODO: Uncertain
                // SCUMM 4 has LE root chunks
                if (file.RootChunk.SelectSingle("/LE") != null)
                {
                    return 4;
                }
                return 3;
            }
            
            if (file is SCUMM5File)
            {
                // SCUMM 8 has room scripts chunk
                if (file.RootChunk.SelectSingle("/LECF/LFLF/RMSC") != null)
                {
                    return 8;
                }
                // SCUMM 7 has AKOS costumes
                if (file.RootChunk.SelectSingle("/LECF/LFLF/AKOS") != null)
                {
                    return 7;
                }
                // SCUMM 5 has a 24 byte IMHD
                Chunk imhdChunk = file.RootChunk.SelectSingle("/LECF/LFLF/ROOM/OBIM/IMHD");
                if (imhdChunk != null && imhdChunk.Size == 24)
                {
                    return 5;
                }

                // It's not 5, 7 or 8, so must be 6:
                return 6;
            }
            // Not a SCUMM file
            return -1;
        }

        public static Size GetRoomDimensions(Chunk roomChunk)
        {
            Size result = new Size();
            if (roomChunk.ChunkTypeId == "ROOM")
            {
                Chunk rmhdChunk = roomChunk.SelectSingle("RMHD");
                if (rmhdChunk == null)
                {
                    throw new DecodingException("RMHD chunk not found");
                }
                BinReader reader = rmhdChunk.GetReader();
                if (rmhdChunk.Size >= 8 && rmhdChunk.Size <= 14)
                {
                    // SCUMM 5.x
                    reader.Position = 8;
                    result.Width = reader.ReadU16LE();
                    result.Height = reader.ReadU16LE();
                }
                else if (rmhdChunk.Size == 18)
                {
                    // FT/Dig
                    reader.Position = 12;
                    result.Width = reader.ReadU16LE();
                    result.Height = reader.ReadU16LE();
                }
                else if (rmhdChunk.Size == 32)
                {
                    // CMI
                    reader.Position = 12;
                    result.Width = (int) reader.ReadU32LE();
                    result.Height = (int) reader.ReadU32LE();
                }
                else
                {
                    throw new DecodingException("Unknown RMHD format");
                }
            }
            else if (roomChunk.ChunkTypeId == "RO")
            {
                Chunk rmhdChunk = roomChunk.SelectSingle("HD");
                if (rmhdChunk == null)
                {
                    throw new DecodingException("HD chunk not found");
                }
                BinReader reader = rmhdChunk.GetReader();
                reader.Position = 6;
                result.Width = reader.ReadU16LE();
                result.Height = reader.ReadU16LE();
            }
            return result;
        }

        private static Chunk GetObjectImageHeader(Chunk imageChunk)
        {
            Chunk result = imageChunk.SelectSingle("IMHD");
            if (result == null)
            {
                throw new DecodingException("IMHD chunk not found");
            }
            return result;
        }

        public static ObjectInfo GetObjectInfo(Chunk chunk)
        {
            ObjectInfo info = new ObjectInfo();
            Chunk headerChunk = GetObjectImageHeader(chunk);
            SRFile file = chunk.File;

            file.Position = headerChunk.Offset + 8;
            uint mmucusVersion = file.ReadU16LE();

            if (headerChunk.Size >= 34 && mmucusVersion == 730)
            {
                // SCUMM 7
                file.Position = headerChunk.Offset + 12;
                info.Version = 7;
                info.Name = "";
                info.Id = file.ReadU16LE();
                info.ImageCount = file.ReadU16LE();
                file.ReadU16LE();
                file.ReadU16LE();
                info.X = file.ReadU16LE();
                info.Y = file.ReadU16LE();
                info.Width = file.ReadU16LE();
                info.Height = file.ReadU16LE();
            }
            else if (headerChunk.Size == 24)
            {
                // SCUMM 5
                file.Position = headerChunk.Offset + 8;
                info.Version = 5;
                info.Name = "";
                info.Id = file.ReadU16LE();
                info.ImageCount = file.ReadU16LE();
                file.ReadU16LE();
                file.ReadU16LE();
                info.X = file.ReadU16LE();
                info.Y = file.ReadU16LE();
                info.Width = file.ReadU16LE();
                info.Height = file.ReadU16LE();
            }
            else
            {
                mmucusVersion = 0;
                if (headerChunk.Size >= 52)
                {
                    file.Position = headerChunk.Offset + 48;
                    mmucusVersion = file.ReadU32LE();
                }
                if (mmucusVersion == 801)
                {
                    file.Position = headerChunk.Offset + 8;
                    info.Version = 8;
                    info.Name = file.ReadStringZ();
                    file.Position = headerChunk.Offset + 52;
                    info.Id = -1;
                    info.ImageCount = file.ReadU32LE();
                    info.X = file.ReadS32LE();
                    info.Y = file.ReadS32LE();
                    info.Width = file.ReadS32LE();
                    info.Height = file.ReadS32LE();
                }
                else
                {
                    // SCUMM 6
                    file.Position = headerChunk.Offset + 8;
                    info.Version = 6;
                    info.Name = "";
                    info.Id = file.ReadU16LE();
                    info.ImageCount = file.ReadU16LE();
                    file.ReadU16LE();
                    file.ReadU16LE();
                    info.X = file.ReadU16LE();
                    info.Y = file.ReadU16LE();
                    info.Width = file.ReadU16LE();
                    info.Height = file.ReadU16LE();
                }
            }

            return info;
        }
    }
}
