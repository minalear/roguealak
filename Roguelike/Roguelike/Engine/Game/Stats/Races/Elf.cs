﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Engine.Game.Combat;
using Roguelike.Engine.UI.Controls;
using Microsoft.Xna.Framework;

namespace Roguelike.Engine.Game.Stats.Races
{
    public class Elf : Race
    {
        public Elf()
            : base("Elf")
        {
            this.Description = "The pointed ear humanoids known as the Elves are one of the most ancient races to walk the mortal realm.  Casted out of heaven eons ago, the Elves formed clans and castes and crafted the world as they saw fit.  Once when the mortal races began popping up, the Elves bestowed their knowledge onto them, only to be betrayed centuries later by Humans.  Now with dwindling numbers, Elves have become much more distant and do not contact the outside world often.";
            this.SubCultures = new List<Culture>() { new Elf_Forest(), new Elf_Mountain(), new Elf_Savage() };
        }

        public override PlayerStats AddRacialStats(PlayerStats package)
        {
            package.Strength += 4;
            package.Agility += 6;
            package.Dexterity += 6;

            package.Intelligence += 6;
            package.Willpower += 6;
            package.Wisdom += 6;

            package.Constitution += 3;
            package.Endurance += 3;
            package.Fortitude += 5;

            return package;
        }
    }

    public class Elf_Forest : Culture
    {
        public Elf_Forest()
            : base("Forest")
        {
            this.Description = "The Forest Elves are seen to be the most wise and powerful of the Elvish clans.  They are primarily lorekeepers and guardians of their homelands.  They tend to keep to themselves, but often share knowledge with whoever seeks them out.";
            this.Trait = new ForestTrait(this);
            this.SkinColors = new List<Color>() { new Color(231, 191, 191), new Color(217, 188, 146), new Color(243, 220, 190) };
        }

        public class ForestTrait : Effect
        {
            public ForestTrait(Culture culture)
                : base(0)
            {
                EffectName = "Forest Elf";
                EffectDescription = culture.Description;
                EffectType = EffectTypes.Trait;

                IsHarmful = false;
                IsImmuneToPurge = true;
            }
        }
    }

    public class Elf_Mountain : Culture
    {
        public Elf_Mountain()
            : base("Mountain")
        {
            this.Description = "When casted out, a small group of Elves decided to craft mountains to call home.  These mountain ranges are some of the world's largest and most remote geological structures.  No Mortal creature has walked the halls of the mountain homes and the Mountain Elves would like to keep it that way.";
            this.Trait = new MountainTrait(this);
            this.SkinColors = new List<Color>() { new Color(175, 241, 239), new Color(157, 180, 194), new Color(167, 157, 194) };
        }

        public class MountainTrait : Effect
        {
            public MountainTrait(Culture culture)
                : base(0)
            {
                EffectName = "Mountain Elf";
                EffectDescription = culture.Description;
                EffectType = EffectTypes.Trait;

                IsHarmful = false;
                IsImmuneToPurge = true;
            }
        }
    }

    public class Elf_Savage : Culture
    {
        public Elf_Savage()
            : base("Savage")
        {
            this.Description = "The Savage Elves are a group of Elf-folk who took their exile from Heaven very badly.  Driven mad from their lost home, these Elves formed horns and roved the barren wastes attacking (and consuming) any that would stray too close.  Eventually, once when the Orcish people arrived to the moral realm, they allied together and form one of the strongest, most brutal alliances known to the mortal races.  If it wasn't for constant in-fighting, this horde would have conquered the world easily.";
            this.Trait = new SavageTrait(this);
            this.SkinColors = new List<Color>() { new Color(187, 215, 168), new Color(215, 215, 204), new Color(135, 137, 76) };
        }

        public class SavageTrait : Effect
        {
            public SavageTrait(Culture culture)
                : base(0)
            {
                EffectName = "Savage Elf";
                EffectDescription = culture.Description;
                EffectType = EffectTypes.Trait;

                IsHarmful = false;
                IsImmuneToPurge = true;
            }
        }
    }
}
