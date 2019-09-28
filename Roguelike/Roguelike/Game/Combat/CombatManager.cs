using System;
using Roguelike.Engine.Game.Combat;
using Roguelike.Engine.Game.Entities;
using Roguelike.Engine.Game.Stats;

namespace Roguelike.Engine.Game
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
