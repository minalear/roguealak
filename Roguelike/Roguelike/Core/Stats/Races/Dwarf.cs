using System;
using System.Collections.Generic;
using Roguelike.Core.Combat;

namespace Roguelike.Core.Stats.Races
{
    public class Dwarf : Race
    {
        public Dwarf()
            : base("Dwarf")
        {
            this.Description = "A short, sturdy creature fond of drink and industry.";
            this.SubCultures = new List<Culture>() { new Dwarf_Iron(), new Dwarf_Island(), new Dwarf_Pale() };
        }

        public override PlayerStats AddRacialStats(PlayerStats package)
        {
            package.Strength += 6;
            package.Agility += 6;
            package.Dexterity += 6;

            package.Intelligence += 3;
            package.Willpower += 3;
            package.Wisdom += 3;

            package.Constitution += 7;
            package.Endurance += 7;
            package.Fortitude += 4;

            return package;
        }
    }

    public class Dwarf_Island : Culture
    {
        public Dwarf_Island()
            : base("Island")
        {
            this.Description = "A thousand years ago, a sect of dwarves decided to leave their mountain homes and sail the sea.  They crash landed on an island chain not too long after.  In the following years, they enslaved the island pygmies and established a trading civilization that spans the globe.";
            this.Trait = new IslandTrait(this);
            this.SkinColors = new List<Color>() { new Color(236, 213, 143), new Color(201, 161, 134), new Color(249, 245, 182) };
        }

        public class IslandTrait : Effect
        {
            public IslandTrait(Culture culture)
                : base(0)
            {
                EffectName = "Island Dwarf";
                EffectDescription = culture.Description;
                EffectType = EffectTypes.Trait;

                IsHarmful = false;
                IsImmuneToPurge = true;
            }
        }
    }

    public class Dwarf_Iron : Culture
    {
        public Dwarf_Iron()
            : base("Iron")
        {
            this.Description = "The Iron Dwarves are your stereotypical mountain dwarves that tend to dig too deep and cherish hordes of treasures.  They are the chief supplier of raw materials to the world and construct fantastical underground civilizations that can span miles.";
            this.Trait = new IronTrait(this);
            this.SkinColors = new List<Color>() { new Color(181, 169, 153), new Color(181, 153, 153), new Color(178, 181, 153) };
        }

        public class IronTrait : Effect
        {
            public IronTrait(Culture culture)
                : base(0)
            {
                EffectName = "Iron Dwarf";
                EffectDescription = culture.Description;
                EffectType = EffectTypes.Trait;

                IsHarmful = false;
                IsImmuneToPurge = true;
            }
        }
    }

    public class Dwarf_Pale : Culture
    {
        public Dwarf_Pale()
            : base("Pale")
        {
            this.Description = "The Pale Dwarves, like the Island Dwarves, used to be a part of the Iron Dwarf clans, but a freak mine shaft collapsed caused their entire district to fall into the earthen depths.  Not having any dwarves of the miner caste, they were forced to survive in the pitch black for centuries before being discovered again.  Over that period of time, these dwarves became more pale, bug eyed and hostile and attacked their unlucky saviors.";
            this.Trait = new PaleTrait(this);
            this.SkinColors = new List<Color>() { new Color(222, 219, 197), new Color(255, 249, 207), new Color(234, 215, 195) };
        }

        public class PaleTrait : Effect
        {
            public PaleTrait(Culture culture)
                : base(0)
            {
                EffectName = "Pale Dwarf";
                EffectDescription = culture.Description;
                EffectType = EffectTypes.Trait;

                IsHarmful = false;
                IsImmuneToPurge = true;
            }
        }
    }
}
