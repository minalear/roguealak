using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using Roguelike.Engine.UI.Controls;
using Roguelike.Core;
using Roguelike.Core.Stats;
using Roguelike.Core.Combat;
using Roguelike.Core.Stats.Races;
using Roguelike.Core.Stats.Classes;

namespace Roguelike.Engine.UI.Interfaces
{
    public class CharacterCreation : Interface
    {
        public CharacterCreation()
        {
            mainTitle = new Title(this, "Character Creation", GraphicConsole.BufferWidth / 2, 1, Title.TextAlignModes.Center);
            backButton = new Button(this, "X", GraphicConsole.BufferWidth - 1, 0, 1, 1) { KeyShortcut = Keys.Escape };
            backButton.Click += backButton_Click;          

            itemLists = new ScrollingList(this, 35, 4, 25, 34);
            itemLists.Selected += itemLists_Selected;
            informationBox = new TextBox(this, 88, 4, 35, 34);
            detailedStatBox = new TextBox(this, 63, 4, 22, 34);

            nextButton = new Button(this, "Next", GraphicConsole.BufferWidth - 6, GraphicConsole.BufferHeight - 4, 6, 3);
            nextButton.Click += nextButton_Click;
            resetButton = new Button(this, "Reset", nextButton.Position.X - 8, GraphicConsole.BufferHeight - 4, 7, 3);
            resetButton.Click += resetButton_Click;

            #region General Box
            nameTitle = new Title(this, "Name: ", 3, 4, Title.TextAlignModes.Left);
            raceTitle = new Title(this, "Race: ", 3, 5, Title.TextAlignModes.Left);
            cultureTitle = new Title(this, "Culture: ", 3, 6, Title.TextAlignModes.Left);
            classTitle = new Title(this, "Class: ", 3, 7, Title.TextAlignModes.Left);
            traitTitle = new Title(this, "Trait: ", 3, 8, Title.TextAlignModes.Left);

            nameInput = new InputBox(this, 13, 4, 19, 1) { CharacterLimit = 18 };
            raceSelect = new Button(this, "[Choose Race]", 13, 5, 19, 1);
            cultureSelect = new Button(this, "[Choose Culture]", 13, 6, 19, 1);
            classSelect = new Button(this, "[Choose Class]", 13, 7, 19, 1);
            traitSelect = new Button(this, "[Choose Trait]", 13, 8, 19, 1);

            randomName = new Button(this, "φ", 11, 4, 1, 1);
            randomName.Click += randomName_Click;

            raceSelect.Click += raceSelect_Click;
            cultureSelect.Click += cultureSelect_Click;
            classSelect.Click += classSelect_Click;
            traitSelect.Click += traitSelect_Click;
            #endregion
            #region Stats Box
            physicalTitle = new Title(this, "Physical Stats", 16, 13, Title.TextAlignModes.Center) { TextColor = Color.LightGray };
            spellTitle = new Title(this, "Spell Stats", 16, 21, Title.TextAlignModes.Center) { TextColor = Color.LightGray };
            defenseTitle = new Title(this, "Defensive Stats", 16, 29, Title.TextAlignModes.Center) { TextColor = Color.LightGray };
            freePointsTitle = new Title(this, "Free Points: ##", 3, 37, Title.TextAlignModes.Left);
            statsResetButton = new Button(this, "Reset Points", 20, 37, 12, 1);
            statsResetButton.Click += statsResetButton_Click;

            strButton = new Button(this, "-STR-", 3, 14, 5, 1); strButton.Click += strButton_Click;
            agiButton = new Button(this, "-AGI-", 3, 16, 5, 1); agiButton.Click += agiButton_Click;
            dexButton = new Button(this, "-DEX-", 3, 18, 5, 1); dexButton.Click += dexButton_Click;

            intButton = new Button(this, "-INT-", 3, 22, 5, 1); intButton.Click += intButton_Click;
            wilButton = new Button(this, "-WIL-", 3, 24, 5, 1); wilButton.Click += wilButton_Click;
            wisButton = new Button(this, "-WIS-", 3, 26, 5, 1); wisButton.Click += wisButton_Click;

            conButton = new Button(this, "-CON-", 3, 30, 5, 1); conButton.Click += conButton_Click;
            endButton = new Button(this, "-END-", 3, 32, 5, 1); endButton.Click += endButton_Click;
            frtButton = new Button(this, "-FRT-", 3, 34, 5, 1); frtButton.Click += frtButton_Click;

            strAdd = new Button(this, "+", 25, 14, 3, 1); strAdd.Click += strAdd_Click;
            strRem = new Button(this, "-", 28, 14, 3, 1); strRem.Click += strRem_Click;

            agiAdd = new Button(this, "+", 25, 16, 3, 1); agiAdd.Click += agiAdd_Click;
            agiRem = new Button(this, "-", 28, 16, 3, 1); agiRem.Click += agiRem_Click;

            dexAdd = new Button(this, "+", 25, 18, 3, 1); dexAdd.Click += dexAdd_Click;
            dexRem = new Button(this, "-", 28, 18, 3, 1); dexRem.Click += dexRem_Click;


            intAdd = new Button(this, "+", 25, 22, 3, 1); intAdd.Click += intAdd_Click;
            intRem = new Button(this, "-", 28, 22, 3, 1); intRem.Click += intRem_Click;

            wilAdd = new Button(this, "+", 25, 24, 3, 1); wilAdd.Click += wilAdd_Click;
            wilRem = new Button(this, "-", 28, 24, 3, 1); wilRem.Click += wilRem_Click;

            wisAdd = new Button(this, "+", 25, 26, 3, 1); wisAdd.Click += wisAdd_Click;
            wisRem = new Button(this, "-", 28, 26, 3, 1); wisRem.Click += wisRem_Click;


            conAdd = new Button(this, "+", 25, 30, 3, 1); conAdd.Click += conAdd_Click;
            conRem = new Button(this, "-", 28, 30, 3, 1); conRem.Click += conRem_Click;

            endAdd = new Button(this, "+", 25, 32, 3, 1); endAdd.Click += endAdd_Click;
            endRem = new Button(this, "-", 28, 32, 3, 1); endRem.Click += endRem_Click;

            frtAdd = new Button(this, "+", 25, 34, 3, 1); frtAdd.Click += frtAdd_Click;
            frtRem = new Button(this, "-", 28, 34, 3, 1); frtRem.Click += frtRem_Click;
            #endregion

            popupControl = new Popup(this);

            character = new TokenTile() { Token = '@', ForegroundColor = Color4.Black, BackgroundColor = Color4.Black };
            colorLeftButton = new Button(this, "«", 8, 40, 2, 3);
            colorRightButton = new Button(this, "» ", 15, 40, 2, 3);

            colorLeftButton.Click += colorLeftButton_Click;
            colorRightButton.Click += colorRightButton_Click;

            calculateStats();
        }

