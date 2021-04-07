using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Decompilers.SCUMM
{
    public class SCUMMCommand
    {
        public ulong Offset { get; set; }
        public SCUMMOpcode Opcode { get; protected set; }
        public SCUMMParameter[] Parameters { get; protected set; }

        public SCUMMCommand(SCUMMOpcode opcode, params SCUMMParameter[] parameters)
        {
            Opcode = opcode;
            Parameters = parameters;
        }

        public override string ToString()
        {
            string[] parts = Opcode.ToString().SplitAtFirst('_');
            string type;
            string name;
            if (parts.Length < 2)
            {
                type = "";
                name = parts[0];
            }
            else
            {
                type = parts[0];
                name = parts[1];
            }
            
            return String.Format("{0,5}  {1,-2}  {2,-24} {3}", Offset, type, name, String.Join(", ", (object[])Parameters));
        }
    }
}
