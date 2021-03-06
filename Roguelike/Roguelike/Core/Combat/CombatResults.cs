﻿using System;
using Roguelike.Core.Entities;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Combat
{
    public class CombatResults
    {
        public void ProcessResults()
        {
            Target.DrainHealth(AppliedDamage);
            Caster.DrainHealth(ReflectedDamage);

            HPLeech = UsedAbility.CalculateHealthLeech(AppliedDamage, Caster);
            MPLeech = UsedAbility.CalculateManaLeech(AppliedDamage, Caster);

            outputCombatLogInfo();
        }

        private void outputCombatLogInfo()
        {
            MessageCenter.Message message = new MessageCenter.Message("", Caster.ParentEntity);
            
            if (UsedAbility.AbilityType == AbilityTypes.Physical || UsedAbility.AbilityType == AbilityTypes.Magical)
            {
                if (DidMiss)
                {
                    message.ShortMessage = String.Format("{0} missed their attack!", Caster.UnitName);
                    message.DetailedMessage = String.Format("{0} missed their attack against {1}.", Caster.UnitName, Target.UnitName);
                    message.DetailedMessage += "  Used the ability " + UsedAbility.AbilityName + ".";
                }
                else if (DidAvoid)
                {
                    message.ShortMessage = String.Format("{0} avoided the attack!", Caster.UnitName);
                    message.DetailedMessage = String.Format("{0} avoided {1}'s attack.", Caster.UnitName, Target.UnitName);
                    message.DetailedMessage += "  Used the ability " + UsedAbility.AbilityName + ".";
                }
                else if (DidCrit)
                {
                    message.ShortMessage = String.Format("{0} scored a critical strike! ({1} damage)", Caster.UnitName, AppliedDamage);
                    message.DetailedMessage = String.Format("{0} dealt critical damage against {1} worth {2}.  {3} damage absorbed and {4} damage reflected.", Caster.UnitName, Target.UnitName, AppliedDamage, AbsorbedDamage, ReflectedDamage);
                    message.DetailedMessage += "  Used the ability " + UsedAbility.AbilityName + ".";
                }
                else
                {
                    message.ShortMessage = String.Format("{0} scored a strike! ({1} damage)", Caster.UnitName, AppliedDamage);
                    message.DetailedMessage = String.Format("{0} attacked {1} for {2} damage.  {3} damage absorbed and {4} damage reflected.", Caster.UnitName, Target.UnitName, AppliedDamage, AbsorbedDamage, ReflectedDamage);
                    message.DetailedMessage += "  Used the ability " + UsedAbility.AbilityName + ".";
                }

                if (HPLeech > 0)
                {
                    message.DetailedMessage += string.Format("  {0} HP Leeched.", HPLeech);
                    Caster.AddHealth(HPLeech);
                }
                if (MPLeech > 0)
                {
                    message.DetailedMessage += string.Format("  {0} MP Leeched.", MPLeech);
                    Caster.AddMana(MPLeech);
                }
            }

            MessageCenter.PostMessage(message);
        }

        public int PureDamage, AppliedDamage, ReflectedDamage, AbsorbedDamage, HPLeech, MPLeech;
        public StatsPackage Caster, Target;
        public bool DidCrit = false, DidMiss = false, DidAvoid = false, CanAfford = true;

        public Ability UsedAbility { get; set; }
    }
}
