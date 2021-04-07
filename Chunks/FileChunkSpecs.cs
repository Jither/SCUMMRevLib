using System;
using System.Collections.Generic;

namespace SCUMMRevLib.Chunks
{
    public static class FileChunkSpecs
    {
        private static readonly Dictionary<string, FileChunkSpec> specs;
        public static FileChunkSpec Unknown;

        public static FileChunkSpec GetSpec(string key)
        {
            if (specs.ContainsKey(key))
            {
                return specs[key];
            }
            return Unknown;
        }

        static FileChunkSpecs()
        {
            Unknown = new FileChunkSpec("???", "Unknown file", ImageIndex.Unknown);

            specs = new Dictionary<string, FileChunkSpec>();

            // File chunk type IDs should be LOWER CASE!!!
            Add(".imu", "Compressed(?) iMUSE Cue", ImageIndex.FolderSounds);
            Add(".imx", "Compressed iMUSE Cue", ImageIndex.FolderSounds);
            Add(".imc", "Compressed iMUSE Cue", ImageIndex.FolderSounds);
            Add(".wav", "(Compressed) WAV File", ImageIndex.FolderSounds);
            Add(".lip", "Lip Synchronization File", ImageIndex.Lips);
            Add(".3do", "3D Object File", ImageIndex.ThreeD);
            Add(".cos", "Costume File", ImageIndex.Costume);
            Add(".mat", "Material File", ImageIndex.Material);
            Add(".key", "Keyframe File", ImageIndex.FolderKeys);
            Add(".bm", "Bitmap File", ImageIndex.FolderImages);
            Add(".zbm", "Z-Buffer File", ImageIndex.ZBuffer);
            Add(".cmp", "Color Map File", ImageIndex.FolderPalettes);
            Add(".set", "Set File (Room)", ImageIndex.Room);
            Add(".txt", "Plain Text File", ImageIndex.Text);
            Add(".snm", "SMUSH Animation File", ImageIndex.FolderMovies);
            Add(".lua", "(Compiled) LUA Script File", ImageIndex.FolderScripts);
            Add(".laf", "LucasArts Font File", ImageIndex.CharacterSet);
            
            Add(".s", "Behind the Magic Script File", ImageIndex.FolderScripts);
            Add(".slb", "SweetView Script File", ImageIndex.FolderScripts);
            Add(".jpg", "JPEG Image File", ImageIndex.Image);
            Add(".sad", "SMUSH Audio File", ImageIndex.FolderSounds);
            Add(".znm", "ZLib Compressed SMUSH Animation File", ImageIndex.FolderMovies);
            Add(".csi", "?", ImageIndex.Unknown);

            Add(".gcf", "Font Info?", ImageIndex.CharacterSet);
            Add(".spr", "Font Info?", ImageIndex.CharacterSet);
            Add(".bmp", "Bitmap", ImageIndex.Image);
            Add(".wm", "Voice File", ImageIndex.FolderSounds);
            Add(".baf", "3D Model File?", ImageIndex.ThreeD);
            Add(".cosb", "Binary Costume File", ImageIndex.Costume);
            Add(".tga", "TrueVision Targa Image File", ImageIndex.Image);
            Add(".meshb", "Binary 3D Mesh File", ImageIndex.ThreeD);
            Add(".setb", "Binary Set File (Room)", ImageIndex.Room);
            Add(".sklb", "Binary Skeleton File", ImageIndex.ThreeD);
            Add(".aif", "Audio Interchange File", ImageIndex.DigitalSound);
            Add(".sur", "Surface File?", ImageIndex.Material);
            Add(".anim", "Animation Key File?", ImageIndex.Keyframe);
            Add(".animb", "Binary Animation Key File?", ImageIndex.Keyframe);
            Add(".til", "BEEGEE Tile (background)", ImageIndex.Image);
            Add(".tab", "Language Table", ImageIndex.Table);
            Add(".ttf", "TrueType Font", ImageIndex.CharacterSet);
            Add(".wvc", "VIMA Compressed Audio File", ImageIndex.DigitalSound);
            Add(".sprb", "Binary Sprite File?", ImageIndex.CharacterSet);
            Add(".scx", "Audio File [something]", ImageIndex.DigitalSound);

            Add(".csv", "Strange CSV File", ImageIndex.Text);
            Add(".xml", "Strange XML File", ImageIndex.Text);
            Add(".png", "Portable Network Graphics File", ImageIndex.Image);
            Add(".dxt", "DirectX Texture File", ImageIndex.Image);
            Add(".font", "Font File", ImageIndex.CharacterSet);
            Add(".fx", "Shader effects definition file", ImageIndex.Palette);

            // Telltale
            Add(".d3dmesh", "Direct3D Mesh", ImageIndex.ThreeD);
            Add(".d3dtx", "Direct3D Texture", ImageIndex.ObjectImage);
            Add(".chore", "Chore", ImageIndex.Keyframe);
            Add(".lenc", "Encrypted LUA", ImageIndex.Script);
            Add(".landb", "Language DB", ImageIndex.Table);
            Add(".anm", "Animation", ImageIndex.AnimationInfo);
            Add(".t3fxb", "Shader", ImageIndex.Material);
            Add(".scene", "Scene", ImageIndex.RoomInfo);
            Add(".prop", "Prop", ImageIndex.ObjectImage);
            Add(".wbox", "Walking Boxes", ImageIndex.Box);
            Add(".aud", "Audio file", ImageIndex.DigitalSound);
            Add(".fsb", "FMOD Sound Bank", ImageIndex.DigitalSound);
            Add(".vox", "Voice audio file", ImageIndex.DigitalSound);
        }

        private static void Add(string extension, string description, ImageIndex imageIndex)
        {
            specs.Add(extension, new FileChunkSpec(extension, description, imageIndex));
        }
    }
}
