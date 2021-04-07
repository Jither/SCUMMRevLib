using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.FileFormats
{
    public class D64File
    {
        // Standard CBM-1541 sector track layout (see http://en.wikipedia.org/wiki/Commodore_1541)
        private static int[] DISK_SECTOR_TRACKS = 
        {
            // 21 track sectors
	        0, 21, 42, 63, 84, 105, 126, 147, 168, 189, 210, 231, 252, 273, 294, 315, 336,
            // 19 track sectors
	        357, 376, 395, 414, 433, 452, 471,
            // 18 track sectors
	        490, 508, 526, 544, 562, 580,
            // 17 track sectors
	        598, 615, 632, 649, 666
        };
    }
}
