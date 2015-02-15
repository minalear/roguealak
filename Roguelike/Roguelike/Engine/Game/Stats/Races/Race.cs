using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Engine.Game.Combat;
using Roguelike.Engine.UI.Controls;
using Microsoft.Xna.Framework;

namespace Roguelike.Engine.Game.Stats.Races
{
    public class Race : ListItem
    {
        public Race(string name)
        {
            this.raceName = name;
        }

        public virtual PlayerStats AddRacialStats(PlayerStats package)
        {
            return package;
        }

        private string raceName;
        private string raceDescription;
        private List<Culture> raceCultures;

        public string Name { get { return this.raceName; } set { this.raceName = value; } }
        public string Description { get { return this.raceDescription; } set { this.raceDescription = value; } }
        public List<Culture> SubCultures { get { return this.raceCultures; } set { this.raceCultures = value; } }

        public override string ListText
        {
            get
            {
                return this.raceName;
            }
            set
            {
                base.ListText = value;
            }
        }
    }

    public class Culture : ListItem
    {
        private string cultureName;
        private string cultureDescription;
        private Effect cultureTrait;
        private List<Color> skinColors;

        public Culture(string name)
        {
            this.cultureName = name;
        }

        public string Name { get { return this.cultureName; } set { this.cultureName = value; } }
        public string Description { get { return this.cultureDescription; } set { this.cultureDescription = value; } }
        public Effect Trait { get { return this.cultureTrait; } set { this.cultureTrait = value; } }
        public List<Color> SkinColors { get { return this.skinColors; } set { this.skinColors = value; } }

        public override string ListText
        {
            get
            {
                return this.cultureName;
            }
            set
            {
                base.ListText = value;
            }
        }
    }
}
