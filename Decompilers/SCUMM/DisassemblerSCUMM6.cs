namespace SCUMMRevLib.Decompilers.SCUMM
{
    public class DisassemblerSCUMM6 : Disassembler
    {
        public DisassemblerSCUMM6()
        {
            GetVar = GetVarNew;
            GetString = GetStringNew;
        }

        protected override void InitOpcodes()
        {
            AddOpcode(0x00, PushByte);
            AddOpcode(0x01, PushNumber);
            AddOpcode(0x02, PushByteVariable);
            AddOpcode(0x03, PushVariable);
            AddOpcode(0x06, PushByteArrayValue);
            AddOpcode(0x07, PushArrayValue);

            // 0x08
            AddOpcode(0x0a, PushByteArray2Value);
            AddOpcode(0x0b, PushArray2Value);
            AddOpcode(0x0c, Dup);
            AddOpcode(0x0d, Not);
            AddOpcode(0x0e, Eq);
            AddOpcode(0x0f, Neq);

            // 0x10
            AddOpcode(0x10, Gt);
            AddOpcode(0x11, Lt);
            AddOpcode(0x12, Leq);
            AddOpcode(0x13, Geq);
            AddOpcode(0x14, Add);
            AddOpcode(0x15, Sub);
            AddOpcode(0x16, Mul);
            AddOpcode(0x17, Div);

            // 0x18
            AddOpcode(0x18, LAnd);
            AddOpcode(0x19, LOr);
            AddOpcode(0x1a, Pop);

            // ...
            // 0x40
            AddOpcode(0x42, StoreByteVariable);
            AddOpcode(0x43, StoreVariable);

            AddOpcode(0x46, StoreByteArray);
            AddOpcode(0x47, StoreArray);

            // 0x48
            AddOpcode(0x4a, StoreByteArray2);
            AddOpcode(0x4b, StoreArray2);

            AddOpcode(0x4e, IncByteVariable);
            AddOpcode(0x4f, IncVariable);

            // 0x50
            AddOpcode(0x52, IncByteArray);
            AddOpcode(0x53, IncArray);

            AddOpcode(0x56, DecByteVariable);
            AddOpcode(0x57, DecVariable);

            // 0x58
            AddOpcode(0x5a, DecByteArray);
            AddOpcode(0x5b, DecArray);
            AddOpcode(0x5c, If);
            AddOpcode(0x5d, IfNot);
            AddOpcode(0x5e, StartScript);
            AddOpcode(0x5f, StartScriptQuick);

            // 0x60
            AddOpcode(0x60, StartObject);
            AddOpcode(0x61, ObjectState);
            AddOpcode(0x62, ObjectXY);
            AddOpcode(0x63, BlastObject);
            AddOpcode(0x64, BlastObjectWindow);
            AddOpcode(0x65, EndScript);
            AddOpcode(0x66, EndScript);
            AddOpcode(0x67, EndCutScene);

            // 0x68
            AddOpcode(0x68, CutScene);
            AddOpcode(0x69, StopMusic);
            AddOpcode(0x6a, FreezeScripts);
            AddOpcode(0x6b, Userface);
            AddOpcode(0x6c, BreakHere);
            AddOpcode(0x6d, FClassOf);
            AddOpcode(0x6e, ClassOf);
            AddOpcode(0x6f, FStateOf);

            // 0x70
            AddOpcode(0x70, StateOf);
            AddOpcode(0x71, OwnerOf);
            AddOpcode(0x72, FOwnerOf);
            AddOpcode(0x73, Jump);
            AddOpcode(0x74, StartSfx);
            AddOpcode(0x75, StopSound);
            AddOpcode(0x76, StartMusic);
            AddOpcode(0x77, StopObject);

            // 0x78
            AddOpcode(0x78, CameraPanTo);
            AddOpcode(0x79, CameraFollow);
            AddOpcode(0x7a, CameraAt);
            AddOpcode(0x7b, CurrentRoom);
            AddOpcode(0x7c, StopScript);
            AddOpcode(0x7d, WalkActorToObject);
            AddOpcode(0x7e, WalkActorToXY);
            AddOpcode(0x7f, PutActorAtXY);

            // 0x80
            AddOpcode(0x80, PutActorAtObject);
            AddOpcode(0x81, FaceTowards);
            AddOpcode(0x82, DoAnimation);
            AddOpcode(0x83, DoSentence);
            AddOpcode(0x84, PickUpObject);
            AddOpcode(0x85, ComeOutDoor);

            AddOpcode(0x87, FRandom);

            // 0x88
            AddOpcode(0x88, FRandomBetween);

            AddOpcode(0x8a, FActorMoving);
            AddOpcode(0x8b, FScriptRunning);
            AddOpcode(0x8c, FActorRoom);
            AddOpcode(0x8d, FActObjX);
            AddOpcode(0x8e, FActObjY);
            AddOpcode(0x8f, FActObjFacingOld);

            // 0x90
            AddOpcode(0x90, FActorBox);
            AddOpcode(0x91, FActorCostume);
            AddOpcode(0x92, FActorInventory);
            AddOpcode(0x93, FActorInventoryCount);
            AddOpcode(0x94, FFindVerb);
            AddOpcode(0x95, OverRide);
            AddOpcode(0x96, OverRideOff);
            AddOpcode(0x97, NewNameOf);

            // 0x98
            AddOpcode(0x98, FSoundRunning);
            AddOpcode(0x99, SetBox);
            AddOpcode(0x9a, SetBoxPath);
            AddOpcode(0x9b, HeapStuff);
            AddOpcode(0x9c, RoomStuff);
            AddOpcode(0x9d, ActorStuff);
            AddOpcode(0x9e, VerbStuff);
            AddOpcode(0x9f, FFindActor);

            // 0xa0
            AddOpcode(0xa0, FFindObject);
            AddOpcode(0xa1, PseudoRoom);
            AddOpcode(0xa2, FActorElevation);
            AddOpcode(0xa3, FValidVerb);
            AddOpcode(0xa4, AssignArray);
            AddOpcode(0xa5, VerbSets);
            AddOpcode(0xa6, DrawBox);
            AddOpcode(0xa7, Pop);

            // 0xa8
            AddOpcode(0xa8, FActorWidth);
            AddOpcode(0xa9, WaitForStuff);
            AddOpcode(0xaa, FActorScale);
            AddOpcode(0xab, FActorAnimCounter);
            AddOpcode(0xac, SoundKludge);
            AddOpcode(0xad, FInSet);
            AddOpcode(0xae, System);
            AddOpcode(0xaf, FInBox);

            // 0xb0
            AddOpcode(0xb0, SleepJiffies);
            AddOpcode(0xb1, SleepSeconds);
            AddOpcode(0xb2, SleepMinutes);
            AddOpcode(0xb3, StopSentence);
            AddOpcode(0xb4, PrintLine);
            AddOpcode(0xb5, PrintCursor);
            AddOpcode(0xb6, PrintDebug);
            AddOpcode(0xb7, PrintSystem);

            // 0xb8
            AddOpcode(0xb8, SayLine);
            AddOpcode(0xb9, SayLineDefault);
            AddOpcode(0xba, SayLineSimple);
            AddOpcode(0xbb, SayLineSimpleDefault);
            AddOpcode(0xbc, ArrayDim);
            AddOpcode(0xbd, Return);
            AddOpcode(0xbe, FStartScript);
            AddOpcode(0xbf, FStartObject);

            // 0xc0
            AddOpcode(0xc0, ArrayDim2);

            AddOpcode(0xc4, FAbs);
            AddOpcode(0xc5, FProximity2ActObjs);
            AddOpcode(0xc6, FProximityActObjPoint);
            AddOpcode(0xc7, FProximity2Points);

            // 0xc8
            AddOpcode(0xc8, FKludge);
            AddOpcode(0xc9, Kludge);
            AddOpcode(0xca, BreakHereVar);
            AddOpcode(0xcb, FPick);
            AddOpcode(0xcc, FPickDefault);
            AddOpcode(0xcd, ActObjStamp); // Not sure if it's implemented right

            // 0xd0
            AddOpcode(0xd0, GetTimeDate);
            AddOpcode(0xd1, StopTalking);
            AddOpcode(0xd2, FActorVariable);

            AddOpcode(0xd4, ArrayShuffle);
            AddOpcode(0xd5, ChainScript);
            AddOpcode(0xd6, BAnd);
            AddOpcode(0xd7, BOr);

            // 0xd8
            AddOpcode(0xd8, FRoomScriptRunning);
            AddOpcode(0xd9, CloseFile);
            AddOpcode(0xda, OpenFile);
            AddOpcode(0xdb, ReadFile);

            AddOpcode(0xdd, FFindAllObjects);
            AddOpcode(0xde, DeleteFile);

            // 0xe0
            AddOpcode(0xe0, Unknown_E0);
            AddOpcode(0xe1, FPixel);
            AddOpcode(0xe2, ArrayLocalize);
            AddOpcode(0xe3, FPickRandom);
            AddOpcode(0xe4, Unknown_E4);

            // 0xe8
            AddOpcode(0xea, Unknown_EA);
            AddOpcode(0xec, FActorDepth);
            AddOpcode(0xed, FActObjFacing);

            // ...
            // 0xf8
            AddOpcode(0xfa, Unknown_FA);
        }

        protected void ActObjStamp(int opcode)
        {
            Add(SCUMMOpcode.C_ActObjStamp);
        }

        protected virtual void ActorStuff(int opcode)
        {
            Add(SCUMMOpcode.C_ActorStuff);

            var subOpcode = reader.ReadU8();
            switch (subOpcode)
            {
                case 76: Add(SCUMMOpcode.SC_ActorCostume); break;
                case 77: Add(SCUMMOpcode.SC_ActorStepDist); break;
                case 78: Add(SCUMMOpcode.SC_ActorSound_6); break;
                case 79: Add(SCUMMOpcode.SC_ActorAnimationWalk); break;
                case 80: Add(SCUMMOpcode.SC_ActorAnimationTalk); break;
                case 81: Add(SCUMMOpcode.SC_ActorAnimationStand); break;
                case 82: Add(SCUMMOpcode.SC_ActorAnimation); break;
                case 83: Add(SCUMMOpcode.SC_ActorDefault); break;
                case 84: Add(SCUMMOpcode.SC_ActorElevation); break;
                case 85: Add(SCUMMOpcode.SC_ActorAnimationDefault); break;
                case 86: Add(SCUMMOpcode.SC_ActorPalette); break;
                case 87: Add(SCUMMOpcode.SC_ActorTalkColor); break;
                case 88: Add(SCUMMOpcode.SC_ActorName, GetString()); break;
                case 89: Add(SCUMMOpcode.SC_ActorAnimationInit); break;
                case 91: Add(SCUMMOpcode.SC_ActorWidth); break;
                case 92: Add(SCUMMOpcode.SC_ActorScale); break;
                case 93: Add(SCUMMOpcode.SC_ActorNeverZClip); break;
                case 225: // Check if this is really a duplicate
                case 94: Add(SCUMMOpcode.SC_ActorAlwaysZClip); break;
                case 95: Add(SCUMMOpcode.SC_ActorIgnoreBoxes);
                    break;
                case 96: Add(SCUMMOpcode.SC_ActorFollowBoxes); break;
                case 97: Add(SCUMMOpcode.SC_ActorAnimationSpeed); break;
                case 98: Add(SCUMMOpcode.SC_ActorSpecialDraw); break;
                case 99: Add(SCUMMOpcode.SC_ActorTextOffset); break;
                case 197: Add(SCUMMOpcode.SC_ActorInit); break;
                case 198: Add(SCUMMOpcode.SC_ActorVariable); break;
                case 215: Add(SCUMMOpcode.SC_ActorIgnoreTurnsOn); break;
                case 216: Add(SCUMMOpcode.SC_ActorIgnoreTurnsOff); break;
                case 217: Add(SCUMMOpcode.SC_ActorNew); break;
                case 227: Add(SCUMMOpcode.SC_ActorDepth); break;
                case 228: Add(SCUMMOpcode.SC_ActorWalkScript); break;
                case 229: Add(SCUMMOpcode.SC_ActorStop); break;
                case 230: Add(SCUMMOpcode.SC_ActorFace); break;
                case 231: Add(SCUMMOpcode.SC_ActorTurn); break;
                case 233: Add(SCUMMOpcode.SC_ActorWalkPause); break;
                case 234: Add(SCUMMOpcode.SC_ActorWalkResume); break;
                case 235: Add(SCUMMOpcode.SC_ActorTalkScript); break;
                default:
                    throw UnknownSubOpcode("actor", subOpcode);
            }
        }

        protected void Add(int opcode)
        {
            Add(SCUMMOpcode.C_Add);
        }

        protected virtual void ArrayDim(int opcode)
        {
            Add(SCUMMOpcode.C_ArrayDim);

            var subOpcode = reader.ReadU8();
            var arg = GetVar();

            switch (subOpcode)
            {
                case 199: Add(SCUMMOpcode.SC_ArrayDimScummVar, arg); break;
                case 200: Add(SCUMMOpcode.SC_ArrayDimUnknown200_7, arg); break;
                case 201: Add(SCUMMOpcode.SC_ArrayDimUnknown201_7, arg); break;
                case 202: Add(SCUMMOpcode.SC_ArrayDimUnknown202_7, arg); break;
                case 203: Add(SCUMMOpcode.SC_ArrayDimString); break;
                case 204: Add(SCUMMOpcode.SC_ArrayUnDim, arg); break;
                default:
                    throw UnknownSubOpcode("array-dim", subOpcode);
            }
        }

        protected virtual void ArrayDim2(int opcode)
        {
            var subOpcode = reader.ReadU8();
            var arg = GetVar();

            switch (subOpcode)
            {
                case 199: Add(SCUMMOpcode.SC_Array2DimScummVar, arg); break;
                case 200: Add(SCUMMOpcode.SC_Array2DimUnknown200_7, arg); break;
                case 201: Add(SCUMMOpcode.SC_Array2DimUnknown201_7, arg); break;
                case 202: Add(SCUMMOpcode.SC_Array2DimUnknown202_7, arg); break;
                case 203: Add(SCUMMOpcode.SC_Array2DimString); break;
                case 204: Add(SCUMMOpcode.SC_Array2UnDim, arg); break;
                default:
                    throw UnknownSubOpcode("array2-dim", subOpcode);
            }
        }

        protected void ArrayLocalize(int opcode)
        {
            Add(SCUMMOpcode.C_ArrayLocalize);
        }

        protected void ArrayShuffle(int opcode)
        {
            Add(SCUMMOpcode.C_ArrayShuffle, GetVar());
        }

        protected virtual void AssignArray(int opcode)
        {
            Add(SCUMMOpcode.C_AssignArray);

            var subOpcode = reader.ReadU8();
            var arg = GetVar();

            switch (subOpcode)
            {
                case 205: Add(SCUMMOpcode.SC_AssignString, arg, GetString()); break;
                case 208: Add(SCUMMOpcode.SC_AssignArray, arg); break;
                case 212: Add(SCUMMOpcode.SC_AssignArray2, arg); break;
                default:
                    throw UnknownSubOpcode("assign-array", subOpcode);
            }
        }

        protected void BAnd(int opcode)
        {
            Add(SCUMMOpcode.C_BAnd);
        }

        protected void BlastObject(int opcode)
        {
            Add(SCUMMOpcode.C_BlastObject);
        }

        protected void BOr(int opcode)
        {
            Add(SCUMMOpcode.C_BOr);
        }

        protected void BreakHere(int opcode)
        {
            Add(SCUMMOpcode.C_BreakHere);
        }

        protected void BreakHereVar(int opcode)
        {
            Add(SCUMMOpcode.C_BreakHereVar);
        }

        protected void CameraAt(int opcode)
        {
            // TODO: if (version == 6)
            // Add(SCUMMOpcode.C_CameraAt_3);
            // else
            Add(SCUMMOpcode.C_CameraAt);
        }

        protected void CameraFollow(int opcode)
        {
            Add(SCUMMOpcode.C_CameraFollow);
        }

        protected void CameraPanTo(int opcode)
        {
            // TODO: if (version == 6)
            // Add(SCUMMOpcode.C_CameraPanTo_3);
            // else
            Add(SCUMMOpcode.C_CameraPanTo);
        }

        protected void ChainScript(int opcode)
        {
            Add(SCUMMOpcode.C_ChainScript);
        }

        protected void ClassOf(int opcode)
        {
            Add(SCUMMOpcode.C_ClassOf);
        }

        protected void ComeOutDoor(int opcode)
        {
            // TODO: if (version == 6)
            // Add(SCUMMOpcode.C_ComeOutDoor_6);
            // else
            Add(SCUMMOpcode.C_ComeOutDoor);
        }

        protected void CurrentRoom(int opcode)
        {
            Add(SCUMMOpcode.C_CurrentRoom);
        }

        protected void CutScene(int opcode)
        {
            Add(SCUMMOpcode.C_CutScene);
        }

        protected void DecArray(int opcode)
        {
            Add(SCUMMOpcode.C_DecArray, GetVar());
        }

        protected void DecVariable(int opcode)
        {
            Add(SCUMMOpcode.C_DecVariable, GetVar());
        }

        protected void Div(int opcode)
        {
            Add(SCUMMOpcode.C_Div);
        }

        protected void DoAnimation(int opcode)
        {
            Add(SCUMMOpcode.C_DoAnimation);
        }

        protected void DoSentence(int opcode)
        {
            Add(SCUMMOpcode.C_DoSentence);
        }

        protected void DrawBox(int opcode)
        {
            Add(SCUMMOpcode.C_DrawBox);
        }

        protected void Dup(int opcode)
        {
            Add(SCUMMOpcode.C_Dup);
        }

        protected void EndCutScene(int opcode)
        {
            Add(SCUMMOpcode.C_EndCutScene);
        }

        protected void EndScript(int opcode)
        {
            Add(SCUMMOpcode.C_EndScript);
        }

        protected void Eq(int opcode)
        {
            Add(SCUMMOpcode.C_Eq);
        }

        protected void FaceTowards(int opcode)
        {
            Add(SCUMMOpcode.C_FaceTowards);
        }

        protected void FreezeScripts(int opcode)
        {
            Add(SCUMMOpcode.C_FreezeScripts);
        }

        protected void GetTimeDate(int opcode)
        {
            Add(SCUMMOpcode.C_GetTimeDate);
        }

        protected void Geq(int opcode)
        {
            Add(SCUMMOpcode.C_Geq);
        }

        protected void Gt(int opcode)
        {
            Add(SCUMMOpcode.C_Gt);
        }

        protected virtual void HeapStuff(int opcode)
        {
            Add(SCUMMOpcode.C_HeapStuff);

            var subOpcode = reader.ReadU8();
            switch (subOpcode)
            {
                case 100:    Add(SCUMMOpcode.SC_HeapLoadScript); break;
                case 101:    Add(SCUMMOpcode.SC_HeapLoadSound); break;
                case 102:    Add(SCUMMOpcode.SC_HeapLoadCostume); break;
                case 103:    Add(SCUMMOpcode.SC_HeapLoadRoom); break;
                case 104:    Add(SCUMMOpcode.SC_HeapNukeScript); break;
                case 105:    Add(SCUMMOpcode.SC_HeapNukeSound); break;
                case 106:    Add(SCUMMOpcode.SC_HeapNukeCostume); break;
                case 107:    Add(SCUMMOpcode.SC_HeapNukeRoom); break;
                case 108:    Add(SCUMMOpcode.SC_HeapLockScript); break;
                case 109:    Add(SCUMMOpcode.SC_HeapLockSound); break;
                case 110:   Add(SCUMMOpcode.SC_HeapLockCostume); break;
                case 111:   Add(SCUMMOpcode.SC_HeapLockRoom); break;
                case 112:   Add(SCUMMOpcode.SC_HeapUnlockScript); break;
                case 113:   Add(SCUMMOpcode.SC_HeapUnlockSound); break;
                case 114:   Add(SCUMMOpcode.SC_HeapUnlockCostume); break;
                case 115:   Add(SCUMMOpcode.SC_HeapUnlockRoom); break;
                case 116:   Add(SCUMMOpcode.SC_HeapClearHeap); break;
  	            case 117:   Add(SCUMMOpcode.SC_HeapLoadCharset); break; 
                case 118:   Add(SCUMMOpcode.SC_HeapNukeCharset); break;
                case 119:
                    //if (version == 6)
                    //{
                    //  Add(SCUMMOpcode.SC_HeapLoadObject_3);
                    //}
                    //else
                    //{
                    Add(SCUMMOpcode.SC_HeapLoadObject);
                    //}
                    break;
                default:
                    throw UnknownSubOpcode("heap", subOpcode);
            }
        }

        protected void If(int opcode)
        {
            var arg = GetNumberSigned();
            Add(SCUMMOpcode.C_If, arg);
        }

        protected void IfNot(int opcode)
        {
            var arg = GetNumberSigned();
            Add(SCUMMOpcode.C_IfNot, arg);
        }

        protected void IncArray(int opcode)
        {
            var arg = GetVar();
            Add(SCUMMOpcode.C_IncArray, arg);
        }

        protected void IncVariable(int opcode)
        {
            var arg = GetVar();
            Add(SCUMMOpcode.C_IncVariable, arg);
        }

        protected void Jump(int opcode)
        {
            var arg = GetNumberSigned();
            Add(SCUMMOpcode.C_Jump, arg);
        }

        protected void Kludge(int opcode)
        {
            Add(SCUMMOpcode.C_Kludge);
        }

        protected void LAnd(int opcode)
        {
            Add(SCUMMOpcode.C_LAnd);
        }

        protected void Leq(int opcode)
        {
            Add(SCUMMOpcode.C_Leq);
        }

        protected void LOr(int opcode)
        {
            Add(SCUMMOpcode.C_LOr);
        }

        protected void Lt(int opcode)
        {
            Add(SCUMMOpcode.C_Lt);
        }

        protected void Mod(int opcode)
        {
            Add(SCUMMOpcode.C_Mod);
        }

        protected void Neq(int opcode)
        {
            Add(SCUMMOpcode.C_Neq);
        }

        protected void Mul(int opcode)
        {
            Add(SCUMMOpcode.C_Mul);
        }

        protected void NewNameOf(int opcode)
        {
            var arg = GetString();
            Add(SCUMMOpcode.C_NewNameOf, arg);
        }

        protected void Not(int opcode)
        {
            Add(SCUMMOpcode.C_Not);
        }

        protected void OverRide(int opcode)
        {
            Add(SCUMMOpcode.C_Override);
        }

        protected void OverRideOff(int opcode)
        {
            Add(SCUMMOpcode.C_OverrideOff);
        }

        protected void OwnerOf(int opcode)
        {
            Add(SCUMMOpcode.C_OwnerOf);
        }

        protected void PickUpObject(int opcode)
        {
            // TODO: if (version == 6)
            //Add(SCUMMOpcode.C_PickUpObject);
            // else
            Add(SCUMMOpcode.C_PickUpObject);
        }

        protected void Pop(int opcode)
        {
            Add(SCUMMOpcode.C_Pop);
        }

        protected void PrintCursor(int opcode)
        {
            Add(SCUMMOpcode.C_PrintCursor);
            _HandlePrint();
        }

        protected void PrintDebug(int opcode)
        {
            Add(SCUMMOpcode.C_PrintDebug);
            _HandlePrint();
        }

        protected void PrintLine(int opcode)
        {
            Add(SCUMMOpcode.C_PrintLine);
            _HandlePrint();
        }

        protected void PrintSystem(int opcode)
        {
            Add(SCUMMOpcode.C_PrintSystem);
            _HandlePrint();
        }

        protected void PushArrayValue(int opcode)
        {
            var arg = GetVar();
            Add(SCUMMOpcode.C_PushArrayValue, arg);
        }

        protected void PushArray2Value(int opcode)
        {
            var arg = GetVar();
            Add(SCUMMOpcode.C_PushArray2Value, arg);
        }

        protected void PushByte(int opcode)
        {
            var arg = GetByte();
            Add(SCUMMOpcode.C_PushByte, arg);

        }

        protected void PushNumber(int opcode)
        {
            var arg = GetNumberSigned();
            Add(SCUMMOpcode.C_PushNumber, arg);
        }

        protected void PushVariable(int opcode)
        {
            var arg = GetVar();
            Add(SCUMMOpcode.C_PushVariable, arg);
        }

        protected void PutActorAtObject(int opcode)
        {
            // TODO: if (version == 6)
            //{
                //Add(SCUMMOpcode.C_PutActorAtObject_6);
            //}
            //else
            //{
                Add(SCUMMOpcode.C_PutActorAtObject_7);
            //}
        }

        protected void PutActorAtXY(int opcode)
        {
            Add(SCUMMOpcode.C_PutActorAtXY_6);
        }

        protected void Return(int opcode)
        {
            Add(SCUMMOpcode.C_Return);
        }

        protected virtual void RoomStuff(int opcode)
        {
            Add(SCUMMOpcode.C_RoomStuff);

            var subOpcode = reader.ReadU8();
            switch (subOpcode)
            {
                case 172: Add(SCUMMOpcode.SC_RoomScroll); break;
                case 174: Add(SCUMMOpcode.SC_RoomSetScreen); break;
                case 175: Add(SCUMMOpcode.SC_RoomPalette); break;
                case 176: Add(SCUMMOpcode.SC_RoomShakeOn); break;
                case 177: Add(SCUMMOpcode.SC_RoomShakeOff); break;
                case 179: Add(SCUMMOpcode.SC_RoomIntensity); break;
                case 180: Add(SCUMMOpcode.SC_RoomSaveLoadGame); break;
                case 181: Add(SCUMMOpcode.SC_RoomFade); break;
                case 182: Add(SCUMMOpcode.SC_RoomRgbIntensity); break;
                case 183: Add(SCUMMOpcode.SC_RoomSetShadowPalette); break;
                case 184: Add(SCUMMOpcode.SC_RoomSaveString); break; //TODO: Check if this is right
                case 185: Add(SCUMMOpcode.SC_RoomLoadString); break; //TODO: Check if this is right
                case 186: Add(SCUMMOpcode.SC_RoomTransform); break; //TODO: Check if this is right
                case 187: Add(SCUMMOpcode.SC_RoomCycleSpeed); break;
                case 213: Add(SCUMMOpcode.SC_RoomNewPalette); break;
                default:
                    throw UnknownSubOpcode("room", subOpcode);
            }
        }

        protected void SayLine(int opcode)
        {
            Add(SCUMMOpcode.C_SayLine);
            _HandlePrint(true);
        }

        protected void SayLineDefault(int opcode)
        {
            Add(SCUMMOpcode.C_SayLineDefault);
            _HandlePrint(true);
        }

        protected void SayLineSimple(int opcode)
        {
            Add(SCUMMOpcode.C_SayLineSimple, GetString());
        }

        protected void SayLineSimpleDefault(int opcode)
        {
            Add(SCUMMOpcode.C_SayLineSimpleDefault, GetString());
        }

        protected void SetBox(int opcode)
        {
            Add(SCUMMOpcode.C_SetBox);
        }

        protected void SetBoxPath(int opcode)
        {
            Add(SCUMMOpcode.SC_SetBoxPath);
        }

        protected void SetBoxSet(int opcode)
        {
            Add(SCUMMOpcode.C_SetBoxSet);
        }

        protected void SleepJiffies(int opcode)
        {
            Add(SCUMMOpcode.C_SleepJiffies);
        }

        protected void SleepMinutes(int opcode)
        {
            Add(SCUMMOpcode.C_SleepMinutes);
        }

        protected void SleepSeconds(int opcode)
        {
            Add(SCUMMOpcode.C_SleepSeconds);
        }

        protected void SoundKludge(int opcode)
        {
            Add(SCUMMOpcode.C_SoundKludge);
            // TODO: Fix
        }

        protected void StartMusic(int opcode)
        {
            Add(SCUMMOpcode.C_StartMusic);
        }

        protected void StartObject(int opcode)
        {
            Add(SCUMMOpcode.C_StartObject);
        }

        protected void StartScript(int opcode)
        {
            Add(SCUMMOpcode.C_StartScript);
        }

        protected void StartScriptQuick(int opcode)
        {
            Add(SCUMMOpcode.C_StartScriptQuick);
        }

        protected void StartSfx(int opcode)
        {
            Add(SCUMMOpcode.C_StartSound);
        }

        protected virtual void StartVideo(int opcode)
        {
            Add(SCUMMOpcode.C_StartVideo);
        }

        protected void StateOf(int opcode)
        {
            Add(SCUMMOpcode.C_StateOf);
        }

        protected void StopSound(int opcode)
        {
            Add(SCUMMOpcode.C_StopSound);
        }

        protected void StopObject(int opcode)
        {
            Add(SCUMMOpcode.C_StopObject);
        }

        protected void StopScript(int opcode)
        {
            Add(SCUMMOpcode.C_StopScript);
        }

        protected void StopSentence(int opcode)
        {
            Add(SCUMMOpcode.C_StopSentence);
        }

        protected void StoreArray(int opcode)
        {
            var arg = GetVar();
            Add(SCUMMOpcode.C_StoreArray, arg);
        }

        protected void StoreArray2(int opcode)
        {
            var arg = GetVar();
            Add(SCUMMOpcode.C_StoreArray2, arg);
        }

        protected void StoreVariable(int opcode)
        {
            var arg = GetVar();
            Add(SCUMMOpcode.C_StoreVariable, arg);
        }

        protected void Sub(int opcode)
        {
            Add(SCUMMOpcode.C_Sub);
        }

        protected virtual void System(int opcode)
        {
            Add(SCUMMOpcode.C_System);

            var subOpcode = reader.ReadU8();
            switch (subOpcode)
            {
                case 158: Add(SCUMMOpcode.SC_SystemRestart); break;
                case 159: Add(SCUMMOpcode.SC_SystemPause); break;
                case 160: Add(SCUMMOpcode.SC_SystemQuit); break;
                default:
                    throw UnknownSubOpcode("system", subOpcode);
            }
        }

        protected virtual void Userface(int opcode)
        {
            Add(SCUMMOpcode.C_Userface);

            var subOpcode = reader.ReadU8();
            switch (subOpcode)
            {
                case 144: Add(SCUMMOpcode.SC_CursorOn); break;
                case 145: Add(SCUMMOpcode.SC_CursorOff); break;
                case 146: Add(SCUMMOpcode.SC_UserputOn); break;
                case 147: Add(SCUMMOpcode.SC_UserputOff); break;
                case 148: Add(SCUMMOpcode.SC_CursorSoftOn); break;
                case 149: Add(SCUMMOpcode.SC_CursorSoftOff); break;
                case 150: Add(SCUMMOpcode.SC_UserputSoftOn); break;
                case 151: Add(SCUMMOpcode.SC_UserputSoftOff); break;
                case 153: Add(SCUMMOpcode.SC_CursorImage_3); break;
                case 154: Add(SCUMMOpcode.SC_CursorHotspot_6); break;
                case 156: Add(SCUMMOpcode.SC_CharsetSet); break;
                case 157: Add(SCUMMOpcode.SC_CharsetColor); break;
                case 214: Add(SCUMMOpcode.SC_CursorTransparent); break;
                default:
                    throw UnknownSubOpcode("userface", subOpcode);
            }
        }

        protected virtual void VerbSets(int opcode)
        {
            Add(SCUMMOpcode.C_VerbSets);
            var subOpcode = reader.ReadU8();
            switch (subOpcode)
            {
                case 141: Add(SCUMMOpcode.SC_VerbsSave); break;
                case 142: Add(SCUMMOpcode.SC_VerbsRestore); break;
                case 143: Add(SCUMMOpcode.SC_VerbsDelete); break;
                default:
                    throw UnknownSubOpcode("verbsets", subOpcode);
            }
        }

        protected virtual void VerbStuff(int opcode)
        {
            Add(SCUMMOpcode.C_VerbStuff);

            var subOpcode = reader.ReadU8();

            switch (subOpcode)
            {
                case 124: Add(SCUMMOpcode.SC_VerbImage_3); break;
                case 125: Add(SCUMMOpcode.SC_VerbName, GetString()); break;
                case 126: Add(SCUMMOpcode.SC_VerbColor); break;
                case 127: Add(SCUMMOpcode.SC_VerbHicolor); break;
                case 128: Add(SCUMMOpcode.SC_VerbAt); break;
                case 129: Add(SCUMMOpcode.SC_VerbOn); break;
                case 130: Add(SCUMMOpcode.SC_VerbOff); break;
                case 131: Add(SCUMMOpcode.SC_VerbDelete); break;
                case 132: Add(SCUMMOpcode.SC_VerbNew); break;
                case 133: Add(SCUMMOpcode.SC_VerbDimColor); break;
                case 134: Add(SCUMMOpcode.SC_VerbDim); break;
                case 135: Add(SCUMMOpcode.SC_VerbKey); break;
                case 136: Add(SCUMMOpcode.SC_VerbCenter); break;
                case 137: Add(SCUMMOpcode.SC_VerbNameStr); break;
                case 139: Add(SCUMMOpcode.SC_VerbImage); break;
                case 140: Add(SCUMMOpcode.SC_VerbBkColor); break;
                case 196: Add(SCUMMOpcode.SC_VerbInit); break;
                case 255: Add(SCUMMOpcode.SC_VerbEnd); break;
                default:
                    throw UnknownSubOpcode("verb", subOpcode);
            }
        }

        protected virtual void WaitForStuff(int opcode)
        {
            Add(SCUMMOpcode.C_WaitForStuff);
            var subOpcode = reader.ReadU8();

            switch (subOpcode)
            {
                case 168: Add(SCUMMOpcode.SC_WaitForActor_6, GetNumberSigned()); break;
                case 169: Add(SCUMMOpcode.SC_WaitForMessage); break;
                case 170: Add(SCUMMOpcode.SC_WaitForCamera); break;
                case 171: Add(SCUMMOpcode.SC_WaitForSentence); break;
                case 226: Add(SCUMMOpcode.SC_WaitForAnimation, GetNumberSigned()); break;
                case 232: Add(SCUMMOpcode.SC_WaitForTurn, GetNumberSigned()); break;
                default:
                    throw UnknownSubOpcode("wait-for-stuff", subOpcode);
            }
        }

        protected void WalkActorToObject(int opcode)
        {
            Add(SCUMMOpcode.C_WalkToObject);
        }

        protected void WalkActorToXY(int opcode)
        {
            Add(SCUMMOpcode.C_WalkToXY);
        }

        // ---- FUNCTIONS ----

        protected void FAbs(int opcode)
        {
            Add(SCUMMOpcode.F_Abs);
        }

        protected void FActObjFacing(int opcode)
        {
            Add(SCUMMOpcode.F_ActObjFacing);
        }

        protected void FActObjX(int opcode)
        {
            Add(SCUMMOpcode.F_ActObjX);
        }

        protected void FActObjY(int opcode)
        {
            Add(SCUMMOpcode.F_ActObjY);
        }

        protected void FActorBox(int opcode)
        {
            Add(SCUMMOpcode.F_ActorBox);
        }

        protected void FActorCostume(int opcode)
        {
            Add(SCUMMOpcode.F_ActorCostume);
        }

        protected void FActorDepth(int opcode)
        {
            Add(SCUMMOpcode.F_ActorDepth);
        }

        protected void FActorElevation(int opcode)
        {
            Add(SCUMMOpcode.F_ActorElevation);
        }

        protected void FActorInventory(int opcode)
        {
            Add(SCUMMOpcode.F_ActorInventory);
        }

        protected void FActorInventoryCount(int opcode)
        {
            Add(SCUMMOpcode.F_ActorInventoryCount);
        }

        protected void FActorMoving(int opcode)
        {
            Add(SCUMMOpcode.F_ActorMoving);
        }

        protected void FActorRoom(int opcode)
        {
            Add(SCUMMOpcode.F_ActorRoom);
        }

        protected void FActorScale(int opcode)
        {
            Add(SCUMMOpcode.F_ActorScale);
        }

        protected void FActorVariable(int opcode)
        {
            Add(SCUMMOpcode.F_ActorVariable);
        }

        protected void FActorWidth(int opcode)
        {
            Add(SCUMMOpcode.F_ActorWidth);
        }

        protected void FClassOf(int opcode)
        {
            Add(SCUMMOpcode.F_ClassOf);
        }

        protected void FFindActor(int opcode)
        {
            Add(SCUMMOpcode.F_FindActor);
        }

        protected void FFindAllObjects(int opcode)
        {
            Add(SCUMMOpcode.F_FindAllObjects);
        }

        protected void FFindObject(int opcode)
        {
            Add(SCUMMOpcode.F_FindObject);
        }

        protected void FInBox(int opcode)
        {
            Add(SCUMMOpcode.F_InBox);
        }

        protected void FInSet(int opcode)
        {
            Add(SCUMMOpcode.F_InSet);
        }

        protected void FKludge(int opcode)
        {
            Add(SCUMMOpcode.F_Kludge);
        }

        protected void FOwnerOf(int opcode)
        {
            Add(SCUMMOpcode.F_OwnerOf);
        }

        protected void FPick(int opcode)
        {
            Add(SCUMMOpcode.F_Pick);
        }

        protected void FPickDefault(int opcode)
        {
            Add(SCUMMOpcode.F_PickDefault);
        }

        protected void FPickRandom(int opcode)
        {
            Add(SCUMMOpcode.F_PickRandom);
        }

        protected void FPixel(int opcode)
        {
            Add(SCUMMOpcode.F_PickRandom);
        }

        protected void FProximity2ActObjs(int opcode)
        {
            Add(SCUMMOpcode.F_Proximity2ActObjs);
        }

        protected void FProximity2Points(int opcode)
        {
            Add(SCUMMOpcode.F_Proximity2Points);
        }

        protected void FRandom(int opcode)
        {
            Add(SCUMMOpcode.F_Random);
        }

        protected void FRandomBetween(int opcode)
        {
            Add(SCUMMOpcode.F_RandomBetween);
        }

        protected void FScriptRunning(int opcode)
        {
            Add(SCUMMOpcode.F_ScriptRunning);
        }

        protected void FSoundRunning(int opcode)
        {
            Add(SCUMMOpcode.F_SoundRunning);
        }

        protected void FStartObject(int opcode)
        {
            Add(SCUMMOpcode.F_StartObject);
        }

        protected void FStartScript(int opcode)
        {
            Add(SCUMMOpcode.F_StartScript);
        }

        protected void FStateOf(int opcode)
        {
            Add(SCUMMOpcode.F_StateOf);
        }

        protected void FValidVerb(int opcode)
        {
            Add(SCUMMOpcode.F_ValidVerb);
        }

        // SCUMM 6-7 specific:

        void BlastObjectWindow(int opcode)
        {
            Add(SCUMMOpcode.C_BlastObjectWindow);
        }

        void CloseFile(int opcode)
        {
            Add(SCUMMOpcode.C_CloseFile);
        }

        void DecByteArray(int opcode)
        {
            Add(SCUMMOpcode.C_DecByteArray, GetByteVar());
        }

        void DecByteVariable(int opcode)
        {
            Add(SCUMMOpcode.C_DecByteVariable, GetByteVar());
        }

        void DeleteFile(int opcode)
        {
            Add(SCUMMOpcode.C_DeleteFile);
        }

        void IncByteArray(int opcode)
        {
            Add(SCUMMOpcode.C_IncByteArray, GetByteVar());
        }

        void IncByteVariable(int opcode)
        {
            Add(SCUMMOpcode.C_IncByteVariable, GetByteVar());
        }

        void ObjectState(int opcode)
        {
            Add(SCUMMOpcode.C_ObjectState);
        }

        void ObjectXY(int opcode)
        {
            Add(SCUMMOpcode.C_ObjectXY);
        }

        void OpenFile(int opcode)
        {
            Add(SCUMMOpcode.C_OpenFile);
        }

        void PseudoRoom(int opcode)
        {
            Add(SCUMMOpcode.C_Pseudoroom);
        }

        //TODO: Check byte stuff - is it actually the var id that's byte or the pushed value? In any event PushByteArray2Value is definitely wrong
        void PushByteArrayValue(int opcode)
        {
            Add(SCUMMOpcode.C_PushByteArrayValue, GetByteVar());
        }

        void PushByteArray2Value(int opcode)
        {
            Add(SCUMMOpcode.C_PushByteArray2Value, GetByteVar());
        }

        void PushByteVariable(int opcode)
        {
            Add(SCUMMOpcode.C_PushByteVariable, GetByteVar());
        }

        void ReadFile(int opcode)
        {
            Add(SCUMMOpcode.C_ReadFile);
        }

        void StopMusic(int opcode)
        {
            Add(SCUMMOpcode.C_StopMusic);
        }

        void StopTalking(int opcode)
        {
            Add(SCUMMOpcode.C_StopTalking);
        }

        void StoreByteArray(int opcode)
        {
            Add(SCUMMOpcode.C_StoreByteArray, GetByteVar());
        }

        void StoreByteArray2(int opcode)
        {
            Add(SCUMMOpcode.C_StoreByteArray2, GetByteVar());
        }

        void StoreByteVariable(int opcode)
        {
            Add(SCUMMOpcode.C_StoreByteVariable, GetByteVar());
        }

        void FActObjFacingOld(int opcode)
        {
            Add(SCUMMOpcode.F_ActObjFacingOld);
        }

        void FActorAnimCounter(int opcode)
        {
            Add(SCUMMOpcode.F_ActorAnimCounter);
        }

        void FFindVerb(int opcode)
        {
            Add(SCUMMOpcode.F_FindVerb);
        }

        void FProximityActObjPoint(int opcode)
        {
            Add(SCUMMOpcode.F_ProximityActObjPoint);
        }

        void FRoomScriptRunning(int opcode)
        {
            Add(SCUMMOpcode.F_RoomScriptRunning);
        }

        // TODO: Other than E4 (SetBoxSet), these unknown opcodes should never happen?
        void Unknown_E0(int opcode)
        {
            var subOpcodeArg = GetByte();
            Add(SCUMMOpcode.C_UnknownE0, subOpcodeArg);
        }

        void Unknown_E4(int opcode)
        {
            Add(SCUMMOpcode.C_UnknownE4);
        }

        void Unknown_EA(int opcode)
        {
            var subOpcodeArg = GetByte();
            Add(SCUMMOpcode.C_UnknownEA, subOpcodeArg);
        }

        void Unknown_FA(int opcode)
        {
            Add(SCUMMOpcode.C_UnknownFA);
        }

        protected virtual void _HandlePrint(bool baseOpArg = false)
        {
            var subOpcode = reader.ReadU8();

            switch (subOpcode)
            {
                case 65: Add(SCUMMOpcode.SC_PrintAt); break;
                case 66: Add(SCUMMOpcode.SC_PrintColor); break;
                case 67: Add(SCUMMOpcode.SC_PrintRight); break;
                case 69: Add(SCUMMOpcode.SC_PrintCenter); break;
                case 71: Add(SCUMMOpcode.SC_PrintLeft); break;
                case 72: Add(SCUMMOpcode.SC_PrintOverhead); break;
                case 74: Add(SCUMMOpcode.SC_PrintMumble); break;
                case 75: Add(SCUMMOpcode.SC_PrintString, GetString()); break;
                case 0xfe: Add(baseOpArg ? SCUMMOpcode.SC_PrintBaseOp : SCUMMOpcode.SC_PrintBaseOpNoArg); break;
                case 0xff: Add(SCUMMOpcode.SC_PrintEnd); break;
                default:
                    throw UnknownSubOpcode("print", subOpcode);
            }
        }
    }
}
