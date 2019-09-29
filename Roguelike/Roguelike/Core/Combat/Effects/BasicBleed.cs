using System;
using OpenTK.Graphics;
using Roguelike.Core.Entities;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Combat.Effects
{
    public class BasicBleed : Effect
    {
        private static Color4 bleedColor = Color4.Red;

        public BasicBleed(StatsPackage package)
            : base(package, 8)
        {
            EffectName = "Bleeding";
            EffectDescription = "You are bleeding!";
            EffectType = EffectTypes.Physical;

            IsHarmful = true;
            IsImmuneToPurge = false;
        }

        public override void UpdateStep()
        {
            //Color bleeding entity
            parent.ParentEntity.BlendColor(bleedColor);

            //Bleed on the ground
            int result = Engine.RNG.Next(0, 100);
            if (result <= 45)
                parent.ParentEntity.ParentLevel.StainTile(parent.ParentEntity.X, parent.ParentEntity.Y, bleedColor);

            //Deal 5% of current health
            int damage = (int)(parent.Health * 0.05);
            if (damage <= 0)
                damage = 1;

            parent.DealDOTDamage(damage, this);

            base.UpdateStep();
        }
    }
}
