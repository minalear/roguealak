using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Roguelike.Core.Combat;
using Roguelike.Engine.UI.Controls;
using Roguelike.Core.Entities;

namespace Roguelike.Core.Stats.Classes
{
    public class Summoner : Class
    {
        public Summoner()
            : base("Summoner")
        {
            this.Description = "The summoner is a warlock that does not focus on the offensive spellcasting, but of the summoning type.  The base level Summoner can summon an Impling to fight for him in battle and will sacrifice himself whenever his master takes mortal damage.";
            this.InheritAbilities = new List<Ability>() { new Ability_Summon() };
        }
        public override PlayerStats CalculateStats(PlayerStats stats)
        {
            stats.Strength += 0;
            stats.Agility += 0;
            stats.Dexterity += 0;

            stats.Intelligence += 20;
            stats.Willpower += 0;
            stats.Wisdom += 0;

            stats.Constitution += 20;
            stats.Endurance += 0;
            stats.Fortitude += 0;

            return base.CalculateStats(stats);
        }

        //Ability Passive, Assist, Aggressive
        public class Ability_Summon : Ability
        {
            public Ability_Summon()
                : base()
            {
                this.AbilityName = "Summon Imp";
                this.AbilityNameShort = "Smmn Imp";

                this.abilityType = AbilityTypes.Magical;
                this.TargetingType = TargetingTypes.GroundTarget;
                this.abilityCost = 100;
                this.Range = 10;
            }

            public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
            {
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }

            public override void CastAbilityGround(StatsPackage caster, int x0, int y0, int radius, Level level)
            {
                if (this.CanCastAbility(caster, x0, y0) && !caster.HasEffect(typeof(Effect_SoulLink)))
                {
                    Entity_Impling imp = new Entity_Impling(caster.ParentEntity, level) { X = x0, Y = y0 };
                    level.Entities.Add(imp);

                    caster.ApplyEffect(new Effect_SoulLink(imp, caster));
                }
            }
        }

        public class Effect_SoulLink : Effect
        {
            Entity_Impling impling;

            public Effect_SoulLink(Entity_Impling imp, StatsPackage package)
                : base(package, 0)
            {
                this.EffectName = "Soul Link - Impling";
                this.EffectDescription = "You have an impling serving by your side.  He will sacrifice himself to save his master.";

                this.EffectType = EffectTypes.Magical;
                this.IsImmuneToPurge = true;
                this.IsHarmful = false;

                this.impling = imp;
            }

            public override void OnDeath()
            {
                this.parent.Health = (int)(this.parent.MaxHealth.EffectiveValue * 0.5);
                this.parent.ParentEntity.DoPurge = false;

                impling.DoPurge = true;
                impling.OnDeath();
                this.parent.RemoveEffect(this.GetType());

                for (int i = 0; i < parent.AppliedEffects.Count; i++)
                {
                    if (parent.AppliedEffects[i].IsHarmful)
                        parent.PurgeEffect(parent.AppliedEffects[i]);
                }
            }

            public override void OnMove()
            {
                //If the impling is not on the same level as the player
                if (impling.ParentLevel != this.parent.ParentEntity.ParentLevel)
                {
                    this.impling.DoPurge = true;

                    this.impling = new Entity_Impling(this.parent.ParentEntity, this.parent.ParentEntity.ParentLevel);
                    this.impling.X = this.Parent.ParentEntity.X + 1;
                    this.impling.Y = this.Parent.ParentEntity.Y;

                    this.parent.ParentEntity.ParentLevel.Entities.Add(this.impling);
                }

                base.OnMove();
            }
        }
        public class Entity_Impling : Entity
        {
            private Entity owner;
            private Point ownerPosition;

            private int sightRange = 6;
            private int attackRange = 12;

            private List<Point> path = new List<Point>();
            private bool hasTarget = false;
            private Entity target;

            public Entity_Impling(Entity owner, Level level)
                : base(level)
            {
                this.owner = owner;
                ownerPosition = new Point(owner.X, owner.Y);

                this.token = 'I';
                this.ForegroundColor = Color.Green;
                this.IsSolid = false;

                this.EntityType = EntityTypes.NPC;

                this.statsPackage = new Stats.StatsPackage(this)
                {
                    UnitName = "Impling",

                    AttackPower = 25,
                    Health = 25,
                    MaxHealth = 25,
                    PhysicalHitChance = 100
                };

                this.statsPackage.AbilityList[0] = new Combat.Abilities.BasicAttack() { Range = 100 };
            }

            public override void UpdateStep()
            {
                //Aggressive Branch
                //Scan for Enemies => Engage|MoveToPlayer

                if (this.hasTarget)
                {
                    if (this.target.StatsPackage.IsDead())
                        this.hasTarget = false;
                    else
                    {
                        this.target.Attack(this, this.statsPackage.AbilityList[0]);
                    }
                }
                else
                {
                    this.findEnemy();
                    moveTowardsPlayer();
                }

                ownerPosition.X = owner.X;
                ownerPosition.Y = owner.Y;

                base.UpdateStep();
            }
            public override void OnDeath()
            {
                MessageCenter.PostMessage(
                    "Impling sacrificed himself to save his master!",
                    string.Format("{0} sacrificed himself to save his master, {1}.", this.statsPackage.UnitName, this.owner.StatsPackage.UnitName),
                    this);
                this.owner.StatsPackage.RemoveEffect(typeof(Effect_SoulLink));

                base.OnDeath();
            }

            private void findEnemy()
            {
                List<Entity> potentialTargets = this.parentLevel.GetEntities(new Circle() { Radius = this.sightRange, X = this.X, Y = this.Y });

                for (int i = 0; i < potentialTargets.Count; i++)
                {
                    if (potentialTargets[i].EntityType == EntityTypes.Enemy && this.parentLevel.IsLineOfSight(new Point(this.X, this.Y), new Point(potentialTargets[i].X, potentialTargets[i].Y)))
                    {
                        this.target = potentialTargets[i];
                        this.hasTarget = true;

                        break;
                    }
                }
            }
            private void moveTowardsPlayer()
            {
                this.path = Pathing.PathCalculator.CalculatePath(new Point(this.X, this.Y), ownerPosition, this.parentLevel);

                if (this.path.Count > 1)
                    this.MoveToTile(this.path[1].X, this.path[1].Y);
            }
            private void moveTowardsTarget()
            {
                if (target != null)
                {
                    Point targetPosition = new Point(target.X, target.Y);
                    this.path = Pathing.PathCalculator.CalculatePath(new Point(this.X, this.Y), targetPosition, this.parentLevel);

                    if (this.path.Count > 1)
                        this.MoveToTile(this.path[1].X, this.path[1].Y);
                }
            }

            public enum AggressionTypes { Aggressive, Passive, Assist }
        }
    }
}