        public override void DrawStep()
        {
            drawUIBorders();
            writeStatInfo();

            base.DrawStep();
        }
        public override void UpdateStep()
        {
            freePointsTitle.Text = "Free Points: " + availablePoints;
            calculateStats();

            base.UpdateStep();
        }

        private void drawUIBorders()
        {
            //General Box
            drawBox(1, 3, 12, 9);
            drawBox(12, 3, 32, 9);
            GraphicConsole.Put('╦', 12, 3);
            GraphicConsole.Put('╩', 12, 9);

            //Stats Box
            drawBox(1, 11, 32, 36);
            drawBox(1, 36, 32, 38);
            GraphicConsole.Put('╠',  1, 36);
            GraphicConsole.Put('╣', 32, 36);

            //List Box
            drawBox(34, 3, 60, 38);

            //Detailed Stats Box
            drawBox(62, 3, 85, 38);

            //Information Panel
            drawBox(87, 3, 123, 38);

            //Extra Panel
            drawBox(1, 39, 7, 43);
            drawBox(7, 39, 17, 43);

            GraphicConsole.SetCursor(10, 40);
            GraphicConsole.Write("Color");

            GraphicConsole.Put('╦', 7, 39);
            GraphicConsole.Put('╩', 7, 43);

            GraphicConsole.SetColors(character.ForegroundColor, character.BackgroundColor);
            GraphicConsole.Put(character.Token, 4, 41);
            GraphicConsole.ResetColor();
        }
        private void writeStatInfo()
        {
            #region Melee Physical Damage Stats
            //Strength
            GraphicConsole.SetCursor(10, 14); GraphicConsole.Write(strRaceMod.ToString());
            GraphicConsole.SetCursor(14, 14); GraphicConsole.Write(strClassMod.ToString());
            GraphicConsole.SetCursor(18, 14); GraphicConsole.Write(strFreeMod.ToString());
            GraphicConsole.SetCursor(22, 14); GraphicConsole.Write((strRaceMod + strClassMod + strFreeMod).ToString());

            //Agility
            GraphicConsole.SetCursor(10, 16); GraphicConsole.Write(agiRaceMod.ToString());
            GraphicConsole.SetCursor(14, 16); GraphicConsole.Write(agiClassMod.ToString());
            GraphicConsole.SetCursor(18, 16); GraphicConsole.Write(agiFreeMod.ToString());
            GraphicConsole.SetCursor(22, 16); GraphicConsole.Write((agiRaceMod + agiClassMod + agiFreeMod).ToString());

            //Dexterity
            GraphicConsole.SetCursor(10, 18); GraphicConsole.Write(dexRaceMod.ToString());
            GraphicConsole.SetCursor(14, 18); GraphicConsole.Write(dexClassMod.ToString());
            GraphicConsole.SetCursor(18, 18); GraphicConsole.Write(dexFreeMod.ToString());
            GraphicConsole.SetCursor(22, 18); GraphicConsole.Write((dexRaceMod + dexClassMod + dexFreeMod).ToString());
            #endregion
            #region Spell Physical Damage Stats
            //Intelligence
            GraphicConsole.SetCursor(10, 22); GraphicConsole.Write(intRaceMod.ToString());
            GraphicConsole.SetCursor(14, 22); GraphicConsole.Write(intClassMod.ToString());
            GraphicConsole.SetCursor(18, 22); GraphicConsole.Write(intFreeMod.ToString());
            GraphicConsole.SetCursor(22, 22); GraphicConsole.Write((intRaceMod + intClassMod + intFreeMod).ToString());

            //Willpower
            GraphicConsole.SetCursor(10, 24); GraphicConsole.Write(wilRaceMod.ToString());
            GraphicConsole.SetCursor(14, 24); GraphicConsole.Write(wilClassMod.ToString());
            GraphicConsole.SetCursor(18, 24); GraphicConsole.Write(wilFreeMod.ToString());
            GraphicConsole.SetCursor(22, 24); GraphicConsole.Write((wilRaceMod + wilClassMod + wilFreeMod).ToString());

            //Wisdom
            GraphicConsole.SetCursor(10, 26); GraphicConsole.Write(wisRaceMod.ToString());
            GraphicConsole.SetCursor(14, 26); GraphicConsole.Write(wisClassMod.ToString());
            GraphicConsole.SetCursor(18, 26); GraphicConsole.Write(wisFreeMod.ToString());
            GraphicConsole.SetCursor(22, 26); GraphicConsole.Write((wisRaceMod + wisClassMod + wisFreeMod).ToString());
            #endregion
            #region Defense Stats
            //Constitution
            GraphicConsole.SetCursor(10, 30); GraphicConsole.Write(conRaceMod.ToString());
            GraphicConsole.SetCursor(14, 30); GraphicConsole.Write(conClassMod.ToString());
            GraphicConsole.SetCursor(18, 30); GraphicConsole.Write(conFreeMod.ToString());
            GraphicConsole.SetCursor(22, 30); GraphicConsole.Write((conRaceMod + conClassMod + conFreeMod).ToString());

            //Endurance
            GraphicConsole.SetCursor(10, 32); GraphicConsole.Write(endRaceMod.ToString());
            GraphicConsole.SetCursor(14, 32); GraphicConsole.Write(endClassMod.ToString());
            GraphicConsole.SetCursor(18, 32); GraphicConsole.Write(endFreeMod.ToString());
            GraphicConsole.SetCursor(22, 32); GraphicConsole.Write((endRaceMod + endClassMod + endFreeMod).ToString());

            //Fortitude
            GraphicConsole.SetCursor(10, 34); GraphicConsole.Write(frtRaceMod.ToString());
            GraphicConsole.SetCursor(14, 34); GraphicConsole.Write(frtClassMod.ToString());
            GraphicConsole.SetCursor(18, 34); GraphicConsole.Write(frtFreeMod.ToString());
            GraphicConsole.SetCursor(22, 34); GraphicConsole.Write((frtRaceMod + frtClassMod + frtFreeMod).ToString());
            #endregion
        }
        private void setStatInformation(string stat)
        {
            if (stat == "STR")
                informationBox.Text = "Strength<br><br><tb>Strength empowers your physical attacks and is key for warriors and knights alike.  Allows you to wear bigger and better armor and hold larger, more awe-inspiring swords.";
            else if (stat == "AGI")
                informationBox.Text = "Agility<br><br><tb>Agility grants you the ability to wield even more powerful ranged weapons and a few agile melee weapons.";
            else if (stat == "DEX")
                informationBox.Text = "Dexterity<br><br><tb>Dexterity allows you to move around the battlefield like a trained ninja assassin thing.  Your enemies will be less likely to hit you and you will be more likely to hit their weak spots.";
            else if (stat == "INT")
                informationBox.Text = "Intelligence<br><br><tb>Those adventurers of the mental variation will prefer Intelligence over the more brawn style stats, as it provides power to boost your spells and to learn even more powerful incantations.";
            else if (stat == "WIL")
                informationBox.Text = "Willpower<br><br><tb>Wizards, Incantors, Cabal Leaders and other cloth wearing fiends depend on their willpower to defeat their enemies.  Willpower provides your spells the ability to hit opponents as well as protect you mentally.";
            else if (stat == "WIS")
                informationBox.Text = "Wisdom<br><br><tb>Wisdom slightly empowers your spells and grants you higher mana regeneration.  Wise spell wielders typically last longer than the more rash varient.";
            else if (stat == "CON")
                informationBox.Text = "Constitution<br><br><tb>Constitution allows you to survive longer and take more devastating blows from your enemies.";
            else if (stat == "END")
                informationBox.Text = "Endurance<br><br><tb>Endurance, much like Constitution, is a defensive stat, but slightly different.  :D";
            else if (stat == "FRT")
                informationBox.Text = "Fortitude<br><br><tb>To survive on the battlefield, adventurers must steel themselves against mental attacks if they wish to get far.";
        }
        private void drawBox(int x0, int y0, int x1, int y1)
        {
            DrawingUtilities.DrawLine(x0, y0, x1, y0, '═');
            DrawingUtilities.DrawLine(x0, y1, x1, y1, '═');
            DrawingUtilities.DrawLine(x0, y0 + 1, x0, y1 - 1, '║');
            DrawingUtilities.DrawLine(x1, y0 + 1, x1, y1 - 1, '║');

            GraphicConsole.Put('╔', x0, y0);
            GraphicConsole.Put('╚', x0, y1);
            GraphicConsole.Put('╗', x1, y0);
            GraphicConsole.Put('╝', x1, y1);
        }

