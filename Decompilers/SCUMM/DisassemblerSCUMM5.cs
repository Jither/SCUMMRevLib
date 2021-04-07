namespace SCUMMRevLib.Decompilers.SCUMM
{
    public class DisassemblerSCUMM5 : DisassemblerSCUMM4
    {
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
                    case 2:
                        SCUMMParameter stepx = GetVarOrByte(subOpcode, 0x80);
                        SCUMMParameter stepy = GetVarOrByte(subOpcode, 0x40);
                        Add(SCUMMOpcode.SC_ActorStepDist, stepx, stepy);
                        break;
                    case 3:
                        SCUMMParameter soundTable = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorSound_3, soundTable);
                        break;
                    case 4:
                        SCUMMParameter chore = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorAnimationWalk, chore);
                        break;
                    case 5:
                        SCUMMParameter choreStart = GetVarOrByte(subOpcode, 0x80);
                        SCUMMParameter choreStop = GetVarOrByte(subOpcode, 0x40);
                        Add(SCUMMOpcode.SC_ActorAnimationTalk, choreStart, choreStop);
                        break;
                    case 6:
                        chore = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorAnimationStand, chore);
                        break;
                    case 7:
                        SCUMMParameter anim1 = GetVarOrByte(subOpcode, 0x80);
                        SCUMMParameter anim2 = GetVarOrByte(subOpcode, 0x40);
                        SCUMMParameter anim3 = GetVarOrByte(subOpcode, 0x20);
                        Add(SCUMMOpcode.SC_ActorAnimation, anim1, anim2, anim3);
                        break;
                    case 8:
                        Add(SCUMMOpcode.SC_ActorDefault);
                        break;
                    case 9:
                        SCUMMParameter elevation = GetVarOrWord(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorElevation, elevation);
                        break;
                    case 10:
                        Add(SCUMMOpcode.SC_ActorAnimationDefault);
                        break;
                    case 11:
                        SCUMMParameter slot = GetVarOrByte(subOpcode, 0x80);
                        SCUMMParameter j = GetVarOrByte(subOpcode, 0x40);
                        Add(SCUMMOpcode.SC_ActorPalette, slot, j);
                        break;
                    case 12:
                        SCUMMParameter color = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorTalkColor, color);
                        break;
                    case 13:
                        SCUMMParameter name = GetString();
                        Add(SCUMMOpcode.SC_ActorName, name);
                        break;
                    case 14:
                        chore = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorAnimationInit, chore);
                        break;
                    case 15:
                        SCUMMParameter list = GetParams(32, "actor color");
                        Add(SCUMMOpcode.SC_ActorPaletteList, list);
                        break;
                    case 16:
                        SCUMMParameter width = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorWidth, width);
                        break;
                    case 17:
                        SCUMMParameter scalex = GetVarOrByte(subOpcode, 0x80);
                        SCUMMParameter scaley = GetVarOrByte(subOpcode, 0x40);
                        Add(SCUMMOpcode.SC_ActorScale, scalex, scaley);
                        break;
                    case 18:
                        Add(SCUMMOpcode.SC_ActorNeverZClip);
                        break;
                    case 19:
                        SCUMMParameter zclip = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorAlwaysZClip, zclip);
                        break;
                    case 20:
                        Add(SCUMMOpcode.SC_ActorIgnoreBoxes);
                        break;
                    case 21:
                        Add(SCUMMOpcode.SC_ActorFollowBoxes);
                        break;
                    case 22:
                        SCUMMParameter speed = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorAnimationSpeed, speed);
                        break;
                    case 23:
                        SCUMMParameter shadow = GetVarOrByte(subOpcode, 0x80);
                        Add(SCUMMOpcode.SC_ActorShadow, shadow); // TODO: Early SpecialDraw?
                        break;
                    default:
                        throw UnknownSubOpcode("actor", subOpcode);

                }
                count++;
            }
        }

        protected override void DrawObject(int opcode)
        {
            SCUMMParameter objectNo = GetVarOrWord(opcode, 0x80);
            Add(SCUMMOpcode.C_DrawObject, objectNo);

            byte subOpcode = reader.ReadU8();
            switch (subOpcode & 0x1f)
            {
                case 0x01:
                    SCUMMParameter x = GetVarOrWord(subOpcode, 0x80);
                    SCUMMParameter y = GetVarOrWord(subOpcode, 0x40);
                    Add(SCUMMOpcode.C_DrawObjectAt, objectNo, x, y);
                    break;
                case 0x02:
                    SCUMMParameter imageNo = GetVarOrWord(subOpcode, 0x80);
                    Add(SCUMMOpcode.C_DrawObjectImage, objectNo, imageNo);
                    break;
                default:
                    throw UnknownSubOpcode("draw-object", subOpcode);

            }
        }
    }
}
