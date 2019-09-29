using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using Roguelike.Core.Combat;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Core.Stats.Races
{
    public class Race : ListItem
    {
        public Race(string name)
        {
            raceName = name;
        }

        public virtual PlayerStats AddRacialStats(PlayerStats package)
        {
            return package;
        }

        private string raceName;
        private string raceDescription;
        private List<Culture> raceCultures;

        public string Name { get { return raceName; } set { raceName = value; } }
        public string Description { get { return raceDescription; } set { raceDescription = value; } }
        public List<Culture> SubCultures { get { return raceCultures; } set { raceCultures = value; } }

        public override string ListText
        {
            get
            {
                return raceName;
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
        private List<Color4> skinColors;

        public Culture(string name)
        {
            cultureName = name;
        }

        public string Name { get { return cultureName; } set { cultureName = value; } }
        public string Description { get { return cultureDescription; } set { cultureDescription = value; } }
        public Effect Trait { get { return cultureTrait; } set { cultureTrait = value; } }
        public List<Color4> SkinColors { get { return skinColors; } set { skinColors = value; } }

        public override string ListText
        {
            get
            {
                return cultureName;
            }
            set
            {
                base.ListText = value;
            }
        }
    }
}
