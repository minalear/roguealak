using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Engine.Game.Combat;
using Roguelike.Engine.UI.Controls;
using Microsoft.Xna.Framework;

namespace Roguelike.Engine.Game.Stats.Races
{
    public class Human : Race
    {
        public Human()
            : base("Human")
        {
            this.Description = "The race of Man is one of the youngest and most versatile races to walk the physical realm.  What they lack in pure strength or intelligence, they make up in persistence and virility.";
            this.SubCultures = new List<Culture>() { new Human_Eastern(), new Human_Western(), new Human_Nordic() };
        }

        public override PlayerStats AddRacialStats(PlayerStats package)
        {
            package.Strength += 5;
            package.Agility += 5;
            package.Dexterity += 5;

            package.Intelligence += 5;
            package.Willpower += 5;
            package.Wisdom += 5;

            package.Constitution += 5;
            package.Endurance += 5;
            package.Fortitude += 5;

            return package;
        }
    }

    //Agility/Monk warriors
    public class Human_Eastern : Culture
    {
        public Human_Eastern()
            : base("Eastern")
        {
            this.Description = "If a Westerner was asked about his Eastern kindred, he would typically reply with 'Eccentric,' 'Bizarre,' or 'Completely  Insane.'  These warriors of faith and skill rely more on their allies, their weapons and their mind than sheer strength.  Eastern Humans typically make the better Tacticians or Rogues.";
            this.Trait = new EasternTrait(this);
            this.SkinColors = new List<Color>() { new Color(227, 196, 103), new Color(188, 151, 98), new Color(242, 226, 151) };
        }

        public class EasternTrait : Effect
        {
            public EasternTrait(Culture culture)
                : base(0)
            {
                EffectName = "Eastern Human";
                EffectDescription = culture.Description;
                EffectType = EffectTypes.Trait;

                IsHarmful = false;
                IsImmuneToPurge = true;
            }

            public override void CalculateStats()
            {
                this.parent.AttackPower.ModValue += 10;
            }
        }
    }

    //Mages, Priests and other casters
    public class Human_Western : Culture
    {
        public Human_Western()
            : base("Western")
        {
            this.Description = "The Western Kingdoms of Man are a bastion of the Arcane and Intellect.  Most Westerners lead a very eclectic lifestyle, prefering to either socialize with their peers or study in their magical towers of fortitude.  They typically leave all traditional norms outside and act on a whim.";
            this.Trait = new WesternTrait(this);
            this.SkinColors = new List<Color>() { new Color(142, 88, 62), new Color(224, 210, 147), new Color(199, 164, 100) };
        }

        public class WesternTrait : Effect
        {
            public WesternTrait(Culture culture)
                : base(0)
            {
                EffectName = "Western Human";
                EffectDescription = culture.Description;
                EffectType = EffectTypes.Trait;

                IsHarmful = false;
                IsImmuneToPurge = true;
            }

            public override void CalculateStats()
            {
                this.parent.SpellPower.ModValue += 10;
            }
        }
    }

    //Berserkers and other str warriors
    public class Human_Nordic : Culture
    {
        public Human_Nordic()
            : base("Nordic")
        {
            this.Description = "The Humans of the Northern Wastes have all but conquered their surrounding land and are looking to expand to the southron lands in search of glory.  These people are Raiders, Beserkers and Bards, and are proud of their history and spit on their enemies' ancestors.";
            this.Trait = new NordicTrait(this);
            this.SkinColors = new List<Color>() { new Color(253, 251, 230), new Color(243, 234, 229), new Color(241, 231, 195) };
        }

        public class NordicTrait : Effect
        {
            public NordicTrait(Culture culture)
                : base(0)
            {
                EffectName = "Nordic Human";
                EffectDescription = culture.Description;
                EffectType = EffectTypes.Trait;

                IsHarmful = false;
                IsImmuneToPurge = true;
            }

            public override void CalculateStats()
            {
                this.parent.PhysicalReduction.ModValue += 10;
            }
        }
    }
}
