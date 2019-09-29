using System;
using System.Collections.Generic;
using Roguelike.Core.Combat;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Core.Stats.Classes
{
    public class Class : ListItem
    {
        public Class(string name)
        {
            className = name;

            inheritAbilities = new List<Ability>();
            inheritEffects = new List<Effect>();
            classTraits = new List<Effect>();
        }

        public virtual PlayerStats CalculateStats(PlayerStats stats)
        {
            stats.AbilityList.Add(basicAttack);
            for (int i = 0; i < inheritAbilities.Count; i++)
                stats.AbilityList.Add(inheritAbilities[i]);
            for (int i = 0; i < inheritEffects.Count; i++)
                stats.ApplyEffect(inheritEffects[i]);

            return stats;
        }

        private string className;
        private string classDescription;

        private List<Ability> inheritAbilities;
        private List<Effect> inheritEffects;
        private List<Effect> classTraits;
        private Ability basicAttack = new Combat.Abilities.BasicAttack();

        public string Name { get { return className; } set { className = value; } }
        public string Description { get { return classDescription; } set { classDescription = value; } }
        public List<Ability> InheritAbilities { get { return inheritAbilities; } set { inheritAbilities = value; } }
        public List<Effect> InheritEffects { get { return inheritEffects; } set { inheritEffects = value; } }
        public List<Effect> ClassTraits { get { return classTraits; } set { classTraits = value; } }
        public Ability BasicAttack { get { return basicAttack; } set { basicAttack = value; } }

        public override string ListText { get { return className; } set { base.ListText = value; } }
    }
}
