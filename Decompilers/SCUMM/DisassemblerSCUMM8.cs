namespace SCUMMRevLib.Decompilers.SCUMM
{
    public class DisassemblerSCUMM8 : DisassemblerSCUMM6
    {
        public DisassemblerSCUMM8()
        {
            GetVar = GetVar8;
        }

        protected override void InitOpcodes()
        {
            AddOpcode(0x01, PushNumber);
            AddOpcode(0x02, PushVariable);
            AddOpcode(0x03, PushArrayValue);
            AddOpcode(0x04, PushArray2Value);
            AddOpcode(0x05, Dup);
            AddOpcode(0x06, Pop);
            AddOpcode(0x07, Not);

            AddOpcode(0x08, Eq);
            AddOpcode(0x09, Neq);
            AddOpcode(0x0a, Gt);
            AddOpcode(0x0b, Lt);
            AddOpcode(0x0c, Leq);
            AddOpcode(0x0d, Geq);
            AddOpcode(0x0e, Add);
            AddOpcode(0x0f, Sub);

            AddOpcode(0x10, Mul);
            AddOpcode(0x11, Div);
            AddOpcode(0x12, LAnd);
            AddOpcode(0x13, LOr);
            AddOpcode(0x14, BAnd);
            AddOpcode(0x15, BOr);
            AddOpcode(0x16, Mod);

            AddOpcode(0x64, If);
            AddOpcode(0x65, IfNot);
            AddOpcode(0x66, Jump);
            AddOpcode(0x67, BreakHere);

            AddOpcode(0x68, BreakHereVar);
            AddOpcode(0x69, WaitForStuff);
            AddOpcode(0x6a, SleepJiffies);
            AddOpcode(0x6b, SleepSeconds);
            AddOpcode(0x6c, SleepMinutes);
            AddOpcode(0x6d, StoreVariable);
            AddOpcode(0x6e, IncVariable);
            AddOpcode(0x6f, DecVariable);

            AddOpcode(0x70, ArrayDim);
            AddOpcode(0x71, StoreArray);
            AddOpcode(0x72, IncArray);
            AddOpcode(0x73, DecArray);
            AddOpcode(0x74, ArrayDim2);
            AddOpcode(0x75, StoreArray2);
            AddOpcode(0x76, AssignArray);
            AddOpcode(0x77, ArrayShuffle);

            AddOpcode(0x78, ArrayLocalize);
            AddOpcode(0x79, StartScript);
            AddOpcode(0x7a, StartScriptQuick);
            AddOpcode(0x7b, EndScript);
            AddOpcode(0x7c, StopScript);
            AddOpcode(0x7d, ChainScript);
            AddOpcode(0x7e, Return);
            AddOpcode(0x7f, StartObject);

            AddOpcode(0x80, StopObject);
            AddOpcode(0x81, CutScene);
            AddOpcode(0x82, EndCutScene);
            AddOpcode(0x83, FreezeScripts);
            AddOpcode(0x84, OverRide);
            AddOpcode(0x85, OverRideOff);
            AddOpcode(0x86, StopSentence);
            AddOpcode(0x87, _Debug);

            AddOpcode(0x88, DebugWindex);
            AddOpcode(0x89, ClassOf);
            AddOpcode(0x8a, StateOf);
            AddOpcode(0x8b, OwnerOf);
            AddOpcode(0x8c, CameraPanTo);
            AddOpcode(0x8d, CameraFollow);
            AddOpcode(0x8e, CameraAt);
            AddOpcode(0x8f, SayLine);

            AddOpcode(0x90, SayLineDefault);
            AddOpcode(0x91, SayLineSimple);
            AddOpcode(0x92, SayLineSimpleDefault);
            AddOpcode(0x93, PrintLine);
            AddOpcode(0x94, PrintCursor);
            AddOpcode(0x95, PrintDebug);
            AddOpcode(0x96, PrintSystem);
            AddOpcode(0x97, BlastText);

            AddOpcode(0x98, DrawObject);

            AddOpcode(0x9a, BlastObject);

            AddOpcode(0x9c, Userface);
            AddOpcode(0x9d, CurrentRoom);
            AddOpcode(0x9e, ComeOutDoor);
            AddOpcode(0x9f, WalkActorToObject);

            AddOpcode(0xa0, WalkActorToXY);
            AddOpcode(0xa1, PutActorAtXY);
            AddOpcode(0xa2, PutActorAtObject);
            AddOpcode(0xa3, FaceTowards);
            AddOpcode(0xa4, DoAnimation);
            AddOpcode(0xa5, DoSentence);
            AddOpcode(0xa6, PickUpObject);
            AddOpcode(0xa7, SetBox);

            AddOpcode(0xa8, SetBoxPath);
            AddOpcode(0xa9, SetBoxSet);
            AddOpcode(0xaa, HeapStuff);
            AddOpcode(0xab, RoomStuff);
            AddOpcode(0xac, ActorStuff);
            AddOpcode(0xad, CameraStuff);
            AddOpcode(0xae, VerbStuff);
            AddOpcode(0xaf, StartSfx);

            AddOpcode(0xb0, StartMusic);
            AddOpcode(0xb1, StopSound);
            AddOpcode(0xb2, SoundKludge);
            AddOpcode(0xb3, System);
            AddOpcode(0xb4, VerbSets);
            AddOpcode(0xb5, NewNameOf);
            AddOpcode(0xb6, GetTimeDate);
            AddOpcode(0xb7, DrawBox);

            AddOpcode(0xb8, ActObjStamp);
            AddOpcode(0xb9, StartVideo);
            AddOpcode(0xba, Kludge);

            AddOpcode(0xc8, FStartScript);
            AddOpcode(0xc9, FStartObject);
            AddOpcode(0xca, FPick);
            AddOpcode(0xcb, FPickDefault);
            AddOpcode(0xcc, FPickRandom);
            AddOpcode(0xcd, FInSet);
            AddOpcode(0xce, FRandom);
            AddOpcode(0xcf, FRandomBetween);

            AddOpcode(0xd0, FClassOf);
            AddOpcode(0xd1, FStateOf);
            AddOpcode(0xd2, FOwnerOf);
            AddOpcode(0xd3, FScriptRunning);
            AddOpcode(0xd4, FObjectRunning);
            AddOpcode(0xd5, FSoundRunning);
            AddOpcode(0xd6, FAbs);
            AddOpcode(0xd7, FPixel);

            AddOpcode(0xd8, FKludge);
            AddOpcode(0xd9, FInBox);
            AddOpcode(0xda, FValidVerb);
            AddOpcode(0xdb, FFindActor);
            AddOpcode(0xdc, FFindObject);
            AddOpcode(0xde, FFindAllObjects);
            AddOpcode(0xdf, FActorInventory);

            AddOpcode(0xe0, FActorInventoryCount);
            AddOpcode(0xe1, FActorVariable);
            AddOpcode(0xe2, FActorRoom);
            AddOpcode(0xe3, FActorBox);
            AddOpcode(0xe4, FActorMoving);
            AddOpcode(0xe5, FActorCostume);
            AddOpcode(0xe6, FActorScale);
            AddOpcode(0xe7, FActorDepth);

            AddOpcode(0xe8, FActorElevation);
            AddOpcode(0xe9, FActorWidth);
            AddOpcode(0xea, FActObjFacing);
            AddOpcode(0xeb, FActObjX);
            AddOpcode(0xec, FActObjY);
            AddOpcode(0xed, FActorChore);
            AddOpcode(0xee, FProximity2ActObjs);
            AddOpcode(0xef, FProximity2Points);

            AddOpcode(0xf0, FObjectImageX);
            AddOpcode(0xf1, FObjectImageY);
            AddOpcode(0xf2, FObjectImageWidth);
            AddOpcode(0xf3, FObjectImageHeight);
            AddOpcode(0xf4, FVerbX);
            AddOpcode(0xf5, FVerbY);
            AddOpcode(0xf6, FStringWidth);
            AddOpcode(0xf7, FActorZPlane);
        }

        protected SCUMMParameter GetVar8()
        {
            return MakeVar(reader.ReadU32LE());
        }

        private SCUMMParameter MakeVar(uint value)
        {
            SCUMMParameter index = null;

            if ((value & 0x20000000) != 0)
            {
                uint i = reader.ReadU32LE();
                index = (i & 0x20000000) != 0 ? MakeVar(i & 0xdfffffff) : new SCUMMParameter(SCUMMParameterType.Number, i & 0x0fffffff);

                value = value & 0xdfffffff;
            }

            if ((value & 0x80000000) != 0)
            {
                return new SCUMMParameter(SCUMMParameterType.BitVar, value & 0x7fffffff, index);
            }

            if ((value & 0x40000000) != 0)
            {
                return new SCUMMParameter(SCUMMParameterType.LocVar, value & 0x0fffffff, index);
            }

            return new SCUMMParameter(SCUMMParameterType.Var, value);
        }

        protected override SCUMMParameter GetNumber()
        {
            return new SCUMMParameter(SCUMMParameterType.Number, reader.ReadU32LE());
        }

        protected override SCUMMParameter GetNumberSigned()
        {
            return new SCUMMParameter(SCUMMParameterType.Number, reader.ReadS32LE());
        }

        protected override void ActorStuff(int opcode)
        {
            Add(SCUMMOpcode.C_ActorStuff);

            // TODO: Remove subtraction
            byte subOpcode = (byte)(reader.ReadU8() - 0x64);
            switch (subOpcode)
            {
                case 0: Add(SCUMMOpcode.SC_ActorCostume); break;
                case 1: Add(SCUMMOpcode.SC_ActorStepDist); break;
                case 3: Add(SCUMMOpcode.SC_ActorAnimationDefault); break;
                case 4: Add(SCUMMOpcode.SC_ActorAnimationInit); break;
                case 5: Add(SCUMMOpcode.SC_ActorAnimationTalk); break;
                case 6: Add(SCUMMOpcode.SC_ActorAnimationWalk); break;
                case 7: Add(SCUMMOpcode.SC_ActorAnimationStand); break;
                case 8: Add(SCUMMOpcode.SC_ActorAnimationSpeed); break;
                case 9: Add(SCUMMOpcode.SC_ActorDefault); break;
                case 10: Add(SCUMMOpcode.SC_ActorElevation); break;
                case 11: Add(SCUMMOpcode.SC_ActorPalette); break;
                case 12: Add(SCUMMOpcode.SC_ActorTalkColor); break;
                case 13: Add(SCUMMOpcode.SC_ActorName, GetString()); break;
                case 14: Add(SCUMMOpcode.SC_ActorWidth); break;
                case 15: Add(SCUMMOpcode.SC_ActorScale); break;
                case 16: Add(SCUMMOpcode.SC_ActorNeverZClip); break;
                case 17: Add(SCUMMOpcode.SC_ActorAlwaysZClip); break;
                case 18: Add(SCUMMOpcode.SC_ActorIgnoreBoxes); break;
                case 19: Add(SCUMMOpcode.SC_ActorFollowBoxes); break;
                case 20: Add(SCUMMOpcode.SC_ActorSpecialDraw); break;
                case 21: Add(SCUMMOpcode.SC_ActorTextOffset); break;
                case 22: Add(SCUMMOpcode.SC_ActorInit); break;
                case 23: Add(SCUMMOpcode.SC_ActorVariable); break;
                case 24: Add(SCUMMOpcode.SC_ActorIgnoreTurnsOn); break;
                case 25: Add(SCUMMOpcode.SC_ActorIgnoreTurnsOff); break;
                case 26: Add(SCUMMOpcode.SC_ActorNew); break;
                case 27: Add(SCUMMOpcode.SC_ActorDepth); break;
                case 28: Add(SCUMMOpcode.SC_ActorStop); break;
                case 29: Add(SCUMMOpcode.SC_ActorFace); break;
                case 30: Add(SCUMMOpcode.SC_ActorTurn); break;
                case 31: Add(SCUMMOpcode.SC_ActorWalkScript); break;
                case 32: Add(SCUMMOpcode.SC_ActorTalkScript); break;
                case 33: Add(SCUMMOpcode.SC_ActorWalkPause); break;
                case 34: Add(SCUMMOpcode.SC_ActorWalkResume); break;
                case 35: Add(SCUMMOpcode.SC_ActorVolume); break;
                case 36: Add(SCUMMOpcode.SC_ActorFrequency); break;
                case 37: Add(SCUMMOpcode.SC_ActorPan); break;
                default:
                    throw UnknownSubOpcode("actor", subOpcode);
            }
        }

        protected override void ArrayDim(int opcode)
        {
            Add(SCUMMOpcode.C_ArrayDim);

            var subOpcode = reader.ReadU8();
            var arg = GetVar();

            switch (subOpcode)
            {
                case 10: Add(SCUMMOpcode.SC_ArrayDimScummVar, arg); break;
                case 11: Add(SCUMMOpcode.SC_ArrayDimString); break;
                case 12: Add(SCUMMOpcode.SC_ArrayUnDim, arg); break;
                default:
                    throw UnknownSubOpcode("array-dim", subOpcode);
            }
        }

        protected override void ArrayDim2(int opcode)
        {
            var subOpcode = reader.ReadU8();
            var arg = GetVar();

            switch (subOpcode)
            {
                case 10: Add(SCUMMOpcode.SC_Array2DimScummVar, arg); break;
                case 11: Add(SCUMMOpcode.SC_Array2DimString); break;
                case 12: Add(SCUMMOpcode.SC_Array2UnDim, arg); break;
                default:
                    throw UnknownSubOpcode("array2-dim", subOpcode);
            }
        }

        protected override void AssignArray(int opcode)
        {
            Add(SCUMMOpcode.C_AssignArray);

            var subOpcode = reader.ReadU8();
            var arg = GetVar();

            switch (subOpcode)
            {
                case 0x14: Add(SCUMMOpcode.SC_AssignString, arg, GetString()); break;
                case 0x15: Add(SCUMMOpcode.SC_AssignArray, arg); break;
                case 0x16: Add(SCUMMOpcode.SC_AssignArray2, arg); break;
                default:
                    throw UnknownSubOpcode("assign-array", subOpcode);
            }
        }

        protected void BlastText(int opcode)
        {
            Add(SCUMMOpcode.C_BlastText);
            _HandlePrint();
        }

        protected void CameraStuff(int opcode)
        {
            Add(SCUMMOpcode.C_CameraStuff);

            var subOpcode = reader.ReadU8();

            switch (subOpcode)
            {
                case 50: Add(SCUMMOpcode.SC_CameraPause); break;
                case 51: Add(SCUMMOpcode.SC_CameraResume); break;
                default:
                    throw UnknownSubOpcode("camera", subOpcode);
            }
        }

        protected void _Debug(int opcode)
        {
            Add(SCUMMOpcode.C_Debug);
        }

        protected void DebugWindex(int opcode)
        {
            Add(SCUMMOpcode.C_DebugWindex, GetString());
        }

        protected void DrawObject(int opcode)
        {
            // TODO: Is this the right opcode?
            Add(SCUMMOpcode.C_DrawObject);
        }

        protected override void HeapStuff(int opcode)
        {
            Add(SCUMMOpcode.C_HeapStuff);

            // TODO: Remove subtraction
            var subOpcode = (byte)(reader.ReadU8() - 0x3c);
            switch (subOpcode)
            {
                case 0: Add(SCUMMOpcode.SC_HeapLoadCharset); break;
                case 1: Add(SCUMMOpcode.SC_HeapLoadCostume); break;
                case 2: Add(SCUMMOpcode.SC_HeapLoadObject); break;
                case 3: Add(SCUMMOpcode.SC_HeapLoadRoom); break;
                case 4: Add(SCUMMOpcode.SC_HeapLoadScript); break;
                case 5: Add(SCUMMOpcode.SC_HeapLoadSound); break;
                case 6: Add(SCUMMOpcode.SC_HeapNukeCostume); break;
                case 7: Add(SCUMMOpcode.SC_HeapNukeRoom); break;
                case 8: Add(SCUMMOpcode.SC_HeapNukeScript); break;
                case 9: Add(SCUMMOpcode.SC_HeapNukeSound); break;
                case 10: Add(SCUMMOpcode.SC_HeapLockCostume); break;
                case 11: Add(SCUMMOpcode.SC_HeapLockRoom); break;
                case 12: Add(SCUMMOpcode.SC_HeapLockScript); break;
                case 13: Add(SCUMMOpcode.SC_HeapLockSound); break;
                case 14: Add(SCUMMOpcode.SC_HeapUnlockCostume); break;
                case 15: Add(SCUMMOpcode.SC_HeapUnlockRoom); break;
                case 16: Add(SCUMMOpcode.SC_HeapUnlockScript); break;
                case 17: Add(SCUMMOpcode.SC_HeapUnlockSound); break;
                default:
                    throw UnknownSubOpcode("heap", subOpcode);
            }
        }

        protected override void RoomStuff(int opcode)
        {
            Add(SCUMMOpcode.C_RoomStuff);

            // TODO: Remove subtraction
            var subOpcode = (byte)(reader.ReadU8() - 0x52);
            switch (subOpcode)
            {
                case 0: Add(SCUMMOpcode.SC_RoomPalette); break;
                case 3: Add(SCUMMOpcode.SC_RoomIntensity); break;
                case 5: Add(SCUMMOpcode.SC_RoomFade); break;
                case 6: Add(SCUMMOpcode.SC_RoomRgbIntensity); break;
                case 7: Add(SCUMMOpcode.SC_RoomTransform); break;
                case 8: Add(SCUMMOpcode.SC_RoomCycleSpeed); break;
                case 9: Add(SCUMMOpcode.SC_RoomCopyPalette); break;
                case 10: Add(SCUMMOpcode.SC_RoomNewPalette); break;
                case 11: Add(SCUMMOpcode.SC_RoomSaveGame); break;
                case 12: Add(SCUMMOpcode.SC_RoomLoadGame); break;
                case 13: Add(SCUMMOpcode.SC_RoomSaturation); break;
                default:
                    throw UnknownSubOpcode("room", subOpcode);
            }
        }

        protected override void StartVideo(int opcode)
        {
            Add(SCUMMOpcode.C_StartVideo_8, GetString());
        }

        protected override void System(int opcode)
        {
            Add(SCUMMOpcode.C_System);

            var subOpcode = reader.ReadU8();
            switch (subOpcode)
            {
                case 0x28: Add(SCUMMOpcode.SC_SystemRestart); break;
                case 0x29: Add(SCUMMOpcode.SC_SystemQuit); break;
                default:
                    throw UnknownSubOpcode("system", subOpcode);
            }
        }

        protected override void Userface(int opcode)
        {
            Add(SCUMMOpcode.C_Userface);

            // TODO: Remove subtraction
            byte subOpcode = (byte)(reader.ReadU8() - 0xdc);
            switch (subOpcode)
            {
                case 0: Add(SCUMMOpcode.SC_CursorOn); break;
                case 1: Add(SCUMMOpcode.SC_CursorOff); break;
                case 2: Add(SCUMMOpcode.SC_CursorSoftOn); break;
                case 3: Add(SCUMMOpcode.SC_CursorSoftOff); break;
                case 4: Add(SCUMMOpcode.SC_UserputOn); break;
                case 5: Add(SCUMMOpcode.SC_UserputOff); break;
                case 6: Add(SCUMMOpcode.SC_UserputSoftOn); break;
                case 7: Add(SCUMMOpcode.SC_UserputSoftOff); break;
                case 8: Add(SCUMMOpcode.SC_CursorImage); break;
                case 9: Add(SCUMMOpcode.SC_CursorHotspot_6); break;
                case 10: Add(SCUMMOpcode.SC_CursorTransparent); break;
                case 11: Add(SCUMMOpcode.SC_CharsetSet); break;
                case 12: Add(SCUMMOpcode.SC_CharsetColor); break;
                case 13: Add(SCUMMOpcode.SC_CursorPut); break;
                default:
                    throw UnknownSubOpcode("userface", subOpcode);
            }
        }

        protected override void VerbSets(int opcode)
        {
            Add(SCUMMOpcode.C_VerbSets);
            var subOpcode = reader.ReadU8();
            switch (subOpcode)
            {
                case 0xb4: Add(SCUMMOpcode.SC_VerbsSave); break;
                case 0xb5: Add(SCUMMOpcode.SC_VerbsRestore); break;
                case 0xb6: Add(SCUMMOpcode.SC_VerbsDelete); break;
                default:
                    throw UnknownSubOpcode("verbsets", subOpcode);
            }
        }

        protected override void VerbStuff(int opcode)
        {
            Add(SCUMMOpcode.C_VerbStuff);

            // TODO: Remove subtraction
            var subOpcode = (byte)(reader.ReadU8() - 0x96);

            switch (subOpcode)
            {
                case 0: Add(SCUMMOpcode.SC_VerbInit); break;
                case 1: Add(SCUMMOpcode.SC_VerbNew); break;
                case 2: Add(SCUMMOpcode.SC_VerbDelete); break;
                case 3: Add(SCUMMOpcode.SC_VerbName, GetString()); break;
                case 4: Add(SCUMMOpcode.SC_VerbAt); break;
                case 5: Add(SCUMMOpcode.SC_VerbOn); break;
                case 6: Add(SCUMMOpcode.SC_VerbOff); break;
                case 7: Add(SCUMMOpcode.SC_VerbColor); break;
                case 8: Add(SCUMMOpcode.SC_VerbHicolor); break;
                case 10: Add(SCUMMOpcode.SC_VerbDimColor); break;
                case 11: Add(SCUMMOpcode.SC_VerbDim); break;
                case 12: Add(SCUMMOpcode.SC_VerbKey); break;
                case 13: Add(SCUMMOpcode.SC_VerbImage); break;
                case 14: Add(SCUMMOpcode.SC_VerbNameStr); break;
                case 15: Add(SCUMMOpcode.SC_VerbCenter); break;
                case 16: Add(SCUMMOpcode.SC_VerbCharset); break;
                case 17: Add(SCUMMOpcode.SC_VerbLineSpacing); break;
                default:
                    throw UnknownSubOpcode("verb", subOpcode);
            }
        }

        protected override void WaitForStuff(int opcode)
        {
            Add(SCUMMOpcode.C_WaitForStuff);
            // TODO: Remove subtraction
            var subOpcode = (byte)(reader.ReadU8() - 0x1e);

            switch (subOpcode)
            {
                case 0: Add(SCUMMOpcode.SC_WaitForActor_6, GetNumberSigned()); break;
                case 1: Add(SCUMMOpcode.SC_WaitForMessage); break;
                case 2: Add(SCUMMOpcode.SC_WaitForCamera); break;
                case 3: Add(SCUMMOpcode.SC_WaitForSentence); break;
                case 4: Add(SCUMMOpcode.SC_WaitForAnimation, GetNumberSigned()); break;
                case 5: Add(SCUMMOpcode.SC_WaitForTurn, GetNumberSigned()); break;
                default:
                    throw UnknownSubOpcode("wait-for-stuff", subOpcode);
            }
        }



        protected void FActorChore(int opcode)
        {
            Add(SCUMMOpcode.F_ActorChore);
        }

        protected void FActorZPlane(int opcode)
        {
            Add(SCUMMOpcode.F_ActorZPlane);
        }

        protected void FObjectImageX(int opcode)
        {
            Add(SCUMMOpcode.F_ObjectImageX);
        }
        
        protected void FObjectImageY(int opcode)
        {
            Add(SCUMMOpcode.F_ObjectImageY);
        }
        
        protected void FObjectImageWidth(int opcode)
        {
            Add(SCUMMOpcode.F_ObjectImageWidth);
        }

        protected void FObjectImageHeight(int opcode)
        {
            Add(SCUMMOpcode.F_ObjectImageHeight);
        }

        protected void FObjectRunning(int opcode)
        {
            Add(SCUMMOpcode.F_ObjectRunning);
        }

        protected void FVerbX(int opcode)
        {
            Add(SCUMMOpcode.F_VerbX);
        }
        
        protected void FVerbY(int opcode)
        {
            Add(SCUMMOpcode.F_VerbY);
        }
        
        protected void FStringWidth(int opcode)
        {
            Add(SCUMMOpcode.F_StringWidth, GetString());
        }

        protected override void _HandlePrint(bool baseOpArg = false)
        {
            // TODO: Remove subtraction
            var subOpcode = (byte)(reader.ReadU8() - 0xc8);

            switch (subOpcode)
            {
                case 0: Add(baseOpArg ? SCUMMOpcode.SC_PrintBaseOp : SCUMMOpcode.SC_PrintBaseOpNoArg); break;
                case 1: Add(SCUMMOpcode.SC_PrintEnd); break;
                case 2: Add(SCUMMOpcode.SC_PrintAt); break;
                case 3: Add(SCUMMOpcode.SC_PrintColor); break;
                case 4: Add(SCUMMOpcode.SC_PrintCenter); break;
                case 5: Add(SCUMMOpcode.SC_PrintCharset); break;
                case 6: Add(SCUMMOpcode.SC_PrintLeft); break;
                case 7: Add(SCUMMOpcode.SC_PrintOverhead); break;
                case 8: Add(SCUMMOpcode.SC_PrintMumble); break;
                case 9: Add(SCUMMOpcode.SC_PrintString, GetString()); break;
                case 10: Add(SCUMMOpcode.SC_PrintWrap); break;
                default:
                    throw UnknownSubOpcode("print", subOpcode);
            }
        }
    }
}
