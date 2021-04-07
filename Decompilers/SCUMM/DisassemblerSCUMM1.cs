using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Decompilers.SCUMM
{
    public class DisassemblerSCUMM1 : Disassembler
    {
        public DisassemblerSCUMM1() : base()
        {
            GetString = GetStringOld;
            GetVar = GetVarOld;
        }

        protected override void InitOpcodes()
        {
            // TODO: Check relation operators and use of negation across the board (>= <= > < == != IfNot NotStateOf etc.)
            // SCUMMVM note: jumpRelative(condition) jumps if condition is NOT true, but note that it also inverts terms (e.g. b > a)
            // Our convention is that If jumps if the condition is true. I.e. If (a > b) means "jump if a is greater than b".
            // TODO: Replace all coordinate reads with e.g. GetCoord - this allows us to reuse a LOT between SCUMM1 and SCUMM3.
            AddOpcode(0x00, EndObject);
            AddOpcode(0x01, PutActorAtXY);
            AddOpcode(0x02, StartMusic);
            AddOpcode(0x03, FActorRoom);
            AddOpcode(0x04, IfGt);
            AddOpcode(0x05, DrawObject);
            AddOpcode(0x06, FActorElevation);
            AddOpcode(0x07, StateOf8Set);
            AddOpcode(0x08, IfEq);
            AddOpcode(0x09, FaceTowards);
            AddOpcode(0x0a, StoreVariableIndirect);
            AddOpcode(0x0b, VerbOf);
            AddOpcode(0x0c, HeapStuff);
            AddOpcode(0x0d, WalkToActor);
            AddOpcode(0x0e, PutActorAtObject);
            AddOpcode(0x0f, IfStateOf8);
            AddOpcode(0x10, FOwnerOf);
            AddOpcode(0x11, DoAnimation);
            AddOpcode(0x12, CameraPanTo);
            AddOpcode(0x13, ActorStuff);
            AddOpcode(0x14, SayLine);
            AddOpcode(0x15, FFindActor);
            AddOpcode(0x16, FRandom);
            AddOpcode(0x17, StateOf2Clear);
            AddOpcode(0x18, Jump);
            AddOpcode(0x19, DoSentence);
            AddOpcode(0x1a, StoreVariable);
            AddOpcode(0x1b, StoreBitVariable);
            AddOpcode(0x1c, StartSound);
            AddOpcode(0x1d, IfClassOf); // Really IfNotClassOf?
            AddOpcode(0x1e, WalkToXY);
            AddOpcode(0x1f, IfNotStateOf2);
            AddOpcode(0x20, StopMusic);
            AddOpcode(0x21, PutActorAtXY);
            AddOpcode(0x22, SaveLoadVariables);
            AddOpcode(0x23, FActorY);
            AddOpcode(0x24, ComeOutDoor);
            AddOpcode(0x25, DrawObject);
            AddOpcode(0x26, AssignArray);
            AddOpcode(0x27, StateOf4Set);
            AddOpcode(0x28, IfVar);
            AddOpcode(0x29, OwnerOf);
            AddOpcode(0x2a, AddIndirect);
            AddOpcode(0x2b, SleepForVar);
            AddOpcode(0x2c, StoreByteVariable);
            AddOpcode(0x2d, PutActorInRoom);
            AddOpcode(0x2e, SleepFor);
            AddOpcode(0x2f, IfStateOf4);
            AddOpcode(0x30, SetBox);
            AddOpcode(0x31, GetBitVariable);
            AddOpcode(0x32, CameraAt);
            AddOpcode(0x33, RoomStuff);
            AddOpcode(0x34, FProximity2ActObjs);
            AddOpcode(0x35, FFindObject);
            AddOpcode(0x36, WalkToObject);
            AddOpcode(0x37, StateOf1Set);
            AddOpcode(0x38, IfLt);
            AddOpcode(0x39, DoSentence);
            AddOpcode(0x3a, Sub);
            AddOpcode(0x3b, WaitForActor);
            AddOpcode(0x3c, StopSound);
            AddOpcode(0x3d, ActorElevation);
            AddOpcode(0x3e, WalkToXY);
            AddOpcode(0x3f, IfStateOf1);
            AddOpcode(0x40, CutScene);
            AddOpcode(0x41, PutActorAtXY);
            AddOpcode(0x42, StartScript);
            AddOpcode(0x43, FActorX);
            AddOpcode(0x44, IfLeq);
            AddOpcode(0x45, DrawObject);
            AddOpcode(0x46, Inc);
            AddOpcode(0x47, StateOf8Clear);
            AddOpcode(0x48, IfNeq);
            AddOpcode(0x49, FaceTowards);
            AddOpcode(0x4a, ChainScript);
            AddOpcode(0x4b, VerbOf);
            AddOpcode(0x4c, WaitForSentence);
            AddOpcode(0x4d, WalkToActor);
            AddOpcode(0x4e, PutActorAtObject);
            AddOpcode(0x4f, IfNotStateOf8);
            AddOpcode(0x50, PickUpObject);
            AddOpcode(0x51, DoAnimation);
            AddOpcode(0x52, CameraFollow);
            AddOpcode(0x53, ActorStuff);
            AddOpcode(0x54, NewNameOf);
            AddOpcode(0x55, FFindActor);
            AddOpcode(0x56, FActorMoving);
            AddOpcode(0x57, StateOf2Set);
            AddOpcode(0x58, OverRide);
            AddOpcode(0x59, DoSentence);
            AddOpcode(0x5a, Add);
            AddOpcode(0x5b, StoreBitVariable);
            //AddOpcode(0x5c, Return);
            AddOpcode(0x5d, IfNotClassOf);
            AddOpcode(0x5e, WalkToXY);
            AddOpcode(0x5f, IfStateOf2);
            AddOpcode(0x60, Userface);
            AddOpcode(0x61, PutActorAtXY);
            AddOpcode(0x62, StopScript);
            AddOpcode(0x63, FActorFacing);
            AddOpcode(0x64, ComeOutDoor);
            AddOpcode(0x65, DrawObject);
            AddOpcode(0x66, FClosestActor);
            AddOpcode(0x67, StateOf4Clear);
            AddOpcode(0x68, FScriptRunning);
            AddOpcode(0x69, OwnerOf);
            AddOpcode(0x6a, SubIndirect);
            //AddOpcode(0x6b, _Debug);
            AddOpcode(0x6c, FVerbOf);
            AddOpcode(0x6d, PutActorInRoom);
            //AddOpcode(0x6e, StopObject);
            AddOpcode(0x6f, IfNotStateOf4);
            AddOpcode(0x70, Lights);
            AddOpcode(0x71, FActorCostume);
            AddOpcode(0x72, CameraInRoom); // SCUMMVM: LoadRoom? CurrentRoom?
            AddOpcode(0x73, RoomStuff);
            AddOpcode(0x74, FProximity2ActObjs);
            AddOpcode(0x75, FFindObject);
            AddOpcode(0x76, WalkToObject);
            AddOpcode(0x77, StateOf1Clear);
            AddOpcode(0x78, IfGeq);
            AddOpcode(0x79, DoSentence);
            AddOpcode(0x7a, VerbStuff);
            AddOpcode(0x7b, FActorBox);
            AddOpcode(0x7c, FSoundRunning);
            AddOpcode(0x7d, ActorElevation);
            AddOpcode(0x7e, WalkToXY);
            AddOpcode(0x7f, IfNotStateOf1);
            AddOpcode(0x80, BreakHere);
            AddOpcode(0x81, PutActorAtXY);
            AddOpcode(0x82, StartMusic);
            AddOpcode(0x83, FActorRoom);
            AddOpcode(0x84, IfGt);
            AddOpcode(0x85, DrawObject);
            AddOpcode(0x86, FActorElevation);
            AddOpcode(0x87, StateOf8Set);
            AddOpcode(0x88, IfEq);
            AddOpcode(0x89, FaceTowards);
            AddOpcode(0x8a, StoreVariableIndirect);
            AddOpcode(0x8b, VerbOf);
            AddOpcode(0x8c, HeapStuff);
            AddOpcode(0x8d, WalkToActor);
            AddOpcode(0x8e, PutActorAtObject);
            AddOpcode(0x8f, IfStateOf8);
            AddOpcode(0x90, FOwnerOf);
            AddOpcode(0x91, DoAnimation);
            AddOpcode(0x92, CameraPanTo);
            AddOpcode(0x93, ActorStuff);
            AddOpcode(0x94, SayLine);
            AddOpcode(0x95, FFindActor);
            AddOpcode(0x96, FRandom);
            AddOpcode(0x97, StateOf2Clear);
            AddOpcode(0x98, Restart);
            AddOpcode(0x99, DoSentence);
            AddOpcode(0x9a, StoreVariable);
            AddOpcode(0x9b, StoreBitVariable);
            AddOpcode(0x9c, StartSound);
            AddOpcode(0x9d, IfNotClassOf);
            AddOpcode(0x9e, WalkToXY);
            AddOpcode(0x9f, IfNotStateOf2);
            AddOpcode(0xa0, EndScript);
            AddOpcode(0xa1, PutActorAtXY);
            AddOpcode(0xa2, SaveLoadVariables);
            AddOpcode(0xa3, FActorY);
            AddOpcode(0xa4, ComeOutDoor);
            AddOpcode(0xa5, DrawObject);
            AddOpcode(0xa6, AssignArray);
            AddOpcode(0xa7, StateOf4Set);
            AddOpcode(0xa8, IfNotVar);
            AddOpcode(0xa9, OwnerOf);
            AddOpcode(0xaa, AddIndirect);
            AddOpcode(0xab, SwitchCostumeSet); //TODO: Only C64 and NES
            AddOpcode(0xac, DrawSentence);
            AddOpcode(0xad, PutActorInRoom);
            AddOpcode(0xae, WaitForMessage);
            AddOpcode(0xaf, IfStateOf4);
            AddOpcode(0xb0, SetBox);
            AddOpcode(0xb1, GetBitVariable);
            AddOpcode(0xb2, CameraAt);
            AddOpcode(0xb3, RoomStuff);
            AddOpcode(0xb4, FProximity2ActObjs);
            AddOpcode(0xb5, FFindObject);
            AddOpcode(0xb6, WalkToObject);
            AddOpcode(0xb7, StateOf1Set);
            AddOpcode(0xb8, IfLt);
            AddOpcode(0xb9, DoSentence);
            AddOpcode(0xba, Sub);
            AddOpcode(0xbb, WaitForActor);
            AddOpcode(0xbc, StopSound);
            AddOpcode(0xbd, ActorElevation);
            AddOpcode(0xbe, WalkToXY);
            AddOpcode(0xbf, IfStateOf1);
            AddOpcode(0xc0, EndCutScene);
            AddOpcode(0xc1, PutActorAtXY);
            AddOpcode(0xc2, StartScript);
            AddOpcode(0xc3, FActorX);
            AddOpcode(0xc4, IfLeq);
            AddOpcode(0xc5, DrawObject);
            AddOpcode(0xc6, Dec);
            AddOpcode(0xc7, StateOf8Clear);
            AddOpcode(0xc8, IfNeq);
            AddOpcode(0xc9, FaceTowards);
            AddOpcode(0xca, ChainScript);
            AddOpcode(0xcb, VerbOf);
            AddOpcode(0xcc, Pseudoroom);
            AddOpcode(0xcd, WalkToActor);
            AddOpcode(0xce, PutActorAtObject);
            AddOpcode(0xcf, IfNotStateOf8);
            AddOpcode(0xd0, PickUpObject);
            AddOpcode(0xd1, DoAnimation);
            AddOpcode(0xd2, CameraFollow);
            AddOpcode(0xd3, ActorStuff);
            AddOpcode(0xd4, NewNameOf);
            AddOpcode(0xd5, FFindActor);
            AddOpcode(0xd6, FActorMoving);
            AddOpcode(0xd7, StateOf2Set);
            AddOpcode(0xd8, SayLineDefault);
            AddOpcode(0xd9, DoSentence);
            AddOpcode(0xda, Add);
            AddOpcode(0xdb, StoreBitVariable);
            //AddOpcode(0xdc, Unknown);
            AddOpcode(0xdd, IfNotClassOf);
            AddOpcode(0xde, WalkToXY);
            AddOpcode(0xdf, IfStateOf2);
            AddOpcode(0xe0, Userface);
            AddOpcode(0xe1, PutActorAtXY);
            AddOpcode(0xe2, StopScript);
            AddOpcode(0xe3, FActorFacing);
            AddOpcode(0xe4, ComeOutDoor);
            AddOpcode(0xe5, DrawObject);
            AddOpcode(0xe6, FClosestActor);
            AddOpcode(0xe7, StateOf4Clear);
            AddOpcode(0xe8, FScriptRunning);
            AddOpcode(0xe9, OwnerOf);
            AddOpcode(0xea, SubIndirect);
            //AddOpcode(0xeb, _Debug);
            AddOpcode(0xec, FVerbOf);
            AddOpcode(0xed, PutActorInRoom);
            //AddOpcode(0xee, StopObject);
            AddOpcode(0xef, IfNotStateOf4);
            AddOpcode(0xf0, Lights);
            AddOpcode(0xf1, FActorCostume);
            AddOpcode(0xf2, CameraInRoom);
            AddOpcode(0xf3, RoomStuff);
            AddOpcode(0xf4, FProximity2ActObjs);
            AddOpcode(0xf5, FFindObject);
            AddOpcode(0xf6, WalkToObject);
            AddOpcode(0xf7, StateOf1Clear);
            AddOpcode(0xf8, IfGeq);
            AddOpcode(0xf9, DoSentence);
            AddOpcode(0xfa, VerbStuff);
            AddOpcode(0xfb, FActorBox);
            AddOpcode(0xfc, FSoundRunning);
            AddOpcode(0xfd, ActorElevation);
            AddOpcode(0xfe, WalkToXY);
            AddOpcode(0xff, IfNotStateOf1);
        }

        protected virtual SCUMMParameter GetCoordinateVarOrLiteral(int opcode, int mask)
        {
            return GetVarOrByte(opcode, mask);
        }

        protected virtual SCUMMParameter GetCoordinateLiteral()
        {
            return GetByte();
        }

        protected void ActorElevation(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter elevation = GetVarOrByte(opcode, 0x40);
            Add(SCUMMOpcode.C_ActorElevation, actor, elevation);
        }

        protected virtual void ActorStuff(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter arg = GetVarOrByte(opcode, 0x40);
            
            Add(SCUMMOpcode.C_ActorStuff, actor);

            var subOpcode = reader.ReadU8();

            switch (subOpcode)
            {
                case 1:
                    Add(SCUMMOpcode.SC_ActorSound_3, arg);
                    break;
                case 2:
                    // TODO: Version 1 has no slot (it uses actor ID as the slot)
                    var slot = GetByte();
                    Add(SCUMMOpcode.SC_ActorPalette, slot, arg);
                    break;
                case 3: // Doesn't use arg?
                    Add(SCUMMOpcode.SC_ActorName, GetString());
                    break;
                case 4:
                    Add(SCUMMOpcode.SC_ActorCostume, arg);
                    break;
                case 5:
                    Add(SCUMMOpcode.SC_ActorTalkColor, arg);
                    break;
                default:
                    throw UnknownSubOpcode("actor", subOpcode);
            }
        }

        protected void Add(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter term = GetNumber();
            Add(SCUMMOpcode.C_AddVariable, var, term);
        }

        protected void AddIndirect(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter term = GetNumber();
            Add(SCUMMOpcode.C_AddVariableIndirect, var, term);
        }

        protected void AssignArray(int opcode)
        {
            SCUMMParameter var = GetVar();
            byte count = reader.ReadU8();
            SCUMMParameter[] values = new SCUMMParameter[count];

            for (int i = 0; i < count; i++)
            {
                if ((opcode & 0x80) != 0)
                {
                    values[i] = GetNumber();
                }
                else
                {
                    values[i] = GetByte();
                }
            }

            SCUMMParameter arr = new SCUMMParameter(SCUMMParameterType.Array, values);

            Add(SCUMMOpcode.C_AssignArray, var, arr);
        }

        protected void AssignArray_Word(int opcode)
        {
            SCUMMParameter var = GetVar();
            byte count = reader.ReadU8();
            SCUMMParameter[] values = new SCUMMParameter[count];

            for (int i = 0; i < count; i++)
            {
                values[i] = GetByte();
            }

            SCUMMParameter arr = new SCUMMParameter(SCUMMParameterType.Array, values);

            Add(SCUMMOpcode.C_AssignArray, var, arr);
        }

        protected void BreakHere(int opcode)
        {
            int count = 1;
            while (reader.ReadU8() == opcode) // Multiple breaks are written as break-here <count>
            {
                count++;
            }
            reader.Position = reader.Position - 1; // Track back

            if (count > 1)
            {
                Add(SCUMMOpcode.C_BreakHereCount, new SCUMMParameter(SCUMMParameterType.Number, count));
            }
            else
            {
                Add(SCUMMOpcode.C_BreakHere);
            }
        }

        protected void CameraAt(int opcode)
        {
            SCUMMParameter x = GetCoordinateVarOrLiteral(opcode, 0x80);
            Add(SCUMMOpcode.C_CameraAt, x);
        }

        protected void CameraFollow(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.C_CameraFollow, actor);
        }

        protected void CameraInRoom(int opcode)
        {
            SCUMMParameter roomNo = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.C_CameraInRoom, roomNo);
        }

        protected void CameraPanTo(int opcode)
        {
            SCUMMParameter x = GetCoordinateVarOrLiteral(opcode, 0x80);
            Add(SCUMMOpcode.C_CameraPanTo, x);
        }

        protected virtual void ChainScript(int opcode)
        {
            SCUMMParameter scriptNo = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.C_ChainScript, scriptNo);
        }

        protected void ComeOutDoor(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            SCUMMParameter roomNo = GetVarOrByte(opcode, 0x40);
            SCUMMParameter walkToX = GetCoordinateLiteral();
            SCUMMParameter walkToY = GetCoordinateLiteral();
            Add(SCUMMOpcode.C_ComeOutDoor, objectNo, roomNo, walkToX, walkToY);
        }

        protected virtual void CutScene(int opcode)
        {
            Add(SCUMMOpcode.C_CutScene);
        }

        protected void Dec(int opcode)
        {
            SCUMMParameter var = GetVar();
            Add(SCUMMOpcode.C_DecVariable, var);
        }

        protected void DoAnimation(int opcode)
        {
            // C_DoAnimation (var1, 0) seems to do *something* wrong...
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter anim = GetVarOrByte(opcode, 0x40);
            Add(SCUMMOpcode.C_DoAnimation, actor, anim);
        }

        protected virtual void DoSentence(int opcode)
        {
            SCUMMParameter verb = GetVarOrByte(opcode, 0x80);

            // TODO: This one is nasty... variable value may dictate amount of bytes in code (but does it ever?)
            if (verb.Type == SCUMMParameterType.Number)
            {
                int verbValue = Convert.ToInt32(verb.Value);
                switch (verbValue)
                {
                    case 0xfb:
                        Add(SCUMMOpcode.C_StopSentence);
                        return;
                    case 0xfc:
                        Add(SCUMMOpcode.C_ResetSentence);
                        break;
                }
            }
            SCUMMParameter object1 = GetVarOrWord(opcode, 0x40);
            SCUMMParameter object2 = GetVarOrWord(opcode, 0x20);
            Add(SCUMMOpcode.C_DoSentence, verb, object1, object2);
        }

        protected void DrawSentence(int opcode)
        {
            Add(SCUMMOpcode.C_DrawSentence);
        }

        protected virtual void DrawObject(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            SCUMMParameter x = GetCoordinateVarOrLiteral(opcode, 0x40);
            SCUMMParameter y = GetCoordinateVarOrLiteral(opcode, 0x20);
            Add(SCUMMOpcode.C_DrawObjectAt, objectNo, x, y);
        }

        protected void EndCutScene(int opcode)
        {
            Add(SCUMMOpcode.C_EndCutScene);
        }

        protected void EndObject(int opcode)
        {
            Add(SCUMMOpcode.C_EndObject);
        }

        protected void EndScript(int opcode)
        {
            Add(SCUMMOpcode.C_EndScript);
        }

        protected void FaceTowards(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter actobj = GetVarOrWord(opcode, 0x40);
            Add(SCUMMOpcode.C_FaceTowards, actor, actobj);
        }

        protected void GetBitVariable(int opcode)
        {
            SCUMMParameter varAssign = GetVar();
            SCUMMParameter bitvar = GetNumber();
            SCUMMParameter offs = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.C_GetBitVariable, varAssign, bitvar, offs);
        }

        protected virtual void HeapStuff(int opcode)
        {
            Add(SCUMMOpcode.C_HeapStuff);

            SCUMMParameter resource = GetVarOrByte(opcode, 0x80);
            byte subOpcode = reader.ReadU8();

            switch (subOpcode)
            {
                case 0x21: Add(SCUMMOpcode.SC_HeapLoadCostume, resource); break;
                case 0x22: Add(SCUMMOpcode.SC_HeapUnlockCostume, resource); break;
                case 0x23: Add(SCUMMOpcode.SC_HeapLockCostume, resource); break;

                case 0x31: Add(SCUMMOpcode.SC_HeapLoadRoom, resource); break;
                case 0x32: Add(SCUMMOpcode.SC_HeapUnlockRoom, resource); break;
                case 0x33: Add(SCUMMOpcode.SC_HeapLockRoom, resource); break;

                case 0x51: Add(SCUMMOpcode.SC_HeapLoadScript, resource); break;
                case 0x52: Add(SCUMMOpcode.SC_HeapUnlockScript, resource); break;
                case 0x53: Add(SCUMMOpcode.SC_HeapLockScript, resource); break;

                case 0x61: Add(SCUMMOpcode.SC_HeapLoadSound, resource); break;
                case 0x62: Add(SCUMMOpcode.SC_HeapUnlockSound, resource); break;
                case 0x63: Add(SCUMMOpcode.SC_HeapLockSound, resource); break;

                default:
                    throw UnknownSubOpcode("heap", subOpcode);

            }
        }

        protected virtual void IfClassOf(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            SCUMMParameter clss = GetVarOrWord(opcode, 0x40);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfClassOf, obj, clss, jumpDest);
        }

        protected virtual void IfNotClassOf(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            SCUMMParameter clss = GetVarOrWord(opcode, 0x40);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfNotClassOf, obj, clss, jumpDest);
        }

        protected void IfEq(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter varOrLit = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfEq, var, varOrLit, jumpDest);
        }

        protected void IfGeq(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter varOrLit = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfGeq, var, varOrLit, jumpDest);
        }

        protected void IfGt(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter varOrLit = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfGt, var, varOrLit, jumpDest);
        }

        protected void IfLeq(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter varOrLit = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfLeq, var, varOrLit, jumpDest);
        }

        protected void IfLt(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter varOrLit = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfLt, var, varOrLit, jumpDest);
        }

        protected void IfNeq(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter varOrLit = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfNeq, var, varOrLit, jumpDest);
        }

        protected void IfNotStateOf1(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfNotStateOf1, obj, jumpDest);
        }

        protected void IfNotStateOf2(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfNotStateOf2, obj, jumpDest);
        }

        protected void IfNotStateOf4(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfNotStateOf4, obj, jumpDest);
        }

        protected void IfNotStateOf8(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfNotStateOf8, obj, jumpDest);
        }

        protected void IfStateOf1(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfStateOf1, obj, jumpDest);
        }

        protected void IfStateOf2(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfStateOf2, obj, jumpDest);
        }

        protected void IfStateOf4(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfStateOf4, obj, jumpDest);
        }

        protected void IfStateOf8(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfStateOf8, obj, jumpDest);
        }

        protected void IfNotVar(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfNotVar, var, jumpDest);
        }

        protected void IfVar(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_IfVar, var, jumpDest);
        }

        protected void Inc(int opcode)
        {
            SCUMMParameter var = GetVar();
            Add(SCUMMOpcode.C_IncVariable, var);
        }

        protected void Jump(int opcode)
        {
            SCUMMParameter jumpDest = GetNumberSigned();
            Add(SCUMMOpcode.C_Jump, jumpDest);
        }

        protected void Lights(int opcode)
        {
            SCUMMParameter lo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter hi = GetByte();
            SCUMMParameter a = GetByte();
            Add(SCUMMOpcode.C_Lights, lo, hi, a);
        }

        protected void NewNameOf(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            SCUMMParameter str = GetString();
            Add(SCUMMOpcode.C_NewNameOf, obj, str);
        }

        protected virtual void OverRide(int opcode)
        {
            Add(SCUMMOpcode.C_Override);
        }

        protected void OwnerOf(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            SCUMMParameter owner = GetVarOrByte(opcode, 0x40);
            Add(SCUMMOpcode.C_OwnerOf, objectNo, owner);
        }

        protected void PickUpObject(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_PickUpObject, objectNo);
        }

        protected void Pseudoroom(int opcode)
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

            Add(SCUMMOpcode.C_Pseudoroom, room, new SCUMMParameter(SCUMMParameterType.Array, rooms.ToArray()));
        }

        protected void PutActorAtObject(int opcode)
        {
            SCUMMParameter actorNo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x40);
            Add(SCUMMOpcode.C_PutActorAtObject, actorNo, objectNo);
        }

        protected void PutActorAtXY(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter x = GetCoordinateVarOrLiteral(opcode, 0x40);
            SCUMMParameter y = GetCoordinateVarOrLiteral(opcode, 0x20);
            Add(SCUMMOpcode.C_PutActorAtXY, actor, x, y);
        }

        protected void PutActorInRoom(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter room = GetVarOrByte(opcode, 0x40);
            Add(SCUMMOpcode.C_PutActorInRoom, actor, room);
        }

        protected void Restart(int opcode)
        {
            Add(SCUMMOpcode.C_Restart);
        }

        protected virtual void RoomStuff(int opcode)
        {
            Add(SCUMMOpcode.C_RoomStuff);
            SCUMMParameter a = GetVarOrByte(opcode, 0x80);
            SCUMMParameter b = GetVarOrByte(opcode, 0x40);
            byte subOpcode = reader.ReadU8();

            switch (subOpcode & 0x1f)
            {
                case 0x01:
                    Add(SCUMMOpcode.SC_RoomScroll, a, b);
                    break;
                case 0x02:
                    Add(SCUMMOpcode.SC_RoomColor, a, b);
                    break;
                default:
                    throw UnknownSubOpcode("room", subOpcode);
            }
        }

        protected void SaveLoadVariables(int opcode)
        {
            Add(SCUMMOpcode.C_SaveLoadVariables);
        }

        protected virtual void SayLine(int opcode)
        {
            SCUMMParameter actorNo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter line = GetString();
            if (actorNo.Type == SCUMMParameterType.Number && Convert.ToInt32(actorNo.Value) >= 252)
            {
                int actorNoValue = Convert.ToInt32(actorNo.Value);
                switch (actorNoValue)
                {
                    // TODO: These should have "Simple" opcode variations
                    case 252: Add(SCUMMOpcode.C_PrintSystem, line); break;
                    case 253: Add(SCUMMOpcode.C_PrintDebug, line); break;
                    case 254: Add(SCUMMOpcode.C_PrintText, line); break;
                    case 255: Add(SCUMMOpcode.C_PrintLine, line); break;
                }
            }
            else
            {
                Add(SCUMMOpcode.C_SayLineSimple, actorNo, line);
            }
        }

        protected virtual void SayLineDefault(int opcode)
        {
            SCUMMParameter line = GetString();
            Add(SCUMMOpcode.C_SayLineSimpleDefault, line);
        }

        protected void SetBox(int opcode)
        {
            Add(SCUMMOpcode.C_BoxStuff);

            byte subOpcode = reader.ReadU8();

            switch (subOpcode & 0x1f)
            {
                case 0x01: Add(SCUMMOpcode.SC_SetBoxSpecial); break;
                case 0x02: Add(SCUMMOpcode.SC_SetBoxScale); break;
                case 0x03:
                    SCUMMParameter value = GetVarOrByte(subOpcode, 0x80);
                    Add(SCUMMOpcode.SC_SetBoxSlot, value);
                    break;
                case 0x04: Add(SCUMMOpcode.SC_SetBoxPath); break;
                default:
                    throw UnknownSubOpcode("set-box", subOpcode);
            }
        }

        protected virtual uint GetDelay()
        {
            return (uint)(0xffffff - (reader.ReadU8() + (reader.ReadU8() << 8) + (reader.ReadU8() << 16)));
        }

        protected void SleepFor(int opcode)
        {
            uint t = GetDelay();
            SCUMMParameter time;

            if (t % 3600 == 0)
            {
                time = new SCUMMParameter(SCUMMParameterType.Number, t / 3600);
                Add(SCUMMOpcode.C_SleepMinutes, time);
            }
            else if (t % 60 == 0)
            {
                time = new SCUMMParameter(SCUMMParameterType.Number, t / 60);
                Add(SCUMMOpcode.C_SleepSeconds, time);
            }
            else
            {
                time = new SCUMMParameter(SCUMMParameterType.Number, t);
                Add(SCUMMOpcode.C_SleepJiffies, time);
            }
        }

        protected void SleepForVar(int opcode)
        {
            SCUMMParameter var = GetVar();
            Add(SCUMMOpcode.C_SleepForVar, var);
        }

        protected void StartMusic(int opcode)
        {
            SCUMMParameter musicNo = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.C_StartMusic, musicNo);
        }

        protected virtual void StartScript(int opcode)
        {
            SCUMMParameter scriptNo = GetVarOrByte(opcode, 0x80);

            Add(SCUMMOpcode.C_StartScript, scriptNo);
        }


        protected void StartSound(int opcode)
        {
            SCUMMParameter soundNo = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.C_StartSound, soundNo);
        }

        protected void StateOf1Clear(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_StateOf1Clear, obj);
        }

        protected void StateOf2Clear(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_StateOf2Clear, obj);
        }

        protected void StateOf4Clear(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_StateOf4Clear, obj);
        }

        protected void StateOf8Clear(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_StateOf8Clear, obj);
        }

        protected void StateOf1Set(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_StateOf1Set, obj);
        }

        protected void StateOf2Set(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_StateOf2Set, obj);
        }

        protected void StateOf4Set(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_StateOf4Set, obj);
        }

        protected void StateOf8Set(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_StateOf8Set, obj);
        }

        protected void StopMusic(int opcode)
        {
            Add(SCUMMOpcode.C_StopMusic);
        }

        protected void StopScript(int opcode)
        {
            SCUMMParameter scriptNo = GetVarOrByte(opcode, 0x80);
            if (scriptNo.Type == SCUMMParameterType.Number && Convert.ToInt32(scriptNo.Value) == 0)
            {
                Add(SCUMMOpcode.C_StopScriptDefault);
            }
            else
            {
                Add(SCUMMOpcode.C_StopScript, scriptNo);
            }
        }

        protected void StopSound(int opcode)
        {
            SCUMMParameter soundNo = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.C_StopSound, soundNo);
        }

        protected void StoreBitVariable(int opcode)
        {
            SCUMMParameter var = GetNumber();
            SCUMMParameter offs = GetVarOrByte(opcode, 0x80);
            SCUMMParameter setOrClear = GetVarOrByte(opcode, 0x40);
            Add(SCUMMOpcode.C_StoreBitVariable, var, offs, setOrClear);
        }

        protected void StoreByteVariable(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter value = GetByte();
            Add(SCUMMOpcode.C_StoreByteVariable, var, value);
        }

        protected void StoreVariable(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter value = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_StoreVariable, var, value);
        }

        protected void StoreVariableIndirect(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter value = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_StoreVariableIndirect, var, value);
        }

        protected void Sub(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter term = GetNumber();
            Add(SCUMMOpcode.C_SubVariable, var, term);
        }

        protected void SubIndirect(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter term = GetNumber();
            Add(SCUMMOpcode.C_SubVariableIndirect, var, term);
        }

        protected void SwitchCostumeSet(int opcode)
        {
            SCUMMParameter param = GetByte();
            Add(SCUMMOpcode.C_SwitchCostumeSet, param);
        }

        protected virtual void Userface(int opcode)
        {
            SCUMMParameter param = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_Userface_1, param);
        }

        protected void VerbOf(int opcode)
        {
            SCUMMParameter obj = GetVarOrWord(opcode, 0x80);
            SCUMMParameter value = GetByte();
            Add(SCUMMOpcode.C_VerbOf, obj, value);
        }

        protected virtual void VerbStuff(int opcode)
        {
            Add(SCUMMOpcode.C_VerbStuff);
            byte verbNo = reader.ReadU8();

            switch (verbNo)
            {
                case 0x00:
                    var slot = GetVarOrByte(opcode, 0x80);
                    Add(SCUMMOpcode.SC_VerbDelete, slot);
                    return;
                case 0xff:
                    slot = GetByte();
                    var state = GetByte();
                    Add(SCUMMOpcode.SC_VerbToggle, slot, state);
                    return;
                default: // New verb
                    var x = GetByte();
                    var y = GetByte();
                    slot = GetVarOrByte(opcode, 0x80);
                    var prep = GetByte();
                    var name = GetString();
                    // TODO: This is not right at all...
                    Add(SCUMMOpcode.SC_VerbNew, slot, prep);
                    Add(SCUMMOpcode.SC_VerbAt, x, y);
                    Add(SCUMMOpcode.SC_VerbName, name);
                    return;
            }
        }

        protected void WaitForActor(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.C_WaitForActor_3, actor);
        }

        protected void WaitForSentence(int opcode)
        {
            Add(SCUMMOpcode.C_WaitForSentence_3);
        }

        protected void WaitForMessage(int opcode)
        {
            Add(SCUMMOpcode.C_WaitForMessage_3);
        }

        protected void WalkToActor(int opcode)
        {
            SCUMMParameter actor1 = GetVarOrByte(opcode, 0x80);
            SCUMMParameter actor2 = GetVarOrByte(opcode, 0x40);
            byte prox = reader.ReadU8();
            if (prox != 0xff)
            {
                SCUMMParameter proxParam = new SCUMMParameter(SCUMMParameterType.Number, prox);
                Add(SCUMMOpcode.C_WalkToActorWithin, actor1, actor2, proxParam);
            }
            else
            {
                Add(SCUMMOpcode.C_WalkToActor, actor1, actor2);
            }
        }

        protected void WalkToObject(int opcode)
        {
            SCUMMParameter actorNo = GetVarOrByte(opcode, 0x80);
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x40);
            Add(SCUMMOpcode.C_WalkToObject, actorNo, objectNo);
        }

        protected void WalkToXY(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            SCUMMParameter x = GetCoordinateVarOrLiteral(opcode, 0x40);
            SCUMMParameter y = GetCoordinateVarOrLiteral(opcode, 0x20);
            Add(SCUMMOpcode.C_WalkToXY, actor, x, y);
        }

        // ---- Functions ----
        protected void FActorBox(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_ActorBox, var, actor);
        }

        protected void FActorCostume(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_ActorCostume, var, actor);
        }

        protected void FActorElevation(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_ActorElevation, var, actor);
        }

        protected void FActorFacing(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_ActorFacing, var, actor);
        }

        protected void FActorMoving(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actorNo = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_ActorMoving, var, actorNo);
        }

        protected void FActorRoom(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_ActorRoom, var, actor);
        }

        protected virtual void FActorX(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_ActorX, var, actor);
        }

        protected virtual void FActorY(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_ActorY, var, actor);
        }

        protected void FClosestActor(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.F_ClosestActor, var, objectNo);
        }

        protected void FFindActor(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter x = GetCoordinateVarOrLiteral(opcode, 0x80);
            SCUMMParameter y = GetCoordinateVarOrLiteral(opcode, 0x40);
            Add(SCUMMOpcode.F_FindActor, var, x, y);
        }

        protected void FFindObject(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter x = GetCoordinateVarOrLiteral(opcode, 0x80);
            SCUMMParameter y = GetCoordinateVarOrLiteral(opcode, 0x40);
            Add(SCUMMOpcode.F_FindObject, var, x, y);
        }

        protected void FOwnerOf(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.F_OwnerOf, var, objectNo);
        }

        protected void FProximity2ActObjs(int opcode)
        {
            // TODO: At least one of these is really WalkTo
            SCUMMParameter var = GetVar();
            SCUMMParameter obj1 = GetVarOrWord(opcode, 0x80);
            SCUMMParameter obj2 = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.F_Proximity2ActObjs, var, obj1, obj2);
        }

        protected void FRandom(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter max = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_Random, var, max);
        }

        protected void FScriptRunning(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter scriptNo = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_ScriptRunning, var, scriptNo);
        }

        protected void FSoundRunning(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter soundNo = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_SoundRunning, var, soundNo);
        }

        protected void FVerbOf(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.F_OwnerOf, var, objectNo);
        }

        protected virtual void _HandlePrint()
        {
            bool done = false;
            while (!done)
            {
                byte subOpcode = reader.ReadU8();
                if (subOpcode == 0xff)
                {
                    break;
                }
                switch (subOpcode & 0x1f)
                {
                    case 0x00:
                        SCUMMParameter x = GetVarOrWord(subOpcode, 0x80);
                        SCUMMParameter y = GetVarOrWord(subOpcode, 0x40);
                        Add(SCUMMOpcode.SC_PrintAt, x, y);
                        break;
                    case 0x01:
                        SCUMMParameter index = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_PrintColor, index);
                        break;
                    case 0x02:
                        SCUMMParameter clipped = GetVarOrWord(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_PrintClipped, clipped);
                        break;
                    case 0x03:
                        Add(SCUMMOpcode.SC_PrintErase);
                        break;
                    case 0x04:
                        Add(SCUMMOpcode.SC_PrintCenter);
                        break;
                    case 0x05:
                        Add(SCUMMOpcode.SC_PrintTo);
                        break;
                    case 0x06:
                        Add(SCUMMOpcode.SC_PrintLeft);
                        break;
                    case 0x07:
                        Add(SCUMMOpcode.SC_PrintOverhead);
                        break;
                    case 0x08:
                        SCUMMParameter cdstart = GetVarOrWord(subOpcode, 0x80);
                        SCUMMParameter cdlen = GetVarOrWord(subOpcode, 0x40);
                        Add(SCUMMOpcode.SC_PrintTalkie, cdstart, cdlen);
                        break;
                    case 0x0f: // SO_TEXT_STRING
                        Add(SCUMMOpcode.SC_PrintString, GetString());
                        done = true;
                        break;
                    default:
                        throw UnknownSubOpcode("print", subOpcode);
                }
            }
        }
    }
}
