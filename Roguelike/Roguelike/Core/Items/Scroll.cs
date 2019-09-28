using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Core.Combat;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Items
{
    public class Scroll : Item
    {
        private Ability ability;
        public Scroll(Ability ability)
            : base(ItemTypes.Scroll)
        {
            this.Name = "Scroll of " + ability.AbilityName;
            this.Description = "This is a scroll that lets you cast the ability " + ability.AbilityName;

            this.ability = ability;
            this.RemoveOnUse = true;
        }

        public Ability ScrollAbility { get { return this.ability; } set { this.ability = value; } }
    }
}
