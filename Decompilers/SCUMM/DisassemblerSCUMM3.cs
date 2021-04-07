using System;
using System.Collections.Generic;

namespace SCUMMRevLib.Decompilers.SCUMM
{
    public class DisassemblerSCUMM3 : DisassemblerSCUMM1
    {
        public DisassemblerSCUMM3()
        {
            GetVar = GetVarNew;
            GetString = GetStringNew;
        }

        protected override void InitOpcodes()
        {
            AddOpcode(0x00, EndObject);
            AddOpcode(0x01, PutActorAtXY);
            AddOpcode(0x02, StartMusic);
            AddOpcode(0x03, FActorRoom);
            AddOpcode(0x04, IfGt);                  // Or GEQ?
            AddOpcode(0x05, DrawObject);
            AddOpcode(0x06, FActorElevation);
            AddOpcode(0x07, StateOf);
            AddOpcode(0x08, IfEq);
            AddOpcode(0x09, FaceTowards);
            AddOpcode(0x0a, StartScript);
            AddOpcode(0x0b, FValidVerb);
            AddOpcode(0x0c, HeapStuff);
            AddOpcode(0x0d, WalkToActor);
            AddOpcode(0x0e, PutActorAtObject);
            AddOpcode(0x0f, FStateOf);
            AddOpcode(0x10, FOwnerOf);
            AddOpcode(0x11, DoAnimation);
            AddOpcode(0x12, CameraPanTo);
            AddOpcode(0x13, ActorStuff);
            AddOpcode(0x14, SayLine);
            AddOpcode(0x15, FFindActor);
            AddOpcode(0x16, FRandom);
            AddOpcode(0x17, LAnd);
            AddOpcode(0x18, Jump);
            AddOpcode(0x19, DoSentence);
            AddOpcode(0x1a, StoreVariable);
            AddOpcode(0x1b, Mul);
            AddOpcode(0x1c, StartSound);
            AddOpcode(0x1d, IfClassOf);
            AddOpcode(0x1e, WalkToXY);
            AddOpcode(0x1f, IfInBox);
            AddOpcode(0x20, StopMusic);
            AddOpcode(0x21, PutActorAtXY);
            AddOpcode(0x22, FAnimationCounter);
            AddOpcode(0x23, FActorY);
            AddOpcode(0x24, ComeOutDoor);
            AddOpcode(0x25, PickUpObjectInRoom);
            AddOpcode(0x26, AssignArray);
            AddOpcode(0x27, StringStuff);
            AddOpcode(0x28, IfVar);
            AddOpcode(0x29, OwnerOf);
            AddOpcode(0x2a, StartScript);
            AddOpcode(0x2b, SleepForVar);
            AddOpcode(0x2c, Userface);
            AddOpcode(0x2d, PutActorInRoom);
            AddOpcode(0x2e, SleepFor);
            //AddOpcode(0x2f, Unknown);
            AddOpcode(0x30, SetBox);
            AddOpcode(0x31, FInventorySize);
            AddOpcode(0x32, CameraAt);
            AddOpcode(0x33, RoomStuff);
            AddOpcode(0x34, FProximity2ActObjs);
            AddOpcode(0x35, FFindObject);
            AddOpcode(0x36, WalkToObject);
            AddOpcode(0x37, StartObject);
            AddOpcode(0x38, IfLt);              // Or LEQ?
            AddOpcode(0x39, DoSentence);
            AddOpcode(0x3a, Sub);
            AddOpcode(0x3b, WaitForActor);
            AddOpcode(0x3c, StopSound);
            AddOpcode(0x3d, FFindInventory);
            AddOpcode(0x3e, WalkToXY);
            AddOpcode(0x3f, DrawBox);
            AddOpcode(0x40, CutScene);
            AddOpcode(0x41, PutActorAtXY);
            AddOpcode(0x42, ChainScript);
            AddOpcode(0x43, FActorX);
            AddOpcode(0x44, IfLeq);
            //AddOpcode(0x45, Unknown);
            AddOpcode(0x46, Inc);
            AddOpcode(0x47, StateOf);
            AddOpcode(0x48, IfNeq);
            AddOpcode(0x49, FaceTowards);
            AddOpcode(0x4a, StartScript);
            AddOpcode(0x4b, FValidVerb);
            AddOpcode(0x4c, WaitForSentence);
            AddOpcode(0x4d, WalkToActor);
            AddOpcode(0x4e, PutActorAtObject);
            //AddOpcode(0x4f, Unknown);
            AddOpcode(0x50, PickUpObject); // MI1
            AddOpcode(0x51, DoAnimation);
            AddOpcode(0x52, CameraFollow);
            AddOpcode(0x53, ActorStuff);
            AddOpcode(0x54, NewNameOf);
            AddOpcode(0x55, FFindActor);
            AddOpcode(0x56, FActorMoving);
            AddOpcode(0x57, LOr);
            AddOpcode(0x58, OverRide);
            AddOpcode(0x59, DoSentence);
            AddOpcode(0x5a, Add);
            AddOpcode(0x5b, Div);
            AddOpcode(0x5c, Fades); // MI1: Fades
            AddOpcode(0x5d, ClassOf);
            AddOpcode(0x5e, WalkToXY);
            AddOpcode(0x5f, IfInBox);
            AddOpcode(0x60, FreezeScripts);
            AddOpcode(0x61, PutActorAtXY);
            AddOpcode(0x62, StopScript);
            AddOpcode(0x63, FActorFacing);
            AddOpcode(0x64, ComeOutDoor);
            AddOpcode(0x65, PickUpObjectInRoom);
            AddOpcode(0x66, FClosestActor);
            AddOpcode(0x67, FStringWidth);
            AddOpcode(0x68, FScriptRunning);
            AddOpcode(0x69, OwnerOf);
            AddOpcode(0x6a, StartScript);
            AddOpcode(0x6b, _Debug);
            AddOpcode(0x6c, FActorWidth);
            AddOpcode(0x6d, PutActorInRoom);
            AddOpcode(0x6e, StopObject);
            //AddOpcode(0x6f, Unknown);
            AddOpcode(0x70, Lights);
            AddOpcode(0x71, FActorCostume);
            AddOpcode(0x72, CameraInRoom);
            AddOpcode(0x73, RoomStuff);
            AddOpcode(0x74, FProximity2ActObjs);
            AddOpcode(0x75, FFindObject);
            AddOpcode(0x76, WalkToObject);
            AddOpcode(0x77, StartObject);
            AddOpcode(0x78, IfGeq);
            AddOpcode(0x79, DoSentence);
            AddOpcode(0x7a, VerbStuff);
            AddOpcode(0x7b, FActorBox);
            AddOpcode(0x7c, FSoundRunning);
            AddOpcode(0x7d, FFindInventory);
            AddOpcode(0x7e, WalkToXY);
            AddOpcode(0x7f, DrawBox);
            AddOpcode(0x80, BreakHere);
            AddOpcode(0x81, PutActorAtXY);
            AddOpcode(0x82, StartMusic);
            AddOpcode(0x83, FActorRoom);
            AddOpcode(0x84, IfGt);
            AddOpcode(0x85, DrawObject);
            AddOpcode(0x86, FActorElevation);
            AddOpcode(0x87, StateOf);
            AddOpcode(0x88, IfEq);
            AddOpcode(0x89, FaceTowards);
            AddOpcode(0x8a, StartScript);
            AddOpcode(0x8b, FValidVerb);
            AddOpcode(0x8c, HeapStuff);
            AddOpcode(0x8d, WalkToActor);
            AddOpcode(0x8e, PutActorAtObject);
            AddOpcode(0x8f, FStateOf);
            AddOpcode(0x90, FOwnerOf);
            AddOpcode(0x91, DoAnimation);
            AddOpcode(0x92, CameraPanTo);
            AddOpcode(0x93, ActorStuff);
            AddOpcode(0x94, SayLine);
            AddOpcode(0x95, FFindActor);
            AddOpcode(0x96, FRandom);
            AddOpcode(0x97, LAnd);
            AddOpcode(0x98, System);
            AddOpcode(0x99, DoSentence);
            AddOpcode(0x9a, StoreVariable);
            AddOpcode(0x9b, Mul);
            AddOpcode(0x9c, StartSound);
            AddOpcode(0x9d, IfClassOf);
            AddOpcode(0x9e, WalkToXY);
            AddOpcode(0x9f, IfInBox);
            AddOpcode(0xa0, EndScript);
            AddOpcode(0xa1, PutActorAtXY);
            AddOpcode(0xa2, FAnimationCounter);
            AddOpcode(0xa3, FActorY);
            AddOpcode(0xa4, ComeOutDoor);
            AddOpcode(0xa5, PickUpObjectInRoom);
            AddOpcode(0xa6, AssignArray_Word);
            AddOpcode(0xa7, SaveLoadVariables);
            AddOpcode(0xa8, IfNotVar);
            AddOpcode(0xa9, OwnerOf);
            AddOpcode(0xaa, StartScript);
            AddOpcode(0xab, VerbSets);
            AddOpcode(0xac, StoreExpression);
            AddOpcode(0xad, PutActorInRoom);
            AddOpcode(0xae, WaitForMessage);
            //AddOpcode(0xaf, Unknown);
            AddOpcode(0xb0, SetBox);
            AddOpcode(0xb1, FInventorySize);
            AddOpcode(0xb2, CameraAt);
            AddOpcode(0xb3, RoomStuff);
            AddOpcode(0xb4, FProximity2ActObjs);
            AddOpcode(0xb5, FFindObject);
            AddOpcode(0xb6, WalkToObject);
            AddOpcode(0xb7, StartObject);
            AddOpcode(0xb8, IfLt);
            AddOpcode(0xb9, DoSentence);
            AddOpcode(0xba, Sub);
            AddOpcode(0xbb, WaitForActor);
            AddOpcode(0xbc, StopSound);
            AddOpcode(0xbd, FFindInventory);
            AddOpcode(0xbe, WalkToXY);
            AddOpcode(0xbf, DrawBox);
            AddOpcode(0xc0, EndCutScene);
            AddOpcode(0xc1, PutActorAtXY);
            AddOpcode(0xc2, ChainScript);
            AddOpcode(0xc3, FActorX);
            AddOpcode(0xc4, IfLeq);
            //AddOpcode(0xc5, Unknown);
            AddOpcode(0xc6, Dec);
            AddOpcode(0xc7, StateOf);
            AddOpcode(0xc8, IfNeq);
            AddOpcode(0xc9, FaceTowards);
            AddOpcode(0xca, StartScript);
            AddOpcode(0xcb, FValidVerb);
            AddOpcode(0xcc, Pseudoroom);
            AddOpcode(0xcd, WalkToActor);
            AddOpcode(0xce, PutActorAtObject);
            //AddOpcode(0xcf, Unknown);
            //AddOpcode(0xd0, Unknown);
            AddOpcode(0xd1, DoAnimation);
            AddOpcode(0xd2, CameraFollow);
            AddOpcode(0xd3, ActorStuff);
            AddOpcode(0xd4, NewNameOf);
            AddOpcode(0xd5, FFindActor);
            AddOpcode(0xd6, FActorMoving);
            AddOpcode(0xd7, LOr);
            AddOpcode(0xd8, SayLineDefault);
            AddOpcode(0xd9, DoSentence);
            AddOpcode(0xda, Add);
            AddOpcode(0xdb, Div);
            //AddOpcode(0xdc, Unknown);
            AddOpcode(0xdd, ClassOf);
            AddOpcode(0xde, WalkToXY);
            AddOpcode(0xdf, IfInBox);
            AddOpcode(0xe0, FreezeScripts);
            AddOpcode(0xe1, PutActorAtXY);
            AddOpcode(0xe2, StopScript);
            AddOpcode(0xe3, FActorFacing);
            AddOpcode(0xe4, ComeOutDoor);
            AddOpcode(0xe5, PickUpObjectInRoom);
            AddOpcode(0xe6, FClosestActor);
            AddOpcode(0xe7, FStringWidth);
            AddOpcode(0xe8, FScriptRunning);
            AddOpcode(0xe9, OwnerOf);
            AddOpcode(0xea, StartScript);
            AddOpcode(0xeb, _Debug);
            AddOpcode(0xec, FActorWidth);
            AddOpcode(0xed, PutActorInRoom);
            AddOpcode(0xee, StopObject);
            //AddOpcode(0xef, Unknown);
            AddOpcode(0xf0, Lights);
            AddOpcode(0xf1, FActorCostume);
            AddOpcode(0xf2, CameraInRoom);
            AddOpcode(0xf3, RoomStuff);
            AddOpcode(0xf4, FProximity2ActObjs);
            AddOpcode(0xf5, FFindObject);
            AddOpcode(0xf6, WalkToObject);
            AddOpcode(0xf7, StartObject);
            AddOpcode(0xf8, IfGeq);
            AddOpcode(0xf9, DoSentence);
            AddOpcode(0xfa, VerbStuff);
            AddOpcode(0xfb, FActorBox);
            AddOpcode(0xfc, FSoundRunning);
            AddOpcode(0xfd, FFindInventory);
            AddOpcode(0xfe, WalkToXY);
            AddOpcode(0xff, DrawBox);
        }