        private void addToStat(string stat)
        {
            int amount = getModAmount();
            if (availablePoints >= amount)
            {
                if (stat == "STR")
                    strFreeMod += amount;
                else if (stat == "AGI")
                    agiFreeMod += amount;
                else if (stat == "DEX")
                    dexFreeMod += amount;
                else if (stat == "INT")
                    intFreeMod += amount;
                else if (stat == "WIL")
                    wilFreeMod += amount;
                else if (stat == "WIS")
                    wisFreeMod += amount;
                else if (stat == "CON")
                    conFreeMod += amount;
                else if (stat == "END")
                    endFreeMod += amount;
                else if (stat == "FRT")
                    frtFreeMod += amount;

                availablePoints -= amount;
            }
        }
        private void remFromStat(string stat)
        {
            int amount = getModAmount();
            if (stat == "STR")
            {
                if (strFreeMod >= amount)
                {
                    strFreeMod -= amount;
                    availablePoints += amount;
                }
            }
            else if (stat == "AGI")
            {
                if (agiFreeMod >= amount)
                {
                    agiFreeMod -= amount;
                    availablePoints += amount;
                }
            }
            else if (stat == "DEX")
            {
                if (dexFreeMod >= amount)
                {
                    dexFreeMod -= amount;
                    availablePoints += amount;
                }
            }
            else if (stat == "INT")
            {
                if (intFreeMod >= amount)
                {
                    intFreeMod -= amount;
                    availablePoints += amount;
                }
            }
            else if (stat == "WIL")
            {
                if (wilFreeMod >= amount)
                {
                    wilFreeMod -= amount;
                    availablePoints += amount;
                }
            }
            else if (stat == "WIS")
            {
                if (wisFreeMod >= amount)
                {
                    wisFreeMod -= amount;
                    availablePoints += amount;
                }
            }
            else if (stat == "CON")
            {
                if (conFreeMod >= amount)
                {
                    conFreeMod -= amount;
                    availablePoints += amount;
                }
            }
            else if (stat == "END")
            {
                if (endFreeMod >= amount)
                {
                    endFreeMod -= amount;
                    availablePoints += amount;
                }
            }
            else if (stat == "FRT")
            {
                if (frtFreeMod >= amount)
                {
                    frtFreeMod -= amount;
                    availablePoints += amount;
                }
            }
        }
        private int getModAmount()
        {
            TimeSpan pressed = new TimeSpan(0, 0, 0, 0, 125);
            if (InputManager.KeyWasPressedFor(OpenTK.Input.Key.LShift, pressed))
                return 5;
            else if (InputManager.KeyWasPressedFor(OpenTK.Input.Key.LControl, pressed))
                return 10;
            return 1;
        }
        private void calculateStats()
        {
            stats = new PlayerStats();
            
            stats.Strength = strRaceMod + strClassMod + strFreeMod;
            stats.Agility = agiRaceMod + agiClassMod + agiFreeMod;
            stats.Dexterity = dexRaceMod + dexClassMod + dexFreeMod;

            stats.Intelligence = intRaceMod + intClassMod + intFreeMod;
            stats.Willpower = wilRaceMod + wilClassMod + wilFreeMod;
            stats.Wisdom = wisRaceMod + wisClassMod + wisFreeMod;

            stats.Constitution = conRaceMod + conClassMod + conFreeMod;
            stats.Endurance = endRaceMod + endClassMod + endFreeMod;
            stats.Fortitude = frtRaceMod + frtClassMod + frtFreeMod;

            if (chosenCulture != null)
                stats.ApplyEffect(chosenCulture.Trait);
            if (chosenClass != null)
            {
                for (int i = 0; i < chosenClass.InheritEffects.Count; i++)
                    stats.ApplyEffect(chosenClass.InheritEffects[i]);
            }
            if (chosenTrait != null)
                stats.ApplyEffect(chosenTrait);

            stats.CalculateStats();

            detailedStatBox.Text =
                "---Physical Stats---<br>" +
                "Attack Power  : " + stats.AttackPower.EffectiveValue + "<br>" +
                "Hit Chance    : " + stats.PhysicalHitChance.EffectiveValue + "<br>" +
                "Crit Chance   : " + stats.PhysicalCritChance.EffectiveValue + "<br>" +
                "Crit Power    : " + stats.PhysicalCritPower.EffectiveValue + "<br>" +
                "Haste         : " + stats.PhysicalHaste.EffectiveValue + "<br>" +
                "Reduction     : " + stats.PhysicalReduction.EffectiveValue + "<br>" +
                "Reflection    : " + stats.PhysicalReflection.EffectiveValue + "<br>" +
                "Avoidance     : " + stats.PhysicalAvoidance.EffectiveValue + "<br><br>" +

                "---Magical  Stats---<br>" +
                "Spell Power   : " + stats.SpellPower.EffectiveValue + "<br>" +
                "Hit Chance    : " + stats.SpellHitChance.EffectiveValue + "<br>" +
                "Crit Chance   : " + stats.SpellCritChance.EffectiveValue + "<br>" +
                "Crit Power    : " + stats.SpellCritPower.EffectiveValue + "<br>" +
                "Haste         : " + stats.SpellHaste.EffectiveValue + "<br>" +
                "Reduction     : " + stats.SpellReduction.EffectiveValue + "<br>" +
                "Reflection    : " + stats.SpellReflection.EffectiveValue + "<br>" +
                "Avoidance     : " + stats.SpellAvoidance.EffectiveValue + "<br><br>" +

                "------Resources-----<br>" +
                "Health        : " + stats.MaxHealth + "<br>" +
                "Mana          : " + stats.MaxMana + "<br>" +
                "HP Regen      : " + stats.HPRegen + "<br>" +
                "MP Regen      : " + stats.MPRegen + "<br>";
        }

