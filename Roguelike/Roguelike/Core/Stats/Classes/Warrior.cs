﻿using System;
using System.Collections.Generic;
using Roguelike.Core.Combat;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Core.Stats.Classes
{
    public class Warrior : Class
    {
        public Warrior()
            : base("Warrior")
        {
            Description = "For as long as war has raged, heroes from every race have aimed to master the art of battle. Warriors combine strength, leadership, and a vast knowledge of arms and armor to wreak havoc in glorious combat. Some protect from the front lines with shields, locking down enemies while allies support the warrior from behind with spell and bow. Others forgo the shield and unleash their rage at the closest threat with a variety of deadly weapons.";
            InheritAbilities = new List<Ability>() { new Ability_ShieldWall(), new Ability_MightyBash() };
            InheritEffects = new List<Effect>() { new Effect_Spirit() };
        }
        public override PlayerStats CalculateStats(PlayerStats stats)
        {
            stats.Strength += 2;
            stats.Agility += 0;
            stats.Dexterity += 0;

            stats.Intelligence += -1;
            stats.Willpower += 0;
            stats.Wisdom += 0;

            stats.Constitution += 2;
            stats.Endurance += -1;
            stats.Fortitude += 0;
            
            return base.CalculateStats(stats);
        }
    }

    public class Ability_ShieldWall : Ability
    {
        public Ability_ShieldWall()
            : base()
        {
            AbilityName = "Shield Wall";
            AbilityNameShort = "Shld Wll";

            abilityType = AbilityTypes.Physical;
            TargetingType = TargetingTypes.Self;
            abilityCost = 10;
        }

        public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
        {
            caster.ApplyEffect(new Effect_ShieldWall());
            return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
        }
    }
    public class Ability_MightyBash : Ability
    {
        public Ability_MightyBash()
            : base()
        {
            AbilityName = "Mighty Bash";
            AbilityNameShort = "Mty Bsh";

            TargetingType = TargetingTypes.EntityTarget;
            AbilityType = AbilityTypes.Physical;
            abilityCost = 15;
            Range = 15;
        }

        public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
        {
            CombatResults results = DoesAttackHit(caster, target);

            if (!results.DidMiss && !results.DidAvoid)
            {
                int damage = (int)(caster.AttackPower.EffectiveValue * 2.5);

                results.PureDamage = damage;
                results.AbsorbedDamage = CalculateAbsorption(damage, target);
                results.AppliedDamage = results.PureDamage - results.AbsorbedDamage;
                results.ReflectedDamage = CalculateReflectedDamage(results.AppliedDamage, target);
            }

            return results;
        }
    }

    public class Effect_ShieldWall : Effect
    {
        public Effect_ShieldWall()
            : base(5)
        {
            EffectName = "Shield Wall";
            IsHarmful = false;

            EffectDescription = "The Warrior's steel resolve is increased for a short time.";
        }

        public override void CalculateStats()
        {
            parent.PhysicalReduction.ModValue += 80;
            parent.SpellReduction.ModValue += 40;

            base.CalculateStats();
        }
    }
    public class Effect_Spirit : Effect
    {
        public Effect_Spirit()
            : base(0)
        {
            EffectName = "Warrior's Spirit";
            IsHarmful = false;
            IsImmuneToPurge = true;

            EffectDescription = "Due to the Warrior's natural prowess in combat, their Attack Power is increased.";
        }

        public override void CalculateStats()
        {
            parent.AttackPower.ModValue += parent.AttackPower.BaseValue * 0.15;

            base.CalculateStats();
        }
    }
}
