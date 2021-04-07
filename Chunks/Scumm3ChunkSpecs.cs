using System.Collections.Generic;

namespace SCUMMRevLib.Chunks
{
    public static class SCUMM3ChunkSpecs
    {
        private static readonly Dictionary<string, SCUMM3ChunkSpec> specs;
        public static SCUMM3ChunkSpec Unknown;

        static SCUMM3ChunkSpecs()
        {
            Unknown = new SCUMM3ChunkSpec("??", "Unknown Block", false, 0, ImageIndex.Unknown);
            specs = new Dictionary<string, SCUMM3ChunkSpec>();

            Add("LE", "LucasArts Entertainment Company File", true, ImageIndex.FolderScumm);
            Add("FO", "File Offsets", true, ImageIndex.Table);
            Add("LF", "LucasArts File Format", true, 8, ImageIndex.FolderScumm);
            Add("RO", "Room", true, ImageIndex.Room);
            Add("HD", "Room Header", true, ImageIndex.RoomInfo);
            Add("CC", "Color Cycle Parameters", true, ImageIndex.Cycle);
            Add("SP", "?", true, ImageIndex.Unknown);
            Add("BX", "Box Description and Matrix", true, ImageIndex.Box);
            Add("BM", "Bitmap", true, ImageIndex.RoomImage);
            Add("PA", "Palette", true, ImageIndex.Palette);
            Add("SA", "Unknown", false, ImageIndex.Unknown);
            Add("OI", "Object Image", true, ImageIndex.ObjectImage);
            Add("SO", "Sound", true, ImageIndex.FolderSounds);
            Add("WA", "Wave", true, ImageIndex.DigitalSound);
            Add("AD", "Adlib", true, ImageIndex.Midi);
            Add("SC", "Script", true, ImageIndex.Script);
            Add("NL", "Number of Local Scripts?", true, ImageIndex.ScriptInfo);
            Add("SL", "Local Script?", true, ImageIndex.Script);
            Add("OC", "Object Code?", true, ImageIndex.Script);
            Add("EN", "Entry Script", true, ImageIndex.RoomEntry);
            Add("EX", "Exit Script", true, ImageIndex.RoomExit);
            Add("LC", "Number of Local Scripts", true, ImageIndex.ScriptInfo);
            Add("LS", "Local Script", true, ImageIndex.Script);
            Add("CO", "Costume", true, ImageIndex.Costume);
            Add("AM", "Some Amiga Specific Chunk", true, ImageIndex.Amiga);
            Add("RN", "Room Names", true, ImageIndex.Table);
            Add("0R", "Directory of Rooms", true, ImageIndex.DirectoryRooms);
            Add("0S", "Directory of Scripts", true, ImageIndex.DirectoryScripts);
            Add("0N", "Directory of Sounds", true, ImageIndex.DirectorySounds);
            Add("0C", "Directory of Costumes", true, ImageIndex.DirectoryCostumes);
            Add("0O", "Directory of Objects", true, ImageIndex.DirectoryObjects);
        }

        public static SCUMM3ChunkSpec GetSpec(string key, Chunk parent)
        {
            if (specs.ContainsKey(key))
            {
                return specs[key];
            }
            return Unknown;
        }

        private static void Add(string id, string description, bool hasChildren, ImageIndex imageIndex)
        {
            specs.Add(id, new SCUMM3ChunkSpec(id, description, hasChildren, 6, imageIndex));
        }

        private static void Add(string id, string description, bool hasChildren, uint childOffset, ImageIndex imageIndex)
        {
            specs.Add(id, new SCUMM3ChunkSpec(id, description, hasChildren, childOffset, imageIndex));
        }

    }
}