        void backButton_Click(object sender, MouseButtons button)
        {
            nameInput.Text = " ";
            GameManager.ChangeGameState(GameStates.MainMenu);
        }
        void randomName_Click(object sender, MouseButtons button)
        {
            nameInput.Text = Factories.NameGenerator.GenerateFirstName() + " " + Factories.NameGenerator.GenerateLastName();
        }

        void nextButton_Click(object sender, MouseButtons button)
        {
            if (chosenRace == null)
                popupControl.DisplayMessage("Please choose a race.");
            else if (chosenCulture == null)
                popupControl.DisplayMessage("Please choose a culture.");
            else if (chosenClass == null)
                popupControl.DisplayMessage("Please choose a class.");
            else if (availablePoints > 0)
                popupControl.DisplayMessage("Please spend all your points.");
            else if (nameInput.Text.Length <= 0)
                popupControl.DisplayMessage("Please choose a name.");
            else
            {
                PlayerStats player = new PlayerStats();
                player = chosenRace.AddRacialStats(player);
                player = chosenClass.CalculateStats(player);
                player.ApplyEffect(chosenCulture.Trait);
                
                if (chosenTrait != null)
                    player.ApplyEffect(chosenTrait);

                player.Name = nameInput.Text.Trim();
                player.Race = chosenRace.Name;
                player.Culture = chosenCulture.Name;
                player.Class = chosenClass.Name;

                player.Strength = strRaceMod + strClassMod + strFreeMod;
                player.Agility = agiRaceMod + agiClassMod + agiFreeMod;
                player.Dexterity = dexRaceMod + dexClassMod + dexFreeMod;

                player.Intelligence = intRaceMod + intClassMod + intFreeMod;
                player.Willpower = wilRaceMod + wilClassMod + wilFreeMod;
                player.Wisdom = wisRaceMod + wisClassMod + wisFreeMod;

                player.Constitution = conRaceMod + conClassMod + conFreeMod;
                player.Endurance = endRaceMod + endClassMod + endFreeMod;
                player.Fortitude = frtRaceMod + frtClassMod + frtFreeMod;

                //Prevents the text from persisting through to the next interface
                nameInput.Text = string.Empty;

                GameManager.SetupGame(player);
                GameManager.ChangeGameState(GameStates.Game);

                GameManager.Player.ForegroundColor = character.ForegroundColor;
                GameManager.Player.Token = character.Token;

                GameManager.Step();
            }
        }
        void resetButton_Click(object sender, MouseButtons button)
        {
            nameInput.Text = string.Empty;

            chosenRace = null;
            chosenCulture = null;
            chosenClass = null;
            chosenTrait = null;

            colorIndex = 0;
            character.ForegroundColor = Color.Black;

            itemLists.ClearSelection();

            raceSelect.Text = "[Choose Race]";
            cultureSelect.Text = "[Choose Culture]";
            classSelect.Text = "[Choose Class]";
            traitSelect.Text = "[Choose Trait]";

            availablePoints = MAX_FREE_POINTS;

            strRaceMod = 0;
            strClassMod = 0;
            strFreeMod = 0;

            agiRaceMod = 0;
            agiClassMod = 0;
            agiFreeMod = 0;

            dexRaceMod = 0;
            dexClassMod = 0;
            dexFreeMod = 0;


            intRaceMod = 0;
            intClassMod = 0;
            intFreeMod = 0;

            wilRaceMod = 0;
            wilClassMod = 0;
            wilFreeMod = 0;

            wisRaceMod = 0;
            wisClassMod = 0;
            wisFreeMod = 0;


            conRaceMod = 0;
            conClassMod = 0;
            conFreeMod = 0;

            endRaceMod = 0;
            endClassMod = 0;
            endFreeMod = 0;

            frtRaceMod = 0;
            frtClassMod = 0;
            frtFreeMod = 0;
        }
        void colorLeftButton_Click(object sender, MouseButtons button)
        {
            if (chosenCulture != null)
            {
                colorIndex--;
                if (colorIndex < 0)
                    colorIndex = chosenCulture.SkinColors.Count - 1;
                character.ForegroundColor = chosenCulture.SkinColors[colorIndex];
            }
        }
        void colorRightButton_Click(object sender, MouseButtons button)
        {
            if (chosenCulture != null)
            {
                colorIndex++;
                if (colorIndex >= chosenCulture.SkinColors.Count)
                    colorIndex = 0;
                character.ForegroundColor = chosenCulture.SkinColors[colorIndex];
            }
        }

