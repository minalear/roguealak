using System;
using Roguelike.Engine.Game.Entities;

namespace Roguelike.Engine.Game.Combat.Abilities
{
    public class CarpetBomb : Ability
    {
        public CarpetBomb()
            : base()
        {
            this.AbilityName = "Carpet Bomb";
            this.AbilityNameShort = "Crpt Bmb";

            this.abilityCost = 25;
            this.TargetingType = TargetingTypes.GroundTarget;
        }

        public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
        {
            Bomb bomb = new Bomb(target.ParentEntity.ParentLevel, 5) { X = target.ParentEntity.X, Y = target.ParentEntity.Y };
            target.ParentEntity.ParentLevel.Entities.Add(bomb);

            return new CombatResults() { UsedAbility = this, Caster = caster, Target = target };
        }

        public override void CastAbilityGround(Stats.StatsPackage caster, int x0, int y0, int radius, Level level)
        {
            radius = 50;
            if (this.CanCastAbility(caster, x0, y0))
            {
                this.ApplyAbilityCost(caster);

                for (int angle = 0; angle < 360; angle += 1)
                {
                    for (int r = 0; r < radius; r++)
                    {
                        int x = (int)(x0 + 0.5 + r * Math.Cos(angle));
                        int y = (int)(y0 + 0.5 + r * Math.Sin(angle));

                        int result = RNG.Next(0, 250);
                        if (level.GetEntity(x, y) == null && result <= 2 && !level.IsOutOfBounds(x, y))
                        {
                            Bomb bomb = new Bomb(level, 20) { X = x, Y = y };
                            level.Entities.Add(bomb);
                        }
                    }
                }
            }
        }
    }
}
