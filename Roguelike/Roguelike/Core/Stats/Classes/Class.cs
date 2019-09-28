using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Core.Combat;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Core.Stats.Classes
{
    public class Class : ListItem
    {
        public Class(string name)
        {
            this.className = name;

            this.inheritAbilities = new List<Ability>();
            this.inheritEffects = new List<Effect>();
            this.classTraits = new List<Effect>();
        }

        public virtual PlayerStats CalculateStats(PlayerStats stats)
        {
            stats.AbilityList.Add(basicAttack);
            for (int i = 0; i < this.inheritAbilities.Count; i++)
                stats.AbilityList.Add(this.inheritAbilities[i]);
            for (int i = 0; i < this.inheritEffects.Count; i++)
                stats.ApplyEffect(this.inheritEffects[i]);

            return stats;
        }

        private string className;
        private string classDescription;

        private List<Ability> inheritAbilities;
        private List<Effect> inheritEffects;
        private List<Effect> classTraits;
        private Ability basicAttack = new Combat.Abilities.BasicAttack();

        public string Name { get { return this.className; } set { this.className = value; } }
        public string Description { get { return this.classDescription; } set { this.classDescription = value; } }
        public List<Ability> InheritAbilities { get { return this.inheritAbilities; } set { this.inheritAbilities = value; } }
        public List<Effect> InheritEffects { get { return this.inheritEffects; } set { this.inheritEffects = value; } }
        public List<Effect> ClassTraits { get { return this.classTraits; } set { this.classTraits = value; } }
        public Ability BasicAttack { get { return this.basicAttack; } set { this.basicAttack = value; } }

        public override string ListText { get { return this.className; } set { base.ListText = value; } }
    }
}
