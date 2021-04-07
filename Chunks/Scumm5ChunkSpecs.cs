using System.Collections.Generic;

namespace SCUMMRevLib.Chunks
{
    public static class SCUMM5ChunkSpecs
    {
        private static readonly Dictionary<string, SCUMM5ChunkSpec> specs;
        public static SCUMM5ChunkSpec Unknown;

        static SCUMM5ChunkSpecs()
        {
            Unknown = new SCUMM5ChunkSpec("????", "Unknown Block", false, SizeFormat.BasedOnParent, ImageIndex.Unknown);
            specs = new Dictionary<string, SCUMM5ChunkSpec>();

            Add("LECF", "LucasArts Entertainment Company File", true, ImageIndex.FolderScumm);
            Add("LOFF", "Room Offset Table", false, ImageIndex.Table);
            Add("LFLF", "LucasArts File Format", true, ImageIndex.FolderScumm);
            
            Add("ROOM", "Room", true, ImageIndex.Room);
            Add("RMHD", "Room Header" , true, ImageIndex.RoomInfo);
            Add("CYCL", "Color Cycle Parameters" , true, ImageIndex.Cycle);
            Add("TRNS", "Transparency" , true, ImageIndex.Transparency);
            Add("EPAL", "EGA Palette" , true, ImageIndex.Palette);
            Add("BOXD", "Box Description" , false, ImageIndex.Box);
            Add("BOXM", "Box Matrix" , false, ImageIndex.BoxMatrix);
            Add("CLUT", "Color LookUp Table" , true, ImageIndex.Palette);
            Add("SCAL", "Scaling Parameters" , true, ImageIndex.Scale);
            Add("PALS", "Palettes" , true, ImageIndex.FolderPalettes);
            Add("APAL", "A? Palette", true, ImageIndex.Palette);
            Add("WRAP", "Wrapper" , true, ImageIndex.Folder);
            Add("OFFS", "Wrapper Offsets" , true, ImageIndex.Table);
            Add("RMIM", "Room Image" , true, ImageIndex.RoomImage);
            Add("OBIM", "Object Image" , true, ImageIndex.ObjectImage);
            Add("RMSC", "Room Scripts" , true, ImageIndex.FolderScripts);
            Add("RMIH", "Room Image Header" , true, ImageIndex.RoomImageInfo);
            Add("IMHD", "Image Header" , true, ImageIndex.ImageInfo);
            Add("IM00", "Image" , true, ImageIndex.Image);
            Add("IM01", "Image" , true, ImageIndex.Image);
            Add("IM02", "Image" , true, ImageIndex.Image);
            Add("IM03", "Image" , true, ImageIndex.Image);
            Add("IM04", "Image" , true, ImageIndex.Image);
            Add("IM05", "Image" , true, ImageIndex.Image);
            Add("IM06", "Image" , true, ImageIndex.Image);
            Add("IM07", "Image" , true, ImageIndex.Image);
            Add("IM08", "Image" , true, ImageIndex.Image);
            Add("IM09", "Image", true, ImageIndex.Image);
            Add("IM0A", "Image", true, ImageIndex.Image);
            Add("IM0B", "Image", true, ImageIndex.Image);
            Add("IM0C", "Image", true, ImageIndex.Image);
            Add("IM0D", "Image", true, ImageIndex.Image);
            Add("IM0E", "Image", true, ImageIndex.Image);
            Add("IM0F", "Image", true, ImageIndex.Image);
            Add("IMAG", "Image", true, ImageIndex.Image);
            Add("SMAP", "Pixelmap" , true, ImageIndex.Pixelmap);
            Add("BSTR", "Blast [something]" , true, ImageIndex.Pixelmap);
            Add("ZPLN", "Z Plane" , true, ImageIndex.ZBuffer);
            Add("ZSTR", "Z [something]", true, ImageIndex.ZBuffer);
            Add("BOMP", "Blast Object [Map?]" , true, ImageIndex.Pixelmap);
            Add("ZP00", "Z Plane", true, ImageIndex.ZBuffer);
            Add("ZP01", "Z Plane", true, ImageIndex.ZBuffer);
            Add("ZP02", "Z Plane", true, ImageIndex.ZBuffer);
            Add("ZP03", "Z Plane", true, ImageIndex.ZBuffer);
            Add("ZP04", "Z Plane", true, ImageIndex.ZBuffer);
            Add("OBCD", "Object Code" , true, ImageIndex.FolderScripts);
            Add("CDHD", "Code Header" , true, ImageIndex.ScriptInfo);
            Add("VERB", "Verbs" , true, ImageIndex.Verb);
            Add("OBNA", "Object Name" , true, ImageIndex.Text);
            Add("EXCD", "Exit Code" , true, ImageIndex.RoomExit);
            Add("ENCD", "Entry Code" , true, ImageIndex.RoomEntry);
            Add("NLSC", "Number of Local Scripts" , true, ImageIndex.ScriptInfo);
            Add("LSCR", "Local Script" , true, ImageIndex.Script);
            Add("SCRP", "Script" , true, ImageIndex.Script);
            
            Add("SOUN", "Sound" , true, ImageIndex.FolderSounds);
            Add("SOU ", "Sound", true, SizeFormat.BasedOnParent, ImageIndex.FolderSounds);
            Add("ADL ", "Adlib MIDI", false, SizeFormat.WithoutHeader, ImageIndex.Midi);
            Add("SDL ", "SoundBlaster Wave File", false, SizeFormat.WithoutHeader, ImageIndex.DigitalSound);
            Add("ROL ", "Roland MT-32 MIDI", false, SizeFormat.WithoutHeader, ImageIndex.Midi);
            Add("SPK ", "PC Speaker MIDI", false, SizeFormat.WithoutHeader, ImageIndex.Midi);
            Add("GMD ", "General MIDI", false, SizeFormat.WithoutHeader, ImageIndex.Midi);
            Add("AMI ", "Amiga MIDI", false, SizeFormat.WithoutHeader, ImageIndex.Midi);
            Add("MIDI", "General MIDI", false, SizeFormat.WithoutHeader, ImageIndex.Midi);
            Add("VCTL", "Voice - Creative Labs File", true, ImageIndex.DigitalSound);
            Add("VTTL", "Voice", true, ImageIndex.DigitalSound);
            Add("VTLK", "Voice [talk?]", true, ImageIndex.Lips);
            Add("Crea", "Creative Labs Voice File", true, SizeFormat.Creative, ImageIndex.Wave);
            
            Add("AKOS", "Actor Costume" , true, ImageIndex.Costume);
            Add("AKHD", "Actor Header" , true, ImageIndex.CostumeInfo);
            Add("AKPL", "Actor Palette" , true, ImageIndex.Palette);
            Add("RGBS", "RGB Values" , true, ImageIndex.Table);
            Add("AKSQ", "Actor Sequences" , true, ImageIndex.Script);
            Add("AKCD", "Actor Chore Data" , true, ImageIndex.Table);
            Add("AKOF", "Actor Chore Offsets" , true, ImageIndex.Table);
            Add("AKCI", "Actor Chore Info" , true, ImageIndex.Table);
            Add("AKCH", "Actor Chores" , true, ImageIndex.Costume);
            Add("COST", "Costume" , true, ImageIndex.Costume);
            Add("CHAR", "Character set" , true, ImageIndex.CharacterSet);
            
            Add("SAVE", "Savegame" , true, ImageIndex.Save);
            Add("SG09", "Grim Fandango Savegame" , true, ImageIndex.Save);
            Add("VARS", "Savegame Variables" , true, ImageIndex.Vars);
            Add("SNDD", "Savegame Sound [something]" , true, ImageIndex.IMuse);
            Add("GLOB", "Savegame Global" , true, ImageIndex.Globals);
            Add("USED", "Savegame [something]" , true, ImageIndex.Used);
            Add("IMUS", "Savegame iMUSE State" , true, ImageIndex.IMuse);
            Add("MSCR", "Savegame [something]" , true, ImageIndex.Table);
            
            Add("RNAM", "Room Names" , true, ImageIndex.Text);
            Add("MAXS", "Maxima" , true, ImageIndex.Max);
            Add("DROO", "Directory of Rooms" , true, ImageIndex.DirectoryRooms);
            Add("DRSC", "Directory of Room Scripts?", true, ImageIndex.DirectoryScripts);
            Add("DSCR", "Directory of Scripts", true, ImageIndex.DirectoryScripts);
            Add("DSOU", "Directory of Sounds" , true, ImageIndex.DirectorySounds);
            Add("DCOS", "Directory of Costumes" , true, ImageIndex.DirectoryCostumes);
            Add("DCHR", "Directory of Character Sets" , true, ImageIndex.DirectoryCharacters);
            Add("DOBJ", "Directory of Objects" , true, ImageIndex.DirectoryObjects);
            Add("AARY", "Array?" , true, ImageIndex.Table);
            Add("ANAM", "Name?" , true, ImageIndex.Text);
            
            Add("FLUP", "[something] Lookup" , true, ImageIndex.Unknown);
            
            // These should probably all be padded...
            Add("ANIM", "SMUSH Animation File", true, SizeFormat.WithoutHeader, ImageIndex.FolderMovies);
            Add("AHDR", "SMUSH Animation Header", true, SizeFormat.WithoutHeader, ImageIndex.AnimationInfo);
            Add("FRME", "SMUSH Animation Frame", true, SizeFormat.WithoutHeader, ImageIndex.FolderMovies);
            Add("FTCH", "SMUSH Fetch Command", true, SizeFormat.WithoutHeader, ImageIndex.Unknown);
            Add("STOR", "SMUSH Store Flag", true, SizeFormat.WithoutHeader, ImageIndex.Unknown);
            Add("FOBJ", "SMUSH Frame Object", true, SizeFormat.WithoutHeaderPadded, ImageIndex.Movie);
            Add("FRME/TEXT", "SMUSH Text", true, SizeFormat.WithoutHeaderPadded, ImageIndex.Text);
            Add("IACT", "SMUSH iMUSE [something]", true, SizeFormat.WithoutHeaderPadded, ImageIndex.IMuse);
            Add("NPAL", "SMUSH New Palette", true, SizeFormat.WithoutHeader, ImageIndex.Palette);
            Add("XPAL", "SMUSH Transition Palette", true, SizeFormat.WithoutHeader, ImageIndex.Palette);
            Add("PSAD", "Audio?", true, SizeFormat.WithoutHeaderPadded, ImageIndex.DigitalSound);
            Add("SAUD", "SMUSH Audio?", true, SizeFormat.BasedOnParent, ImageIndex.DigitalSound);
            Add("STRK", "SMUSH Audio Track?", true, SizeFormat.WithoutHeader, ImageIndex.DigitalSound);
            Add("SDAT", "SMUSH Audio Data?", true, SizeFormat.BasedOnParent, ImageIndex.Wave);
            Add("TRES", "SMUSH Text Resource", true, SizeFormat.WithoutHeader, ImageIndex.Text);
            
            Add("SANM", "SMUSH Animation", true, SizeFormat.WithoutHeader, ImageIndex.FolderMovies);
            Add("SHDR", "SMUSH Header", true, SizeFormat.WithoutHeader, ImageIndex.AnimationInfo);
            Add("FLHD", "SMUSH FLObject Header?", true, SizeFormat.WithoutHeader, ImageIndex.Movie);
            Add("Bl16", "SMUSH Blocky16?", true, SizeFormat.WithoutHeader, ImageIndex.Movie);
            Add("Wave", "SMUSH Wave", true, SizeFormat.WithoutHeader, ImageIndex.Wave);
            Add("ANNO", "SMUSH Annotation", true, SizeFormat.WithoutHeader, ImageIndex.Text);

            Add("MCMP", "Compression Map", true, SizeFormat.MCMP, ImageIndex.CompressionTable);
            Add("COMP", "Compression Map", true, SizeFormat.COMP, ImageIndex.CompressionTable);
            Add("iMUS", "iMUSE Digital Sound", true, SizeFormat.BasedOnParent, ImageIndex.IMuse);
            Add("MAP ", "iMUSE Map", true, SizeFormat.WithoutHeader, ImageIndex.Map);
            Add("FRMT", "iMUSE Sound Format", true, SizeFormat.WithoutHeader, ImageIndex.Table);
            Add("JUMP", "iMUSE Jump Hook", true, SizeFormat.WithoutHeader, ImageIndex.IMuse);
            Add("REGN", "iMUSE Region", true, SizeFormat.WithoutHeader, ImageIndex.IMuse);
            Add("MAP/TEXT", "iMUSE Text", true, SizeFormat.WithoutHeader, ImageIndex.Text);
            Add("STOP", "iMUSE Stop Hook", true, SizeFormat.WithoutHeader, ImageIndex.IMuse);
            Add("SYNC", "iMUSE Voice Synchronization", true, SizeFormat.WithoutHeader, ImageIndex.IMuse);
            Add("DATA", "iMUSE Sound Data", true, SizeFormat.BasedOnParent, ImageIndex.Wave);

            Add("PLIB", "[something] Library?", true, ImageIndex.Unknown);
            Add("LHDR", "Library Header?", true, ImageIndex.Unknown);
            Add("PDAT", "[something] Data?", true, ImageIndex.Unknown);
            Add("PTCH", "?", true, ImageIndex.Unknown);
            Add("PHDR", "?", true, ImageIndex.Unknown);
            Add("RCNE", "Encrypted Language file", true, ImageIndex.Text);

            // Behind the Magic:
            Add("BUND", "Behind the Magic Bundle", true, SizeFormat.WithoutHeader, ImageIndex.Bundle);
            Add("BNHD", "Bundle Header", false, SizeFormat.WithoutHeader, ImageIndex.Table);
            Add("BNDT", "Bundle Data", false, SizeFormat.WithoutHeader, ImageIndex.Bundle);
        }

        public static SCUMM5ChunkSpec GetSpec(string key, Chunk parent)
        {
            switch (key)
            {
                // TEXT Chunk may appear in both SMUSH and iMUSE - with different specs:
                case "TEXT":
                    key = parent.Name == "FRME" ? "FRME/TEXT" : "MAP/TEXT";
                    break;
            }

            if (specs.ContainsKey(key))
            {
                return specs[key];
            }
            return Unknown;
        }

        private static void Add(string id, string description, bool hasChildren, ImageIndex imageIndex)
        {
            specs.Add(id, new SCUMM5ChunkSpec(id, description, hasChildren, SizeFormat.Standard, imageIndex));
        }

        private static void Add(string id, string description, bool hasChildren, SizeFormat sizeFormat, ImageIndex imageIndex)
        {
            specs.Add(id, new SCUMM5ChunkSpec(id, description, hasChildren, sizeFormat, imageIndex));
        }

    }
}
