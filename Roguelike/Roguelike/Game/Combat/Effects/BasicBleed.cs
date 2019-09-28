using System;
using Roguelike.Engine.Game.Entities;
using Roguelike.Engine.Game.Stats;
using Microsoft.Xna.Framework;

namespace Roguelike.Engine.Game.Combat.Effects
{
    public class BasicBleed : Effect
    {
        private static Color bleedColor = Color.Red;

        public BasicBleed(StatsPackage package)
            : base(package, 8)
        {
            this.EffectName = "Bleeding";
            this.EffectDescription = "You are bleeding!";
            this.EffectType = EffectTypes.Physical;

            this.IsHarmful = true;
            this.IsImmuneToPurge = false;
        }

        public override void UpdateStep()
        {
            //Color bleeding entity
            this.parent.ParentEntity.BlendColor(bleedColor);

            //Bleed on the ground
            int result = RNG.Next(0, 100);
            if (result <= 45)
                this.parent.ParentEntity.ParentLevel.StainTile(this.parent.ParentEntity.X, this.parent.ParentEntity.Y, bleedColor);

            //Deal 5% of current health
            int damage = (int)(this.parent.Health * 0.05);
            if (damage <= 0)
                damage = 1;

            this.parent.DealDOTDamage(damage, this);

            base.UpdateStep();
        }
    }
}