        void raceSelect_Click(object sender, MouseButtons button)
        {
            chosenCulture = null;
            chosenRace = null;

            itemLists.SetList<Race>(races);
            selectionMode = SelectionModes.Race;
            itemLists.ClearSelection();

            colorIndex = 0;
            character.ForegroundColor = Color.Black;
        }
        void cultureSelect_Click(object sender, MouseButtons button)
        {
            if (chosenRace != null)
            {
                chosenCulture = null;
                cultureSelect.Text = "[Choose Culture]";

                selectionMode = SelectionModes.Culture;
                itemLists.SetList<Culture>(chosenRace.SubCultures);
                itemLists.ClearSelection();

                colorIndex = 0;
                character.ForegroundColor = Color.Black;
            }
            else
                popupControl.DisplayMessage("You must choose a Race first.");
        }
        void classSelect_Click(object sender, MouseButtons button)
        {
            if (chosenCulture != null)
            {
                chosenClass = null;
                classSelect.Text = "[Choose Class]";

                selectionMode = SelectionModes.Class;
                itemLists.SetList<Class>(classes);
                itemLists.ClearSelection();
            }
            else
                popupControl.DisplayMessage("You must choose a race and culture first.");
        }
        void traitSelect_Click(object sender, MouseButtons button)
        {
            if (chosenClass == null)
                popupControl.DisplayMessage("Pick a class first!");
            else if (chosenClass.ClassTraits.Count <= 1)
                popupControl.DisplayMessage("There are no more traits to pick from.");
            else
            {
                chosenTrait = null;
                traitSelect.Text = "[Choose Trait]";

                itemLists.SetList<Effect>(chosenClass.ClassTraits);
                selectionMode = SelectionModes.Trait;
                itemLists.ClearSelection();
            }
        }

