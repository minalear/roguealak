using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Core.Combat.Abilities;

namespace Roguelike.Core.Combat
{
    public static class SpellBook
    {
        public static Ability DefaultAction;

        public static Ability HotSlotOne;
        public static Ability HotSlotTwo;
        public static Ability HotSlotThree;
        public static Ability HotSlotFour;

        public static Ability HotSlotFive;
        public static Ability HotSlotSix;
        public static Ability HotSlotSeven;
        public static Ability HotSlotEight;

        public static void SetSpell(int slot, Ability ability)
        {
            if (slot == 1)
                HotSlotOne = ability;
            else if (slot == 2)
                HotSlotTwo = ability;
            else if (slot == 3)
                HotSlotThree = ability;
            else if (slot == 4)
                HotSlotFour = ability;

            else if (slot == 5)
                HotSlotFive = ability;
            else if (slot == 6)
                HotSlotSix = ability;
            else if (slot == 7)
                HotSlotSeven = ability;
            else if (slot == 8)
                HotSlotEight = ability;
            else
                DefaultAction = ability;
        }

        public static void ClearSpells()
        {
            DefaultAction = null;

            HotSlotOne = null;
            HotSlotTwo = null;
            HotSlotThree = null;
            HotSlotFour = null;

            HotSlotFive = null;
            HotSlotSix = null;
            HotSlotSeven = null;
            HotSlotEight = null;
        }
    }
}
