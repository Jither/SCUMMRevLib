using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Decompilers.SCUMM
{
    public enum SCUMMParameterType
    {
        Var,
        BitVar,
        LocVar,
        Number,
        Array,
        String,
        Command,
    }

    public class SCUMMParameter
    {
        public SCUMMParameterType Type { get; set; }
        public object Value { get; set; }
        public SCUMMParameter Index { get; set; }

        public SCUMMParameter(SCUMMParameterType type, object value, SCUMMParameter index = null)
        {
            Type = type;
            Value = value;
            Index = index;
        }

        public override string ToString()
        {
            string result;
            switch (Type)
            {
                case SCUMMParameterType.Var:
                    result = "var" + Value;
                    break;
                case SCUMMParameterType.BitVar:
                    result = "bit" + Value;
                    break;
                case SCUMMParameterType.LocVar:
                    result = "local" + Value;
                    break;
                case SCUMMParameterType.Number:
                    result = Value.ToString();
                    break;
                case SCUMMParameterType.String:
                    result = "\"" + Value + "\"";
                    break;
                case SCUMMParameterType.Array:
                    SCUMMParameter[] arr = Value as SCUMMParameter[];
                    string list = String.Empty;
                    foreach (SCUMMParameter prm in arr)
                    {
                        if (list != String.Empty)
                        {
                            list += ", ";
                        }
                        list += prm;
                    }
                    result = "[" + list + "]";
                    break;
                case SCUMMParameterType.Command:
                    result = Value + "()";
                    break;
                default:
                    result = "<unknown>";
                    break;
            }
            
            if (Index != null)
            {
                result = String.Format("{0}[{1}]", result, Index);
            }
            
            return result;
        }
    }
}