        void itemLists_Selected(object sender, int index)
        {
            if (selectionMode == SelectionModes.Race)
            {
                chosenRace = (Race)itemLists.GetSelection();
                informationBox.Text = chosenRace.Name + "<br><br><tb>" + chosenRace.Description;

                raceSelect.Text = chosenRace.Name;

                chosenCulture = null;
                cultureSelect.Text = "[Choose Culture]";
                chosenClass = null;
                classSelect.Text = "[Choose Class]";
                chosenTrait = null;
                traitSelect.Text = "[Choose Trait]";

                PlayerStats tempStats = new PlayerStats();
                tempStats = chosenRace.AddRacialStats(tempStats);

                strRaceMod = tempStats.Strength;
                agiRaceMod = tempStats.Agility;
                dexRaceMod = tempStats.Dexterity;

                intRaceMod = tempStats.Intelligence;
                wilRaceMod = tempStats.Willpower;
                wisRaceMod = tempStats.Wisdom;

                conRaceMod = tempStats.Constitution;
                endRaceMod = tempStats.Strength;
                frtRaceMod = tempStats.Strength;

                colorIndex = 0;
                character.ForegroundColor = Color.Black;
            }
            else if (selectionMode == SelectionModes.Culture)
            {
                chosenCulture = (Culture)itemLists.GetSelection();
                informationBox.Text = chosenCulture.Name + "<br><br><tb>" + chosenCulture.Description;

                cultureSelect.Text = chosenCulture.Name;

                chosenClass = null;
                classSelect.Text = "[Choose Class]";
                chosenTrait = null;
                traitSelect.Text = "[Choose Trait]";

                colorIndex = 0;
                character.ForegroundColor = chosenCulture.SkinColors[colorIndex];
            }
            else if (selectionMode == SelectionModes.Class)
            {
                chosenClass = (Class)itemLists.GetSelection();
                informationBox.Text = chosenClass.Name + "<br><br><tb>" + chosenClass.Description;

                classSelect.Text = chosenClass.Name;

                chosenTrait = null;
                traitSelect.Text = "[Choose Trait]";

                PlayerStats tempStats = new PlayerStats();
                tempStats = chosenClass.CalculateStats(tempStats);

                strClassMod = tempStats.Strength;
                agiClassMod = tempStats.Agility;
                dexClassMod = tempStats.Dexterity;

                intClassMod = tempStats.Intelligence;
                wilClassMod = tempStats.Willpower;
                wisClassMod = tempStats.Wisdom;

                conClassMod = tempStats.Constitution;
                endClassMod = tempStats.Endurance;
                frtClassMod = tempStats.Fortitude;

                if (chosenClass.ClassTraits.Count == 0)
                    traitSelect.Text = "[None Available]";
                else if (chosenClass.ClassTraits.Count == 1)
                {
                    chosenTrait = chosenClass.ClassTraits[0];
                    traitSelect.Text = chosenTrait.EffectName;
                }
                else
                    traitSelect.Text = "[Choose Trait]";
            }
            else if (selectionMode == SelectionModes.Trait)
            {
                chosenTrait = (Effect)itemLists.GetSelection();
                informationBox.Text = chosenTrait.EffectName + "<br><br><tb>" + chosenTrait.EffectDescription;

                traitSelect.Text = chosenTrait.EffectName;
            }

            calculateStats();
        }

