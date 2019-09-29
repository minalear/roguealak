using System;
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
            Name = "Scroll of " + ability.AbilityName;
            Description = "This is a scroll that lets you cast the ability " + ability.AbilityName;

            ability = ability;
            RemoveOnUse = true;
        }

        public Ability ScrollAbility { get { return ability; } set { ability = value; } }
    }
}
