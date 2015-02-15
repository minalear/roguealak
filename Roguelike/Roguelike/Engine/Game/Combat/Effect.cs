using System;
using Roguelike.Engine.Game.Entities;
using Roguelike.Engine.Game.Stats;
using Microsoft.Xna.Framework;

namespace Roguelike.Engine.Game.Combat
{
    public class Effect : UI.Controls.ListItem
    {
        protected StatsPackage parent;

        protected int duration = 0;
        protected bool hasDuration = false;
        protected bool doPurge = false;

        public string EffectName { get; set; }
        public string EffectDescription { get; set; }

        public Effect(int duration)
        {
            this.duration = duration;

            if (duration > 0)
                this.hasDuration = true;
            this.EffectType = EffectTypes.Hybrid;

            this.TextColor = Color.White;
        }
        public Effect(StatsPackage package, int duration)
        {
            this.parent = package;
            this.duration = duration;

            if (duration > 0)
                this.hasDuration = true;
            this.EffectType = EffectTypes.Hybrid;

            this.TextColor = Color.White;
        }

        public virtual void UpdateStep()
        {
            if (this.hasDuration)
            {
                if (this.duration <= 0)
                    this.OnElapsed();
                this.duration--;
            }
        }

        public virtual void OnAttack(CombatResults results) { } //Whenever the holder attacks
        public virtual void OnDefend(CombatResults results) { } //Whenever the holder is attacked

        public virtual int OnHealthLoss(int amount) { return amount; } //Whenever the holder loses health
        public virtual int OnHealthGain(int amount) { return amount; } //Whenever the holder gaines health
        public virtual int OnManaLoss(int amount) { return amount; } //Whenever the holder loses mana
        public virtual int OnManaGain(int amount) { return amount; } //Whenever the holder loses mana

        public virtual void OnMove() { } //Whenever the entity moves
        public virtual void OnDeath() { } //Whenever the entity dies
        public virtual void OnSpawn() { } //Called whenever the entity is spawned (by external force)

        public virtual void OnApplication() { } //Whenever the effect is applied
        public virtual void OnElapsed() //Removed due to lapsed duration
        {
            this.doPurge = true;
        }
        public virtual void OnRemoval() //Removed with duration still remaining
        {
            if (!this.IsImmuneToPurge)
                this.doPurge = true;
        }

        public virtual void CalculateStats() { }
        public override string ToString()
        {
            return this.EffectName;
        }

        public int Duration { get { return this.duration; } set { this.duration = value; } }
        public bool DoPurge { get { return this.doPurge; } set { this.doPurge = value; } }
        public EffectTypes EffectType { get; set; }
        public bool IsImmuneToPurge { get; set; }
        public bool IsHarmful { get; set; }
        public override string ListText { get { return this.EffectName; } set { base.ListText = value; } }
        public StatsPackage Parent { get { return this.parent; } set { this.parent = value; } }
    }

    public enum EffectTypes { Magical, Physical, Hybrid, Trait }
}
