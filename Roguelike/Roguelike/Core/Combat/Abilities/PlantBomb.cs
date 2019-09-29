using System;
using Roguelike.Core.Entities;

namespace Roguelike.Core.Combat.Abilities
{
    public class PlantBomb : Ability
    {
        public PlantBomb()
            : base()
        {
            AbilityName = "Plant Bomb";
            AbilityNameShort = "Plnt Bmb";

            abilityCost = 10;
            TargetingType = TargetingTypes.GroundTarget;
            range = 10;
        }

        public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
        {
            Bomb bomb = new Bomb(target.ParentEntity.ParentLevel, 5) { X = target.ParentEntity.X, Y = target.ParentEntity.Y };
            target.ParentEntity.ParentLevel.Entities.Add(bomb);

            return new CombatResults() { UsedAbility = this, Caster = caster, Target = target };
        }

        public override void CastAbilityGround(Stats.StatsPackage caster, int x, int y, int radius, Level level)
        {
            if (CanCastAbility(caster, x, y))
            {
                ApplyAbilityCost(caster);

                Bomb bomb = new Bomb(level, 5) { X = x, Y = y };
                level.Entities.Add(bomb);
            }
        }
    }
}
