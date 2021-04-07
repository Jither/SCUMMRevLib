using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Utils
{
    public static class StringExtensions
    {
        public static string[] SplitAtFirst(this string str, char separator)
        {
            return str.Split(new []{'_'}, 2);
        }
    }
}
