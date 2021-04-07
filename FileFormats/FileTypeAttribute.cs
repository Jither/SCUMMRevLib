using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.FileFormats
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FileTypeAttribute : Attribute
    {
        public string Description { get; protected set; }
        public string[] Extensions { get; protected set; }

        public FileTypeAttribute(string description, params string[] extensions)
        {
            this.Description = description;
            this.Extensions = extensions;
        }
    }
}
