using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Utils
{
    public class FileTypeInfo
    {
        public string Description { get; protected set; }
        public List<string> Extensions { get; protected set; }

        public FileTypeInfo(string name, params string[] extensions)
        {
            Description = name;
            Extensions = extensions.ToList();
        }

        public override string ToString()
        {
            return String.Format("{0}|{1}", Description, String.Join(";", Extensions));
        }

        public void AddExtension(string ext)
        {
            if (!Extensions.Contains(ext))
            {
                Extensions.Add(ext);
            }
        }

        public void SortExtensions()
        {
            Extensions.Sort();
        }
    }
}
