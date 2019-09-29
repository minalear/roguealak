using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using Roguelike.Core.Combat;
using Roguelike.Engine;

namespace Roguelike.Core.Stats.Classes
{
    public class Skullomancer : Class
    {
        public Skullomancer()
            : base("Skullomancer")
        {
            Description = "The Skullomancer is a punk kid with a bucket of paint that decided to vandalize the neighborhood.";
            InheritAbilities = new List<Ability>() { new Ability_PaintSquare(), new Ability_PaintCircle(), new Ability_PaintSkull(), new Ability_PaintDot() };
        }
        public override PlayerStats CalculateStats(PlayerStats stats)
        {
            stats.Strength += 0;
            stats.Agility += 0;
            stats.Dexterity += 0;

            stats.Intelligence += 10;
            stats.Willpower += 0;
            stats.Wisdom += 0;

            stats.Constitution += 10;
            stats.Endurance += 0;
            stats.Fortitude += 0;
            
            return base.CalculateStats(stats);
        }

        public class Ability_PaintSquare : Ability
        {
            public Ability_PaintSquare()
                : base()
            {
                AbilityName = "Paint Square";
                AbilityNameShort = "Pnt Sqre";

                TargetingType = TargetingTypes.GroundTarget;
                AbilityType = AbilityTypes.Magical;
                abilityCost = 0;
                Range = -1;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }

            public override void CastAbilityGround(StatsPackage caster, int x0, int y0, int radius, Level level)
            {
                Rectangle area = new Rectangle(x0 - 5, y0 - 2, 11, 5);
                level.StainTile(area, Color.Blue);
            }
        }
        public class Ability_PaintCircle : Ability
        {
            public Ability_PaintCircle()
                : base()
            {
                AbilityName = "Paint Circle";
                AbilityNameShort = "Pnt Crcl";

                TargetingType = TargetingTypes.GroundTarget;
                AbilityType = AbilityTypes.Magical;
                abilityCost = 0;
                Range = 15;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }

            public override void CastAbilityGround(StatsPackage caster, int x0, int y0, int radius, Level level)
            {
                Circle circle = new Circle() { X = x0, Y = y0, Radius = 5 };
                level.StainTile(circle, Color4.Orange);
            }
        }
        public class Ability_PaintSkull : Ability
        {
            public Ability_PaintSkull()
                : base()
            {
                AbilityName = "Paint Skull";
                AbilityNameShort = "Pnt Skll";

                TargetingType = TargetingTypes.GroundTarget;
                AbilityType = AbilityTypes.Magical;
                abilityCost = 0;
                Range = 10;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }

            public override void CastAbilityGround(StatsPackage caster, int x0, int y0, int radius, Level level)
            {
                Color4 color = getColor();

                GameManager.CurrentLevel.StainTile(x0 - 2, y0 - 3, color);
                GameManager.CurrentLevel.StainTile(x0 - 1, y0 - 3, color);
                GameManager.CurrentLevel.StainTile(x0 + 0, y0 - 3, color);
                GameManager.CurrentLevel.StainTile(x0 + 1, y0 - 3, color);
                GameManager.CurrentLevel.StainTile(x0 + 2, y0 - 3, color);

                GameManager.CurrentLevel.StainTile(x0 - 3, y0 - 2, color);
                GameManager.CurrentLevel.StainTile(x0 - 2, y0 - 2, color);
                GameManager.CurrentLevel.StainTile(x0 - 1, y0 - 2, color);
                GameManager.CurrentLevel.StainTile(x0 + 0, y0 - 2, color);
                GameManager.CurrentLevel.StainTile(x0 + 1, y0 - 2, color);
                GameManager.CurrentLevel.StainTile(x0 + 2, y0 - 2, color);
                GameManager.CurrentLevel.StainTile(x0 + 3, y0 - 2, color);

                GameManager.CurrentLevel.StainTile(x0 - 3, y0 - 1, color);
                GameManager.CurrentLevel.StainTile(x0 - 2, y0 - 1, color);
                GameManager.CurrentLevel.StainTile(x0 + 0, y0 - 1, color);
                GameManager.CurrentLevel.StainTile(x0 + 2, y0 - 1, color);
                GameManager.CurrentLevel.StainTile(x0 + 3, y0 - 1, color);

                GameManager.CurrentLevel.StainTile(x0 - 3, y0 + 0, color);
                GameManager.CurrentLevel.StainTile(x0 - 2, y0 + 0, color);
                GameManager.CurrentLevel.StainTile(x0 - 1, y0 + 0, color);
                GameManager.CurrentLevel.StainTile(x0 + 1, y0 + 0, color);
                GameManager.CurrentLevel.StainTile(x0 + 2, y0 + 0, color);
                GameManager.CurrentLevel.StainTile(x0 + 3, y0 + 0, color);

                GameManager.CurrentLevel.StainTile(x0 - 2, y0 + 1, color);
                GameManager.CurrentLevel.StainTile(x0 - 1, y0 + 1, color);
                GameManager.CurrentLevel.StainTile(x0 + 0, y0 + 1, color);
                GameManager.CurrentLevel.StainTile(x0 + 1, y0 + 1, color);
                GameManager.CurrentLevel.StainTile(x0 + 2, y0 + 1, color);

                GameManager.CurrentLevel.StainTile(x0 - 2, y0 + 2, color);
                GameManager.CurrentLevel.StainTile(x0 - 1, y0 + 2, color);
                GameManager.CurrentLevel.StainTile(x0 + 0, y0 + 2, color);
                GameManager.CurrentLevel.StainTile(x0 + 1, y0 + 2, color);
                GameManager.CurrentLevel.StainTile(x0 + 2, y0 + 2, color);

                GameManager.CurrentLevel.StainTile(x0 - 2, y0 + 3, color);
                GameManager.CurrentLevel.StainTile(x0 + 0, y0 + 3, color);
                GameManager.CurrentLevel.StainTile(x0 + 2, y0 + 3, color);
            }

            private Color4 getColor()
            {
                return Color4.Red;
            }
        }
        public class Ability_PaintDot : Ability
        {
            public Ability_PaintDot()
                : base()
            {
                AbilityName = "Cut Thyself";
                AbilityNameShort = "Cut Self";

                TargetingType = TargetingTypes.Self;
                AbilityType = AbilityTypes.Physical;
                abilityCost = 0;
                Range = 1;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                caster.ApplyEffect(new Combat.Effects.BasicBleed(caster));
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }
        }
    }
}
