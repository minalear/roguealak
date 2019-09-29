using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using Roguelike.Core.Combat;
using Roguelike.Engine;
using Roguelike.Core.Entities;

namespace Roguelike.Core.Stats.Classes
{
    public class Summoner : Class
    {
        public Summoner()
            : base("Summoner")
        {
            Description = "The summoner is a warlock that does not focus on the offensive spellcasting, but of the summoning type.  The base level Summoner can summon an Impling to fight for him in battle and will sacrifice himself whenever his master takes mortal damage.";
            InheritAbilities = new List<Ability>() { new Ability_Summon() };
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
                AbilityName = "Summon Imp";
                AbilityNameShort = "Smmn Imp";

                abilityType = AbilityTypes.Magical;
                TargetingType = TargetingTypes.GroundTarget;
                abilityCost = 100;
                Range = 10;
            }

            public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
            {
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }

            public override void CastAbilityGround(StatsPackage caster, int x0, int y0, int radius, Level level)
            {
                if (CanCastAbility(caster, x0, y0) && !caster.HasEffect(typeof(Effect_SoulLink)))
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
                EffectName = "Soul Link - Impling";
                EffectDescription = "You have an impling serving by your side.  He will sacrifice himself to save his master.";

                EffectType = EffectTypes.Magical;
                IsImmuneToPurge = true;
                IsHarmful = false;

                impling = imp;
            }

            public override void OnDeath()
            {
                parent.Health = (int)(parent.MaxHealth.EffectiveValue * 0.5);
                parent.ParentEntity.DoPurge = false;

                impling.DoPurge = true;
                impling.OnDeath();
                parent.RemoveEffect(GetType());

                for (int i = 0; i < parent.AppliedEffects.Count; i++)
                {
                    if (parent.AppliedEffects[i].IsHarmful)
                        parent.PurgeEffect(parent.AppliedEffects[i]);
                }
            }

            public override void OnMove()
            {
                //If the impling is not on the same level as the player
                if (impling.ParentLevel != parent.ParentEntity.ParentLevel)
                {
                    impling.DoPurge = true;

                    impling = new Entity_Impling(parent.ParentEntity, parent.ParentEntity.ParentLevel);
                    impling.X = Parent.ParentEntity.X + 1;
                    impling.Y = Parent.ParentEntity.Y;

                    parent.ParentEntity.ParentLevel.Entities.Add(impling);
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

                token = 'I';
                ForegroundColor = Color4.Green;
                IsSolid = false;

                EntityType = EntityTypes.NPC;

                statsPackage = new Stats.StatsPackage(this)
                {
                    UnitName = "Impling",

                    AttackPower = 25,
                    Health = 25,
                    MaxHealth = 25,
                    PhysicalHitChance = 100
                };

                statsPackage.AbilityList[0] = new Combat.Abilities.BasicAttack() { Range = 100 };
            }

            public override void UpdateStep()
            {
                //Aggressive Branch
                //Scan for Enemies => Engage|MoveToPlayer

                if (hasTarget)
                {
                    if (target.StatsPackage.IsDead())
                        hasTarget = false;
                    else
                    {
                        target.Attack(this, statsPackage.AbilityList[0]);
                    }
                }
                else
                {
                    findEnemy();
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
                    string.Format("{0} sacrificed himself to save his master, {1}.", statsPackage.UnitName, owner.StatsPackage.UnitName),
                    this);
                owner.StatsPackage.RemoveEffect(typeof(Effect_SoulLink));

                base.OnDeath();
            }

            private void findEnemy()
            {
                List<Entity> potentialTargets = parentLevel.GetEntities(new Circle() { Radius = sightRange, X = X, Y = Y });

                for (int i = 0; i < potentialTargets.Count; i++)
                {
                    if (potentialTargets[i].EntityType == EntityTypes.Enemy && parentLevel.IsLineOfSight(new Point(X, Y), new Point(potentialTargets[i].X, potentialTargets[i].Y)))
                    {
                        target = potentialTargets[i];
                        hasTarget = true;

                        break;
                    }
                }
            }
            private void moveTowardsPlayer()
            {
                //path = Pathing.PathCalculator.CalculatePath(new Point(X, Y), ownerPosition, parentLevel);

                if (path.Count > 1)
                    MoveToTile(path[1].X, path[1].Y);
            }
            private void moveTowardsTarget()
            {
                if (target != null)
                {
                    Point targetPosition = new Point(target.X, target.Y);
                    //path = Pathing.PathCalculator.CalculatePath(new Point(X, Y), targetPosition, parentLevel);

                    if (path.Count > 1)
                        MoveToTile(path[1].X, path[1].Y);
                }
            }

            public enum AggressionTypes { Aggressive, Passive, Assist }
        }
    }
}
