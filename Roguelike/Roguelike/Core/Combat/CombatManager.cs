using System;
using Roguelike.Core.Combat;
using Roguelike.Core.Entities;
using Roguelike.Core.Stats;

namespace Roguelike.Core
{
    public static class CombatManager
    {
        public static void Initialize()
        {

        }
        
        public static void PerformAbility(StatsPackage caster, StatsPackage target, Ability ability)
        {
            CombatResults combatResults = ability.CastAbilityTarget(caster, target);
            caster.OnAttack(combatResults);
            target.OnDefend(combatResults);

            combatResults.ProcessResults();
        }
    }
}
