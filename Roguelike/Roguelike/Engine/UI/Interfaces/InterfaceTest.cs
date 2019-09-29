using System;
using OpenTK.Graphics;
using Roguelike.Engine.Console;
using Roguelike.Engine.UI.Controls;
using Roguelike.Core;
using Roguelike.Core.Stats;

namespace Roguelike.Engine.UI.Interfaces
{
    public class InterfaceTest : Interface
    {
        PlayerStats leftUnit, rightUnit;

        Title leftUnitName, rightUnitName;
        Title strOne, agiOne, dexOne, intOne, wilOne, wisOne, conOne, endOne, frtOne;
        Title strTwo, agiTwo, dexTwo, intTwo, wilTwo, wisTwo, conTwo, endTwo, frtTwo;
        TextBox leftUnitInfo, rightUnitInfo;

        Button[] leftSpells = new Button[4];
        Button[] rightSpells = new Button[4];

        Button reset, heal, turn;

        LeftAdjustmentButtons leftButtonSet;
        RightAdjustmentButtons rightButtonSet;

        public InterfaceTest()
        {
            leftUnit = new PlayerStats(null) { UnitName = "Randolph", Gender = "Male", Class = "Warrior" };
            rightUnit = new PlayerStats(null) { UnitName = "Janodolph", Gender = "Male", Class = "Magician" };

            //leftUnit = ClassGenerator.GenerateRandomStats();
            leftUnit.ParentEntity = new Core.Entities.Player(null) { X = 0, Y = 0 };
            //rightUnit = ClassGenerator.GenerateRandomStats();
            rightUnit.ParentEntity = new Core.Entities.Player(null) { X = 1, Y = 0 };

            leftUnit.SetInitialStats();
            rightUnit.SetInitialStats();

            setupInterface();

            //Unit One
            hpBarOne = new BarTitle(this, 22, 5, "HP", 15);
            mpBarOne = new BarTitle(this, 39, 5, "MP", 15);
            mpBarOne.BarColor = Color4.DodgerBlue;
            mpBarOne.FillColor = Color4.DarkBlue;

            //Unit Two
            hpBarTwo = new BarTitle(this, 70, 5, "HP", 15);
            mpBarTwo = new BarTitle(this, 87, 5, "MP", 15);
            mpBarTwo.BarColor = Color4.DodgerBlue;
            mpBarTwo.FillColor = Color4.DarkBlue;

            leftUnitName = new Title(this, leftUnit.UnitName + " the " + leftUnit.Class, 38, 3, Title.TextAlignModes.Center);
            rightUnitName = new Title(this, rightUnit.UnitName + " the " + rightUnit.Class, 86, 3, Title.TextAlignModes.Center);
        }

