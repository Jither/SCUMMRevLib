using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCUMMRevLib.Decompilers.SCUMM
{
    public class SCUMM3Decompiler : SCUMM5Decompiler
    {
        private const byte SO_COSTUME_3 = 0x01;
        private const byte SO_ZCLIP_3 = 0x02;
        private const byte SO_BOXES_3 = 0x03;
        private const byte SO_STEP_DIST_3 = 0x04;
        private const byte SO_SOUND_3 = 0x05;
        private const byte SO_WALK_ANIMATION_3 = 0x06;
        private const byte SO_TALK_ANIMATION_3 = 0x07;
        private const byte SO_STAND_ANIMATION_3 = 0x08;
        private const byte SO_ANIMATION_3 = 0x09;
        private const byte SO_DEFAULT_3 = 0x0a;
        private const byte SO_ELEVATION_3 = 0x0b;
        private const byte SO_ANIMATION_DEFAULT_3 = 0x0c;
        private const byte SO_PALETTE_3 = 0x0d;
        private const byte SO_TALK_COLOR_3 = 0x0e;
        private const byte SO_ACTOR_NAME_3 = 0x0f;
        private const byte SO_INIT_ANIMATION_3 = 0x10;
        private const byte SO_PALETTE_LIST_3 = 0x11;
        private const byte SO_ACTOR_WIDTH_3 = 0x12;
        private const byte SO_ACTOR_SCALE_3 = 0x13;

        protected override SCUMMCommand ActorStuff(int opcode)
        {
            SCUMMParameter actor = GetVarOrByte(opcode, 0x80);

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
                    case SO_COSTUME_3:
                        SCUMMParameter costume = GetVarOrByte(so, 0x80);
                        args.Add(costume);
                        break;
                    case SO_STEP_DIST_3:
                        SCUMMParameter stepx = GetVarOrByte(so, 0x80);
                        SCUMMParameter stepy = GetVarOrByte(so, 0x40);
                        args.Add(stepx);
                        args.Add(stepy);
                        break;
                    case SO_SOUND_3:
                        SCUMMParameter soundTable = GetVarOrByte(so, 0x80);
                        args.Add(soundTable);
                        break;
                    case SO_WALK_ANIMATION_3:
                        SCUMMParameter chore = GetVarOrByte(so, 0x80);
                        args.Add(chore);
                        break;
                    case SO_TALK_ANIMATION_3:
                        SCUMMParameter choreStart = GetVarOrByte(so, 0x80);
                        SCUMMParameter choreStop = GetVarOrByte(so, 0x40);
                        args.Add(choreStart);
                        args.Add(choreStop);
                        break;
                    case SO_STAND_ANIMATION_3:
                        chore = GetVarOrByte(so, 0x80);
                        args.Add(chore);
                        break;
                    case SO_ANIMATION_3:
                        SCUMMParameter anim1 = GetVarOrByte(so, 0x80);
                        SCUMMParameter anim2 = GetVarOrByte(so, 0x40);
                        SCUMMParameter anim3 = GetVarOrByte(so, 0x20);
                        args.Add(anim1);
                        args.Add(anim2);
                        args.Add(anim3);
                        break;
                    case SO_DEFAULT_3:
                        break;
                    case SO_ELEVATION_3:
                        SCUMMParameter elevation = GetVarOrWord(so, 0x80);
                        args.Add(elevation);
                        break;
                    case SO_ANIMATION_DEFAULT_3:
                        break;
                    case SO_PALETTE_3:
                        SCUMMParameter slot = GetVarOrByte(so, 0x80);
                        SCUMMParameter j = GetVarOrByte(so, 0x40);
                        args.Add(slot);
                        args.Add(j);
                        break;
                    case SO_TALK_COLOR_3:
                        SCUMMParameter color = GetVarOrByte(so, 0x80);
                        args.Add(color);
                        break;
                    case SO_ACTOR_NAME_3:
                        SCUMMParameter name = GetString();
                        args.Add(name);
                        break;
                    case SO_INIT_ANIMATION_3:
                        chore = GetVarOrByte(so, 0x80);
                        args.Add(chore);
                        break;
                    case SO_PALETTE_LIST_3:
                        SCUMMParameter list = GetParams(16, "actor color");
                        args.Add(list);
                        break;
                    case SO_ACTOR_WIDTH_3:
                        SCUMMParameter width = GetVarOrByte(so, 0x80);
                        args.Add(width);
                        break;
                    case SO_ACTOR_SCALE_3:
                        SCUMMParameter scale = GetVarOrByte(so, 0x80);
                        args.Add(scale);
                        break;
                    case SO_ZCLIP_3:
                        SCUMMParameter zclip = GetVarOrByte(so, 0x80);
                        args.Add(zclip);
                        break;
                    case SO_BOXES_3:
                        SCUMMParameter boxes = GetVarOrByte(so, 0x80);
                        args.Add(boxes);
                        break;
                }
            }

            return new SCUMMCommand(SCUMMOpcode.C_ActorStuff_3, actor, new SCUMMParameter(SCUMMParameterType.Array, args.ToArray()));
        }

        protected override SCUMMCommand DrawObject(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            SCUMMParameter x = GetVarOrWord(opcode, 0x40);
            SCUMMParameter y = GetVarOrWord(opcode, 0x20);
            return new SCUMMCommand(SCUMMOpcode.C_DrawObjectAt, objectNo, x, y);
        }


    }
}
