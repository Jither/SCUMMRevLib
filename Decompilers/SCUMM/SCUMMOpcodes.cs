namespace SCUMMRevLib.Decompilers.SCUMM
{
    // ReSharper disable InconsistentNaming
    public enum SCUMMOpcode
    {
        // Opcodes taking SubOpcode
        C_ArrayDim,
        // ActorStuff <actor> - but may be only SO init taking the
        // actor parameter for newer versions, while v3 takes it for all?
        C_ActorStuff,
        C_ActorStuff_3,          // For now, only used in old SCUMM 3 decompiler - remove when gone (unless needed)
        C_BoxStuff,
        C_CameraStuff,
        C_HeapStuff,
        C_RoomStuff,
        C_StringStuff,
        C_System,
        C_Userface,
        C_VerbSets,
        C_VerbStuff,

        // Stack manipulation (SCUMM 6-8)
        // Byte variants are a byte code implementation detail - likely removable
        C_Dup,
        C_Pop,
        C_PushNumber,            // SCUMM 6-8 (word in 6-7, dword in 8)
        C_PushVariable,          // SCUMM 6-8 (word in 6-7, dword in 8)
        C_PushArrayValue,        // SCUMM 6-8 (word in 6-7, dword in 8)
        C_PushArray2Value,       // SCUMM 6-8 (word in 6-7, dword in 8)
        C_PushByte,              // SCUMM 6-7
        C_PushByteArrayValue,    // SCUMM 6-7
        C_PushByteArray2Value,   // SCUMM 6-7
        C_PushByteVariable,      // SCUMM 6-7

        // Stack-based expressions
        C_Add,      // a + b
        C_BAnd,     // a & b
        C_BOr,      // a | b
        C_Div,      // a / b
        C_Eq,       // a == b
        C_Geq,      // a >= b
        C_Gt,       // a > b
        C_LAnd,     // a && b
        C_Leq,      // a <= b
        C_LOr,      // a || b
        C_Lt,       // a < b
        C_Mod,      // a % b
        C_Mul,      // a * b
        C_Neq,      // a != b
        C_Not,      // !a
        C_Sub,      // a - b

        // Variable handling
        // Byte variants are a byte code implementation detail - likely removable
        C_DecByteVariable,
        C_DecVariable,
        C_IncByteVariable,
        C_IncVariable,
        C_StoreArray,
        C_StoreArray2,
        C_StoreByteArray,
        C_StoreByteArray2,
        C_StoreByteVariable,
        C_StoreExpression,
        C_StoreExpressionEnd,
        C_StoreVariable,            // a    = b
        C_StoreVariableIndirect,    // [a]  = b
        C_StoreBitVariable,
        C_GetBitVariable,

        // Variable based arithmetic (pre-SCUMM 6)
        C_AddVariable,              // a   += b
        C_AddVariableIndirect,      // [a] += b
        C_DivVariable,              // a   /= b
        C_LAndVariable,             // a  &&= b
        C_LOrVariable,              // a  ||= b
        C_MulVariable,              // a   *= b
        C_SubVariable,              // a   -= b
        C_SubVariableIndirect,      // [a] -= b

        // Array handling
        C_ArrayLocalize,
        C_ArrayShuffle,
        C_AssignArray,          // WARNING: Different semantics in 6-8 - subopcode
        C_DecArray,
        C_DecByteArray,
        C_IncArray,
        C_IncByteArray,

        // Camera and rooms
        C_CameraAt,
        C_CameraFollow,
        C_CameraInRoom,
        C_CameraPanTo,
        C_CurrentRoom,          // Is this equivalent to C_CameraInRoom? Seems likely (it's currently only used in SCUMM 6-8)
        C_Fades,
        C_Lights,
        C_Pseudoroom,

        // Actors
        C_ActorElevation,       // SCUMM 1-2
        C_ComeOutDoor,
        C_DoAnimation,
        C_FaceTowards,
        // Do some of these apply to objects too? (then rename to ActObj)
        C_PutActorAtXY,
        C_PutActorAtObject,
        C_PutActorAtObject_7,
        C_PutActorAtXY_6,
        C_PutActorInRoom,
        C_StopTalking,          // If this does what I think it does (stop talk animation), other versions may use a different op for the same (see SCUMM Primer)
        C_WalkToXY,
        C_WalkToActor,
        C_WalkToActorWithin,
        C_WalkToObject,
        C_SwitchCostumeSet,

        // (Act)Objects
        C_ClassOf,  // SCUMMVM: SetClass
        C_NewNameOf,
        C_ObjectState,
        C_ObjectXY,
        C_OwnerOf,
        C_PickUpObject,
        C_PickUpObjectInRoom,
        C_StateOf,  // SCUMMVM: SetState
        C_StateOf1Set, // SCUMM 1-2: Bit specific
        C_StateOf2Set, // SCUMM 1-2: Bit specific
        C_StateOf4Set, // SCUMM 1-2: Bit specific
        C_StateOf8Set, // SCUMM 1-2: Bit specific
        C_StateOf1Clear, // SCUMM 1-2: Bit specific
        C_StateOf2Clear, // SCUMM 1-2: Bit specific
        C_StateOf4Clear, // SCUMM 1-2: Bit specific
        C_StateOf8Clear, // SCUMM 1-2: Bit specific

        // Verbs
        C_NewVerb,
        C_VerbOf,   // SCUMM 1-2

        // Drawing
        C_ActObjStamp,          // Same semantics as C_DrawObject? (maybe it's in SCUMM Primer)
        C_BlastObject,
        C_BlastObjectWindow,
        C_BlastText,
        C_DrawBox,
        C_DrawObject,
        C_DrawObjectAt,
        C_DrawObjectImage,
        C_DrawSentence,

        // Script execution / calls
        C_ChainScript,
        C_DoSentence,
        C_EndObject,
        C_EndScript,
        C_FreezeScripts,
        C_FreezeScript,         // Special case of C_FreezeScripts
        C_StartObject,
        C_StartScript,
        C_StartScriptBak,       // Special case of C_StartScript
        C_StartScriptRec,       // Special case of C_StartScript
        C_StartScriptBakRec,    // Special case of C_StartScript
        C_StartScriptQuick,     // Special case of C_StartScript
        C_StopObject,
        C_StopScript,           // Special case of <something>
        C_StopSentence,         // Special case of C_DoSentence
        C_ResetSentence,        // Special case of C_DoSentence (SCUMM 1-2)
        C_StopScriptDefault,
        C_UnfreezeScripts,      // Special case of C_FreezeScripts

        // Waiting / multitasking
        C_BreakHere,
        C_BreakHereCount,       // Special case of C_BreakHere
        C_BreakHereVar,
        C_SleepJiffies,
        C_SleepSeconds,         // Special case of C_SleepJiffies
        C_SleepMinutes,         // Special case of C_SleepJiffies
        C_SleepForVar,
        C_WaitForActor_3,
        C_WaitForMessage_3,
        C_WaitForSentence_3,
        C_WaitForStuff,

        // File handling
        C_CloseFile,
        C_DeleteFile,
        C_OpenFile,
        C_ReadFile,
        C_SaveLoadVariables,

        // User interface
        C_CutScene,
        C_EndCutScene,
        C_Override,
        C_OverrideOff,           // Special case of C_Override
        C_Userface_1,

        // Debug
        C_Debug,
        C_DebugWindex,

        // Walking boxes
        C_SetBox,
        C_SetBoxSet,

        // Misc runtime
        C_GetTimeDate,
        C_Restart,

        // Flow
        C_Jump,
        C_Return,

        // Stack-based conditions (SCUMM 6 and later)
        C_If,
        C_IfNot,

        // Specific conditions (pre SCUMM 6)
        C_IfClassOf,
        C_IfNotClassOf,
        C_IfEq,
        C_IfGeq,
        C_IfGt,
        C_IfInBox,
        C_IfLeq,
        C_IfLt,
        C_IfNeq,
        C_IfNotVar,
        C_IfVar,
        C_IfStateOf1,        // bit specific SCUMM 1-2
        C_IfStateOf2,        // bit specific SCUMM 1-2
        C_IfStateOf4,        // bit specific SCUMM 1-2
        C_IfStateOf8,        // bit specific SCUMM 1-2
        C_IfNotStateOf1,     // bit specific SCUMM 1-2
        C_IfNotStateOf2,     // bit specific SCUMM 1-2
        C_IfNotStateOf4,     // bit specific SCUMM 1-2
        C_IfNotStateOf8,     // bit specific SCUMM 1-2

        // Kludges
        C_Kludge,
        C_SoundKludge,

        // Text
        C_PrintCursor,              // In some older games, these are special cases of C_SayLine (actor > 250)
        C_PrintLine,                // In some older games, these are special cases of C_SayLine (actor > 250)
        C_PrintText,                // In some older games, these are special cases of C_SayLine (actor > 250)
        C_PrintDebug,               // In some older games, these are special cases of C_SayLine (actor > 250)
        C_PrintSystem,              // In some older games, these are special cases of C_SayLine (actor > 250)
        C_SayLine,
        C_SayLineDefault,
        C_SayLineSimple,
        C_SayLineSimpleDefault,

        // Audio/Video
        C_StartMusic,
        C_StartSound,
        C_StartVideo,
        C_StartVideo_8,             // Not sure if actually the same as C_StartVideo - takes direct string argument
        C_StopMusic,
        C_StopSound,

        // Unknown
        C_Unknown,                  // Only here for old SCUMM 5 decompiler - remove when that's gone.

        C_UnknownE0,
        C_UnknownE4,
        C_UnknownEA,
        C_UnknownFA,

        // Functions
        F_Abs,
        F_ActObjFacing,
        F_ActObjFacingOld,
        F_ActObjX,
        F_ActObjY,
        F_ActorAnimCounter,
        F_ActorBox,
        F_ActorChore,
        F_ActorCostume,
        F_ActorDepth,
        F_ActorElevation,
        F_ActorFacing,
        F_ActorInventory,
        F_ActorInventoryCount,
        F_ActorMoving,
        F_ActorRoom,
        F_ActorScale,
        F_ActorVariable,
        F_ActorWidth,
        F_ActorX,
        F_ActorY,
        F_ActorZPlane,
        F_AnimationCounter,
        F_ClassOf,
        F_ClosestActor,
        F_FindActor,
        F_FindAllObjects,
        F_FindInventory,
        F_FindObject,
        F_FindVerb,
        F_InBox,
        F_InSet,
        F_InventorySize,
        F_Kludge,
        F_ObjectImageHeight,
        F_ObjectImageWidth,
        F_ObjectImageX,
        F_ObjectImageY,
        F_ObjectRunning,
        F_OwnerOf,
        F_Pick,
        F_PickDefault,
        F_PickRandom,
        F_ProximityActObjPoint,
        F_Proximity2ActObjs,
        F_Proximity2Points,
        F_Random,
        F_RandomBetween,
        F_RoomScriptRunning,
        F_ScriptRunning,
        F_SoundRunning,
        F_StartObject,
        F_StartScript,
        F_StateOf,
        F_StringWidth,
        F_ValidVerb,
        F_VerbX,
        F_VerbY,

        SC_ExprFactor,
        SC_ExprAdd,
        SC_ExprSub,
        SC_ExprMul,
        SC_ExprDiv,
        SC_ExprCall,

        // Actor stuff
        SC_ActorCostume,
        SC_ActorStepDist,
        SC_ActorSound_3,
        SC_ActorSound_6,
        SC_ActorAnimationWalk,
        SC_ActorAnimationTalk,
        SC_ActorAnimationStand,
        SC_ActorAnimation,
        SC_ActorDefault,
        SC_ActorElevation,
        SC_ActorAnimationDefault,
        SC_ActorPalette,
        SC_ActorTalkColor,
        SC_ActorName,
        SC_ActorAnimationInit,
        SC_ActorWidth,
        SC_ActorHeight,
        SC_ActorScale,
        SC_ActorScale_3,
        SC_ActorNeverZClip,
        SC_ActorAlwaysZClip,
        SC_ActorIgnoreBoxes,
        SC_ActorFollowBoxes,
        SC_ActorAnimationSpeed,
        SC_ActorSpecialDraw,
        SC_ActorTextOffset,
        SC_ActorInit,
        SC_ActorVariable,
        SC_ActorIgnoreTurnsOn,
        SC_ActorIgnoreTurnsOff,
        SC_ActorNew,
        SC_ActorDepth,
        SC_ActorWalkScript,
        SC_ActorStop,
        SC_ActorFace,
        SC_ActorTurn,
        SC_ActorWalkPause,
        SC_ActorWalkResume,
        SC_ActorTalkScript,
        SC_ActorVolume,
        SC_ActorFrequency,
        SC_ActorPan,
        SC_ActorPaletteList,
        SC_ActorShadow,

        SC_ArrayDimScummVar,
        SC_ArrayDimUnknown200_7,
        SC_ArrayDimUnknown201_7,
        SC_ArrayDimUnknown202_7,
        SC_ArrayDimString,
        SC_ArrayUnDim,

        SC_Array2DimScummVar,
        SC_Array2DimUnknown200_7,
        SC_Array2DimUnknown201_7,
        SC_Array2DimUnknown202_7,
        SC_Array2DimString,
        SC_Array2UnDim,

        SC_AssignString,
        SC_AssignArray,
        SC_AssignArray2,

        // Userface
        SC_CursorOn,
        SC_CursorOff,
        SC_UserputOn,
        SC_UserputOff,
        SC_CursorSoftOn,
        SC_CursorSoftOff,
        SC_UserputSoftOn,
        SC_UserputSoftOff,
        SC_UserputClear,
        SC_CursorImage_3,
        SC_CursorImage,
        SC_CursorHotspot,
        SC_CursorHotspot_6,
        SC_CursorTransparent,
        SC_CursorSet,
        SC_CharsetSet,
        SC_CharsetColor,
        SC_CursorPut,

        // Heap stuff
        SC_HeapLoadScript,
        SC_HeapLoadSound,
        SC_HeapLoadCostume,
        SC_HeapLoadRoom,
        SC_HeapNukeScript,
        SC_HeapNukeSound,
        SC_HeapNukeCostume,
        SC_HeapNukeRoom,
        SC_HeapLockScript,
        SC_HeapLockSound,
        SC_HeapLockCostume,
        SC_HeapLockRoom,
        SC_HeapUnlockScript,
        SC_HeapUnlockSound,
        SC_HeapUnlockCostume,
        SC_HeapUnlockRoom,
        SC_HeapClearHeap,
        SC_HeapLoadCharset,
        SC_HeapNukeCharset,
        SC_HeapLoadObject,
        SC_HeapLoadObject_3,

        SC_SystemRestart,
        SC_SystemPause,
        SC_SystemQuit,

        // Print/SayLine
        SC_PrintAt,
        SC_PrintColor,
        SC_PrintRight,
        SC_PrintCenter,
        SC_PrintLeft,
        SC_PrintOverhead,
        SC_PrintMumble,
        SC_PrintString,
        SC_PrintBaseOp,
        SC_PrintBaseOpNoArg,
        SC_PrintEnd,
        SC_PrintCharset,
        SC_PrintWrap,
        SC_PrintClipped,
        SC_PrintErase,
        SC_PrintTo,
        SC_PrintTalkie,

        // SetBox
        SC_SetBoxSpecial,
        SC_SetBoxScale,
        SC_SetBoxSlot,
        SC_SetBoxPath,

        // Room stuff
        SC_RoomScroll,
        SC_RoomColor,
        SC_RoomSetScreen,
        SC_RoomPalette,
        SC_RoomShakeOn,
        SC_RoomShakeOff,
        SC_RoomScale,
        SC_RoomIntensity,
        SC_RoomSaveLoadGame,
        SC_RoomFade,
        SC_RoomRgbIntensity,
        SC_RoomSetShadowPalette,
        SC_RoomSaveString,
        SC_RoomLoadString,
        SC_RoomTransform,
        SC_RoomCycleSpeed,
        SC_RoomNewPalette,
        SC_RoomCopyPalette,
        SC_RoomSaveGame,
        SC_RoomLoadGame,
        SC_RoomSaturation,

        // Verb stuff
        SC_VerbImage_3,
        SC_VerbName,
        SC_VerbColor,
        SC_VerbHicolor,
        SC_VerbAt,
        SC_VerbOn,
        SC_VerbOff,
        SC_VerbDelete,
        SC_VerbNew,
        SC_VerbDimColor,
        SC_VerbDim,
        SC_VerbKey,
        SC_VerbCenter,
        SC_VerbNameStr,
        SC_VerbImage,
        SC_VerbBkColor,
        SC_VerbInit,
        SC_VerbEnd,
        SC_VerbCharset,
        SC_VerbLineSpacing,
        SC_VerbNextTo,
        SC_VerbToggle,          // Should eventually turn into VerbOn/VerbOff

        // WaitFor stuff
        SC_WaitForActor,
        SC_WaitForActor_6,
        SC_WaitForMessage,
        SC_WaitForCamera,
        SC_WaitForSentence,
        SC_WaitForAnimation,
        SC_WaitForTurn,

        // String stuff
        C_StringIsStrval,
        C_StringIsStrvar,
        C_StringIsVal,
        C_VarIsString,
        C_DimString,

        // VerbSets
        SC_VerbsSave,
        SC_VerbsRestore,
        SC_VerbsDelete,

        // Camera stuff
        SC_CameraPause,
        SC_CameraResume,

        // iMUSE
        I_FlushSoundQueue,
        I_InitializeDriver,
        I_TerminateDriver,
        I_Pause,
        I_Resume,
        I_SaveGame,
        I_RestoreGame,
        I_SetMasterVol,
        I_GetMasterVol,
        I_StartSound,
        I_StopSound,
        I_PrepareSound,
        I_StopAllSounds,
        I_GetSoundType,
        I_GetPlayStatus,
        I_SetDebug,

        // iMUSE MIDI
        I_MIDI_PlayerGetParam,
        I_MIDI_PlayerSetPriority,
        I_MIDI_PlayerSetVol,
        I_MIDI_PlayerSetPan,
        I_MIDI_PlayerSetTranspose,
        I_MIDI_PlayerSetDetune,
        I_MIDI_SeqSetSpeed,
        I_MIDI_SeqJump,
        I_MIDI_SeqScan,
        I_MIDI_SeqSetLoop,
        I_MIDI_SeqClearLoop,
        I_MIDI_PartSetPartEnable,
        I_MIDI_SeqSetHook,
        I_MIDI_FadeVol,
        I_MIDI_EnqueueTrigger,
        I_MIDI_EnqueueCommand,
        I_MIDI_ClearCommandQueue,
        I_MIDI_PlayerEnableLiveMidi,
        I_MIDI_PlayerDisableLiveMidi,
        I_MIDI_PlayerGetParam2,
        I_MIDI_HookSetHook,
        I_MIDI_InsertMidiMessage,
        I_MIDI_PartSetVol,
        I_MIDI_QueryQueue,
        I_MIDI_PartPrepareSetups,

        // TODO: iMUSE WAV and CD
    }
    // ReSharper restore InconsistentNaming
}