        public override void DrawStep()
        {
            GraphicConsole.Instance.Clear();

            base.DrawStep();
        }
        public override void UpdateStep()
        {
            //Unit One
            hpBarOne.CurrentValue = leftUnit.Health;
            hpBarOne.MaxValue = leftUnit.MaxHealth;
            mpBarOne.CurrentValue = leftUnit.Mana;
            mpBarOne.MaxValue = leftUnit.MaxMana;

            //Unit Two
            hpBarTwo.CurrentValue = rightUnit.Health;
            hpBarTwo.MaxValue = rightUnit.MaxHealth;
            mpBarTwo.CurrentValue = rightUnit.Mana;
            mpBarTwo.MaxValue = rightUnit.MaxMana;

            setStatTitles();
            setInformationBars();

            leftUnit.CalculateStats();
            rightUnit.CalculateStats();

            base.UpdateStep();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void setupInterface()
        {
            interfaceTitle = new Title(this, "Combat Testing", GraphicConsole.Instance.BufferWidth / 2, 0, Title.TextAlignModes.Center);
            backButton = new Button(this, "X", GraphicConsole.Instance.BufferWidth - 1, 0, 1, 1);
            backButton.Click += backButton_Pressed;

            combatLog = new TextBox(this, 23, 45, GraphicConsole.Instance.BufferWidth - 46, 5);

            //Left Player
            strOne = new Title(this, "STR: ##", 2, 10, Title.TextAlignModes.Left);
            agiOne = new Title(this, "AGI: ##", 2, 11, Title.TextAlignModes.Left);
            dexOne = new Title(this, "DEX: ##", 2, 12, Title.TextAlignModes.Left);

            intOne = new Title(this, "INT: ##", 2, 14, Title.TextAlignModes.Left);
            wilOne = new Title(this, "WIL: ##", 2, 15, Title.TextAlignModes.Left);
            wisOne = new Title(this, "WIS: ##", 2, 16, Title.TextAlignModes.Left);

            conOne = new Title(this, "CON: ##", 2, 18, Title.TextAlignModes.Left);
            endOne = new Title(this, "END: ##", 2, 19, Title.TextAlignModes.Left);
            frtOne = new Title(this, "FRT: ##", 2, 20, Title.TextAlignModes.Left);

            leftUnitInfo = new TextBox(this, 2, 22, 20, 25);

            for (int i = 0; i < leftSpells.Length; i++)
                leftSpells[i] = new Button(this, "[ ABIL " + (i + 1) + " ]", 30, 13 + (i * 4), 10, 3);

            leftSpells[0].Click += LeftSpell_00;
            leftSpells[1].Click += LeftSpell_01;
            leftSpells[2].Click += LeftSpell_02;
            leftSpells[3].Click += LeftSpell_03;

            leftButtonSet = new LeftAdjustmentButtons(this, leftUnit);

            //Right Player
            strTwo = new Title(this, "STR: ##", GraphicConsole.Instance.BufferWidth - 2, 10, Title.TextAlignModes.Right);
            agiTwo = new Title(this, "AGI: ##", GraphicConsole.Instance.BufferWidth - 2, 11, Title.TextAlignModes.Right);
            dexTwo = new Title(this, "DEX: ##", GraphicConsole.Instance.BufferWidth - 2, 12, Title.TextAlignModes.Right);

            intTwo = new Title(this, "INT: ##", GraphicConsole.Instance.BufferWidth - 2, 14, Title.TextAlignModes.Right);
            wilTwo = new Title(this, "WIL: ##", GraphicConsole.Instance.BufferWidth - 2, 15, Title.TextAlignModes.Right);
            wisTwo = new Title(this, "WIS: ##", GraphicConsole.Instance.BufferWidth - 2, 16, Title.TextAlignModes.Right);

            conTwo = new Title(this, "CON: ##", GraphicConsole.Instance.BufferWidth - 2, 18, Title.TextAlignModes.Right);
            endTwo = new Title(this, "END: ##", GraphicConsole.Instance.BufferWidth - 2, 19, Title.TextAlignModes.Right);
            frtTwo = new Title(this, "FRT: ##", GraphicConsole.Instance.BufferWidth - 2, 20, Title.TextAlignModes.Right);

            rightUnitInfo = new TextBox(this, GraphicConsole.Instance.BufferWidth - 20, 22, 20, 25);

            for (int i = 0; i < leftSpells.Length; i++)
                rightSpells[i] = new Button(this, "[ ABIL " + (i + 1) + " ]", 83, 13 + (i * 4), 10, 3);

            rightSpells[0].Click += RightSpell_00;
            rightSpells[1].Click += RightSpell_01;
            rightSpells[2].Click += RightSpell_02;
            rightSpells[3].Click += RightSpell_03;

            rightButtonSet = new RightAdjustmentButtons(this, rightUnit);

            reset = new Button(this, "Reset", 2, 6, 7, 3);
            reset.Click += reset_Pressed;

            heal = new Button(this, "Heal", 10, 6, 7, 3);
            heal.Click += heal_Pressed;

            turn = new Button(this, "Turn", 18, 6, 7, 3);
            turn.Click += turn_Pressed;

            setAbilitiesToBars();
        }
        
        private void setStatTitles()
        {
            //Left Player
            strOne.Text = "STR: " + leftUnit.Strength.ToString();
            agiOne.Text = "AGI: " + leftUnit.Agility.ToString();
            dexOne.Text = "DEX: " + leftUnit.Dexterity.ToString();

            intOne.Text = "INT: " + leftUnit.Intelligence.ToString();
            wilOne.Text = "WIL: " + leftUnit.Willpower.ToString();
            wisOne.Text = "WIS: " + leftUnit.Wisdom.ToString();

            conOne.Text = "CON: " + leftUnit.Constitution.ToString();
            endOne.Text = "END: " + leftUnit.Endurance.ToString();
            frtOne.Text = "FRT: " + leftUnit.Fortitude.ToString();

            //Right Player
            strTwo.Text = "STR: " + rightUnit.Strength.ToString();
            agiTwo.Text = "AGI: " + rightUnit.Agility.ToString();
            dexTwo.Text = "DEX: " + rightUnit.Dexterity.ToString();

            intTwo.Text = "INT: " + rightUnit.Intelligence.ToString();
            wilTwo.Text = "WIL: " + rightUnit.Willpower.ToString();
            wisTwo.Text = "WIS: " + rightUnit.Wisdom.ToString();

            conTwo.Text = "CON: " + rightUnit.Constitution.ToString();
            endTwo.Text = "END: " + rightUnit.Endurance.ToString();
            frtTwo.Text = "FRT: " + rightUnit.Fortitude.ToString();
        }
        private void setInformationBars()
        {
            leftUnitInfo.Text =
                "Atk Pwr: " + leftUnit.AttackPower + "<br>" +
                "Phy Hst: " + leftUnit.PhysicalHaste + "<br>" +
                "Phy Hit: " + leftUnit.PhysicalHitChance + "<br>" +
                "Crt Chn: " + leftUnit.PhysicalCritChance + "<br>" +
                "Crt Pwr: " + leftUnit.PhysicalCritPower + "<br>" +
                "Phy Red: " + leftUnit.PhysicalReduction + "<br>" +
                "Phy Ref: " + leftUnit.PhysicalReflection + "<br>" +
                "Phy Avd: " + leftUnit.PhysicalAvoidance + "<br><br>" +

                "Spl Pwr: " + leftUnit.SpellPower + "<br>" +
                "Spl Hst: " + leftUnit.SpellHaste + "<br>" +
                "Spl Hit: " + leftUnit.SpellHitChance + "<br>" +
                "Crt Chn: " + leftUnit.SpellCritChance + "<br>" +
                "Crt Pwr: " + leftUnit.SpellCritPower + "<br>" +
                "Spl Red: " + leftUnit.SpellReduction + "<br>" +
                "Spl Ref: " + leftUnit.SpellReflection + "<br>" +
                "Spl Avd: " + leftUnit.SpellAvoidance + "<br><br>";

            leftUnitInfo.Text += "Applied Effects<br>";
            foreach (Core.Combat.Effect effect in leftUnit.AppliedEffects)
                leftUnitInfo.Text += effect.EffectName + "<br>";

            rightUnitInfo.Text =
                "Atk Pwr: " + rightUnit.AttackPower + "<br>" +
                "Phy Hst: " + rightUnit.PhysicalHaste + "<br>" +
                "Phy Hit: " + rightUnit.PhysicalHitChance + "<br>" +
                "Crt Chn: " + rightUnit.PhysicalCritChance + "<br>" +
                "Crt Pwr: " + rightUnit.PhysicalCritPower + "<br>" +
                "Phy Red: " + rightUnit.PhysicalReduction + "<br>" +
                "Phy Ref: " + rightUnit.PhysicalReflection + "<br>" +
                "Phy Avd: " + rightUnit.PhysicalAvoidance + "<br><br>" +

                "Spl Pwr: " + rightUnit.SpellPower + "<br>" +
                "Spl Hst: " + rightUnit.SpellHaste + "<br>" +
                "Spl Hit: " + rightUnit.SpellHitChance + "<br>" +
                "Crt Chn: " + rightUnit.SpellCritChance + "<br>" +
                "Crt Pwr: " + rightUnit.SpellCritPower + "<br>" +
                "Spl Red: " + rightUnit.SpellReduction + "<br>" +
                "Spl Ref: " + rightUnit.SpellReflection + "<br>" +
                "Spl Avd: " + rightUnit.SpellAvoidance + "<br><br>";

            rightUnitInfo.Text += "Applied Effects<br>";
            foreach (Core.Combat.Effect effect in rightUnit.AppliedEffects)
                rightUnitInfo.Text += effect.EffectName + "<br>";
        }
        private void setAbilitiesToBars()
        {
            int i;
            for (i = 0 ; i < leftUnit.AbilityList.Count && i < leftSpells.Length; i++)
                leftSpells[i].Text = "[" + leftUnit.AbilityList[i].AbilityNameShort + "]";
            for (; i < leftSpells.Length; i++)
                leftSpells[i].Text = "[ ABIL " + (i + 1) + " ]";

            for (i = 0; i < rightUnit.AbilityList.Count && i < leftSpells.Length; i++)
                rightSpells[i].Text = "[" + rightUnit.AbilityList[i].AbilityNameShort + "]";
            for (; i < rightSpells.Length; i++)
                rightSpells[i].Text = "[ ABIL " + (i + 1) + " ]";
        }

        void reset_Pressed(object sender, MouseButtons button)
        {
            leftUnit = new PlayerStats(null) { UnitName = "Randolph", Gender = "Male", Class = "Warrior" };
            rightUnit = new PlayerStats(null) { UnitName = "Janodolph", Gender = "Male", Class = "Magician" };

            leftUnit.ParentEntity = new Core.Entities.Player(null) { X = 0, Y = 0 };
            rightUnit.ParentEntity = new Core.Entities.Player(null) { X = 1, Y = 0 };

            leftUnit.SetInitialStats();
            rightUnit.SetInitialStats();

            leftUnitName.Text = leftUnit.UnitName + " the " + leftUnit.Class;
            rightUnitName.Text = rightUnit.UnitName + " the " + rightUnit.Class;

            leftButtonSet.stats = leftUnit;
            rightButtonSet.stats = rightUnit;

            setAbilitiesToBars();
        }
        void backButton_Pressed(object sender, MouseButtons button)
        {
            GameManager.ChangeGameState(GameStates.MainMenu);
        }
        void heal_Pressed(object sender, MouseButtons button)
        {
            leftUnit.Health = (int)leftUnit.MaxHealth.EffectiveValue;
            leftUnit.Mana = (int)leftUnit.MaxMana.EffectiveValue;

            rightUnit.Health = (int)rightUnit.MaxHealth.EffectiveValue;
            rightUnit.Mana = (int)rightUnit.MaxMana.EffectiveValue;

            for (int i = 0; i < leftUnit.AppliedEffects.Count; i++)
            {
                if (leftUnit.AppliedEffects[i].IsHarmful)
                {
                    leftUnit.AppliedEffects.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < rightUnit.AppliedEffects.Count; i++)
            {
                if (rightUnit.AppliedEffects[i].IsHarmful)
                {
                    rightUnit.AppliedEffects.RemoveAt(i);
                    i--;
                }
            }
        }
        void turn_Pressed(object sender, MouseButtons button)
        {
            combatTurn();
        }

        void LeftSpell_00(object sender, MouseButtons button)
        {
            castLeftAbility(0);
        }
        void LeftSpell_01(object sender, MouseButtons button)
        {
            castLeftAbility(1);
        }
        void LeftSpell_02(object sender, MouseButtons button)
        {
            castLeftAbility(2);
        }
        void LeftSpell_03(object sender, MouseButtons button)
        {
            castLeftAbility(3);
        }

        void RightSpell_00(object sender, MouseButtons button)
        {
            castRightAbility(0);
        }
        void RightSpell_01(object sender, MouseButtons button)
        {
            castRightAbility(1);
        }
        void RightSpell_02(object sender, MouseButtons button)
        {
            castRightAbility(2);
        }
        void RightSpell_03(object sender, MouseButtons button)
        {
            castRightAbility(3);
        }

        private void castLeftAbility(int index)
        {
            if (leftUnit.AbilityList.Count >= index + 1)
            {
                if (leftUnit.AbilityList[index].TargetingType == Core.Combat.TargetingTypes.EntityTarget)
                {
                    CombatManager.PerformAbility(leftUnit, rightUnit, leftUnit.AbilityList[index]);
                    combatTurn();
                }
                else if (leftUnit.AbilityList[index].TargetingType == Core.Combat.TargetingTypes.Self)
                {
                    CombatManager.PerformAbility(leftUnit, leftUnit, leftUnit.AbilityList[index]);
                    combatTurn();
                }
            }
        }
        private void castRightAbility(int index)
        {
            if (rightUnit.AbilityList.Count >= index + 1)
            {
                if (rightUnit.AbilityList[index].TargetingType == Core.Combat.TargetingTypes.EntityTarget)
                {
                    CombatManager.PerformAbility(rightUnit, leftUnit, rightUnit.AbilityList[index]);
                    combatTurn();
                }
                else if (rightUnit.AbilityList[index].TargetingType == Core.Combat.TargetingTypes.Self)
                {
                    CombatManager.PerformAbility(rightUnit, rightUnit, rightUnit.AbilityList[index]);
                    combatTurn();
                }
            }
        }

        private void combatTurn()
        {
            leftUnit.UpdateStep();
            rightUnit.UpdateStep();
        }

        Button backButton;
        Title interfaceTitle;
        BarTitle hpBarOne, hpBarTwo;
        BarTitle mpBarOne, mpBarTwo;

        TextBox combatLog;
    }

    public class LeftAdjustmentButtons : Control
    {
        #region Variables
        Button strAdd, strRem;
        Button agiAdd, agiRem;
        Button dexAdd, dexRem;

        Button intAdd, intRem;
        Button wilAdd, wilRem;
        Button wisAdd, wisRem;

        Button conAdd, conRem;
        Button endAdd, endRem;
        Button frtAdd, frtRem;

        public PlayerStats stats;
        #endregion

        public LeftAdjustmentButtons(Interface parent, PlayerStats stats)
            : base(parent)
        {
            this.stats = stats;

            strAdd = new Button(this, "+", 11, 10, 1, 1);
            agiAdd = new Button(this, "+", 11, 11, 1, 1);
            dexAdd = new Button(this, "+", 11, 12, 1, 1);

            intAdd = new Button(this, "+", 11, 14, 1, 1);
            wilAdd = new Button(this, "+", 11, 15, 1, 1);
            wisAdd = new Button(this, "+", 11, 16, 1, 1);

            conAdd = new Button(this, "+", 11, 18, 1, 1);
            endAdd = new Button(this, "+", 11, 19, 1, 1);
            frtAdd = new Button(this, "+", 11, 20, 1, 1);



            strRem = new Button(this, "-", 13, 10, 1, 1);
            agiRem = new Button(this, "-", 13, 11, 1, 1);
            dexRem = new Button(this, "-", 13, 12, 1, 1);

            intRem = new Button(this, "-", 13, 14, 1, 1);
            wilRem = new Button(this, "-", 13, 15, 1, 1);
            wisRem = new Button(this, "-", 13, 16, 1, 1);

            conRem = new Button(this, "-", 13, 18, 1, 1);
            endRem = new Button(this, "-", 13, 19, 1, 1);
            frtRem = new Button(this, "-", 13, 20, 1, 1);

            #region Events
            strAdd.Click += strAdd_Pressed;
            strRem.Click += strRem_Pressed;
            agiAdd.Click += agiAdd_Pressed;
            agiRem.Click += agiRem_Pressed;
            dexAdd.Click += dexAdd_Pressed;
            dexRem.Click += dexRem_Pressed;
            intAdd.Click += intAdd_Pressed;
            intRem.Click += intRem_Pressed;
            wilAdd.Click += wilAdd_Pressed;
            wilRem.Click += wilRem_Pressed;
            wisAdd.Click += wisAdd_Pressed;
            wisRem.Click += wisRem_Pressed;
            conAdd.Click += conAdd_Pressed;
            conRem.Click += conRem_Pressed;
            endAdd.Click += endAdd_Pressed;
            endRem.Click += endRem_Pressed;
            frtAdd.Click += frtAdd_Pressed;
            frtRem.Click += frtRem_Pressed;
            #endregion
        }

        #region Event Methods
        void strAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Strength++;
            stats.CalculateStats();
        }
        void strRem_Pressed(object sender, MouseButtons button)
        {
            stats.Strength--;
            stats.CalculateStats();
        }

        void agiAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Agility++;
            stats.CalculateStats();
        }
        void agiRem_Pressed(object sender, MouseButtons button)
        {
            stats.Agility--;
            stats.CalculateStats();
        }