        void statsResetButton_Click(object sender, MouseButtons button)
        {
            strFreeMod = 0;
            agiFreeMod = 0;
            dexFreeMod = 0;

            intFreeMod = 0;
            wilFreeMod = 0;
            wisFreeMod = 0;

            conFreeMod = 0;
            endFreeMod = 0;
            frtFreeMod = 0;

            availablePoints = MAX_FREE_POINTS;
        }

        #region Stat Description Buttons
        void strButton_Click(object sender, MouseButtons button)
        {
            if (button == MouseButtons.Left)
            {
                setStatInformation("STR");
            }
            else if (button == MouseButtons.Right)
            {
                availablePoints += strFreeMod;
                strFreeMod = 0;
            }
        }
        void agiButton_Click(object sender, MouseButtons button)
        {
            if (button == MouseButtons.Left)
            {
                setStatInformation("AGI");
            }
            else if (button == MouseButtons.Right)
            {
                availablePoints += agiFreeMod;
                agiFreeMod = 0;
            }
        }
        void dexButton_Click(object sender, MouseButtons button)
        {
            if (button == MouseButtons.Left)
            {
                setStatInformation("DEX");
            }
            else if (button == MouseButtons.Right)
            {
                availablePoints += dexFreeMod;
                dexFreeMod = 0;
            }
        }

        void intButton_Click(object sender, MouseButtons button)
        {
            if (button == MouseButtons.Left)
            {
                setStatInformation("INT");
            }
            else if (button == MouseButtons.Right)
            {
                availablePoints += intFreeMod;
                intFreeMod = 0;
            }
        }
        void wilButton_Click(object sender, MouseButtons button)
        {
            if (button == MouseButtons.Left)
            {
                setStatInformation("WIL");
            }
            else if (button == MouseButtons.Right)
            {
                availablePoints += wilFreeMod;
                wilFreeMod = 0;
            }
        }
        void wisButton_Click(object sender, MouseButtons button)
        {
            if (button == MouseButtons.Left)
            {
                setStatInformation("WIS");
            }
            else if (button == MouseButtons.Right)
            {
                availablePoints += wisFreeMod;
                wisFreeMod = 0;
            }
        }

