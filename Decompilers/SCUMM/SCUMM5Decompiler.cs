using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Decompilers.SCUMM
{
    public class SCUMM5Decompiler : SCUMMDecompiler
    {
        private const byte SO_COSTUME = 0x01;
        private const byte SO_STEP_DIST = 0x02;
        private const byte SO_SOUND = 0x03;
        private const byte SO_WALK_ANIMATION = 0x04;
        private const byte SO_TALK_ANIMATION = 0x05;
        private const byte SO_STAND_ANIMATION = 0x06;
        private const byte SO_ANIMATION = 0x07;
        private const byte SO_DEFAULT = 0x08;
        private const byte SO_ELEVATION = 0x09;
        private const byte SO_ANIMATION_DEFAULT = 0x0a;
        private const byte SO_PALETTE = 0x0b;
        private const byte SO_TALK_COLOR = 0x0c;
        private const byte SO_ACTOR_NAME = 0x0d;
        private const byte SO_INIT_ANIMATION = 0x0e;
        private const byte SO_PALETTE_LIST = 0x0f;
        private const byte SO_ACTOR_WIDTH = 0x10;
        private const byte SO_SCALE = 0x11;
        private const byte SO_NEVER_ZCLIP = 0x12;
        private const byte SO_ALWAYS_ZCLIP = 0x13;
        private const byte SO_IGNORE_BOXES = 0x14;
        private const byte SO_FOLLOW_BOXES = 0x15;
        private const byte SO_ANIMATION_SPEED = 0x16;
        private const byte SO_SHADOW = 0x17;

        protected override void InitOpcodes()
        {
            opcodeHandlers = new Dictionary<int, Func<int, SCUMMCommand>>
                {
                    {0, EndObject},
                    {1, PutActorAt},
                    {2, StartMusic},
                    {3, FActorRoom},
                    {4, IfGt},
                    {5, DrawObject},
                    {6, FActorElevation},
                    {7, SetState},
                    {8, IfEq},
                    {9, FaceTowards},
                    {10, StartScript},
                    {11, FValidVerb},
                    {12, HeapStuff},
                    {13, WalkToActor},
                    {14, PutActorAtObject},
                    {15, FStateOf},
                    {16, FOwner},
                    {17, DoAnimation},
                    {18, CameraPanTo},
                    {19, ActorStuff},
                    {20, SayLine},
                    {21, FFindActor},
                    {22, FRandom},
                    {23, And},
                    {24, Jump},
                    {25, DoSentence},
                    {26, Ass},
                    {27, Mul},
                    {28, StartSound},
                    {29, IfClass},
                    {30, WalkTo},
                    {31, IfInBox},
                    {32, StopMusic},
                    {33, PutActorAt},
                    {34, FAnimationCounter},
                    {35, FActorY},
                    {36, ComeOutDoor},
                    {37, PickUpObject},
                    {38, AssignArray_Byte},
                    {39, StringStuff},
                    {40, IfVar},
                    {41, Owner},
                    {42, StartScript},
                    {43, SleepForVar},
                    {44, Userface},
                    {45, PutActorInRoom},
                    {46, SleepFor},
                    {47, Unknown},
                    {48, SetBox},
                    {49, FInventorySize},
                    {50, CameraAt},
                    {51, RoomStuff},
                    {52, FProximity},
                    {53, FFindObject},
                    {54, WalkToObject},
                    {55, StartObject},
                    {56, IfLt},
                    {57, DoSentence},
                    {58, Sub},
                    {59, FActorScale},
                    {60, StopSound},
                    {61, FFindInventory},
                    {62, WalkTo},
                    {63, DrawBox},
                    {64, CutScene},
                    {65, PutActorAt},
                    {66, ChainScript},
                    {67, FActorX},
                    {68, IfLeq},
                    {69, Unknown},
                    {70, Inc},
                    {71, SetState},
                    {72, IfNeq},
                    {73, FaceTowards},
                    {74, StartScript},
                    {75, FValidVerb},
                    {76, SoundKludge},
                    {77, WalkToActor},
                    {78, PutActorAtObject},
                    {79, Unknown},
                    {80, PickUpObject_3}, // MI1
                    {81, DoAnimation},
                    {82, CameraFollow},
                    {83, ActorStuff},
                    {84, NewName},
                    {85, FFindActor},
                    {86, FActorMoving},
                    {87, Or},
                    {88, OverRide},
                    {89, DoSentence},
                    {90, Add},
                    {91, Div},
                    {92, Fades}, // MI1: Fades
                    {93, SetClass},
                    {94, WalkTo},
                    {95, IfInBox},
                    {96, FreezeScripts},
                    {97, PutActorAt},
                    {98, StopScript},
                    {99, FActorFacing},
                    {100, ComeOutDoor},
                    {101, PickUpObject},
                    {102, FClosestActor},
                    {103, FStringWidth},
                    {104, FScriptRunning},
                    {105, Owner},
                    {106, StartScript},
                    {107, _Debug},
                    {108, FActorWidth},
                    {109, PutActorInRoom},
                    {110, StopObject},
                    {111, Unknown},
                    {112, Lights},
                    {113, FActorCostume},
                    {114, CameraInRoom},
                    {115, RoomStuff},
                    {116, FProximity},
                    {117, FFindObject},
                    {118, WalkToObject},
                    {119, StartObject},
                    {120, IfGeq},
                    {121, DoSentence},
                    {122, NewVerb},
                    {123, FActorBox},
                    {124, FSoundRunning},
                    {125, FFindInventory},
                    {126, WalkTo},
                    {127, DrawBox},
                    {128, BreakHere},
                    {129, PutActorAt},
                    {130, StartMusic},
                    {131, FActorRoom},
                    {132, IfGt},
                    {133, DrawObject},
                    {134, FActorElevation},
                    {135, SetState},
                    {136, IfEq},
                    {137, FaceTowards},
                    {138, StartScript},
                    {139, FValidVerb},
                    {140, HeapStuff},
                    {141, WalkToActor},
                    {142, PutActorAtObject},
                    {143, FStateOf},
                    {144, FOwner},
                    {145, DoAnimation},
                    {146, CameraPanTo},
                    {147, ActorStuff},
                    {148, SayLine},
                    {149, FFindActor},
                    {150, FRandom},
                    {151, And},
                    {152, System},
                    {153, DoSentence},
                    {154, Ass},
                    {155, Mul},
                    {156, StartSound},
                    {157, IfClass},
                    {158, WalkTo},
                    {159, IfInBox},
                    {160, EndScript},
                    {161, PutActorAt},
                    {162, FAnimationCounter},
                    {163, FActorY},
                    {164, ComeOutDoor},
                    {165, PickUpObject},
                    {166, AssignArray_Word},
                    {167, SaveLoadVariables},
                    {168, IfNotVar},
                    {169, Owner},
                    {170, StartScript},
                    {171, VerbSets},
                    {172, AssComplex},
                    {173, PutActorInRoom},
                    {174, WaitForStuff},
                    {175, Unknown},
                    {176, SetBox},
                    {177, FInventorySize},
                    {178, CameraAt},
                    {179, RoomStuff},
                    {180, FProximity},
                    {181, FFindObject},
                    {182, WalkToObject},
                    {183, StartObject},
                    {184, IfLt},
                    {185, DoSentence},
                    {186, Sub},
                    {187, FActorScale},
                    {188, StopSound},
                    {189, FFindInventory},
                    {190, WalkTo},
                    {191, DrawBox},
                    {192, EndCutScene},
                    {193, PutActorAt},
                    {194, ChainScript},
                    {195, FActorX},
                    {196, IfLeq},
                    {197, Unknown},
                    {198, Dec},
                    {199, SetState},
                    {200, IfNeq},
                    {201, FaceTowards},
                    {202, StartScript},
                    {203, FValidVerb},
                    {204, Pseudoroom},
                    {205, WalkToActor},
                    {206, PutActorAtObject},
                    {207, Unknown},
                    {208, Unknown},
                    {209, DoAnimation},
                    {210, CameraFollow},
                    {211, ActorStuff},
                    {212, NewName},
                    {213, FFindActor},
                    {214, FActorMoving},
                    {215, Or},
                    {216, SayLineDefault},
                    {217, DoSentence},
                    {218, Add},
                    {219, Div},
                    {220, Unknown},
                    {221, SetClass},
                    {222, WalkTo},
                    {223, IfInBox},
                    {224, FreezeScripts},
                    {225, PutActorAt},
                    {226, StopScript},
                    {227, FActorFacing},
                    {228, ComeOutDoor},
                    {229, PickUpObject},
                    {230, FClosestActor},
                    {231, FStringWidth},
                    {232, FScriptRunning},
                    {233, Owner},
                    {234, StartScript},
                    {235, _Debug},
                    {236, FActorWidth},
                    {237, PutActorInRoom},
                    {238, StopObject},
                    {239, Unknown},
                    {240, Lights},
                    {241, FActorCostume},
                    {242, CameraInRoom},
                    {243, RoomStuff},
                    {244, FProximity},
                    {245, FFindObject},
                    {246, WalkToObject},
                    {247, StartObject},
                    {248, IfGeq},
                    {249, DoSentence},
                    {250, NewVerb},
                    {251, FActorBox},
                    {252, FSoundRunning},
                    {253, FFindInventory},
                    {254, WalkTo},
                    {255, DrawBox}
                };
        }

        protected SCUMMParameter GetVar()
        {
            return MakeVar(reader.ReadU16LE());
        }

        protected SCUMMParameter MakeVar(int value)
        {
            SCUMMParameter index = null;

            if ((value & 0x2000) != 0)
            {
                int i = reader.ReadU16LE();
                if ((i & 0x2000) != 0)
                {
                    // index is itself a variable
                    index = MakeVar(i & 0xdfff);
                }
                else
                {
                    index = new SCUMMParameter(SCUMMParameterType.Number, i & 0x0fff);
                }

                value = value & 0xdfff;
            }

            if ((value & 0x8000) != 0)
            {
                return new SCUMMParameter(SCUMMParameterType.BitVar, value & 0x7fff, index);
            }

            if ((value & 0x4000) != 0)
            {
                return new SCUMMParameter(SCUMMParameterType.LocVar, value & 0x0fff, index);
            }

            return new SCUMMParameter(SCUMMParameterType.Var, value);
        }

        protected SCUMMParameter GetVarOrWord(int opcode, int mask)
        {
            if ((opcode & mask) != 0)
            {
                // Variable
                return GetVar();
            }
            return new SCUMMParameter(SCUMMParameterType.Number, reader.ReadU16LE());
        }

        protected SCUMMParameter GetVarOrByte(int opcode, int mask)
        {
            if ((opcode & mask) != 0)
            {
                // Variable
                return GetVar();
            }
            return new SCUMMParameter(SCUMMParameterType.Number, reader.ReadU8());
        }

        protected SCUMMParameter GetWord()
        {
            return new SCUMMParameter(SCUMMParameterType.Number, reader.ReadU16LE());
        }

        protected SCUMMParameter GetWordSigned()
        {
            return new SCUMMParameter(SCUMMParameterType.Number, reader.ReadS16LE());
        }

        protected SCUMMParameter GetByte()
        {
            return new SCUMMParameter(SCUMMParameterType.Number, reader.ReadU8());
        }

        protected SCUMMParameter GetParams(int maxParams, string cmd)
        {
            List<SCUMMParameter> result = new List<SCUMMParameter>();
            int count = 0;
            while (true)
            {
                byte so = reader.ReadU8();
                if (so == 0xff) break;
                result.Add(GetVarOrWord(so, 0x80));
                count++;
                SanityCheck(count <= maxParams, String.Format("More than {0} parameters to {1}", maxParams, cmd));
            }
            return new SCUMMParameter(SCUMMParameterType.Array, result.ToArray());
        }

        protected SCUMMParameter GetString()
        {
            return new SCUMMParameter(SCUMMParameterType.String, reader.ReadStringZ());
        }

        protected SCUMMParameter GetPrintString()
        {
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                byte b = reader.ReadU8();
                if (b == 0)
                {
                    break;
                }
                if (b == 0xff || b == 0xfe)
                {
                    byte msg = reader.ReadU8();
                    switch (msg)
                    {
                        case 0x00: // MSGS_end
                            builder.Append("<end>");
                            break;
                        case 0x01: // MSGS_nextline
                            builder.Append("\",\"");
                            break;
                        case 0x02: // MSGS_no_crlf
                            builder.Append("\"+"); // TODO: Make this property outside the string
                            break;
                        case 0x03: // MSGS_wait
                            builder.Append("\":\"");
                            break;
                        case 0x04: // MSGS_variable
                            builder.AppendFormat("%N{0}%", GetVar());
                            break;
                        case 0x05: // MSGS_verb
                            builder.AppendFormat("%V{0}%", GetWord());
                            break;
                        case 0x06: // MSGS_actor_object
                            builder.AppendFormat("%A{0}%", GetWord());
                            break;
                        case 0x07: // MSGS_string
                            builder.AppendFormat("%S{0}%", GetWord());
                            break;
                        case 0x08: // MSGS_verbnextline
                            builder.Append("<verbnextline>");
                            break;
                        case 0x09: // MSGS_actor_animation
                            builder.AppendFormat("%a{0}%",GetWord());
                            break;
                        case 0x0a: // FOA: Talkie info
                            builder.AppendFormat("%t{0},{1},{2},{3},{4},{5},{6}%", GetWord(), GetWord(), GetWord(), GetWord(), GetWord(), GetWord(), GetWord() );
                            break;
                        case 0x0c: // FOA: Color change
                            builder.AppendFormat("%c{0}%", GetWord());
                            break;
                        case 0x0d:
                            builder.AppendFormat("<unknown>{0}", GetWord());
                            break;
                        case 0x0e: // FOA: Charset change
                            builder.AppendFormat("%f{0}%", GetWord());
                            break;
                    }
                }
                else
                {
                    builder.Append((char)b);
                }
            }
            return new SCUMMParameter(SCUMMParameterType.String, builder.ToString());
        }

        private SCUMMCommand EndObject(int opcode)
        {
            return new SCUMMCommand(SCUMMOpcode.C_EndObject);
        }

        private SCUMMCommand Jump(int opcode)
        {
            SCUMMParameter jumpDest = GetWordSigned();
            return new SCUMMCommand(SCUMMOpcode.C_Jump, jumpDest);
        }

        private SCUMMCommand EndScript(int opcode)
        {
            return new SCUMMCommand(SCUMMOpcode.C_EndScript);
        }

        private SCUMMCommand WaitForStuff(int opcode)
        {
            byte so = reader.ReadU8();
            SCUMMParameter soParam = new SCUMMParameter(SCUMMParameterType.Number, so);
            switch (so & 0x1f)
            {
                case 0x01: // SO_WAIT_FOR_ACTOR
                    SCUMMParameter actor = GetVarOrByte(so, 0x80);
                    return new SCUMMCommand(SCUMMOpcode.SC_WaitForActor, actor);
                case 0x02: // SO_WAIT_FOR_MESSAGE
                    return new SCUMMCommand(SCUMMOpcode.SC_WaitForMessage);
                case 0x03: // SO_WAIT_FOR_CAMERA
                    return new SCUMMCommand(SCUMMOpcode.SC_WaitForCamera);
                case 0x04: // SO_WAIT_FOR_SENTENCE
                    return new SCUMMCommand(SCUMMOpcode.SC_WaitForSentence);
            }
            return new SCUMMCommand(SCUMMOpcode.C_WaitForStuff, soParam);
        }

        private SCUMMCommand BreakHere(int opcode)
        {
            int count = 1;
            while (reader.ReadU8() == 128) // Multiple breaks are written as break-here <count>
            {
                count++;
            }
            reader.Position = reader.Position - 1; // Track back

            if (count > 1)
            {
                return new SCUMMCommand(SCUMMOpcode.C_BreakHereCount, new SCUMMParameter(SCUMMParameterType.Number, count));
            }
            return new SCUMMCommand(SCUMMOpcode.C_BreakHere);
        }

        private SCUMMCommand AssignArray_Word(int opcode)
        {
            SCUMMParameter var = GetVar();
            byte count = reader.ReadU8();
            SCUMMParameter[] values = new SCUMMParameter[count];

            for (int i = 0; i < count; i++ )
            {
                values[i] = GetWord();
            }

            SCUMMParameter arr = new SCUMMParameter(SCUMMParameterType.Array, values);

            return new SCUMMCommand(SCUMMOpcode.C_AssignArray, var, arr);
        }

        private SCUMMCommand AssignArray_Byte(int opcode)
        {
            SCUMMParameter var = GetVar();
            byte count = reader.ReadU8();
            SCUMMParameter[] values = new SCUMMParameter[count];

            for (int i = 0; i < count; i++)
            {
                values[i] = GetByte();
            }

            SCUMMParameter arr = new SCUMMParameter(SCUMMParameterType.Array, values);

            return new SCUMMCommand(SCUMMOpcode.C_AssignArray, var, arr);
        }

        private SCUMMCommand Ass(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter value = GetWord();
            return new SCUMMCommand(SCUMMOpcode.C_StoreVariable, var, value);
        }

        private SCUMMCommand Inc(int opcode)
        {
            SCUMMParameter var = GetVar();
            return new SCUMMCommand(SCUMMOpcode.C_IncVariable, var);
        }

        private SCUMMCommand Dec(int opcode)
        {
            SCUMMParameter var = GetVar();
            return new SCUMMCommand(SCUMMOpcode.C_DecVariable, var);
        }

        private SCUMMCommand Div(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter divisor = GetWord();
            return new SCUMMCommand(SCUMMOpcode.C_Div, var, divisor);
        }

        private SCUMMCommand Mul(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter factor = GetWord();
            return new SCUMMCommand(SCUMMOpcode.C_Mul, var, factor);
        }

        private SCUMMCommand Add(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter term = GetWord();
            return new SCUMMCommand(SCUMMOpcode.C_Add, var, term);
        }

        private SCUMMCommand And(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter term = GetWord();
            return new SCUMMCommand(SCUMMOpcode.C_LAnd, var, term);
        }

        private SCUMMCommand Or(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter term = GetWord();
            return new SCUMMCommand(SCUMMOpcode.C_LOr, var, term);
        }

        private SCUMMCommand Sub(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter term = GetWord();
            return new SCUMMCommand(SCUMMOpcode.C_Sub, var, term);
        }

        private SCUMMCommand IfVar(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter jumpDest = GetWordSigned();
            return new SCUMMCommand(SCUMMOpcode.C_IfVar, var, jumpDest);
        }

        private SCUMMCommand IfNotVar(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter jumpDest = GetWordSigned();
            return new SCUMMCommand(SCUMMOpcode.C_IfNotVar, var, jumpDest);
        }

        private SCUMMCommand IfLt(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter varOrLit = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetWordSigned();
            return new SCUMMCommand(SCUMMOpcode.C_IfLt, var, varOrLit, jumpDest);
        }

        private SCUMMCommand IfGt(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter varOrLit = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetWordSigned();
            return new SCUMMCommand(SCUMMOpcode.C_IfGt, var, varOrLit, jumpDest);
        }

        private SCUMMCommand IfLeq(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter varOrLit = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetWordSigned();
            return new SCUMMCommand(SCUMMOpcode.C_IfLeq, var, varOrLit, jumpDest);
        }

        private SCUMMCommand IfGeq(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter varOrLit = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetWordSigned();
            return new SCUMMCommand(SCUMMOpcode.C_IfGeq, var, varOrLit, jumpDest);
        }

        private SCUMMCommand IfEq(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter varOrLit = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetWordSigned();
            return new SCUMMCommand(SCUMMOpcode.C_IfEq, var, varOrLit, jumpDest);
        }

        private SCUMMCommand IfNeq(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter varOrLit = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetWordSigned();
            return new SCUMMCommand(SCUMMOpcode.C_IfNeq, var, varOrLit, jumpDest);
        }

        private SCUMMCommand StopObject(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.C_StopObject, objectNo);
        }

        private SCUMMCommand StopScript(int opcode)
        {
            SCUMMParameter scriptNo = GetVarOrByte(opcode, 0x80);
            if (scriptNo.Type == SCUMMParameterType.Number && Convert.ToInt32(scriptNo.Value) == 0)
            {
                return new SCUMMCommand(SCUMMOpcode.C_StopScriptDefault);
            }
            return new SCUMMCommand(SCUMMOpcode.C_StopScript, scriptNo);
        }

        private SCUMMCommand StartObject(int opcode)
        {
	        SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
	        SCUMMParameter verbNo = GetVarOrByte(opcode, 0x40);
            SCUMMParameter prms = GetParams(16, "start-object");
            return new SCUMMCommand(SCUMMOpcode.C_StartObject, objectNo, verbNo, prms);
        }

        private SCUMMCommand StartScript(int opcode)
        {
            SCUMMParameter scriptNo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter prms = GetParams(16, "start-script");

            if ((opcode & 0x60) == 0x60)
            {
                return new SCUMMCommand(SCUMMOpcode.C_StartScriptBakRec, scriptNo, prms);
            }

            if ((opcode & 0x40) == 0x40)
            {
                return new SCUMMCommand(SCUMMOpcode.C_StartScriptRec, scriptNo, prms);
            }

            if ((opcode & 0x20) == 0x20)
            {
                return new SCUMMCommand(SCUMMOpcode.C_StartScriptBak, scriptNo, prms);
            }

            return new SCUMMCommand(SCUMMOpcode.C_StartScript, scriptNo, prms);
        }

        private SCUMMCommand ChainScript(int opcode)
        {
            SCUMMParameter scriptNo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter prms = GetParams(16, "chain-script");
            return new SCUMMCommand(SCUMMOpcode.C_ChainScript, scriptNo, prms);
        }

        private SCUMMCommand IfInBox(int opcode)
        {
            SCUMMParameter actorNo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter boxNo = GetVarOrByte(opcode, 0x40);
            SCUMMParameter jumpDest = GetWordSigned();
            return new SCUMMCommand(SCUMMOpcode.C_IfInBox, actorNo, boxNo, jumpDest);
        }

        private SCUMMCommand FStateOf(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter objectNo = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_StateOf, var, objectNo);
        }

        private SCUMMCommand SetState(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            SCUMMParameter state = GetVarOrByte(opcode, 0x40);
            return new SCUMMCommand(SCUMMOpcode.C_StateOf, objectNo, state);
        }

        protected virtual SCUMMCommand DrawObject(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            byte subopcode = reader.ReadU8();
            switch (subopcode & 0x1f)
            {
                case 0x01: // DRAW_OBJECT_AT
                    SCUMMParameter x = GetVarOrWord(subopcode, 0x80);
                    SCUMMParameter y = GetVarOrWord(subopcode, 0x40);
                    return new SCUMMCommand(SCUMMOpcode.C_DrawObjectAt, objectNo, x, y);
                case 0x02: // DRAW_OBJECT_IMAGE
                    SCUMMParameter imageNo = GetVarOrWord(subopcode, 0x80);
                    return new SCUMMCommand(SCUMMOpcode.C_DrawObjectImage, objectNo, imageNo);
            }
            SCUMMParameter subopcodeParam = new SCUMMParameter(SCUMMParameterType.Number, subopcode);
            return new SCUMMCommand(SCUMMOpcode.C_DrawObject, objectNo, subopcodeParam);
        }

        private SCUMMCommand PickUpObject(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            SCUMMParameter roomNo = GetVarOrByte(opcode, 0x40);
            return new SCUMMCommand(SCUMMOpcode.C_PickUpObjectInRoom, objectNo, roomNo);
        }

        private SCUMMCommand PickUpObject_3(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.C_PickUpObject, objectNo);
        }

        private SCUMMCommand CameraInRoom(int opcode)
        {
            SCUMMParameter roomNo = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.C_CameraInRoom, roomNo);
        }

        private SCUMMCommand ComeOutDoor(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            SCUMMParameter roomNo = GetVarOrByte(opcode, 0x40);
            SCUMMParameter walkToX = GetWord();
            SCUMMParameter walkToY = GetWord();
            return new SCUMMCommand(SCUMMOpcode.C_ComeOutDoor, objectNo, roomNo, walkToX, walkToY);
        }

        private SCUMMCommand FOwner(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_OwnerOf, var, objectNo);
        }

        private SCUMMCommand Owner(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            SCUMMParameter owner = GetVarOrByte(opcode, 0x40);
            return new SCUMMCommand(SCUMMOpcode.C_OwnerOf, objectNo, owner);
        }

        private SCUMMParameter ReadPrintStr()
        {
            List<SCUMMParameter> result = new List<SCUMMParameter>();
            bool done = false;
            while (!done)
            {
                byte so = reader.ReadU8();
                if (so == 0xff)
                {
                    done = true;
                    break;
                }
                // TODO: Name subopcodes
                result.Add(new SCUMMParameter(SCUMMParameterType.Number, so));
                switch (so & 0x1f)
                {
                    case 0x00: // SO_AT
                        SCUMMParameter x = GetVarOrWord(so, 0x80);
                        SCUMMParameter y = GetVarOrWord(so, 0x40);
                        result.Add(x);
                        result.Add(y);
                        break;
                    case 0x01: // SO_COLOR
                        SCUMMParameter index = GetVarOrByte(so, 0x80);
                        result.Add(index);
                        break;
                    case 0x02: // SO_CLIPPED
                        SCUMMParameter clipped = GetVarOrWord(so, 0x80);
                        result.Add(clipped);
                        break;
                    case 0x03: // SO_ERASE
                    case 0x04: // SO_CENTER
                    case 0x05: // SO_TO
                    case 0x06: // SO_LEFT
                    case 0x07: // SO_OVERHEAD
                        break;
                    case 0x08: // SO_SAY_VOICE
                        SCUMMParameter cdstart = GetVarOrWord(so, 0x80);
                        SCUMMParameter cdlen = GetVarOrWord(so, 0x40);
                        result.Add(cdstart);
                        result.Add(cdlen);
                        break;
                    case 0x0f: // SO_TEXT_STRING
                        SCUMMParameter str = GetPrintString();
                        result.Add(str);
                        done = true;
                        break;
                }
            }
            return new SCUMMParameter(SCUMMParameterType.Array, result.ToArray());
        }

        private SCUMMCommand SayLineDefault(int opcode)
        {
            SCUMMParameter prms = ReadPrintStr();
            return new SCUMMCommand(SCUMMOpcode.C_SayLineDefault, prms);
        }

        private SCUMMCommand SayLine(int opcode)
        {
            SCUMMParameter actorNo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter prms = ReadPrintStr();

            if (actorNo.Type == SCUMMParameterType.Number && Convert.ToInt32(actorNo.Value) >= 252)
            {
                int actorNoValue = Convert.ToInt32(actorNo.Value);
                switch (actorNoValue)
                {
                    case 252:
                        return new SCUMMCommand(SCUMMOpcode.C_PrintSystem, prms);
                    case 253:
                        return new SCUMMCommand(SCUMMOpcode.C_PrintDebug, prms);
                    case 254:
                        return new SCUMMCommand(SCUMMOpcode.C_PrintText, prms);
                    case 255:
                        return new SCUMMCommand(SCUMMOpcode.C_PrintLine, prms);
                }
            }

            return new SCUMMCommand(SCUMMOpcode.C_SayLine, actorNo, prms);
        }

        private SCUMMCommand FProximity(int opcode)
        {
            // TODO: At least one of these is really WalkTo
            SCUMMParameter var = GetVar();
            SCUMMParameter obj1 = GetVarOrWord(opcode, 0x80);
            SCUMMParameter obj2 = GetVarOrWord(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_Proximity2ActObjs, var, obj1, obj2);
        }

        private SCUMMCommand FClosestActor(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_ClosestActor, var, objectNo);
        }

        private SCUMMCommand FRandom(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter max = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_Random, var, max);
        }

        private SCUMMCommand FActorMoving(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actorNo = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_ActorMoving, var, actorNo);
        }

        private SCUMMCommand FSoundRunning(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter soundNo = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_SoundRunning, var, soundNo);
        }

        private SCUMMCommand FScriptRunning(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter scriptNo = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_ScriptRunning, var, scriptNo);
        }

        private SCUMMCommand SleepForVar(int opcode)
        {
            SCUMMParameter var = GetVar();
            return new SCUMMCommand(SCUMMOpcode.C_SleepForVar, var);
        }

        private SCUMMCommand SleepFor(int opcode)
        {
            uint t =(uint)(reader.ReadU8() + reader.ReadU8()*256 + reader.ReadU8()*65536);
            SCUMMParameter time;

            if (t % 3600 == 0)
            {
                time = new SCUMMParameter(SCUMMParameterType.Number, t / 3600);
                return new SCUMMCommand(SCUMMOpcode.C_SleepMinutes, time);
            }

            if (t % 60 == 0)
            {
                time = new SCUMMParameter(SCUMMParameterType.Number, t / 60);
                return new SCUMMCommand(SCUMMOpcode.C_SleepSeconds, time);
            }

            time = new SCUMMParameter(SCUMMParameterType.Number, t);
            return new SCUMMCommand(SCUMMOpcode.C_SleepJiffies, time);
        }

        private SCUMMCommand StartMusic(int opcode)
        {
            SCUMMParameter musicNo = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.C_StartMusic, musicNo);
        }

        private SCUMMCommand StopMusic(int opcode)
        {
            return new SCUMMCommand(SCUMMOpcode.C_StopMusic);
        }

        private SCUMMCommand SoundKludge(int opcode)
        {
            SCUMMParameter prmsParam = GetParams(16, "sound-kludge");
            SCUMMParameter[] prms = prmsParam.Value as SCUMMParameter[];
            if (prms.Length > 0)
            {
                SCUMMParameter param = prms[0];
                if (param.Type == SCUMMParameterType.Number)
                {
                    int so = Convert.ToInt32(param.Value);
                    if (so == 0xffff)
                    {
                        return new SCUMMCommand(SCUMMOpcode.I_FlushSoundQueue);
                    }

                    int engine = so >> 8;
                    if (engine == 0)
                    {
                        switch (so & 0xff)
                        {
                            case 0x00:
                                return new SCUMMCommand(SCUMMOpcode.I_InitializeDriver, prms);
                            case 0x01:
                                return new SCUMMCommand(SCUMMOpcode.I_TerminateDriver, prms);
                            case 0x02:
                                return new SCUMMCommand(SCUMMOpcode.I_Pause, prms);
                            case 0x03:
                                return new SCUMMCommand(SCUMMOpcode.I_Resume, prms);
                            case 0x04:
                                return new SCUMMCommand(SCUMMOpcode.I_SaveGame, prms);
                            case 0x05:
                                return new SCUMMCommand(SCUMMOpcode.I_RestoreGame, prms);
                            case 0x06:
                                return new SCUMMCommand(SCUMMOpcode.I_SetMasterVol, prms);
                            case 0x07:
                                return new SCUMMCommand(SCUMMOpcode.I_GetMasterVol, prms);
                            case 0x08:
                                return new SCUMMCommand(SCUMMOpcode.I_StartSound, prms);
                            case 0x09:
                                return new SCUMMCommand(SCUMMOpcode.I_StopSound, prms);
                            case 0x0a:
                                return new SCUMMCommand(SCUMMOpcode.I_PrepareSound, prms);
                            case 0x0b:
                                Debug.Assert(prms.Length == 1, "Expected 0 arguments to stop-all-sounds");
                                return new SCUMMCommand(SCUMMOpcode.I_StopAllSounds, prms);
                            case 0x0c:
                                return new SCUMMCommand(SCUMMOpcode.I_GetSoundType, prms);
                            case 0x0d:
                                Debug.Assert(prms.Length == 2, "Expected 1 argument to sound-play-status");
                                return new SCUMMCommand(SCUMMOpcode.I_GetPlayStatus, prms);
                            case 0x0e:
                                return new SCUMMCommand(SCUMMOpcode.I_SetDebug, prms);
                                // 272 (0x110) : clear-command-q
                        }
                    }
                    else if (engine == 1) // MIDI
                    {
                        switch (so & 0xff)
                        {
                            case 0x00:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_PlayerGetParam, prms);
                            case 0x01:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_PlayerSetPriority, prms);
                            case 0x02:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_PlayerSetVol, prms);
                            case 0x03:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_PlayerSetPan, prms);
                            case 0x04:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_PlayerSetTranspose, prms);
                            case 0x05:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_PlayerSetDetune, prms);
                            case 0x06:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_SeqSetSpeed, prms);
                            case 0x07:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_SeqJump, prms);
                            case 0x08:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_SeqScan, prms);
                            case 0x09:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_SeqSetLoop, prms);
                            case 0x0a:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_SeqClearLoop, prms);
                            case 0x0b:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_PartSetPartEnable, prms);
                            case 0x0c:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_SeqSetHook, prms);
                            case 0x0d:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_FadeVol, prms);
                            case 0x0e:
                                Debug.Assert(prms.Length == 3, "Expected 2 arguments to q-sound-trigger");
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_EnqueueTrigger, prms);
                            case 0x0f:
                                //Debug.Assert(prms.Length == 3, "Expected 2 arguments to q-sound-command");
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_EnqueueCommand, prms);
                            case 0x10:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_ClearCommandQueue, prms);
                            case 0x11:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_PlayerEnableLiveMidi, prms);
                            case 0x12:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_PlayerDisableLiveMidi, prms);
                            case 0x13:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_PlayerGetParam2, prms);
                            case 0x14:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_HookSetHook, prms);
                            case 0x15:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_InsertMidiMessage, prms);
                            case 0x16:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_PartSetVol, prms);
                            case 0x17:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_QueryQueue, prms);
                            case 0x18:
                                return new SCUMMCommand(SCUMMOpcode.I_MIDI_PartPrepareSetups, prms);
                        }
                    }
                    else if (engine == 2) // Wave
                    {
                        
                    }
                    else if (engine == 3) // CD
                    {
                        
                    }
                }
            }
            return new SCUMMCommand(SCUMMOpcode.C_SoundKludge, prmsParam);
        }

        private SCUMMCommand StartSound(int opcode)
        {
            SCUMMParameter soundNo = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.C_StartSound, soundNo);
        }

        private SCUMMCommand StopSound(int opcode)
        {
            SCUMMParameter soundNo = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.C_StopSound, soundNo);
        }

        private SCUMMCommand WalkToObject(int opcode)
        {
            SCUMMParameter actorNo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x40);
            return new SCUMMCommand(SCUMMOpcode.C_WalkToObject, actorNo, objectNo);
        }

        private SCUMMCommand PutActorAtObject(int opcode)
        {
            SCUMMParameter actorNo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x40);
            return new SCUMMCommand(SCUMMOpcode.C_PutActorAtObject, actorNo, objectNo);
        }

        private SCUMMCommand WalkToActor(int opcode)
        {
            SCUMMParameter actor1 = GetVarOrByte(opcode, 0x80);
            SCUMMParameter actor2 = GetVarOrByte(opcode, 0x40);
            byte prox = reader.ReadU8();
            if (prox != 0xff)
            {
                SCUMMParameter proxParam = new SCUMMParameter(SCUMMParameterType.Number, prox);
                return new SCUMMCommand(SCUMMOpcode.C_WalkToActorWithin, actor1, actor2, proxParam);
            }
            return new SCUMMCommand(SCUMMOpcode.C_WalkToActor, actor1, actor2);
        }

        private SCUMMCommand WalkTo(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter x = GetVarOrWord(opcode, 0x40);
            SCUMMParameter y = GetVarOrWord(opcode, 0x20);
            return new SCUMMCommand(SCUMMOpcode.C_WalkToXY, actor, x, y);
        }

        private SCUMMCommand PutActorAt(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter x = GetVarOrWord(opcode, 0x40);
            SCUMMParameter y = GetVarOrWord(opcode, 0x20);
            return new SCUMMCommand(SCUMMOpcode.C_PutActorAtXY, actor, x, y);
        }

        private SCUMMCommand PutActorInRoom(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter room = GetVarOrByte(opcode, 0x40);
            return new SCUMMCommand(SCUMMOpcode.C_PutActorInRoom, actor, room);
        }

        private SCUMMCommand FValidVerb(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            SCUMMParameter verbNo = GetVarOrWord(opcode, 0x40);

            return new SCUMMCommand(SCUMMOpcode.F_ValidVerb, var, objectNo, verbNo);
        }

        private SCUMMCommand DoSentence(int opcode)
        {
            SCUMMParameter verb = GetVarOrByte(opcode, 0x80);
            
            // TODO: This one is nasty... variable value may dictate amount of bytes in code (but does it ever?)
            if (verb.Type == SCUMMParameterType.Number && Convert.ToInt32(verb.Value) == 254) // Kill sentence
            {
                return new SCUMMCommand(SCUMMOpcode.C_StopSentence, verb);
            }
            SCUMMParameter object1 = GetVarOrWord(opcode, 0x40);
            SCUMMParameter object2 = GetVarOrWord(opcode, 0x20);
            return new SCUMMCommand(SCUMMOpcode.C_DoSentence, verb, object1, object2);
        }

        private SCUMMCommand CutScene(int opcode)
        {
            SCUMMParameter prms = GetParams(16, "cut-scene");
            return new SCUMMCommand(SCUMMOpcode.C_CutScene, prms);
        }

        private SCUMMCommand EndCutScene(int opcode)
        {
            return new SCUMMCommand(SCUMMOpcode.C_EndCutScene);
        }

        private SCUMMCommand OverRide(int opcode)
        {
            SCUMMParameter prm = GetByte();
            if (Convert.ToInt32(prm.Value) == 0)
            {
                return new SCUMMCommand(SCUMMOpcode.C_OverrideOff);
            }
            return new SCUMMCommand(SCUMMOpcode.C_Override, prm);
        }

        private SCUMMCommand CameraAt(int opcode)
        {
            SCUMMParameter x = GetVarOrWord(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.C_CameraAt, x);
        }

        private SCUMMCommand CameraPanTo(int opcode)
        {
            SCUMMParameter x = GetVarOrWord(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.C_CameraPanTo, x);
        }

        private SCUMMCommand CameraFollow(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.C_CameraFollow, actor);
        }

        private SCUMMCommand FreezeScripts(int opcode)
        {
            SCUMMParameter arg = GetVarOrByte(opcode, 0x80);
            if (arg.Type ==SCUMMParameterType.Number)
            {
                switch ((byte)arg.Value)
                {
                    case 0x00:
                        return new SCUMMCommand(SCUMMOpcode.C_UnfreezeScripts);
                    case 0x7F:
                        return new SCUMMCommand(SCUMMOpcode.C_FreezeScripts);
                }
            }
            return new SCUMMCommand(SCUMMOpcode.C_FreezeScript, arg);
        }

        private SCUMMCommand Userface(int opcode)
        {
            byte subopcode = reader.ReadU8();

            SCUMMParameter subopcodeParam = new SCUMMParameter(SCUMMParameterType.Number, subopcode);

            switch (subopcode & 0x1f)
            {
                case 0x01: // SO_CURSOR_ON
                    return new SCUMMCommand(SCUMMOpcode.SC_CursorOn);
                case 0x02: // SO_CURSOR_OFF
                    return new SCUMMCommand(SCUMMOpcode.SC_CursorOff);
                case 0x03: // SO_USERPUT_ON
                    return new SCUMMCommand(SCUMMOpcode.SC_UserputOn);
                case 0x04: // SO_USERPUT_OFF
                    return new SCUMMCommand(SCUMMOpcode.SC_UserputOff);
                case 0x05: // SO_CURSOR_SOFT_ON
                    return new SCUMMCommand(SCUMMOpcode.SC_CursorSoftOn);
                case 0x06: // SO_CURSOR_SOFT_OFF
                    return new SCUMMCommand(SCUMMOpcode.SC_CursorSoftOff);
                case 0x07: // SO_USERPUT_SOFT_ON
                    return new SCUMMCommand(SCUMMOpcode.SC_UserputSoftOn);
                case 0x08: // SO_USERPUT_SOFT_OFF
                    return new SCUMMCommand(SCUMMOpcode.SC_UserputSoftOff);
                case 0x09: // SO_USERPUT_CLEAR
                    return new SCUMMCommand(SCUMMOpcode.SC_UserputClear);
                case 0x0a: // SO_CURSOR_IMAGE
                    SCUMMParameter cursor = GetVarOrByte(subopcode, 0x80);
                    SCUMMParameter letter = GetVarOrByte(subopcode, 0x40);
                    return new SCUMMCommand(SCUMMOpcode.SC_CursorImage_3, cursor, letter);
                case 0x0b: // SO_CURSOR_HOTSPOT
                    cursor = GetVarOrByte(subopcode, 0x80);
                    SCUMMParameter x = GetVarOrByte(subopcode, 0x40);
                    SCUMMParameter y = GetVarOrByte(subopcode, 0x20);
                    return new SCUMMCommand(SCUMMOpcode.SC_CursorHotspot, cursor, x, y);
                case 0x0c: // SO_CURSOR_SET
                    cursor = GetVarOrByte(subopcode, 0x80);
                    return new SCUMMCommand(SCUMMOpcode.SC_CursorSet, cursor);
                case 0x0d: // SO_CHARSET_SET
                    SCUMMParameter charset = GetVarOrByte(subopcode, 0x80);
                    return new SCUMMCommand(SCUMMOpcode.SC_CharsetSet, charset);
                case 0x0e: // SO_CHARSET_COLOR
                    SCUMMParameter colors = GetParams(16, "charset color");
                    return new SCUMMCommand(SCUMMOpcode.SC_CharsetColor, colors);
            }

            return new SCUMMCommand(SCUMMOpcode.C_Userface, subopcodeParam);
        }

        private SCUMMCommand FActorRoom(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_ActorRoom, var, actor);
        }

        private SCUMMCommand FAnimationCounter(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_AnimationCounter, var, actor);
        }

        private SCUMMCommand FActorCostume(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_ActorCostume, var, actor);
        }

        private SCUMMCommand FActorX(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrWord(opcode, 0x80); // Seems this is also for objects
            return new SCUMMCommand(SCUMMOpcode.F_ActorX, var, actor);
        }

        private SCUMMCommand FActorY(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrWord(opcode, 0x80); // Seems this is also for objects
            return new SCUMMCommand(SCUMMOpcode.F_ActorY, var, actor);
        }

        private SCUMMCommand FActorBox(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_ActorBox, var, actor);
        }

        private SCUMMCommand FActorWidth(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_ActorWidth, var, actor);
        }

        private SCUMMCommand FActorFacing(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_ActorFacing, var, actor);
        }

        private SCUMMCommand FActorScale(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_ActorScale, var, actor);
        }

        private SCUMMCommand FaceTowards(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter actobj = GetVarOrWord(opcode, 0x40);
            return new SCUMMCommand(SCUMMOpcode.C_FaceTowards, actor, actobj);
        }

        private SCUMMCommand DoAnimation(int opcode)
        {
            // C_DoAnimation (var1, 0) seems to do *something* wrong...
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter anim = GetVarOrByte(opcode, 0x40);
            return new SCUMMCommand(SCUMMOpcode.C_DoAnimation, actor, anim);
        }

        private SCUMMCommand Lights(int opcode)
        {
            SCUMMParameter lo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter hi = GetByte();
            SCUMMParameter a = GetByte();
            return new SCUMMCommand(SCUMMOpcode.C_Lights, lo, hi, a);
        }

        private SCUMMCommand HeapStuff(int opcode)
        {
            byte subopcode = reader.ReadU8();

            SCUMMParameter resource = null;
            if (subopcode != 0x11) // SO_CLEAR_HEAP
            {
                resource = GetVarOrByte(subopcode, 0x80);
            }
            switch (subopcode & 0x1f)
            {
                case 0x01:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapLoadScript, resource);
                case 0x02:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapLoadSound, resource);
                case 0x03:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapLoadCostume, resource);
                case 0x04:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapLoadRoom, resource);
                case 0x05:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapNukeScript, resource);
                case 0x06:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapNukeSound, resource);
                case 0x07:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapNukeCostume, resource);
                case 0x08:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapNukeRoom, resource);
                case 0x09:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapLockScript, resource);
                case 0x0a:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapLockSound, resource);
                case 0x0b:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapLockCostume, resource);
                case 0x0c:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapLockRoom, resource);
                case 0x0d:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapUnlockScript, resource);
                case 0x0e:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapUnlockSound, resource);
                case 0x0f:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapUnlockCostume, resource);
                case 0x10:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapUnlockRoom, resource);
                case 0x11:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapClearHeap);
                case 0x12:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapLoadCharset, resource);
                case 0x13:
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapNukeCharset, resource);
                case 0x14:
                    SCUMMParameter objectNo = GetVarOrWord(subopcode, 0x40);
                    return new SCUMMCommand(SCUMMOpcode.SC_HeapLoadObject, resource, objectNo);
            }
            SCUMMParameter subopcodeParam = new SCUMMParameter(SCUMMParameterType.Number, subopcode);
            return new SCUMMCommand(SCUMMOpcode.C_HeapStuff, subopcodeParam);
        }

        private SCUMMCommand System(int opcode)
        {
            byte so = reader.ReadU8();
            switch (so)
            {
                case 0x01: // SO_RESTART
                    return new SCUMMCommand(SCUMMOpcode.SC_SystemRestart);
                case 0x02: // SO_PAUSE
                    return new SCUMMCommand(SCUMMOpcode.SC_SystemPause);
                case 0x03: // SO_QUIT
                    return new SCUMMCommand(SCUMMOpcode.SC_SystemQuit);
            }
            SCUMMParameter subopcodeParam = new SCUMMParameter(SCUMMParameterType.Number,so);
            return new SCUMMCommand(SCUMMOpcode.C_System, subopcodeParam);
        }

        private SCUMMCommand _Debug(int opcode)
        {
            SCUMMParameter prm = GetVarOrWord(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.C_Debug, prm);
        }

        private SCUMMCommand SetBox(int opcode)
        {
            byte subopcode = reader.ReadU8();

            switch (subopcode & 0x1f)
            {
                case 0x01: // SO_BOX_SPECIAL
                    return new SCUMMCommand(SCUMMOpcode.SC_SetBoxSpecial);
                case 0x02: // SO_BOX_SCALE
                    return new SCUMMCommand(SCUMMOpcode.SC_SetBoxScale);
                case 0x03: // SO_BOX_SLOT
                    SCUMMParameter value = GetVarOrByte(subopcode, 0x80);
                    return new SCUMMCommand(SCUMMOpcode.SC_SetBoxSlot, value);
                case 0x04: // SO_BOX_PATH
                    return new SCUMMCommand(SCUMMOpcode.SC_SetBoxPath);
            }
            SCUMMParameter subopcodeParam = new SCUMMParameter(SCUMMParameterType.Number, subopcode);
            return new SCUMMCommand(SCUMMOpcode.C_SetBox, subopcodeParam);
        }

        private SCUMMCommand NewVerb(int opcode)
        {
            SCUMMParameter verb = GetVarOrByte(opcode, 0x80);
            List<SCUMMParameter> prms = new List<SCUMMParameter>();

            while (true)
            {
                byte so = reader.ReadU8();
                if (so == 0xff) // SO_END
                {
                    break;
                }
                prms.Add(new SCUMMParameter(SCUMMParameterType.Number, so));

                switch (so & 0x1f)
                {
                    case 0x01: //SO_VERB_IMAGE
                        SCUMMParameter obj = GetVarOrWord(so, 0x80);
                        prms.Add(obj);
                        break;
                    case 0x02: //SO_VERB_NAME
                        SCUMMParameter verbname = GetPrintString();
                        prms.Add(verbname);
                        break;
                    case 0x03: //SO_VERB_COLOR
                        SCUMMParameter color = GetVarOrByte(so, 0x80);
                        prms.Add(color);
                        break;
                    case 0x04: //SO_VERB_HICOLOR
                        color = GetVarOrByte(so, 0x80);
                        prms.Add(color);
                        break;
                    case 0x05: //SO_VERB_AT
                        SCUMMParameter x = GetVarOrWord(so, 0x80);
                        SCUMMParameter y = GetVarOrWord(so, 0x40);
                        prms.Add(x);
                        prms.Add(y);
                        break;
                    case 0x06: //SO_VERB_ON
                    case 0x07: //SO_VERB_OFF
                    case 0x08: //SO_VERB_DELETE
                    case 0x09: //SO_VERB_NEW
                        break;
                    case 0x10: //SO_VERB_DIMCOLOR
                        color = GetVarOrByte(so, 0x80);
                        prms.Add(color);
                        break;
                    case 0x11: //SO_VERB_DIM
                        break;
                    case 0x12: //SO_VERB_KEY
                        SCUMMParameter key = GetVarOrByte(so, 0x80);
                        prms.Add(key);
                        break;
                    case 0x13: //SO_VERB_CENTER
                        break;
                    case 0x14: //SO_VERB_NAME_STR
                        SCUMMParameter str = GetVarOrWord(so, 0x80);
                        // May have more
                        prms.Add(str);
                        break;
                    case 0x15: //SO_VERB_NEXT_TO
                        break;
                    case 0x16: //SO_VERB_IMAGE_IN_ROOM
                        obj = GetVarOrWord(so, 0x80);
                        SCUMMParameter room = GetVarOrByte(so, 0x40);
                        prms.Add(obj);
                        prms.Add(room);
                        break;
                    case 0x17: //SO_VERB_BAKCOLOR
                        color = GetVarOrByte(so, 0x80);
                        prms.Add(color);
                        break;
                }
            }
            return new SCUMMCommand(SCUMMOpcode.C_NewVerb, verb, new SCUMMParameter(SCUMMParameterType.Array, prms.ToArray()));
        }

        private SCUMMCommand FStringWidth(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter str = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_StringWidth, var, str);
        }



        private SCUMMCommand FFindActor(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter x = GetVarOrWord(opcode, 0x80);
            SCUMMParameter y = GetVarOrWord(opcode, 0x40);
            return new SCUMMCommand(SCUMMOpcode.F_FindActor, var, x, y);
        }

        private SCUMMCommand FFindObject(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter x = GetVarOrWord(opcode, 0x80);
            SCUMMParameter y = GetVarOrWord(opcode, 0x40);
            return new SCUMMCommand(SCUMMOpcode.F_FindObject, var, x, y);
        }

        private SCUMMCommand SetClass(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            List<SCUMMParameter> clss = new List<SCUMMParameter>();
            while (true)
            {
                SanityCheck(clss.Count < 32, "More than 32 classes in set-class call");

                int so = reader.ReadU8();
                if (so == 0xff) // SO_END
                {
                    break;
                }
                SCUMMParameter cls = GetVarOrWord(so, 0x80);
                clss.Add(cls);
            }
            return new SCUMMCommand(SCUMMOpcode.C_ClassOf, obj, new SCUMMParameter(SCUMMParameterType.Array, clss.ToArray()));
        }

        private SCUMMCommand IfClass(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            List<SCUMMParameter> clss = new List<SCUMMParameter>();
            while (true)
            {
                SanityCheck(clss.Count < 32, "More than 32 classes in if (class-of ...)");

                int so = reader.ReadU8();
                if (so == 0xff) // SO_END
                {
                    break;
                }
                SCUMMParameter cls = GetVarOrWord(so, 0x80);
                clss.Add(cls);
            }
            SCUMMParameter jumpDest = GetWordSigned();
            return new SCUMMCommand(SCUMMOpcode.C_IfClassOf, obj, new SCUMMParameter(SCUMMParameterType.Array, clss.ToArray()), jumpDest);
        }

        private SCUMMCommand Pseudoroom(int opcode)
        {
            SCUMMParameter room = GetByte();
            List<SCUMMParameter> rooms = new List<SCUMMParameter>();

            while (true)
            {
                byte r = reader.ReadU8();
                if (r == 0)
                {
                    break;
                }
                rooms.Add(new SCUMMParameter(SCUMMParameterType.Number, r));
            }

            return new SCUMMCommand(SCUMMOpcode.C_Pseudoroom, room, new SCUMMParameter(SCUMMParameterType.Array, rooms.ToArray()));
        }

        private SCUMMCommand FActorElevation(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_ActorElevation, var, actor);
        }

        private SCUMMCommand FFindInventory(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter slot = GetVarOrByte(opcode, 0x40);
            return new SCUMMCommand(SCUMMOpcode.F_FindInventory, var, actor, slot);
        }

        private SCUMMCommand FInventorySize(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            return new SCUMMCommand(SCUMMOpcode.F_InventorySize, var, actor);
        }

        private SCUMMCommand DrawBox(int opcode)
        {
            SCUMMParameter x1 = GetVarOrWord(opcode, 0x80);
            SCUMMParameter y1 = GetVarOrWord(opcode, 0x40);
            byte so = reader.ReadU8();
            SCUMMParameter x2 = GetVarOrWord(so, 0x80);
            SCUMMParameter y2 = GetVarOrWord(so, 0x40);
            SCUMMParameter color = GetVarOrByte(so, 0x20);
            return new SCUMMCommand(SCUMMOpcode.C_DrawBox, x1, y1, x2, y2, color);
        }

        private SCUMMCommand AssComplex(int opcode)
        {
            SCUMMParameter var = GetVar();
            List<SCUMMParameter> args = new List<SCUMMParameter>();
            while (true)
            {
                byte so = reader.ReadU8();
                if (so == 0xff)
                {
                    break;
                }

                args.Add(new SCUMMParameter(SCUMMParameterType.Number, so));
                switch (so & 0x1f)
                {
                    case 0x02:
                    case 0x03:
                    case 0x04:
                    case 0x05:
                        break;
                    case 0x06: // Function
                        byte f = reader.ReadU8();
                        args.Add(new SCUMMParameter(SCUMMParameterType.Command, opcodeHandlers[f](f)));
                        break;
                    case 0x01:
                    case 0x81:
                        SCUMMParameter val = GetVarOrWord(so, 0x80);
                        args.Add(val);
                        break;
                }
            }
            return new SCUMMCommand(SCUMMOpcode.C_StoreExpression, var, new SCUMMParameter(SCUMMParameterType.Array, args.ToArray()));
        }

        private SCUMMCommand SaveLoadVariables(int opcode)
        {
            return new SCUMMCommand(SCUMMOpcode.C_SaveLoadVariables);
        }

        private SCUMMCommand Unknown(int opcode)
        {
            return new SCUMMCommand(SCUMMOpcode.C_Unknown, new SCUMMParameter(SCUMMParameterType.Number, opcode));
        }

        protected virtual SCUMMCommand ActorStuff(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);

            List<SCUMMParameter> args = new List<SCUMMParameter>();

            while (true)
            {
                SanityCheck(args.Count < 32, "More than 32 arguments to actor");
                byte so = reader.ReadU8();
                if (so == 0xff)
                {
                    break;
                }

                args.Add(new SCUMMParameter(SCUMMParameterType.Number, so));
                switch (so & 0x1f)
                {
                    case SO_COSTUME:
                        SCUMMParameter costume = GetVarOrByte(so, 0x80);
                        args.Add(costume);
                        break;
                    case SO_STEP_DIST:
                        SCUMMParameter stepx = GetVarOrByte(so, 0x80);
                        SCUMMParameter stepy = GetVarOrByte(so, 0x40);
                        args.Add(stepx);
                        args.Add(stepy);
                        break;
                    case SO_SOUND:
                        SCUMMParameter soundTable = GetVarOrByte(so, 0x80);
                        args.Add(soundTable);
                        break;
                    case SO_WALK_ANIMATION:
                        SCUMMParameter chore = GetVarOrByte(so, 0x80);
                        args.Add(chore);
                        break;
                    case SO_TALK_ANIMATION:
                        SCUMMParameter choreStart = GetVarOrByte(so, 0x80);
                        SCUMMParameter choreStop = GetVarOrByte(so, 0x40);
                        args.Add(choreStart);
                        args.Add(choreStop);
                        break;
                    case SO_STAND_ANIMATION:
                        chore = GetVarOrByte(so, 0x80);
                        args.Add(chore);
                        break;
                    case SO_ANIMATION:
                        SCUMMParameter anim1 = GetVarOrByte(so, 0x80);
                        SCUMMParameter anim2 = GetVarOrByte(so, 0x40);
                        SCUMMParameter anim3 = GetVarOrByte(so, 0x20);
                        args.Add(anim1);
                        args.Add(anim2);
                        args.Add(anim3);
                        break;
                    case SO_DEFAULT:
                        break;
                    case SO_ELEVATION:
                        SCUMMParameter elevation = GetVarOrWord(so, 0x80);
                        args.Add(elevation);
                        break;
                    case SO_ANIMATION_DEFAULT:
                        break;
                    case SO_PALETTE:
                        SCUMMParameter slot = GetVarOrByte(so, 0x80);
                        SCUMMParameter j = GetVarOrByte(so, 0x40);
                        args.Add(slot);
                        args.Add(j);
                        break;
                    case SO_TALK_COLOR:
                        SCUMMParameter color = GetVarOrByte(so, 0x80);
                        args.Add(color);
                        break;
                    case SO_ACTOR_NAME:
                        SCUMMParameter name = GetString();
                        args.Add(name);
                        break;
                    case SO_INIT_ANIMATION:
                        chore = GetVarOrByte(so, 0x80);
                        args.Add(chore);
                        break;
                    case SO_PALETTE_LIST:
                        SCUMMParameter list = GetParams(32, "actor color");
                        args.Add(list);
                        break;
                    case SO_ACTOR_WIDTH:
                        SCUMMParameter width = GetVarOrByte(so, 0x80);
                        args.Add(width);
                        break;
                    case SO_SCALE:
                        SCUMMParameter scalex = GetVarOrByte(so, 0x80);
                        SCUMMParameter scaley = GetVarOrByte(so, 0x40);
                        args.Add(scalex);
                        args.Add(scaley);
                        break;
                    case SO_NEVER_ZCLIP:
                        break;
                    case SO_ALWAYS_ZCLIP:
                        SCUMMParameter zclip = GetVarOrByte(so, 0x80);
                        args.Add(zclip);
                        break;
                    case SO_IGNORE_BOXES:
                        break;
                    case SO_FOLLOW_BOXES:
                        break;
                    case SO_ANIMATION_SPEED:
                        SCUMMParameter speed = GetVarOrByte(so, 0x80);
                        args.Add(speed);
                        break;
                    case SO_SHADOW:
                        SCUMMParameter shadow = GetVarOrByte(so, 0x80);
                        args.Add(shadow);
                        break;
                }
            }

            return new SCUMMCommand(SCUMMOpcode.C_ActorStuff, actor, new SCUMMParameter(SCUMMParameterType.Array, args.ToArray()));
        }

        private SCUMMCommand RoomStuff(int opcode)
        {
            byte so = reader.ReadU8();
            switch (so & 0x1f)
            {
                case 0x01: // SO_ROOM_SCROLL
                    SCUMMParameter x1 = GetVarOrWord(so, 0x80);
                    SCUMMParameter x2 = GetVarOrWord(so, 0x40);
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomScroll, x1, x2);

                case 0x02: // SO_ROOM_COLOR
                    SCUMMParameter color = GetVarOrWord(so, 0x80);
                    SCUMMParameter slot = GetVarOrWord(so, 0x40);
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomColor, color, slot);

                case 0x03: // SO_ROOM_SCREEN
                    SCUMMParameter screen = GetVarOrWord(so, 0x80);
                    SCUMMParameter screento = GetVarOrWord(so, 0x40);
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomSetScreen, screen, screento);

                case 0x04: // SO_ROOM_PALETTE
                    SCUMMParameter red = GetVarOrWord(so, 0x80);
                    SCUMMParameter green = GetVarOrWord(so, 0x40);
                    SCUMMParameter blue = GetVarOrWord(so, 0x20);
                    byte so2 = reader.ReadU8();
                    slot = GetVarOrByte(so2, 0x80);
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomPalette, red, green, blue, slot);

                case 0x05: // SO_ROOM_SHAKE_ON
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomShakeOn);

                case 0x06: // SO_ROOM_SHAKE_OFF
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomShakeOff);

                case 0x07: // SO_ROOM_SCALE
                    SCUMMParameter scl1 = GetVarOrWord(so, 0x80);
                    SCUMMParameter scl1at = GetVarOrWord(so, 0x40);
                    so2 = reader.ReadU8();
                    SCUMMParameter scl2 = GetVarOrWord(so2, 0x80);
                    SCUMMParameter scl2at = GetVarOrWord(so2, 0x40);
                    so2 = reader.ReadU8();
                    SCUMMParameter sclslot = GetVarOrWord(so2, 0x80);
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomScale, scl1, scl1at, scl2, scl2at, sclslot);

                case 0x08: // SO_ROOM_INTENSITY
                    SCUMMParameter intensity = GetVarOrByte(so, 0x80);
                    SCUMMParameter from = GetVarOrByte(so, 0x40);
                    SCUMMParameter to = GetVarOrByte(so, 0x20);
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomIntensity, intensity, from, to);

                case 0x09: // SO_ROOM_SAVEGAME
                    SCUMMParameter mode = GetVarOrByte(so, 0x80);
                    slot = GetVarOrByte(so, 0x40);
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomSaveLoadGame, mode, slot);

                case 0x0a: // SO_ROOM_FADE
                    SCUMMParameter arg = GetVarOrWord(so, 0x80);
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomFade, arg);

                case 0x0b: // SO_RGB_ROOM_INTENSITY
                    red = GetVarOrByte(so, 0x80);
                    green = GetVarOrByte(so, 0x40);
                    blue = GetVarOrByte(so, 0x20);
                    so2 = reader.ReadU8();
                    from = GetVarOrByte(so2, 0x80);
                    to = GetVarOrByte(so2, 0x40);
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomRgbIntensity, red, green, blue, from, to);

                case 0x0c: // SO_ROOM_SHADOW
                    red = GetVarOrByte(so, 0x80);
                    green = GetVarOrByte(so, 0x40);
                    blue = GetVarOrByte(so, 0x20);
                    so2 = reader.ReadU8();
                    from = GetVarOrByte(so2, 0x80);
                    to = GetVarOrByte(so2, 0x40);
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomSetShadowPalette, red, green, blue, from, to);

                case 0x0d: // SO_SAVE_STRING
                    SCUMMParameter str = GetVarOrByte(so, 0x80);
                    SCUMMParameter value = GetPrintString();
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomSaveString, str, value);

                case 0x0e: // SO_LOAD_STRING
                    str = GetVarOrByte(so, 0x80);
                    value = GetPrintString();
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomLoadString, str, value);

                case 0x0f: // SO_ROOM_TRANSFORM
                    SCUMMParameter palstr = GetVarOrByte(so, 0x80);
                    so2 = reader.ReadU8();
                    SCUMMParameter start = GetVarOrByte(so2, 0x80);
                    SCUMMParameter end = GetVarOrByte(so2, 0x40);
                    so2 = reader.ReadU8();
                    SCUMMParameter step = GetVarOrByte(so2, 0x80);
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomTransform, palstr, start, end, step);

                case 0x10: // SO_CYCLE_SPEED
                    slot = GetVarOrByte(so, 0x80);
                    SCUMMParameter speed = GetVarOrByte(so, 0x80);
                    return new SCUMMCommand(SCUMMOpcode.SC_RoomCycleSpeed, slot, speed);

            }
            SCUMMParameter soParam = new SCUMMParameter(SCUMMParameterType.Number, so);
            return new SCUMMCommand(SCUMMOpcode.C_RoomStuff, soParam);
        }

        private SCUMMCommand StringStuff(int opcode)
        {
            byte so = reader.ReadU8();
            switch (so & 0x1f)
            {
                case 0x01: // SO_STRING_IS_STRVAL
                    SCUMMParameter str = GetVarOrByte(so, 0x80);
                    SCUMMParameter strval = GetPrintString();
                    return new SCUMMCommand(SCUMMOpcode.C_StringIsStrval, str, strval);
                case 0x02: // SO_STRING_IS_STRVAR
                    str = GetVarOrByte(so, 0x80);
                    SCUMMParameter source = GetVarOrByte(so, 0x40);
                    return new SCUMMCommand(SCUMMOpcode.C_StringIsStrvar, str, source);
                case 0x03: // SO_STRING_IS_VAL
                    str = GetVarOrByte(so, 0x80);
                    SCUMMParameter index = GetVarOrByte(so, 0x40);
                    SCUMMParameter val = GetVarOrByte(so, 0x20);
                    return new SCUMMCommand(SCUMMOpcode.C_StringIsVal, str, index, val);
                case 0x04: // SO_VAR_IS_STRING
                    SCUMMParameter var = GetVar();
                    str = GetVarOrByte(so, 0x80);
                    index = GetVarOrByte(so, 0x40);
                    return new SCUMMCommand(SCUMMOpcode.C_VarIsString, var, str, index);
                case 0x05: // SO_DIM_STRING
                    str = GetVarOrByte(so, 0x80);
                    SCUMMParameter len = GetVarOrByte(so, 0x40);
                    return new SCUMMCommand(SCUMMOpcode.C_DimString, str, len);
            }
            SCUMMParameter soParam = new SCUMMParameter(SCUMMParameterType.Number, so);
            return new SCUMMCommand(SCUMMOpcode.C_StringStuff, soParam);
        }

        private SCUMMCommand NewName(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            SCUMMParameter str = GetPrintString();
            return new SCUMMCommand(SCUMMOpcode.C_NewNameOf, obj, str);
        }

        private SCUMMCommand VerbSets(int opcode)
        {
            byte so = reader.ReadU8();
            SCUMMParameter start = GetVarOrByte(so, 0x80);
            SCUMMParameter end = GetVarOrByte(so, 0x40);
            SCUMMParameter set = GetVarOrByte(so, 0x20);

            switch (so & 0x1f)
            {
                case 0x01:
                    return new SCUMMCommand(SCUMMOpcode.SC_VerbsSave, start, end, set);
                case 0x02:
                    return new SCUMMCommand(SCUMMOpcode.SC_VerbsRestore, start, end, set);
                case 0x03:
                    return new SCUMMCommand(SCUMMOpcode.SC_VerbsDelete, start, end, set);
            }
            return new SCUMMCommand(SCUMMOpcode.C_VerbSets);
        }

        private SCUMMCommand Fades(int opcode)
        {
            int so = reader.ReadU8();
            if (so == 3) // Is always 3
            {
                SCUMMParameter param = GetVarOrWord(so, 0x80);
                return new SCUMMCommand(SCUMMOpcode.SC_RoomFade, param);
            }
            return new SCUMMCommand(SCUMMOpcode.SC_RoomFade, new SCUMMParameter(SCUMMParameterType.Number, so));
        }
    }
}
