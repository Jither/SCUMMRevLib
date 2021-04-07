using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Utils
{
    public class FileTypeInfos : List<FileTypeInfo>
    {
        public FileTypeInfo this[string desc]
        {
            get 
            {
                var types = from ft in this where ft.Description == desc select ft;
                return types.SingleOrDefault();
            }
        }

        public bool HasDescription(string desc)
        {
            var types = from ft in this where ft.Description == desc select ft;
            return types.Count() > 0;
        }

        public FileTypeInfo GetOrCreate(string desc)
        {
            if (HasDescription(desc))
            {
                return this[desc];
            }
            else
            {
                FileTypeInfo info = new FileTypeInfo(desc);
                Add(info);
                return info;
            }
        }
    }
}
