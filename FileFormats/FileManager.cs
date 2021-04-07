using System;
using System.Collections.Generic;
using SCUMMRevLib.FileFormats.Factories;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.FileFormats
{
    public class FileManager : IFileManager
    {
        protected List<FileFactory> fileFactories;
        public FileTypeInfos FileTypeInfos { get; protected set; }

        public FileManager()
        {
            PopulateFileTypes();
        }

        private void PopulateFileTypes()
        {
            fileFactories = new List<FileFactory>
                              {
                                  new LB83Factory(),
                                  new LABNFactory(), 
                                  new KAPLFactory(), 
                                  new BUNDFactory(),
                                  new ForgeFactory(),
                                  new TTARCH2Factory(),
                                  // SCUMM 3 and 5 files are very general (they'll accept anything with ASCII 
                                  // chars at the right spot), so check more specific formats first
                                  new SCUMM3Factory(),
                                  new SCUMM5Factory(),
                                  new SCUMM1Factory(),
                                  // ttarch needs decryption checks for determining file format, so leave it for last
                                  new TTARCHFactory(),
                                  new UnknownFactory()
                              };

            // Build file type info (for e.g. Open dialog):
            FileTypeInfos = new FileTypeInfos();

            Type attrType = typeof(FileTypeAttribute);
            FileTypeInfo allSupported = new FileTypeInfo("All supported files");
            FileTypeInfos.Add(allSupported);

            foreach (var factory in fileFactories)
            {
                var type = factory.GetType();
                object[] attrs = type.GetCustomAttributes(attrType, false);

                foreach (object attr in attrs)
                {
                    FileTypeAttribute ftAttr = attr as FileTypeAttribute;
                    if (ftAttr == null) continue;

                    string desc = ftAttr.Description;
                    FileTypeInfo info = FileTypeInfos.GetOrCreate(desc);

                    foreach (string ext in ftAttr.Extensions)
                    {
                        info.AddExtension("*." + ext);
                        allSupported.AddExtension("*." + ext);
                    }

                    info.SortExtensions();
                }
            }

            allSupported.SortExtensions();
            
            FileTypeInfo allFiles = new FileTypeInfo("All files");
            allFiles.AddExtension("*.*");
            FileTypeInfos.Add(allFiles);
        }

        public SRFile OpenFile(string path)
        {
            foreach (FileFactory factory in fileFactories)
            {
                SRFile file = factory.Create(path);
                if (file != null)
                {
                    return file;
                }
            }

            return null;
        }
    }
}
