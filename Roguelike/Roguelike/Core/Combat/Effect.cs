using System;
using OpenTK.Graphics;
using Roguelike.Core.Entities;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Combat
{
    public class Effect : Engine.UI.Controls.ListItem
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
                hasDuration = true;
            EffectType = EffectTypes.Hybrid;

            TextColor = Color4.White;
        }
        public Effect(StatsPackage package, int duration)
        {
            parent = package;
            this.duration = duration;

            if (duration > 0)
                hasDuration = true;
            EffectType = EffectTypes.Hybrid;

            TextColor = Color4.White;
        }

        public virtual void UpdateStep()
        {
            if (hasDuration)
            {
                if (duration <= 0)
                    OnElapsed();
                duration--;
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

        public virtual void OnApplication(Entity entity) { } //Whenever the effect is applied
        public virtual void OnElapsed() //Removed due to lapsed duration
        {
            doPurge = true;
        }
        public virtual void OnRemoval() //Removed with duration still remaining
        {
            if (!IsImmuneToPurge)
                doPurge = true;
        }

        public virtual void CalculateStats() { }
        public override string ToString()
        {
            return EffectName;
        }

        public int Duration { get { return duration; } set { duration = value; } }
        public bool DoPurge { get { return doPurge; } set { doPurge = value; } }
        public EffectTypes EffectType { get; set; }
        public bool IsImmuneToPurge { get; set; }
        public bool IsHarmful { get; set; }
        public override string ListText { get { return EffectName; } set { base.ListText = value; } }
        public StatsPackage Parent { get { return parent; } set { parent = value; } }
    }

    public enum EffectTypes { Magical, Physical, Hybrid, Trait }
}
