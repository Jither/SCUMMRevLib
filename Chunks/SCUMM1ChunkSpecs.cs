using System.Collections.Generic;

namespace SCUMMRevLib.Chunks
{
    public static class SCUMM1ChunkSpecs
    {
        public static readonly string RoomName = "ROOMv1";
        public static readonly string SoundName = "SOUNv1";
        public static readonly string CostumeName = "COSTv1";
        public static readonly string ScriptName = "SCRPv1";

        private static readonly Dictionary<string, SCUMM1ChunkSpec> specs;
        public static SCUMM1ChunkSpec Unknown;

        static SCUMM1ChunkSpecs()
        {
            Unknown = new SCUMM1ChunkSpec("????v1", "Unknown Block", false, ImageIndex.Unknown);
            specs = new Dictionary<string, SCUMM1ChunkSpec>();

            Add(RoomName, "Room", true, ImageIndex.Room);
            Add("EXCDv1", "Exit Script", false, ImageIndex.RoomExit);
            Add("ENCDv1", "Entry Script", false, ImageIndex.RoomEntry);
            Add("RMHDv1", "Room Header", false, ImageIndex.RoomInfo);
            Add("RMCHv1", "Room Image Characters", false, ImageIndex.RoomImageInfo);
            Add("RMIMv1", "Room Image", false, ImageIndex.RoomImage);
            Add("ZPLNv1", "Z-Plane", false, ImageIndex.ZBuffer);
            Add("ZPCHv1", "Z-Plane Characters", false, ImageIndex.ZBuffer);
            Add("BOXDv1", "Walking Boxes Definition", false, ImageIndex.Box);
            Add("BOXMv1", "Walking Boxes Matrix", false, ImageIndex.BoxMatrix);
            Add("CLUTv1", "Palette", false, ImageIndex.Palette);
            Add("OBIMv1", "Object Image", false, ImageIndex.ObjectImage);
            Add(SoundName, "Sound", false, ImageIndex.DigitalSound);
            Add("OBCDv1", "Object", false, ImageIndex.DirectoryObjects);
            Add(ScriptName, "Script", false, ImageIndex.Script);
            Add(CostumeName, "Costume", false, ImageIndex.Costume);
        }

        public static SCUMM1ChunkSpec GetSpec(string key, Chunk parent)
        {
            if (specs.ContainsKey(key))
            {
                return specs[key];
            }
            return Unknown;
        }

        private static void Add(string id, string description, bool hasChildren, ImageIndex imageIndex)
        {
            specs.Add(id, new SCUMM1ChunkSpec(id, description, hasChildren, imageIndex));
        }
    }
}
