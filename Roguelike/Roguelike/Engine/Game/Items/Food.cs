using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Engine.Game.Combat;
using Roguelike.Engine.Game.Stats;

namespace Roguelike.Engine.Game.Items
{
    public class Food : Item
    {
        public Food()
            : base(ItemTypes.Food) { }

        public override void OnUse(Entities.Entity entity)
        {
            if (this.onUseEffect != null)
                entity.StatsPackage.ApplyEffect(this.onUseEffect);
        }

        private Effect onUseEffect;
        public Effect OnUseEffect { get { return this.onUseEffect; } set { this.onUseEffect = value; } }
    }
}