        void dexAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Dexterity++;
            stats.CalculateStats();
        }
        void dexRem_Pressed(object sender, MouseButtons button)
        {
            stats.Dexterity--;
            stats.CalculateStats();
        }

        void intAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Intelligence++;
            stats.CalculateStats();
        }
        void intRem_Pressed(object sender, MouseButtons button)
        {
            stats.Intelligence--;
            stats.CalculateStats();
        }

        void wilAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Willpower++;
            stats.CalculateStats();
        }
        void wilRem_Pressed(object sender, MouseButtons button)
        {
            stats.Willpower--;
            stats.CalculateStats();
        }

        void wisAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Wisdom++;
            stats.CalculateStats();
        }
        void wisRem_Pressed(object sender, MouseButtons button)
        {
            stats.Wisdom--;
            stats.CalculateStats();
        }

        void conAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Constitution++;
            stats.CalculateStats();
        }
        void conRem_Pressed(object sender, MouseButtons button)
        {
            stats.Constitution--;
            stats.CalculateStats();
        }

        void endAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Endurance++;
            stats.CalculateStats();
        }
        void endRem_Pressed(object sender, MouseButtons button)
        {
            stats.Endurance--;
            stats.CalculateStats();
        }

        void frtAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Fortitude++;
            stats.CalculateStats();
        }
        void frtRem_Pressed(object sender, MouseButtons button)
        {
            stats.Fortitude--;
            stats.CalculateStats();
        }
        #endregion
    }
    public class RightAdjustmentButtons : Control
    {
        #region Variables
        Button strAdd, strRem;
        Button agiAdd, agiRem;
        Button dexAdd, dexRem;

        Button intAdd, intRem;
        Button wilAdd, wilRem;
        Button wisAdd, wisRem;

        Button conAdd, conRem;
        Button endAdd, endRem;
        Button frtAdd, frtRem;

        public PlayerStats stats;
        #endregion

        public RightAdjustmentButtons(Interface parent, PlayerStats stats)
            : base(parent)
        {
            this.stats = stats;

            strAdd = new Button(this, "+", 111, 10, 1, 1);
            agiAdd = new Button(this, "+", 111, 11, 1, 1);
            dexAdd = new Button(this, "+", 111, 12, 1, 1);

            intAdd = new Button(this, "+", 111, 14, 1, 1);
            wilAdd = new Button(this, "+", 111, 15, 1, 1);
            wisAdd = new Button(this, "+", 111, 16, 1, 1);

            conAdd = new Button(this, "+", 111, 18, 1, 1);
            endAdd = new Button(this, "+", 111, 19, 1, 1);
            frtAdd = new Button(this, "+", 111, 20, 1, 1);



            strRem = new Button(this, "-", 113, 10, 1, 1);
            agiRem = new Button(this, "-", 113, 11, 1, 1);
            dexRem = new Button(this, "-", 113, 12, 1, 1);

            intRem = new Button(this, "-", 113, 14, 1, 1);
            wilRem = new Button(this, "-", 113, 15, 1, 1);
            wisRem = new Button(this, "-", 113, 16, 1, 1);

            conRem = new Button(this, "-", 113, 18, 1, 1);
            endRem = new Button(this, "-", 113, 19, 1, 1);
            frtRem = new Button(this, "-", 113, 20, 1, 1);

            #region Events
            strAdd.Click += strAdd_Pressed;
            strRem.Click += strRem_Pressed;
            agiAdd.Click += agiAdd_Pressed;
            agiRem.Click += agiRem_Pressed;
            dexAdd.Click += dexAdd_Pressed;
            dexRem.Click += dexRem_Pressed;
            intAdd.Click += intAdd_Pressed;
            intRem.Click += intRem_Pressed;
            wilAdd.Click += wilAdd_Pressed;
            wilRem.Click += wilRem_Pressed;
            wisAdd.Click += wisAdd_Pressed;
            wisRem.Click += wisRem_Pressed;
            conAdd.Click += conAdd_Pressed;
            conRem.Click += conRem_Pressed;
            endAdd.Click += endAdd_Pressed;
            endRem.Click += endRem_Pressed;
            frtAdd.Click += frtAdd_Pressed;
            frtRem.Click += frtRem_Pressed;
            #endregion
        }

        #region Event Methods
        void strAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Strength++;
        }
        void strRem_Pressed(object sender, MouseButtons button)
        {
            stats.Strength--;
        }

        void agiAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Agility++;
        }
        void agiRem_Pressed(object sender, MouseButtons button)
        {
            stats.Agility--;
        }

        void dexAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Dexterity++;
        }
        void dexRem_Pressed(object sender, MouseButtons button)
        {
            stats.Dexterity--;
        }

        void intAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Intelligence++;
        }
        void intRem_Pressed(object sender, MouseButtons button)
        {
            stats.Intelligence--;
        }

        void wilAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Willpower++;
        }
        void wilRem_Pressed(object sender, MouseButtons button)
        {
            stats.Willpower--;
        }

        void wisAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Wisdom++;
        }
        void wisRem_Pressed(object sender, MouseButtons button)
        {
            stats.Wisdom--;
        }

        void conAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Constitution++;
        }
        void conRem_Pressed(object sender, MouseButtons button)
        {
            stats.Constitution--;
        }

        void endAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Endurance++;
        }
        void endRem_Pressed(object sender, MouseButtons button)
        {
            stats.Endurance--;
        }

        void frtAdd_Pressed(object sender, MouseButtons button)
        {
            stats.Fortitude++;
        }
        void frtRem_Pressed(object sender, MouseButtons button)
        {
            stats.Fortitude--;
        }
        #endregion
    }
}
