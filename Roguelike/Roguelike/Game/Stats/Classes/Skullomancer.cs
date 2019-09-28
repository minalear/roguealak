using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Roguelike.Engine.Game.Combat;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Engine.Game.Stats.Classes
{
    public class Skullomancer : Class
    {
        public Skullomancer()
            : base("Skullomancer")
        {
            this.Description = "The Skullomancer is a punk kid with a bucket of paint that decided to vandalize the neighborhood.";
            this.InheritAbilities = new List<Ability>() { new Ability_PaintSquare(), new Ability_PaintCircle(), new Ability_PaintSkull(), new Ability_PaintDot() };
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
                this.AbilityName = "Paint Square";
                this.AbilityNameShort = "Pnt Sqre";

                this.TargetingType = TargetingTypes.GroundTarget;
                this.AbilityType = AbilityTypes.Magical;
                this.abilityCost = 0;
                this.Range = -1;
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
                this.AbilityName = "Paint Circle";
                this.AbilityNameShort = "Pnt Crcl";

                this.TargetingType = TargetingTypes.GroundTarget;
                this.AbilityType = AbilityTypes.Magical;
                this.abilityCost = 0;
                this.Range = 15;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }

            public override void CastAbilityGround(StatsPackage caster, int x0, int y0, int radius, Level level)
            {
                Circle circle = new Circle() { X = x0, Y = y0, Radius = 5 };
                level.StainTile(circle, Color.Orange);
            }
        }
        public class Ability_PaintSkull : Ability
        {
            public Ability_PaintSkull()
                : base()
            {
                this.AbilityName = "Paint Skull";
                this.AbilityNameShort = "Pnt Skll";

                this.TargetingType = TargetingTypes.GroundTarget;
                this.AbilityType = AbilityTypes.Magical;
                this.abilityCost = 0;
                this.Range = 10;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }

            public override void CastAbilityGround(StatsPackage caster, int x0, int y0, int radius, Level level)
            {
                Color color = getColor();

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

            private Color getColor()
            {
                return Color.Red;
            }
        }
        public class Ability_PaintDot : Ability
        {
            public Ability_PaintDot()
                : base()
            {
                this.AbilityName = "Cut Thyself";
                this.AbilityNameShort = "Cut Self";

                this.TargetingType = TargetingTypes.Self;
                this.AbilityType = AbilityTypes.Physical;
                this.abilityCost = 0;
                this.Range = 1;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                caster.ApplyEffect(new Combat.Effects.BasicBleed(caster));
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }
        }
    }
}
