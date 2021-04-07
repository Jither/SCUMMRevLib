using System;

namespace SCUMMRevLib.Decompilers.SCUMM
{
    public class DisassemblerSCUMM4 : DisassemblerSCUMM3
    {
        protected override void InitOpcodes()
        {
            base.InitOpcodes();

            AddOpcode(0x3b, FActorScale);
            AddOpcode(0x4c, SoundKludge);
            AddOpcode(0xae, WaitForStuff);
            AddOpcode(0xbb, FActorScale);
        }

        protected void FActorScale(int opcode)
        {
            SCUMMParameter var = GetVar();
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);
            Add(SCUMMOpcode.F_ActorScale, var, actor);
        }

        protected void SoundKludge(int opcode)
        {
            Add(SCUMMOpcode.C_SoundKludge);
            SCUMMParameter prmsParam = GetParams(16, "sound-kludge");
            SCUMMParameter[] prms = prmsParam.Value as SCUMMParameter[];

            if (prms == null || prms.Length == 0)
            {
                throw new SCUMMDecompilerException("Unexpected parameters to sound-kludge: {0}", prms);
            }

            SCUMMParameter param = prms[0];
            if (param.Type != SCUMMParameterType.Number)
            {
                throw new SCUMMDecompilerException("Expected first sound-kludge parameter to be subopcode (number), but got {0}", param.Type);
            }

            int engineSubOpcode = Convert.ToInt32(param.Value);
            if (engineSubOpcode == 0xffff)
            {
                Add(SCUMMOpcode.I_FlushSoundQueue);
            }

            int engine = engineSubOpcode >> 8;
            byte subOpcode = (byte)(engineSubOpcode & 0xff);

            if (engine == 0)
            {
                switch (subOpcode)
                {
                    case 0x00: Add(SCUMMOpcode.I_InitializeDriver, prms); return;
                    case 0x01: Add(SCUMMOpcode.I_TerminateDriver, prms); return;
                    case 0x02: Add(SCUMMOpcode.I_Pause, prms); return;
                    case 0x03: Add(SCUMMOpcode.I_Resume, prms); return;
                    case 0x04: Add(SCUMMOpcode.I_SaveGame, prms); return;
                    case 0x05: Add(SCUMMOpcode.I_RestoreGame, prms); return;
                    case 0x06: Add(SCUMMOpcode.I_SetMasterVol, prms); return;
                    case 0x07: Add(SCUMMOpcode.I_GetMasterVol, prms); return;
                    case 0x08: Add(SCUMMOpcode.I_StartSound, prms); return;
                    case 0x09: Add(SCUMMOpcode.I_StopSound, prms); return;
                    case 0x0a: Add(SCUMMOpcode.I_PrepareSound, prms); return;
                    case 0x0b:
                        SanityCheck(prms.Length == 1, "Expected 0 arguments to stop-all-sounds");
                        Add(SCUMMOpcode.I_StopAllSounds, prms);
                        return;
                    case 0x0c: Add(SCUMMOpcode.I_GetSoundType, prms); return;
                    case 0x0d:
                        SanityCheck(prms.Length == 2, "Expected 1 argument to sound-play-status");
                        Add(SCUMMOpcode.I_GetPlayStatus, prms);
                        return;
                    case 0x0e: Add(SCUMMOpcode.I_SetDebug, prms); return;
                    default:
                        throw UnknownSubOpcode("sound-kludge", subOpcode);
                    // 272 (0x110) : clear-command-q
                }
            }

            if (engine == 1) // MIDI
            {
                switch (subOpcode)
                {
                    case 0x00: Add(SCUMMOpcode.I_MIDI_PlayerGetParam, prms); return;
                    case 0x01: Add(SCUMMOpcode.I_MIDI_PlayerSetPriority, prms); return;
                    case 0x02: Add(SCUMMOpcode.I_MIDI_PlayerSetVol, prms); return;
                    case 0x03: Add(SCUMMOpcode.I_MIDI_PlayerSetPan, prms); return;
                    case 0x04: Add(SCUMMOpcode.I_MIDI_PlayerSetTranspose, prms); return;
                    case 0x05: Add(SCUMMOpcode.I_MIDI_PlayerSetDetune, prms); return;
                    case 0x06: Add(SCUMMOpcode.I_MIDI_SeqSetSpeed, prms); return;
                    case 0x07: Add(SCUMMOpcode.I_MIDI_SeqJump, prms); return;
                    case 0x08: Add(SCUMMOpcode.I_MIDI_SeqScan, prms); return;
                    case 0x09: Add(SCUMMOpcode.I_MIDI_SeqSetLoop, prms); return;
                    case 0x0a: Add(SCUMMOpcode.I_MIDI_SeqClearLoop, prms); return;
                    case 0x0b: Add(SCUMMOpcode.I_MIDI_PartSetPartEnable, prms); return;
                    case 0x0c: Add(SCUMMOpcode.I_MIDI_SeqSetHook, prms); return;
                    case 0x0d: Add(SCUMMOpcode.I_MIDI_FadeVol, prms); return;
                    case 0x0e:
                        SanityCheck(prms.Length == 3, "Expected 2 arguments to q-sound-trigger");
                        Add(SCUMMOpcode.I_MIDI_EnqueueTrigger, prms);
                        return;
                    case 0x0f: Add(SCUMMOpcode.I_MIDI_EnqueueCommand, prms); return;
                    case 0x10: Add(SCUMMOpcode.I_MIDI_ClearCommandQueue, prms); return;
                    case 0x11: Add(SCUMMOpcode.I_MIDI_PlayerEnableLiveMidi, prms); return;
                    case 0x12: Add(SCUMMOpcode.I_MIDI_PlayerDisableLiveMidi, prms); return;
                    case 0x13: Add(SCUMMOpcode.I_MIDI_PlayerGetParam2, prms); return;
                    case 0x14: Add(SCUMMOpcode.I_MIDI_HookSetHook, prms); return;
                    case 0x15: Add(SCUMMOpcode.I_MIDI_InsertMidiMessage, prms); return;
                    case 0x16: Add(SCUMMOpcode.I_MIDI_PartSetVol, prms); return;
                    case 0x17: Add(SCUMMOpcode.I_MIDI_QueryQueue, prms); return;
                    case 0x18: Add(SCUMMOpcode.I_MIDI_PartPrepareSetups, prms); return;
                    default:
                        throw UnknownSubOpcode("sound-kludge", subOpcode);
                }
            }

            if (engine == 2) // Wave
            {

            }

            if (engine == 3) // CD
            {

            }
        }

        protected void WaitForStuff(int opcode)
        {
            Add(SCUMMOpcode.C_WaitForStuff);

            byte subOpcode = reader.ReadU8();
            switch (subOpcode & 0x1f)
            {
                case 0x01:
                    SCUMMParameter actor = GetVarOrByte(subOpcode, 0x80);
                    Add(SCUMMOpcode.SC_WaitForActor, actor);
                    break;
                case 0x02:
                    Add(SCUMMOpcode.SC_WaitForMessage);
                    break;
                case 0x03:
                    Add(SCUMMOpcode.SC_WaitForCamera);
                    break;
                case 0x04:
                    Add(SCUMMOpcode.SC_WaitForSentence);
                    break;
                default:
                    throw UnknownSubOpcode("wait-for-stuff", subOpcode);
            }
        }
    }
}
