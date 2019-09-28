using System;
using Roguelike.Core.Entities;

namespace Roguelike.Core.Combat.Abilities
{
    public class PlantBomb : Ability
    {
        public PlantBomb()
            : base()
        {
            this.AbilityName = "Plant Bomb";
            this.AbilityNameShort = "Plnt Bmb";

            this.abilityCost = 10;
            this.TargetingType = TargetingTypes.GroundTarget;
            this.range = 10;
        }

        public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
        {
            Bomb bomb = new Bomb(target.ParentEntity.ParentLevel, 5) { X = target.ParentEntity.X, Y = target.ParentEntity.Y };
            target.ParentEntity.ParentLevel.Entities.Add(bomb);

            return new CombatResults() { UsedAbility = this, Caster = caster, Target = target };
        }

        public override void CastAbilityGround(Stats.StatsPackage caster, int x, int y, int radius, Level level)
        {
            if (this.CanCastAbility(caster, x, y))
            {
                this.ApplyAbilityCost(caster);

                Bomb bomb = new Bomb(level, 5) { X = x, Y = y };
                level.Entities.Add(bomb);
            }
        }
    }
}
