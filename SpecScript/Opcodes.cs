using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.SpecScript
{
    public class Opcodes
    {
        public static byte END = 0;

        public static byte PUSH_VARIABLE = 1;
        public static byte PUSH_INTEGER = 2;
        public static byte PUSH_STRING = 3;

        public static byte ADD = 20;
        public static byte SUB = 21;
        public static byte MUL = 22;
        public static byte DIV = 23;
        public static byte MOD = 24;
        public static byte LOR = 25;
        public static byte LAND = 26;
        public static byte BOR = 27;
        public static byte BAND = 28;
        public static byte NOT = 29;
        public static byte EQ = 40;
        public static byte NEQ = 41;
        public static byte LT = 42;
        public static byte GT = 43;
        public static byte LEQ = 44;
        public static byte GEQ = 45;

        public static byte DECLARE_VARIABLE = 50;
        public static byte ASSIGN = 51;
        
        public static byte CALL = 100;
        public static byte IF_NOT = 101;
        public static byte JUMP = 103;
        
        public static byte OUTPUT = 200;
        public static byte OFFSET = 201;

        public static byte READ_LE_U = 220;
        public static byte READ_LE_S = 221;
        public static byte READ_BE_U = 222;
        public static byte READ_BE_S = 223;
    }
}