        void conButton_Click(object sender, MouseButtons button)
        {
            if (button == MouseButtons.Left)
            {
                setStatInformation("CON");
            }
            else if (button == MouseButtons.Right)
            {
                availablePoints += conFreeMod;
                conFreeMod = 0;
            }
        }
        void endButton_Click(object sender, MouseButtons button)
        {
            if (button == MouseButtons.Left)
            {
                setStatInformation("END");
            }
            else if (button == MouseButtons.Right)
            {
                availablePoints += endFreeMod;
                endFreeMod = 0;
            }
        }
        void frtButton_Click(object sender, MouseButtons button)
        {
            if (button == MouseButtons.Left)
            {
                setStatInformation("FRT");
            }
            else if (button == MouseButtons.Right)
            {
                availablePoints += frtFreeMod;
                frtFreeMod = 0;
            }
        }
        #endregion
        #region Add/Remove Stat Buttons
        void strAdd_Click(object sender, MouseButtons button)
        {
            addToStat("STR");
        }
        void strRem_Click(object sender, MouseButtons button)
        {
            remFromStat("STR");
        }
        void agiAdd_Click(object sender, MouseButtons button)
        {
            addToStat("AGI");
        }
        void agiRem_Click(object sender, MouseButtons button)
        {
            remFromStat("AGI");
        }
        void dexAdd_Click(object sender, MouseButtons button)
        {
            addToStat("DEX");
        }
        void dexRem_Click(object sender, MouseButtons button)
        {
            remFromStat("DEX");
        }

        void intAdd_Click(object sender, MouseButtons button)
        {
            addToStat("INT");
        }
        void intRem_Click(object sender, MouseButtons button)
        {
            remFromStat("INT");
        }
        void wilAdd_Click(object sender, MouseButtons button)
        {
            addToStat("WIL");
        }
        void wilRem_Click(object sender, MouseButtons button)
        {
            remFromStat("WIL");
        }
        void wisAdd_Click(object sender, MouseButtons button)
        {
            addToStat("WIS");
        }
        void wisRem_Click(object sender, MouseButtons button)
        {
            remFromStat("WIS");
        }

        void conAdd_Click(object sender, MouseButtons button)
        {
            addToStat("CON");
        }
        void conRem_Click(object sender, MouseButtons button)
        {
            remFromStat("CON");
        }
        void endAdd_Click(object sender, MouseButtons button)
        {
            addToStat("END");
        }
        void endRem_Click(object sender, MouseButtons button)
        {
            remFromStat("END");
        }
        void frtAdd_Click(object sender, MouseButtons button)
        {
            addToStat("FRT");
        }
        void frtRem_Click(object sender, MouseButtons button)
        {
            remFromStat("FRT");
        }
        #endregion

        #region Variables
        //Overall
        Title mainTitle;
        Button backButton, nextButton, resetButton, colorLeftButton, colorRightButton;
        ScrollingList itemLists;
        TextBox informationBox, detailedStatBox;
        Popup popupControl;
        TokenTile character;

        //General Box
        Title nameTitle, raceTitle, cultureTitle, classTitle, traitTitle;
        Button raceSelect, cultureSelect, classSelect, traitSelect, randomName;
        InputBox nameInput;

        //Stats Box
        int availablePoints = MAX_FREE_POINTS;
        int strRaceMod, agiRaceMod, dexRaceMod, intRaceMod, wilRaceMod, wisRaceMod, conRaceMod, endRaceMod, frtRaceMod;
        int strClassMod, agiClassMod, dexClassMod, intClassMod, wilClassMod, wisClassMod, conClassMod, endClassMod, frtClassMod;
        int strFreeMod, agiFreeMod, dexFreeMod, intFreeMod, wilFreeMod, wisFreeMod, conFreeMod, endFreeMod, frtFreeMod;
        int colorIndex = 0;

        Button strAdd, strRem, agiAdd, agiRem, dexAdd, dexRem, intAdd, intRem, wilAdd, wilRem, wisAdd, wisRem, conAdd, conRem, endAdd, endRem, frtAdd, frtRem;
        Button strButton, agiButton, dexButton, intButton, wilButton, wisButton, conButton, endButton, frtButton;
        Title physicalTitle, spellTitle, defenseTitle, freePointsTitle;
        Button statsResetButton;

        //Lists
        private List<Race> races = new List<Race>() { new Human(), new Elf(), new Dwarf() };
        private List<Class> classes = new List<Class>() { new Berserker(), new Cleric(), new Mage(), new Rogue(), new Shaman(), new Skullomancer(), new Summoner(), new Warlock(), new Warrior() };

        private Race chosenRace;
        private Culture chosenCulture;
        private Class chosenClass;
        private Effect chosenTrait;

        private PlayerStats stats;
        private const int MAX_FREE_POINTS = 20;

        private SelectionModes selectionMode = SelectionModes.None;
        private enum SelectionModes { Race, Culture, Class, Trait, None }
        #endregion
    }
}