        protected override SCUMMParameter GetCoordinateLiteral()
        {
            return GetNumber();
        }

        protected override SCUMMParameter GetCoordinateVarOrLiteral(int opcode, int mask)
        {
            return GetVarOrWord(opcode, mask);
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

        protected override void ActorStuff(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.C_ActorStuff, actor);

            int count = 0;
            while (true)
            {
                SanityCheck(count < 32, "More than 32 calls to actor-stuff");
                byte subOpcode = reader.ReadU8();
                if (subOpcode == 0xff)
                {
                    break;
                }

                switch (subOpcode & 0x1f)
                {
                    case 1:
                        SCUMMParameter costume = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorCostume, costume);
                        break;
                    // NOTE DISCREPANCY - different subopcodes than SCUMM 5
                    case 4:
                        SCUMMParameter stepx = GetVarOrByte(subOpcode, 0x80);
                        SCUMMParameter stepy = GetVarOrByte(subOpcode, 0x40);
                        Add(SCUMMOpcode.SC_ActorStepDist, stepx, stepy);
                        break;
                    case 5:
                        SCUMMParameter soundTable = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorSound_3, soundTable);
                        break;
                    case 6:
                        SCUMMParameter chore = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorAnimationWalk, chore);
                        break;
                    case 7:
                        SCUMMParameter choreStart = GetVarOrByte(subOpcode, 0x80);
                        SCUMMParameter choreStop = GetVarOrByte(subOpcode, 0x40);
                        Add(SCUMMOpcode.SC_ActorAnimationTalk, choreStart, choreStop);
                        break;
                    case 8:
                        chore = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorAnimationStand, chore);
                        break;
                    case 9:
                        SCUMMParameter anim1 = GetVarOrByte(subOpcode, 0x80);
                        SCUMMParameter anim2 = GetVarOrByte(subOpcode, 0x40);
                        SCUMMParameter anim3 = GetVarOrByte(subOpcode, 0x20);
                        Add(SCUMMOpcode.SC_ActorAnimation, anim1, anim2, anim3);
                        break;
                    case 10:
                        Add(SCUMMOpcode.SC_ActorDefault);
                        break;
                    case 11:
                        SCUMMParameter elevation = GetVarOrWord(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorElevation, elevation);
                        break;
                    case 12:
                        Add(SCUMMOpcode.SC_ActorAnimationDefault);
                        break;
                    case 13:
                        SCUMMParameter slot = GetVarOrByte(subOpcode, 0x80);
                        SCUMMParameter j = GetVarOrByte(subOpcode, 0x40);
                        Add(SCUMMOpcode.SC_ActorPalette, slot, j);
                        break;
                    case 14:
                        SCUMMParameter color = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorTalkColor, color);
                        break;
                    case 15:
                        SCUMMParameter name = GetString();
                        Add(SCUMMOpcode.SC_ActorName, name);
                        break;
                    case 16:
                        chore = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorAnimationInit, chore);
                        break;
                    case 17:
                        SCUMMParameter list = GetParams(32, "palette-list");
                        Add(SCUMMOpcode.SC_ActorPaletteList, list);
                        break;
                    case 18:
                        SCUMMParameter width = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorWidth, width);
                        break;
                    case 19:
                        SCUMMParameter scale = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorScale_3, scale);
                        break;
                    /*case 1:
                        Add(SCUMMOpcode.SC_ActorNeverZClip);
                        break;*/
                    // NOTE DISCREPANCY - different subopcodes than SCUMM5
                    case 2:
                        SCUMMParameter zclip = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorAlwaysZClip, zclip);
                        break;
                    /*case 2:
                        Add(SCUMMOpcode.SC_ActorIgnoreBoxes);
                        break;*/
                    case 3:
                        Add(SCUMMOpcode.SC_ActorFollowBoxes);
                        break;
                    default:
                        throw UnknownSubOpcode("actor", subOpcode);
                }
                count++;
            }
        }

        protected void StoreExpression(int opcode)
        {
            SCUMMParameter var = GetVar();
            Add(SCUMMOpcode.C_StoreExpression, var);

            while (true)
            {
                byte subOpcode = reader.ReadU8();
                if (subOpcode == 0xff)
                {
                    break;
                }

                switch (subOpcode & 0x1f)
                {
                    case 0x01:
                    // case 0x81:
                        SCUMMParameter val = GetVarOrWord(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ExprFactor, val);
                        break;
                    case 0x02:
                        Add(SCUMMOpcode.SC_ExprAdd);
                        break;
                    case 0x03:
                        Add(SCUMMOpcode.SC_ExprSub);
                        break;
                    case 0x04:
                        Add(SCUMMOpcode.SC_ExprMul);
                        break;
                    case 0x05:
                        Add(SCUMMOpcode.SC_ExprDiv);
                        break;
                    case 0x06: // Function
                        Add(SCUMMOpcode.SC_ExprCall);
                        byte f = reader.ReadU8();
                        opcodeHandlers[f](f);
                        break;
                    default:
                        throw UnknownSubOpcode("expression", subOpcode);
                }
            }
            Add(SCUMMOpcode.C_StoreExpressionEnd);
        }

        protected override void ChainScript(int opcode)
        {
            SCUMMParameter scriptNo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter prms = GetParams(16, "chain-script");
            Add(SCUMMOpcode.C_ChainScript, scriptNo, prms);
        }

        protected void ClassOf(int opcode)
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
            Add(SCUMMOpcode.C_ClassOf, obj, new SCUMMParameter(SCUMMParameterType.Array, clss.ToArray()));
        }

        protected override void CutScene(int opcode)
        {
            SCUMMParameter prms = GetParams(16, "cut-scene");
            Add(SCUMMOpcode.C_CutScene, prms);
        }

        protected void _Debug(int opcode)
        {
            SCUMMParameter prm = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_Debug, prm);
        }

        protected void Div(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter divisor = GetNumber();
            Add(SCUMMOpcode.C_DivVariable, var, divisor);
        }

        protected override void DoSentence(int opcode)
        {
            SCUMMParameter verb = GetVarOrByte(opcode, 0x80);

            // TODO: This one is nasty... variable value may dictate amount of bytes in code (but does it ever?)
            if (verb.Type == SCUMMParameterType.Number && Convert.ToInt32(verb.Value) == 254) // Kill sentence
            {
                Add(SCUMMOpcode.C_StopSentence);
                return;
            }
            SCUMMParameter object1 = GetVarOrWord(opcode, 0x40);
            SCUMMParameter object2 = GetVarOrWord(opcode, 0x20);
            Add(SCUMMOpcode.C_DoSentence, verb, object1, object2);
        }

        protected void DrawBox(int opcode)
        {
            SCUMMParameter x1 = GetVarOrWord(opcode, 0x80);
            SCUMMParameter y1 = GetVarOrWord(opcode, 0x40);
            byte so = reader.ReadU8();
            SCUMMParameter x2 = GetVarOrWord(so, 0x80);
            SCUMMParameter y2 = GetVarOrWord(so, 0x40);
            SCUMMParameter color = GetVarOrByte(so, 0x20);
            Add(SCUMMOpcode.C_DrawBox, x1, y1, x2, y2, color);
        }

        protected void Fades(int opcode)
        {
            Add(SCUMMOpcode.C_Fades);

            byte subOpcode = reader.ReadU8();

            switch (subOpcode)
            {
                case 0x03:
                    SCUMMParameter param = GetVarOrWord(subOpcode, 0x80);
                    Add(SCUMMOpcode.SC_RoomFade, param);
                    break;
                default:
                    throw UnknownSubOpcode("fades", subOpcode);
            }
        }

        protected void FreezeScripts(int opcode)
        {
            SCUMMParameter arg = GetVarOrByte(opcode, 0x80);
            if (arg.Type == SCUMMParameterType.Number)
            {
                switch ((byte)arg.Value)
                {
                    case 0x00: Add(SCUMMOpcode.C_UnfreezeScripts); return;
                    case 0x7F: Add(SCUMMOpcode.C_FreezeScripts); return;
                }
            }
            Add(SCUMMOpcode.C_FreezeScript, arg);
        }

        protected override void HeapStuff(int opcode)
        {
            Add(SCUMMOpcode.C_HeapStuff);

            byte subOpcode = reader.ReadU8();

            SCUMMParameter resource = null;
            if (subOpcode != 0x11) // SC_HeapClear
            {
                resource = GetVarOrByte(subOpcode, 0x80);
            }
            switch (subOpcode & 0x1f)
            {
                case 0x01: Add(SCUMMOpcode.SC_HeapLoadScript, resource); break;
                case 0x02: Add(SCUMMOpcode.SC_HeapLoadSound, resource); break;
                case 0x03: Add(SCUMMOpcode.SC_HeapLoadCostume, resource); break;
                case 0x04: Add(SCUMMOpcode.SC_HeapLoadRoom, resource); break;
                case 0x05: Add(SCUMMOpcode.SC_HeapNukeScript, resource); break;
                case 0x06: Add(SCUMMOpcode.SC_HeapNukeSound, resource); break;
                case 0x07: Add(SCUMMOpcode.SC_HeapNukeCostume, resource); break;
                case 0x08: Add(SCUMMOpcode.SC_HeapNukeRoom, resource); break;
                case 0x09: Add(SCUMMOpcode.SC_HeapLockScript, resource); break;
                case 0x0a: Add(SCUMMOpcode.SC_HeapLockSound, resource); break;
                case 0x0b: Add(SCUMMOpcode.SC_HeapLockCostume, resource); break;
                case 0x0c: Add(SCUMMOpcode.SC_HeapLockRoom, resource); break;
                case 0x0d: Add(SCUMMOpcode.SC_HeapUnlockScript, resource); break;
                case 0x0e: Add(SCUMMOpcode.SC_HeapUnlockSound, resource); break;
                case 0x0f: Add(SCUMMOpcode.SC_HeapUnlockCostume, resource); break;
                case 0x10: Add(SCUMMOpcode.SC_HeapUnlockRoom, resource); break;
                case 0x11: Add(SCUMMOpcode.SC_HeapClearHeap); break;
                case 0x12: Add(SCUMMOpcode.SC_HeapLoadCharset, resource); break;
                case 0x13: Add(SCUMMOpcode.SC_HeapNukeCharset, resource); break;
                case 0x14:
                    SCUMMParameter objectNo = GetVarOrWord(subOpcode, 0x40);
                    Add(SCUMMOpcode.SC_HeapLoadObject, resource, objectNo);
                    break;
                default:
                    throw UnknownSubOpcode("heap", subOpcode);

            }
        }

        protected override void IfClassOf(int opcode)
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
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfClassOf, obj, new SCUMMParameter(SCUMMParameterType.Array, clss.ToArray()), jumpDest);
        }

        protected void IfInBox(int opcode)
        {
            SCUMMParameter actorNo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter boxNo = GetVarOrByte(opcode, 0x40);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfInBox, actorNo, boxNo, jumpDest);
        }

        protected void LAnd(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter term = GetNumber();
            Add(SCUMMOpcode.C_LAndVariable, var, term);
        }

        protected void LOr(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter term = GetNumber();
            Add(SCUMMOpcode.C_LOrVariable, var, term);
        }

        protected void Mul(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter factor = GetNumber();
            Add(SCUMMOpcode.C_MulVariable, var, factor);
        }

        protected override void OverRide(int opcode)
        {
            SCUMMParameter prm = GetByte();
            if (Convert.ToInt32(prm.Value) == 0)
            {
                Add(SCUMMOpcode.C_OverrideOff);
            }
            else
            {
                Add(SCUMMOpcode.C_Override, prm);
            }
        }

        protected void PickUpObjectInRoom(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            SCUMMParameter roomNo = GetVarOrByte(opcode, 0x40);
            Add(SCUMMOpcode.C_PickUpObjectInRoom, objectNo, roomNo);
        }

        protected override void RoomStuff(int opcode)
        {
            Add(SCUMMOpcode.C_RoomStuff);

            byte subOpcode = reader.ReadU8();

            switch (subOpcode & 0x1f)
            {
                case 0x01:
                    SCUMMParameter x1 = GetVarOrWord(subOpcode, 0x80);
                    SCUMMParameter x2 = GetVarOrWord(subOpcode, 0x40);
                    Add(SCUMMOpcode.SC_RoomScroll, x1, x2);
                    break;
                case 0x02:
                    SCUMMParameter color = GetVarOrWord(subOpcode, 0x80);
                    SCUMMParameter slot = GetVarOrWord(subOpcode, 0x40);
                    Add(SCUMMOpcode.SC_RoomColor, color, slot);
                    break;
                case 0x03:
                    SCUMMParameter screen = GetVarOrWord(subOpcode, 0x80);
                    SCUMMParameter screento = GetVarOrWord(subOpcode, 0x40);
                    Add(SCUMMOpcode.SC_RoomSetScreen, screen, screento);
                    break;
                case 0x04:
                    SCUMMParameter red = GetVarOrWord(subOpcode, 0x80);
                    SCUMMParameter green = GetVarOrWord(subOpcode, 0x40);
                    SCUMMParameter blue = GetVarOrWord(subOpcode, 0x20);
                    byte so2 = reader.ReadU8();
                    slot = GetVarOrByte(so2, 0x80);
                    Add(SCUMMOpcode.SC_RoomPalette, red, green, blue, slot);
                    break;
                case 0x05:
                    Add(SCUMMOpcode.SC_RoomShakeOn);
                    break;
                case 0x06:
                    Add(SCUMMOpcode.SC_RoomShakeOff);
                    break;
                case 0x07:
                    SCUMMParameter scl1 = GetVarOrWord(subOpcode, 0x80);
                    SCUMMParameter scl1At = GetVarOrWord(subOpcode, 0x40);
                    so2 = reader.ReadU8();
                    SCUMMParameter scl2 = GetVarOrWord(so2, 0x80);
                    SCUMMParameter scl2At = GetVarOrWord(so2, 0x40);
                    so2 = reader.ReadU8();
                    SCUMMParameter sclslot = GetVarOrWord(so2, 0x80);
                    Add(SCUMMOpcode.SC_RoomScale, scl1, scl1At, scl2, scl2At, sclslot);
                    break;
                case 0x08:
                    SCUMMParameter intensity = GetVarOrByte(subOpcode, 0x80);
                    SCUMMParameter from = GetVarOrByte(subOpcode, 0x40);
                    SCUMMParameter to = GetVarOrByte(subOpcode, 0x20);
                    Add(SCUMMOpcode.SC_RoomIntensity, intensity, from, to);
                    break;
                case 0x09:
                    SCUMMParameter mode = GetVarOrByte(subOpcode, 0x80);
                    slot = GetVarOrByte(subOpcode, 0x40);
                    Add(SCUMMOpcode.SC_RoomSaveLoadGame, mode, slot);
                    break;
                case 0x0a:
                    SCUMMParameter arg = GetVarOrWord(subOpcode, 0x80);
                    Add(SCUMMOpcode.SC_RoomFade, arg);
                    break;
                case 0x0b:
                    red = GetVarOrByte(subOpcode, 0x80);
                    green = GetVarOrByte(subOpcode, 0x40);
                    blue = GetVarOrByte(subOpcode, 0x20);
                    so2 = reader.ReadU8();
                    from = GetVarOrByte(so2, 0x80);
                    to = GetVarOrByte(so2, 0x40);
                    Add(SCUMMOpcode.SC_RoomRgbIntensity, red, green, blue, from, to);
                    break;
                case 0x0c:
                    red = GetVarOrByte(subOpcode, 0x80);
                    green = GetVarOrByte(subOpcode, 0x40);
                    blue = GetVarOrByte(subOpcode, 0x20);
                    so2 = reader.ReadU8();
                    from = GetVarOrByte(so2, 0x80);
                    to = GetVarOrByte(so2, 0x40);
                    Add(SCUMMOpcode.SC_RoomSetShadowPalette, red, green, blue, from, to);
                    break;
                case 0x0d:
                    SCUMMParameter str = GetVarOrByte(subOpcode, 0x80);
                    SCUMMParameter value = GetString();
                    Add(SCUMMOpcode.SC_RoomSaveString, str, value);
                    break;
                case 0x0e:
                    str = GetVarOrByte(subOpcode, 0x80);
                    value = GetString();
                    Add(SCUMMOpcode.SC_RoomLoadString, str, value);
                    break;
                case 0x0f:
                    SCUMMParameter palstr = GetVarOrByte(subOpcode, 0x80);
                    so2 = reader.ReadU8();
                    SCUMMParameter start = GetVarOrByte(so2, 0x80);
                    SCUMMParameter end = GetVarOrByte(so2, 0x40);
                    so2 = reader.ReadU8();
                    SCUMMParameter step = GetVarOrByte(so2, 0x80);
                    Add(SCUMMOpcode.SC_RoomTransform, palstr, start, end, step);
                    break;
                case 0x10:
                    slot = GetVarOrByte(subOpcode, 0x80);
                    SCUMMParameter speed = GetVarOrByte(subOpcode, 0x80);
                    Add(SCUMMOpcode.SC_RoomCycleSpeed, slot, speed);
                    break;
                default:
                    throw UnknownSubOpcode("room", subOpcode);
            }
        }

        protected override void SayLine(int opcode)
        {
            SCUMMParameter actorNo = GetVarOrByte(opcode, 0x80);
            if (actorNo.Type == SCUMMParameterType.Number && Convert.ToInt32(actorNo.Value) >= 252)
            {
                int actorNoValue = Convert.ToInt32(actorNo.Value);
                switch (actorNoValue)
                {
                    case 252: Add(SCUMMOpcode.C_PrintSystem); break;
                    case 253: Add(SCUMMOpcode.C_PrintDebug); break;
                    case 254: Add(SCUMMOpcode.C_PrintText); break;
                    case 255: Add(SCUMMOpcode.C_PrintLine); break;
                }
            }
            else
            {
                Add(SCUMMOpcode.C_SayLineSimple, actorNo);
            }
            _HandlePrint();
        }

        protected override void SayLineDefault(int opcode)
        {
            Add(SCUMMOpcode.C_SayLineSimpleDefault);
            _HandlePrint();
        }


        protected void StateOf(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            SCUMMParameter state = GetVarOrByte(opcode, 0x40);
            Add(SCUMMOpcode.C_StateOf, objectNo, state);
        }

        protected override uint GetDelay()
        {
            return (uint)(reader.ReadU8() + (reader.ReadU8() << 8) + (reader.ReadU8() << 16));
        }

        protected void StartObject(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            SCUMMParameter verbNo = GetVarOrByte(opcode, 0x40);
            SCUMMParameter prms = GetParams(16, "start-object");
            Add(SCUMMOpcode.C_StartObject, objectNo, verbNo, prms);
        }

        protected override void StartScript(int opcode)
        {
            SCUMMParameter scriptNo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter prms = GetParams(16, "start-script");

            if ((opcode & 0x60) == 0x60)
            {
                Add(SCUMMOpcode.C_StartScriptBakRec, scriptNo, prms);
            }
            else if ((opcode & 0x40) == 0x40)
            {
                Add(SCUMMOpcode.C_StartScriptRec, scriptNo, prms);
            }
            else if ((opcode & 0x20) == 0x20)
            {
                Add(SCUMMOpcode.C_StartScriptBak, scriptNo, prms);
            }
            else
            {
                Add(SCUMMOpcode.C_StartScript, scriptNo, prms);
            }
        }

        protected void StopObject(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_StopObject, objectNo);
        }

        protected void StringStuff(int opcode)
        {
            // TODO: These may be mappable to other commands (Assign? Store?)
            Add(SCUMMOpcode.C_StringStuff);

            byte subOpcode = reader.ReadU8();
            
            switch (subOpcode & 0x1f)
            {
                case 0x01:
                    SCUMMParameter str = GetVarOrByte(subOpcode, 0x80);
                    SCUMMParameter strval = GetString();
                    Add(SCUMMOpcode.C_StringIsStrval, str, strval);
                    break;
                case 0x02:
                    str = GetVarOrByte(subOpcode, 0x80);
                    SCUMMParameter source = GetVarOrByte(subOpcode, 0x40);
                    Add(SCUMMOpcode.C_StringIsStrvar, str, source);
                    break;
                case 0x03:
                    str = GetVarOrByte(subOpcode, 0x80);
                    SCUMMParameter index = GetVarOrByte(subOpcode, 0x40);
                    SCUMMParameter val = GetVarOrByte(subOpcode, 0x20);
                    Add(SCUMMOpcode.C_StringIsVal, str, index, val);
                    break;
                case 0x04:
                    SCUMMParameter var = GetVar();
                    str = GetVarOrByte(subOpcode, 0x80);
                    index = GetVarOrByte(subOpcode, 0x40);
                    Add(SCUMMOpcode.C_VarIsString, var, str, index);
                    break;
                case 0x05:
                    str = GetVarOrByte(subOpcode, 0x80);
                    SCUMMParameter len = GetVarOrByte(subOpcode, 0x40);
                    Add(SCUMMOpcode.C_DimString, str, len);
                    break;
                default:
                    throw UnknownSubOpcode("string-stuff", subOpcode);

            }
        }

        protected void System(int opcode)
        {
            Add(SCUMMOpcode.C_System);

            byte subOpcode = reader.ReadU8();
            switch (subOpcode)
            {
                case 0x01: Add(SCUMMOpcode.SC_SystemRestart); break;
                case 0x02: Add(SCUMMOpcode.SC_SystemPause); break;
                case 0x03: Add(SCUMMOpcode.SC_SystemQuit); break;
                default:
                    throw UnknownSubOpcode("system", subOpcode);
            }
        }

        protected override void Userface(int opcode)
        {
            Add(SCUMMOpcode.C_Userface);
            byte subOpcode = reader.ReadU8();

            switch (subOpcode & 0x1f)
            {
                case 0x01: Add(SCUMMOpcode.SC_CursorOn); break;
                case 0x02: Add(SCUMMOpcode.SC_CursorOff); break;
                case 0x03: Add(SCUMMOpcode.SC_UserputOn); break;
                case 0x04: Add(SCUMMOpcode.SC_UserputOff); break;
                case 0x05: Add(SCUMMOpcode.SC_CursorSoftOn); break;
                case 0x06: Add(SCUMMOpcode.SC_CursorSoftOff); break;
                case 0x07: Add(SCUMMOpcode.SC_UserputSoftOn); break;
                case 0x08: Add(SCUMMOpcode.SC_UserputSoftOff); break;
                case 0x09: Add(SCUMMOpcode.SC_UserputClear); break;
                case 0x0a: 
                    SCUMMParameter cursor = GetVarOrByte(subOpcode, 0x80);
                    SCUMMParameter letter = GetVarOrByte(subOpcode, 0x40);
                    Add(SCUMMOpcode.SC_CursorImage_3, cursor, letter);
                    break;
                case 0x0b:
                    cursor = GetVarOrByte(subOpcode, 0x80);
                    SCUMMParameter x = GetVarOrByte(subOpcode, 0x40);
                    SCUMMParameter y = GetVarOrByte(subOpcode, 0x20);
                    Add(SCUMMOpcode.SC_CursorHotspot, cursor, x, y);
                    break;
                case 0x0c:
                    cursor = GetVarOrByte(subOpcode, 0x80);
                    Add(SCUMMOpcode.SC_CursorSet, cursor);
                    break;
                case 0x0d:
                    SCUMMParameter charset = GetVarOrByte(subOpcode, 0x80);
                    Add(SCUMMOpcode.SC_CharsetSet, charset);
                    break;
                case 0x0e:
                    SCUMMParameter colors = GetParams(16, "charset color");
                    Add(SCUMMOpcode.SC_CharsetColor, colors);
                    break;
                default:
                    throw UnknownSubOpcode("userface", subOpcode);
            }
        }

        protected void VerbSets(int opcode)
        {
            Add(SCUMMOpcode.C_VerbSets);
            byte subOpcode = reader.ReadU8();
            SCUMMParameter start = GetVarOrByte(subOpcode, 0x80);
            SCUMMParameter end = GetVarOrByte(subOpcode, 0x40);
            SCUMMParameter set = GetVarOrByte(subOpcode, 0x20);

            switch (subOpcode & 0x1f)
            {
                case 0x01: Add(SCUMMOpcode.SC_VerbsSave, start, end, set); break;
                case 0x02: Add(SCUMMOpcode.SC_VerbsRestore, start, end, set); break;
                case 0x03: Add(SCUMMOpcode.SC_VerbsDelete, start, end, set); break;
                default:
                    throw UnknownSubOpcode("verb-sets", subOpcode);
            }
        }

        protected override void VerbStuff(int opcode)
        {
            SCUMMParameter verb = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.C_VerbStuff, verb);

            while (true)
            {
                byte subOpcode = reader.ReadU8();
                if (subOpcode == 0xff)
                {
                    break;
                }

                switch (subOpcode & 0x1f)
                {
                    case 0x01:
                        SCUMMParameter obj = GetVarOrWord(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_VerbImage_3, obj);
                        break;
                    case 0x02:
                        SCUMMParameter verbname = GetString();
                        Add(SCUMMOpcode.SC_VerbName, verbname);
                        break;
                    case 0x03:
                        SCUMMParameter color = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_VerbColor, color);
                        break;
                    case 0x04:
                        color = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_VerbHicolor, color);
                        break;
                    case 0x05:
                        SCUMMParameter x = GetVarOrWord(subOpcode, 0x80);
                        SCUMMParameter y = GetVarOrWord(subOpcode, 0x40);
                        Add(SCUMMOpcode.SC_VerbAt, x, y);
                        break;
                    case 0x06:
                        Add(SCUMMOpcode.SC_VerbOn);
                        break;
                    case 0x07:
                        Add(SCUMMOpcode.SC_VerbOff);
                        break;
                    case 0x08:
                        Add(SCUMMOpcode.SC_VerbDelete);
                        break;
                    case 0x09:
                        Add(SCUMMOpcode.SC_VerbNew);
                        break;
                    case 0x10:
                        color = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_VerbDimColor, color);
                        break;
                    case 0x11:
                        Add(SCUMMOpcode.SC_VerbDim);
                        break;
                    case 0x12:
                        SCUMMParameter key = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_VerbKey, key);
                        break;
                    case 0x13:
                        Add(SCUMMOpcode.SC_VerbCenter);
                        break;
                    case 0x14:
                        SCUMMParameter str = GetVarOrWord(subOpcode, 0x80);
                        // May have more
                        Add(SCUMMOpcode.SC_VerbNameStr, str);
                        break;
                    case 0x15:
                        Add(SCUMMOpcode.SC_VerbNextTo);
                        break;
                    case 0x16:
                        obj = GetVarOrWord(subOpcode, 0x80);
                        SCUMMParameter room = GetVarOrByte(subOpcode, 0x40);
                        // TODO: Not sure if this is correct opcode
                        Add(SCUMMOpcode.SC_VerbImage, obj, room);
                        break;
                    case 0x17:
                        color = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_VerbBkColor, color);
                        break;
                    default:
                        throw UnknownSubOpcode("verb", subOpcode);
                }
            }
        }

        // ---- Functions ----
        protected void FActorWidth(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_ActorWidth, var, actor);
        }

        protected override void FActorX(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrWord(opcode, 0x80); // Seems this is also for objects
            Add(SCUMMOpcode.F_ActorX, var, actor);
        }

        protected override void FActorY(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrWord(opcode, 0x80); // Seems this is also for objects
            Add(SCUMMOpcode.F_ActorY, var, actor);
        }

        protected void FAnimationCounter(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_AnimationCounter, var, actor);
        }

        protected void FFindInventory(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter slot = GetVarOrByte(opcode, 0x40);
            Add(SCUMMOpcode.F_FindInventory, var, actor, slot);
        }

        protected void FInventorySize(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_InventorySize, var, actor);
        }

        protected void FStateOf(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter objectNo = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_StateOf, var, objectNo);
        }

        protected void FStringWidth(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter str = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_StringWidth, var, str);
        }

        protected void FValidVerb(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            SCUMMParameter verbNo = GetVarOrWord(opcode, 0x40);

            Add(SCUMMOpcode.F_ValidVerb, var, objectNo, verbNo);
        }
    }
}
