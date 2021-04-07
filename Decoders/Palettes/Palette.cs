using System;
using System.Collections.Generic;
using System.Linq;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decoders.Palettes
{
    public class Palette
    {
        private readonly PaletteColor[] entries;

        public int Count
        {
            get
            {
                return entries.Length;
            }
        }
        public PaletteColor this[int index]
        {
            get
            {
                if (index < 0 || index > entries.Length)
                {
                    return PaletteColor.Empty;
                }
                return entries[index];
            }
            set
            {
                if (index < 0 || index > entries.Length)
                {
                    throw new ArgumentOutOfRangeException("index", "Attempt to write palette value outside bounds.");
                }
                entries[index] = value;
            }
        }

        public PaletteColor[] ToArray()
        {
            return entries.Clone() as PaletteColor[];
        }

        public IList<PaletteColor> ToList()
        {
            return entries.ToList();
        }

        public Palette(int size)
        {
            entries = new PaletteColor[size];
        }
    }
}
