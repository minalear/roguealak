using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelike.Engine.UI;
using Roguelike.Engine.UI.Controls;
using Roguelike.Engine.Game;
using Roguelike.Engine.Game.Combat;
using Roguelike.Engine.Game.Entities;
using Roguelike.Engine.Game.Items;

namespace Roguelike.Engine.UI.Interfaces
{
    public class GameInterface : Interface
    {
        private BarTitle healthBar, manaBar;
        private Title playerTitle;
        private Popup popupControl;

        private LeftTab leftTab;
        private RightTab rightTab;
        private BottomTab bottomTab;
        
        private Button leftPanelButton, rightPanelButton, combatLogButton;
        private Button menuButton, mapButton;
        private Hotbar hotbar;

        public GameInterface()
        {
            healthBar = new BarTitle(this, 2, 1, "HP", 20);
            manaBar = new BarTitle(this, GraphicConsole.BufferWidth - 23, 1, "MP", 20);
            manaBar.BarColor = Color.DodgerBlue;
            manaBar.FillColor = Color.DarkBlue;

            playerTitle = new Title(this, "[Name] of [Town] the [Title] - Level ##", GraphicConsole.BufferWidth / 2, 1, Title.TextAlignModes.Center);

            popupControl = new Popup(this);

            leftPanelButton = new Button(this, "▓", 1, 48, 1, 1);
            leftPanelButton.Click += leftPanelButton_Pressed;

            rightPanelButton = new Button(this, "▓", 123, 48, 1, 1);
            rightPanelButton.Click += rightPanelButton_Pressed;

            combatLogButton = new Button(this, "<. .>", GraphicConsole.BufferWidth / 2 - 2, GraphicConsole.BufferHeight - 2, 5, 1);
            combatLogButton.Click += combatLogButton_Pressed;

            this.hotbar = new Hotbar(this);
            this.leftTab = new LeftTab(this);
            this.rightTab = new RightTab(this, hotbar);
            this.bottomTab = new BottomTab(this, this.leftTab, this.rightTab);

            this.menuButton = new Button(this, "Menu", 3, GraphicConsole.BufferHeight - 2, 6, 1);
            this.mapButton = new Button(this, "Map*", 10, GraphicConsole.BufferHeight - 2, 6, 1) { KeyShortcut = Keys.M };

            this.menuButton.Click += menuButton_Pressed;
            this.mapButton.Click += mapButton_Pressed;
        }

        #region Button Events
        void leftPanelButton_Pressed(object sender, MouseButtons button)
        {
            this.leftTab.IsVisible = !this.leftTab.IsVisible;
        }
        void rightPanelButton_Pressed(object sender, MouseButtons button)
        {
            rightTab.IsVisible = !this.rightTab.IsVisible;
        }
        void combatLogButton_Pressed(object sender, MouseButtons button)
        {
            if (button == MouseButtons.Left)
                this.bottomTab.IsVisible = !this.bottomTab.IsVisible;
            else if (button == MouseButtons.Right)
                MessageCenter.MessageLog.Clear();
        }
        void mapButton_Pressed(object sender, MouseButtons button)
        {
            GameManager.ChangeGameState(GameStates.Map);
        }
        void menuButton_Pressed(object sender, MouseButtons button)
        {
            GameManager.ChangeGameState(GameStates.Options);
        }
        #endregion

        public override void DrawStep()
        {
            GameManager.DrawGameWorld();

            this.drawInterfaceBars();

            base.DrawStep();
        }

        public override void Update(GameTime gameTime)
        {
            GameManager.CurrentLevel.Update(gameTime);

            //Right Tab
            if (InputManager.KeyWasReleased(Keys.I))
            {
                rightTab.IsVisible = true;
                rightTab.OpenMenu("Inventory");

                this.DrawStep();
            }
            else if (InputManager.KeyWasReleased(Keys.E))
            {
                rightTab.IsVisible = true;
                rightTab.OpenMenu("Equipment");

                this.DrawStep();
            }
            else if (InputManager.KeyWasReleased(Keys.B))
            {
                rightTab.IsVisible = true;
                rightTab.OpenMenu("Spellbook");

                this.DrawStep();
            }

            //Left Tab
            if (InputManager.KeyWasReleased(Keys.C))
            {
                leftTab.IsVisible = true;

                this.DrawStep();
            }
            else if (InputManager.KeyWasReleased(Keys.K))
            {
                leftTab.IsVisible = true;

                this.DrawStep();
            }

            //Bottom Tab
            if (InputManager.KeyWasReleased(Keys.Tab))
            {
                bottomTab.IsVisible = !bottomTab.IsVisible;
                this.DrawStep();
            }

            //Escape
            if (InputManager.KeyWasReleased(Keys.Escape))
            {
                if (leftTab.IsVisible || rightTab.IsVisible || bottomTab.IsVisible)
                {
                    leftTab.IsVisible = false;
                    rightTab.IsVisible = false;
                    bottomTab.IsVisible = false;
                }
                else
                    menuButton_Pressed(this, MouseButtons.Left);

                this.DrawStep();
            }

            base.Update(gameTime);
        }

        public override void UpdateStep()
        {
            healthBar.CurrentValue = GameManager.Player.StatsPackage.Health;
            healthBar.MaxValue = GameManager.Player.StatsPackage.MaxHealth;

            manaBar.CurrentValue = GameManager.Player.StatsPackage.Mana;
            manaBar.MaxValue = GameManager.Player.StatsPackage.MaxMana;

            playerTitle.Text = GameManager.Player.StatsPackage.GetFormattedName();

            base.UpdateStep();
        }

        public override void OnCall()
        {
            GameManager.DrawStep();

            base.OnCall();
        }

        private void drawInterfaceBars()
        {
            GraphicConsole.ResetColor();

            //Header Bar
            DrawingUtilities.DrawLine(1, 1, GraphicConsole.BufferWidth - 2, 1, ' ');
            DrawingUtilities.DrawLine(0, 0, GraphicConsole.BufferWidth, 0, '═');
            DrawingUtilities.DrawLine(0, 2, GraphicConsole.BufferWidth, 2, '═');

            //Left Bar
            DrawingUtilities.DrawLine(0, 1, 0, GraphicConsole.BufferHeight - 2, '│');

            //Right Bar
            DrawingUtilities.DrawLine(GraphicConsole.BufferWidth, 1, GraphicConsole.BufferWidth, GraphicConsole.BufferHeight - 2, '│');

            //Bottom Bar
            DrawingUtilities.DrawLine(1, GraphicConsole.BufferHeight - 2, GraphicConsole.BufferWidth - 2, GraphicConsole.BufferHeight - 2, ' ');
            DrawingUtilities.DrawLine(0, GraphicConsole.BufferHeight, GraphicConsole.BufferWidth, GraphicConsole.BufferHeight, '─');
            DrawingUtilities.DrawLine(0, GraphicConsole.BufferHeight - 3, GraphicConsole.BufferWidth, GraphicConsole.BufferHeight - 3, '─');

            //Bottom Left Corner
            GraphicConsole.Put('├', 0, GraphicConsole.BufferHeight - 3);
            GraphicConsole.Put('└', 0, GraphicConsole.BufferHeight);

            //Bottom Right Corner
            GraphicConsole.Put('┤', GraphicConsole.BufferWidth, GraphicConsole.BufferHeight - 3);
            GraphicConsole.Put('┘', GraphicConsole.BufferWidth, GraphicConsole.BufferHeight);

            //Top Left Corner
            GraphicConsole.Put('╒', 0, 0);
            GraphicConsole.Put('╞', 0, 2);

            //Top Right Corner
            GraphicConsole.Put('╕', GraphicConsole.BufferWidth, 0);
            GraphicConsole.Put('╡', GraphicConsole.BufferWidth, 2);
        }
    }
    public class BarTitle : Control
    {
        public BarTitle(Interface parent, int x, int y, string textRoot, int width)
            : base(parent)
        {
            this.text = textRoot;
            this.formattedText = this.text + ": ##/##";

            this.position = new Point(x, y);
            this.size = new Point(width, 1);
        }

        public override void DrawStep()
        {
            GraphicConsole.SetColors(Color.Transparent, this.fillColor);
            DrawingUtilities.DrawLine(this.Position.X, this.Position.Y, this.Position.X + this.Size.X, this.Position.Y, ' ');

            if (percent != 0)
            {
                int barLength = (int)(percent * this.Size.X);
                GraphicConsole.SetColors(Color.Transparent, this.barColor);
                DrawingUtilities.DrawLine(this.Position.X, this.Position.Y, this.Position.X + barLength, this.Position.Y, ' ');
            }

            GraphicConsole.SetCursor(this.Position.X + (this.Size.X / 2 - this.formattedText.Length / 2), this.Position.Y);
            for (int i = 0; i < formattedText.Length; i++)
            {
                GraphicConsole.SetColors(this.textColor, GraphicConsole.GetColorsAtTile(GraphicConsole.CursorLeft, GraphicConsole.CursorTop)[1]);
                GraphicConsole.Write(this.formattedText[i]);
            }

            base.DrawStep();
        }

        public override void UpdateStep()
        {
            percent = CurrentValue / MaxValue;
            formattedText = this.text + ": " + this.currentValue + @"/" + this.maxValue;

            base.UpdateStep();
        }

        private double currentValue = 50;
        private double maxValue = 100;

        private string text;
        private string formattedText;
        private double percent;

        private Color barColor = Color.Red;
        private Color fillColor = Color.DarkRed;
        private Color textColor = Color.Black;

        public double CurrentValue { get { return this.currentValue; } set { this.currentValue = value; } }
        public double MaxValue { get { return this.maxValue; } set { this.maxValue = value; } }
        public string Text { get { return this.text; } set { this.text = value; this.UpdateStep(); this.DrawStep(); } }
        public double Percent { get { return this.percent; } }
        public Color BarColor { get { return this.barColor; } set { this.barColor = value; } }
        public Color FillColor { get { return this.fillColor; } set { this.fillColor = value; } }
        public Color TextColor { get { return this.textColor; } set { this.textColor = value; } }
    }

    public class LeftTab : Control
    {
        private TextBox leftTabInformation, effectDescriptions;
        private ScrollingList effectList;
        private ToggleButton baseStats, effectiveStats, currentEffects;
        private InformationModes infoMode = InformationModes.BaseStats;
        private ToggleButtonGroup buttonGroup;

        public LeftTab(Interface parent)
            : base(parent)
        {
            this.Position = new Point(1, 3);
            this.Size = new Point(28, GraphicConsole.BufferHeight - 6);

            this.leftTabInformation = new TextBox(this, this.Position.X - 1, this.Position.Y - 3, this.Size.X - 1, this.Size.Y);
            this.leftTabInformation.Text = "Information";

            this.effectList = new ScrollingList(this, this.Position.X - 1, this.Position.Y + 2, this.Size.X, 16);
            this.effectList.Selected += effectList_Selected;
            this.effectDescriptions = new TextBox(this, this.Position.X - 1, this.Position.Y + 19, this.Size.X, 22);

            this.effectList.IsVisible = false;
            this.effectDescriptions.IsVisible = false;

            this.buttonGroup = new ToggleButtonGroup(this);

            this.baseStats = new ToggleButton(this, "⌂", this.Size.X - 1, this.Position.Y - 3, 1, 1);
            this.effectiveStats = new ToggleButton(this, "§", this.Size.X - 1, this.Position.Y - 2, 1, 1);
            this.currentEffects = new ToggleButton(this, "○", this.Size.X - 1, this.Position.Y - 1, 1, 1);
            this.baseStats.Enabled = true;

            this.buttonGroup.AddButton(this.baseStats);
            this.buttonGroup.AddButton(this.effectiveStats);
            this.buttonGroup.AddButton(this.currentEffects);

            this.baseStats.Click += baseStats_Pressed;
            this.effectiveStats.Click += effectiveStats_Pressed;
            this.currentEffects.Click += currentEffects_Pressed;

            this.IsVisible = false;
        }

        void effectList_Selected(object sender, int index)
        {
            this.effectDescriptions.Text = ((Game.Combat.Effect)(this.effectList.Items[index])).EffectDescription;
        }

        public void OpenMenu(string menu)
        {
            if (menu == "Character")
            {
                baseStats.Enabled = true;
                effectiveStats.Enabled = false;
                currentEffects.Enabled = false;

                baseStats_Pressed(this);
            }
            else if (menu == "Stats")
            {
                baseStats.Enabled = false;
                effectiveStats.Enabled = true;
                currentEffects.Enabled = false;

                effectiveStats_Pressed(this);
            }
            else if (menu == "Effects")
            {
                baseStats.Enabled = false;
                effectiveStats.Enabled = false;
                currentEffects.Enabled = true;

                currentEffects_Pressed(this);
            }

            this.parent.UpdateStep();
            this.parent.DrawStep();
        }

        void baseStats_Pressed(object sender)
        {
            this.infoMode = InformationModes.BaseStats;

            this.leftTabInformation.IsVisible = true;
            this.effectList.IsVisible = false;
            this.effectDescriptions.IsVisible = false;
        }
        void effectiveStats_Pressed(object sender)
        {
            this.infoMode = InformationModes.EffectiveStats;

            this.leftTabInformation.IsVisible = true;
            this.effectList.IsVisible = false;
            this.effectDescriptions.IsVisible = false;
        }
        void currentEffects_Pressed(object sender)
        {
            this.infoMode = InformationModes.CurrentEffects;

            this.leftTabInformation.IsVisible = true;
            this.effectList.IsVisible = true;
            this.effectDescriptions.IsVisible = true;
        }

        public override void DrawStep()
        {
            DrawingUtilities.DrawRect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

            DrawingUtilities.DrawLine(this.Size.X + 1, this.Position.Y, this.Size.X + 1, this.Size.Y + 2, '│');
            GraphicConsole.Put('╤', this.Size.X + 1, this.Position.Y - 1);
            GraphicConsole.Put('┴', this.Size.X + 1, this.Size.Y + 3);

            base.DrawStep();
        }

        public override void UpdateStep()
        {
            this.leftTabInformation.Text =
                    GameManager.Player.PlayerStats.Name + "<br>" + //Name
                        "Level: 1<br><br>";

            if (this.infoMode == InformationModes.BaseStats)
            {
                this.leftTabInformation.Text +=
                    "<color red>STR: " + GameManager.Player.PlayerStats.Strength + "<br>" +
                    "<color orange>AGI: " + GameManager.Player.PlayerStats.Agility + "<br>" +
                    "<color yellow>DEX: " + GameManager.Player.PlayerStats.Dexterity + "<br><br>" +

                    "<color blue>INT: " + GameManager.Player.PlayerStats.Intelligence + "<br>" +
                    "<color purple>WIL: " + GameManager.Player.PlayerStats.Willpower + "<br>" +
                    "<color cyan>WIS: " + GameManager.Player.PlayerStats.Wisdom + "<br><br>" +

                    "<color green>CON: " + GameManager.Player.PlayerStats.Constitution + "<br>" +
                    "<color gray>END: " + GameManager.Player.PlayerStats.Endurance + "<br>" +
                    "<color pink>FRT: " + GameManager.Player.PlayerStats.Fortitude;
            }
            else if (this.infoMode == InformationModes.EffectiveStats)
            {
                this.leftTabInformation.Text +=
                    "---Physical Stats---" + "<br><br>" +
                    "Attack Power: " + GameManager.Player.PlayerStats.AttackPower + "<br>" +
                    "       Haste: " + GameManager.Player.PlayerStats.PhysicalHaste + "%<br>" +
                    "  Hit Chance: " + GameManager.Player.PlayerStats.PhysicalHitChance + "%<br>" +
                    " Crit Chance: " + GameManager.Player.PlayerStats.PhysicalCritChance + "%<br>" +
                    "  Crit Power: " + GameManager.Player.PlayerStats.PhysicalCritPower + "<br>" +
                    "   Reduction: " + GameManager.Player.PlayerStats.PhysicalReduction + "%<br>" +
                    "  Reflection: " + GameManager.Player.PlayerStats.PhysicalReflection + "%<br>" +
                    "   Avoidance: " + GameManager.Player.PlayerStats.PhysicalAvoidance + "%<br><br><br>" +

                    "---Magical Stats---" + "<br><br>" +
                    "Spell Power: " + GameManager.Player.PlayerStats.SpellPower + "<br>" +
                    "      Haste: " + GameManager.Player.PlayerStats.SpellHaste + "%<br>" +
                    " Hit Chance: " + GameManager.Player.PlayerStats.SpellHitChance + "%<br>" +
                    "Crit Chance: " + GameManager.Player.PlayerStats.SpellCritChance + "%<br>" +
                    " Crit Power: " + GameManager.Player.PlayerStats.SpellCritPower + "<br>" +
                    "  Reduction: " + GameManager.Player.PlayerStats.SpellReduction + "%<br>" +
                    " Reflection: " + GameManager.Player.PlayerStats.SpellReflection + "%<br>" +
                    "  Avoidance: " + GameManager.Player.PlayerStats.SpellAvoidance + "%";
            }
            else if (this.infoMode == InformationModes.CurrentEffects)
            {
                this.leftTabInformation.Text +=
                    "---Current Effects---" + "<br>";

                this.effectList.SetList(GameManager.Player.StatsPackage.AppliedEffects);
            }

            base.UpdateStep();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private enum InformationModes { BaseStats, EffectiveStats, CurrentEffects }
    }
    public class RightTab : Control
    {
        private ToggleButton inventoryButton, equipmentButton, spellbookButton;
        private ToggleButtonGroup buttonGroup;

        private InventoryControl inventoryControl;
        private EquipmentControl equipmentControl;
        private SpellbookControl spellbookControl;

        public RightTab(Interface parent, Hotbar hotbar)
            : base(parent)
        {
            this.Size = new Point(28, GraphicConsole.BufferHeight - 6);
            this.Position = new Point(GraphicConsole.BufferWidth - this.Size.X - 1, 3);

            this.inventoryButton = new ToggleButton(this, "⌂", -2, 0, 1, 1) { Enabled = true };
            this.equipmentButton = new ToggleButton(this, "♦", -2, 1, 1, 1) { Enabled = false };
            this.spellbookButton = new ToggleButton(this, "♠", -2, 2, 1, 1) { Enabled = false };

            this.buttonGroup = new ToggleButtonGroup(this);
            this.buttonGroup.AddButton(this.inventoryButton);
            this.buttonGroup.AddButton(this.equipmentButton);
            this.buttonGroup.AddButton(this.spellbookButton);

            this.inventoryButton.Click += inventoryButton_Click;
            this.equipmentButton.Click += equipmentButton_Click;
            this.spellbookButton.Click += spellbookButton_Click;

            this.inventoryControl = new InventoryControl(this, 0, 0, 28, 44, hotbar) { IsVisible = true };
            this.equipmentControl = new EquipmentControl(this, 0, 0, 28, 44) { IsVisible = false };
            this.spellbookControl = new SpellbookControl(this, 0, 0, 28, 44, hotbar) { IsVisible = false };

            this.IsVisible = false;
        }
        public override void DrawStep()
        {
            DrawingUtilities.DrawRect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

            //Left Border
            DrawingUtilities.DrawLine(this.Position.X - 1, this.Position.Y, this.Position.X - 1, this.Size.Y + 2, '│');
            GraphicConsole.Put('╤', this.Position.X - 1, this.Position.Y - 1);
            GraphicConsole.Put('┴', this.Position.X - 1, this.Size.Y + 3);

            //Border Around Toggle Switches
            DrawingUtilities.DrawLine(this.Position.X - 3, this.Position.Y, this.Position.X - 3, this.Position.Y + 4, '│');
            GraphicConsole.Put('╤', this.Position.X - 3, this.Position.Y - 1);
            GraphicConsole.Put('└', this.Position.X - 3, this.Position.Y + 4);
            GraphicConsole.Put('─', this.Position.X - 2, this.Position.Y + 4);
            GraphicConsole.Put('┤', this.Position.X - 1, this.Position.Y + 4);

            base.DrawStep();
        }

        public void OpenMenu(string menu)
        {
            if (menu == "Inventory")
            {
                inventoryButton.Enabled = true;
                equipmentButton.Enabled = false;
                spellbookButton.Enabled = false;

                inventoryButton_Click(this);
            }
            else if (menu == "Equipment")
            {
                inventoryButton.Enabled = false;
                equipmentButton.Enabled = true;
                spellbookButton.Enabled = false;

                equipmentButton_Click(this);
            }
            else if (menu == "Spellbook")
            {
                inventoryButton.Enabled = false;
                equipmentButton.Enabled = false;
                spellbookButton.Enabled = true;

                spellbookButton_Click(this);
            }

            this.parent.UpdateStep();
            this.parent.DrawStep();
        }

        void inventoryButton_Click(object sender)
        {
            this.inventoryControl.IsVisible = true;
            this.equipmentControl.IsVisible = false;
            this.spellbookControl.IsVisible = false;
        }
        void equipmentButton_Click(object sender)
        {
            this.inventoryControl.IsVisible = false;
            this.equipmentControl.IsVisible = true;
            this.spellbookControl.IsVisible = false;
        }
        void spellbookButton_Click(object sender)
        {
            this.inventoryControl.IsVisible = false;
            this.equipmentControl.IsVisible = false;
            this.spellbookControl.IsVisible = true;
        }
    }
    public class BottomTab : Control
    {
        private ScrollingList messageList;
        private TextBox messageInformation;

        private LeftTab leftTab;
        private RightTab rightTab;
        private Button contextButton; //Examine, Back button
        private bool detailedMode = false;

        public BottomTab(Control parent, LeftTab leftTab, RightTab rightTab)
            : base(parent)
        {
            this.Position = new Point(30, 40);

            this.messageList = new ScrollingList(this, 1, 0, 63, 7);
            this.messageInformation = new TextBox(this, 1, 0, 64, 7) { IsVisible = false };
            this.contextButton = new Button(this, "►", 0, 0, 1, 1);
            this.contextButton.Click += contextButton_Click;

            this.leftTab = leftTab;
            this.rightTab = rightTab;

            this.IsVisible = false;
        }

        public override void DrawStep()
        {
            this.drawBorders();

            base.DrawStep();
        }
        public override void UpdateStep()
        {
            this.messageList.SetList<MessageCenter.Message>(MessageCenter.MessageLog);

            base.UpdateStep();
        }

        void contextButton_Click(object sender, MouseButtons button)
        {
            detailedMode = !detailedMode;

            if (detailedMode)
            {
                this.messageInformation.IsVisible = true;
                this.messageList.IsVisible = false;

                if (this.messageList.HasSelection)
                    this.messageInformation.Text = ((MessageCenter.Message)this.messageList.GetSelection()).DetailedMessage;
                this.contextButton.Text = "◄";
            }
            else
            {
                this.messageInformation.IsVisible = false;
                this.messageList.IsVisible = true;

                this.contextButton.Text = "►";
            }
        }
        private void drawBorders()
        {
            int x, y;
            int panelWidth, panelHeight;

            x = 30;
            y = GraphicConsole.BufferHeight - 10;
            panelWidth = 94;
            panelHeight = 7;

            DrawingUtilities.DrawLine(30, y - 1, 94, y - 1, '─');
            DrawingUtilities.DrawRect(x, y, panelWidth - x + 1, panelHeight, ' ', true);

            if (this.leftTab.IsVisible)
                GraphicConsole.Put('├', x - 1, y - 1);
            else
            {
                DrawingUtilities.DrawLine(x - 1, y, x - 1, GraphicConsole.BufferHeight - 4, '│');
                GraphicConsole.Put('┌', x - 1, y - 1);
                GraphicConsole.Put('┴', x - 1, GraphicConsole.BufferHeight - 3);
            }

            if (this.rightTab.IsVisible)
                GraphicConsole.Put('┤', panelWidth + 1, y - 1);
            else
            {
                DrawingUtilities.DrawLine(panelWidth + 1, y, panelWidth + 1, GraphicConsole.BufferHeight - 4, '│');
                GraphicConsole.Put('┐', panelWidth + 1, y - 1);
                GraphicConsole.Put('┴', panelWidth + 1, GraphicConsole.BufferHeight - 3);
            }
        }
    }

    public class Hotbar : Control
    {
        private Button button1, button2, button3, button4;
        private Button buttonS1, buttonS2, buttonS3, buttonS4;

        public GameCursor GameCursor;
        private Ability castedAbility;

        private Popup popupControl;

        public Hotbar(Interface parent)
            : base(parent)
        {
            this.button1 = new Button(this, "[ ABIL 1 ]", 70, 48, 10, 1) { KeyShortcut = Keys.D1 };
            this.button2 = new Button(this, "[ ABIL 2 ]", 83, 48, 10, 1) { KeyShortcut = Keys.D2 };
            this.button3 = new Button(this, "[ ABIL 3 ]", 96, 48, 10, 1) { KeyShortcut = Keys.D3 };
            this.button4 = new Button(this, "[ ABIL 4 ]", 109, 48, 10, 1) { KeyShortcut = Keys.D4 };

            this.buttonS1 = new Button(this, "[ ABIL 5 ]", 70, 48, 10, 1) { IsVisible = false, KeyShortcut = Keys.D1 };
            this.buttonS2 = new Button(this, "[ ABIL 6 ]", 83, 48, 10, 1) { IsVisible = false, KeyShortcut = Keys.D2 };
            this.buttonS3 = new Button(this, "[ ABIL 7 ]", 96, 48, 10, 1) { IsVisible = false, KeyShortcut = Keys.D3 };
            this.buttonS4 = new Button(this, "[ ABIL 8 ]", 109, 48, 10, 1) { IsVisible = false, KeyShortcut = Keys.D4 };

            this.button1.Click += button1_Pressed;
            this.button2.Click += button2_Pressed;
            this.button3.Click += button3_Pressed;
            this.button4.Click += button4_Pressed;

            this.buttonS1.Click += buttonS1_Pressed;
            this.buttonS2.Click += buttonS2_Pressed;
            this.buttonS3.Click += buttonS3_Pressed;
            this.buttonS4.Click += buttonS4_Pressed;

            this.GameCursor = new GameCursor(parent, GameManager.Viewport);
            this.GameCursor.AimingMode = GameCursor.AimingModes.Off;
            this.GameCursor.Selected += gameCursor_Selected;

            this.popupControl = new Popup(this);

            this.setupButtonNames();
        }

        #region Row One
        void button1_Pressed(object sender, MouseButtons button)
        {
            if (SpellBook.HotSlotOne != null)
                CastAbility(SpellBook.HotSlotOne);
        }
        void button2_Pressed(object sender, MouseButtons button)
        {
            if (SpellBook.HotSlotTwo != null)
                CastAbility(SpellBook.HotSlotTwo);
        }
        void button3_Pressed(object sender, MouseButtons button)
        {
            if (SpellBook.HotSlotThree != null)
                CastAbility(SpellBook.HotSlotThree);
        }
        void button4_Pressed(object sender, MouseButtons button)
        {
            if (SpellBook.HotSlotFour != null)
                CastAbility(SpellBook.HotSlotFour);
        }
        #endregion
        #region Row Two
        void buttonS1_Pressed(object sender, MouseButtons button)
        {
            if (SpellBook.HotSlotFive != null)
                CastAbility(SpellBook.HotSlotFive);
        }
        void buttonS2_Pressed(object sender, MouseButtons button)
        {
            if (SpellBook.HotSlotSix != null)
                CastAbility(SpellBook.HotSlotSix);
        }
        void buttonS3_Pressed(object sender, MouseButtons button)
        {
            if (SpellBook.HotSlotSeven != null)
                CastAbility(SpellBook.HotSlotSeven);
        }
        void buttonS4_Pressed(object sender, MouseButtons button)
        {
            if (SpellBook.HotSlotEight != null)
                CastAbility(SpellBook.HotSlotEight);
        }
        #endregion

        public override void UpdateStep()
        {
            this.setupButtonNames();

            base.UpdateStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (InputManager.KeyWasPressed(Keys.LeftShift))
                this.swapHotBar();

            base.Update(gameTime);
        }

        private void setupButtonNames()
        {
            if (SpellBook.HotSlotOne != null)
                button1.Text = "[" + SpellBook.HotSlotOne.AbilityNameShort + "]";
            else
                button1.Text = "[   ..   ]";

            if (SpellBook.HotSlotTwo != null)
                button2.Text = "[" + SpellBook.HotSlotTwo.AbilityNameShort + "]";
            else
                button2.Text = "[   ..   ]";

            if (SpellBook.HotSlotThree != null)
                button3.Text = "[" + SpellBook.HotSlotThree.AbilityNameShort + "]";
            else
                button3.Text = "[   ..   ]";

            if (SpellBook.HotSlotFour != null)
                button4.Text = "[" + SpellBook.HotSlotFour.AbilityNameShort + "]";
            else
                button4.Text = "[   ..   ]";


            if (SpellBook.HotSlotFive != null)
                buttonS1.Text = "[" + SpellBook.HotSlotFive.AbilityNameShort + "]";
            else
                buttonS1.Text = "[   ..   ]";

            if (SpellBook.HotSlotSix != null)
                buttonS2.Text = "[" + SpellBook.HotSlotSix.AbilityNameShort + "]";
            else
                buttonS2.Text = "[   ..   ]";

            if (SpellBook.HotSlotSeven != null)
                buttonS3.Text = "[" + SpellBook.HotSlotSeven.AbilityNameShort + "]";
            else
                buttonS3.Text = "[   ..   ]";

            if (SpellBook.HotSlotEight != null)
                buttonS4.Text = "[" + SpellBook.HotSlotEight.AbilityNameShort + "]";
            else
                buttonS4.Text = "[   ..   ]";
        }
        public void CastAbility(Ability ability)
        {
            this.castedAbility = ability;

            if (ability.Range != 0)
                GameCursor.Range = ability.Range;
            else
                GameCursor.Range = 100;

            if (this.castedAbility.TargetingType == TargetingTypes.EntityTarget)
            {
                GameCursor.Enable(GameCursor.AimingModes.Point, castedAbility.Range);
            }
            else if (this.castedAbility.TargetingType == TargetingTypes.GroundTarget)
            {
                GameCursor.Enable(GameCursor.AimingModes.Point, castedAbility.Range);
            }
            else if (this.castedAbility.TargetingType == TargetingTypes.Self)
            {
                CombatManager.PerformAbility(GameManager.Player.StatsPackage, GameManager.Player.StatsPackage, castedAbility);
                GameManager.Step();
            }
        }
        private void swapHotBar()
        {
            this.button1.IsVisible = !this.button1.IsVisible;
            this.button2.IsVisible = !this.button2.IsVisible;
            this.button3.IsVisible = !this.button3.IsVisible;
            this.button4.IsVisible = !this.button4.IsVisible;

            this.buttonS1.IsVisible = !this.buttonS1.IsVisible;
            this.buttonS2.IsVisible = !this.buttonS2.IsVisible;
            this.buttonS3.IsVisible = !this.buttonS3.IsVisible;
            this.buttonS4.IsVisible = !this.buttonS4.IsVisible;

            InterfaceManager.UpdateStep();
            InterfaceManager.DrawStep();
        }

        void gameCursor_Selected(object sender, EventArgs args)
        {
            //TODO: Add prompt when ability is not in range

            if (!castedAbility.CanAffordAbility(GameManager.Player.StatsPackage))
                popupControl.DisplayMessage("You cannot afford to cast that ability.");
            else if (!castedAbility.IsOffCooldown())
                popupControl.DisplayMessage(castedAbility.AbilityName + " is still on cooldown.");
            else if (castedAbility.TargetingType == TargetingTypes.EntityTarget)
            {
                Entity target = GameManager.CurrentLevel.GetEntity(GameCursor.Point0.X, GameCursor.Point0.Y);
                if (target != null)
                {
                    if (!castedAbility.IsLineOfSight(GameManager.Player.StatsPackage, new Point(target.X, target.Y)))
                        popupControl.DisplayMessage(target.StatsPackage.UnitName + " is not in your line of sight.");
                    else
                    {
                        target.Attack(GameManager.Player, castedAbility);
                        GameManager.Step();
                    }
                }
                else
                    popupControl.DisplayMessage(castedAbility.AbilityName + " requires a target.");
            }
            else if (castedAbility.TargetingType == TargetingTypes.GroundTarget)
            {
                if (!castedAbility.IsLineOfSight(GameManager.Player.StatsPackage, new Point(GameCursor.Point0.X, GameCursor.Point0.Y)))
                    popupControl.DisplayMessage("That is not in your line of sight.");
                else
                {
                    castedAbility.CastAbilityGround(GameManager.Player.StatsPackage, GameCursor.Point0.X, GameCursor.Point0.Y, 0, GameManager.CurrentLevel);
                    GameManager.Step();
                }
            }
        }
    }
    public class GameCursor : Control
    {
        private Point point0, point1;
        private AimingModes aimingMode;

        private Color foregroundColorCursor = Color.Goldenrod;
        private Color backgroundColorCursor = Color.Black;
        private Color foregroundColorFill = Color.Crimson;
        private Color backgroundColorFill = Color.Black;

        private const char CURSOR = '#';
        private const char FILL_CURSOR = '•';

        private Rectangle viewport;
        private Rectangle viewportCoords;
        private double delay = 0.0;
        private double maxDelay = 100.0;
        private int range = -1;
        private bool isInRange = false;

        public event Selection Selected;
        public delegate void Selection(object sender, EventArgs args);

        public GameCursor(Control parent, Rectangle viewport)
            : base(parent)
        {
            this.viewport = new Rectangle(
                viewport.X * GraphicConsole.CHAR_WIDTH,
                viewport.Y * GraphicConsole.CHAR_HEIGHT,
                viewport.Width * GraphicConsole.CHAR_WIDTH,
                viewport.Height * GraphicConsole.CHAR_HEIGHT);

            this.viewportCoords = viewport;
        }

        public override void DrawStep()
        {
            if (this.aimingMode != AimingModes.Off)
            {
                if (this.range != -1)
                {
                    GameManager.CurrentLevel.DrawCircle(MatrixLevels.Effect, new Circle() { X = GameManager.Player.X, Y = GameManager.Player.Y, Radius = range }, FILL_CURSOR, foregroundColorFill, backgroundColorFill, true);
                }

                if (aimingMode == AimingModes.Point)
                {
                    if (this.isInRange)
                    {
                        GraphicConsole.SetColors(foregroundColorCursor, backgroundColorCursor);
                        GraphicConsole.Put(CURSOR, point0.X, point0.Y);
                    }
                    else
                    {
                        GraphicConsole.SetColors(Color.Red, backgroundColorCursor);
                        GraphicConsole.Put('X', point0.X, point0.Y);
                    }
                }
                else if (aimingMode == AimingModes.Line)
                {
                    /*GraphicConsole.SetColors(foregroundColorFill, backgroundColorFill);
                    DrawingUtilities.DrawLine(point0.X, point0.Y, point1.X, point1.Y, FILL_CURSOR);*/
                    GameManager.CurrentLevel.DrawLine(MatrixLevels.Effect, DrawingUtilities.GetWorldPositionFromScreen(point0), DrawingUtilities.GetWorldPositionFromScreen(point1), FILL_CURSOR);

                    GraphicConsole.SetColors(foregroundColorCursor, backgroundColorCursor);
                    GraphicConsole.Put(CURSOR, point0.X, point0.Y);
                }
                else if (aimingMode == AimingModes.DrawLine)
                {
                    if (point1 == new Point(-1, -1))
                    {
                        GraphicConsole.SetColors(foregroundColorCursor, backgroundColorCursor);
                        GraphicConsole.Put(CURSOR, point0.X, point0.Y);
                    }
                    else
                    {
                        GraphicConsole.SetColors(foregroundColorFill, backgroundColorFill);
                        DrawingUtilities.DrawLine(point0.X, point0.Y, point1.X, point1.Y, FILL_CURSOR);

                        GraphicConsole.SetColors(foregroundColorCursor, backgroundColorCursor);
                        GraphicConsole.Put(CURSOR, point0.X, point0.Y);
                        GraphicConsole.Put(CURSOR, point1.X, point1.Y);
                    }
                }
            }

            base.DrawStep();
        }
        public override void UpdateStep()
        {
            base.UpdateStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (aimingMode != AimingModes.Off)
            {
                if (delay >= maxDelay)
                {
                    Point mouse = InputManager.GetCurrentMousePosition();

                    if (this.viewport.Contains(mouse))
                    {
                        mouse.X = mouse.X / GraphicConsole.CHAR_WIDTH;
                        mouse.Y = mouse.Y / GraphicConsole.CHAR_HEIGHT;

                        if (aimingMode == AimingModes.Point)
                        {
                            #region PointAiming
                            if (mouse != point0)
                            {
                                Point playerPosition = new Point(GameManager.Player.X, GameManager.Player.Y);
                                Point cursor = DrawingUtilities.GetWorldPositionFromScreen(mouse);

                                float distance = Vector2.Distance(new Vector2(playerPosition.X, playerPosition.Y), new Vector2(cursor.X, cursor.Y));
                                if (distance <= this.range)
                                    this.isInRange = true;
                                else
                                    this.isInRange = false;

                                point0 = mouse;
                                InterfaceManager.DrawStep();
                            }

                            if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
                            {
                                this.OnSelect(null);
                            }
                            #endregion
                        }
                        else if (aimingMode == AimingModes.Line)
                        {
                            #region LineAiming
                            if (mouse != point0)
                            {
                                Point1 = new Point(GameManager.Player.X, GameManager.Player.Y);
                                if (Vector2.Distance(new Vector2(point1.X, point1.Y), new Vector2(mouse.X, mouse.Y)) <= this.range)
                                {
                                    point0 = mouse;
                                }
                                InterfaceManager.DrawStep();
                            }

                            if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
                            {
                                this.OnSelect(null);
                            }
                            #endregion
                        }
                        else if (aimingMode == AimingModes.DrawLine)
                        {
                            #region DrawLineAiming
                            if (point1 == new Point(-1, -1))
                            {
                                if (mouse != point0)
                                {
                                    point0 = mouse;
                                    InterfaceManager.DrawStep();
                                }

                                if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
                                    point1 = point0;
                            }
                            else
                            {
                                if (mouse != point0)
                                {
                                    point0 = mouse;
                                    InterfaceManager.DrawStep();
                                }

                                if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
                                    this.OnSelect(null);
                            }
                            #endregion
                        }

                        if (InputManager.MouseButtonWasClicked(MouseButtons.Right))
                        {
                            this.aimingMode = AimingModes.Off;
                            GameManager.CurrentLevel.ClearLayer(MatrixLevels.Effect);

                            InterfaceManager.DrawStep();
                        }
                    }

                    delay = maxDelay;
                }

                delay += gameTime.ElapsedGameTime.Milliseconds;
            }
            else
                delay = 0.0;

            base.Update(gameTime);
        }

        public void OnSelect(EventArgs args)
        {
            if (this.Selected != null)
                this.Selected(AimingMode, null);

            AimingMode = AimingModes.Off;
            GameManager.CurrentLevel.ClearLayer(MatrixLevels.Effect);

            InterfaceManager.DrawStep();
        }
        public void Enable(AimingModes mode, int range)
        {
            point0 = Point.Zero;
            this.range = range;

            if (mode == AimingModes.DrawLine)
            {
                point1 = new Point(-1, -1);
            }

            this.aimingMode = mode;
            delay = 0.0;
        }

        public AimingModes AimingMode { get { return this.aimingMode; } set { this.aimingMode = value; } }
        public Point Point0 
        {
            get { return DrawingUtilities.GetWorldPositionFromScreen(point0); }
            set { this.point0 = DrawingUtilities.GetScreenPositionFromWorld(point0); }
        }
        public Point Point1 
        {
            get { return new        Point(point1.X + GameManager.CameraOffset.X - viewport.X, point1.Y + GameManager.CameraOffset.Y - viewport.Y); }
            set { this.point1 = new Point( (value.X - GameManager.CameraOffset.X) + viewport.X,  (value.Y - GameManager.CameraOffset.Y) + viewport.Y); }
        }
        public int Range { get { return this.range; } set { this.range = value; } }

        public enum AimingModes { Off, Point, Line, DrawLine, Path }
    }

    //Right Tab Controls
    public class InventoryControl : Control
    {
        private Title inventoryTitle;
        private Title groundTitle;
        private Title goldTracker;

        private ScrollingList inventoryList;
        private ScrollingList groundList;
        private TextBox descriptionBox;

        private Button pickupButton, useButton;
        private Button dropButton;

        private Hotbar hotbar;

        public InventoryControl(Control parent, int x, int y, int width, int height, Hotbar hotbar)
            : base(parent)
        {
            this.position = new Point(x, y);
            this.size = new Point(width, height);
            this.hotbar = hotbar;

            this.inventoryTitle = new Title(this, "-=Inventory=-", width / 2, 0, Title.TextAlignModes.Center);
            this.groundTitle = new Title(this, "-=Ground Items=-", width / 2, 19, Title.TextAlignModes.Center);
            this.goldTracker = new Title(this, "Gold: ###", 0, 1, Title.TextAlignModes.Left) { TextColor = Color.Goldenrod, FillColor = new Color(20, 20, 20) };

            this.inventoryList = new ScrollingList(this, 0, 2, this.Size.X, 14) { FillColor = new Color(20, 20, 20) };
            this.groundList = new ScrollingList(this, 0, 20, this.Size.X, 8) { FillColor = new Color(20, 20, 20) };

            this.useButton = new Button(this, "Use", 0, 16, 14, 3);
            this.dropButton = new Button(this, "Drop", 14, 16, 14, 3);
            this.pickupButton = new Button(this, "Pickup", 0, 28, 28, 3);

            this.descriptionBox = new TextBox(this, 0, 31, this.Size.X, 13) { FillColor = new Color(20, 20, 20) };

            this.inventoryList.Selected += inventoryList_Selected;
            this.groundList.Selected += groundList_Selected;

            this.useButton.Click += useButton_Click;
            this.dropButton.Click += dropButton_Click;
            this.pickupButton.Click += pickupButton_Click;
        }
        public override void UpdateStep()
        {
            this.populateInventoryList();
            this.populateGroundList();

            this.goldTracker.Text = "Gold: " + Inventory.Gold.ToString();

            base.UpdateStep();
        }
        public override void DrawStep()
        {
            GraphicConsole.SetColors(Color.White, new Color(20, 20, 20));
            DrawingUtilities.DrawLine(this.Position.X, this.Position.Y + 1, this.Position.X + this.Size.X - 1, this.Position.Y + 1, ' ');
            GraphicConsole.ResetColor();

            base.DrawStep();
        }

        private void populateInventoryList()
        {
            int index = this.inventoryList.SelectedIndex;
            this.inventoryList.SetList(Inventory.PlayerInventory);

            if (this.inventoryList.Items.Count > index)
                this.inventoryList.SetSelection(index);
        }
        private void populateGroundList()
        {
            List<Item> groundItems = new List<Item>();
            for (int i = 0; i < GameManager.CurrentLevel.FloorItems.Count; i++)
            {
                if (GameManager.CurrentLevel.FloorItems[i].Position.X == GameManager.Player.X && GameManager.CurrentLevel.FloorItems[i].Position.Y == GameManager.Player.Y)
                {
                    groundItems.Add(GameManager.CurrentLevel.FloorItems[i]);
                }
            }
            this.groundList.SetList<Item>(groundItems);
        }

        private void inventoryList_Selected(object sender, int index)
        {
            if (this.inventoryList.HasSelection)
            {
                this.descriptionBox.Text = ((Item)this.inventoryList.GetSelection()).GetDescription();
            }
        }
        private void groundList_Selected(object sender, int index)
        {
            if (this.groundList.HasSelection)
                this.descriptionBox.Text = ((Item)this.groundList.GetSelection()).GetDescription();
        }

        private void useButton_Click(object sender, MouseButtons button)
        {
            if (inventoryList.HasSelection)
            {
                Item item = (Item)this.inventoryList.GetSelection();

                if (item.ItemType == ItemTypes.Scroll)
                {
                    this.hotbar.CastAbility(((Scroll)item).ScrollAbility);
                    item.OnUse(GameManager.Player);
                    Inventory.PurgeItem(item);
                }
                else
                {
                    item.OnUse(GameManager.Player);
                    if (item.RemoveOnUse)
                    {
                        Inventory.PurgeItem(item);
                    }

                    GameManager.Step();
                }
            }
            this.descriptionBox.Text = string.Empty;
        }
        private void dropButton_Click(object sender, MouseButtons button)
        {
            if (inventoryList.HasSelection)
            {
                Item item = (Item)inventoryList.GetSelection();
                item.OnDrop();

                Inventory.DropItem(inventoryList.SelectedIndex, GameManager.CurrentLevel, GameManager.Player.X, GameManager.Player.Y);

                GameManager.Step();
            }
            this.descriptionBox.Text = string.Empty;
        }
        private void pickupButton_Click(object sender, MouseButtons button)
        {
            if (groundList.HasSelection)
            {
                Item item = (Item)groundList.GetSelection();
                GameManager.CurrentLevel.PickupItem(item);
                item.OnPickup();

                if (item.ItemType != ItemTypes.Gold)
                    Inventory.PlayerInventory.Add(item);

                GameManager.Step();
            }
            this.descriptionBox.Text = string.Empty;
        }
    }
    public class EquipmentControl : Control
    {
        private Title equipmentTitle;
        private Title inventoryTitle;

        private ScrollingList equipmentList;
        private ScrollingList filteredList;
        private TextBox descriptionBox;
        private Button equipButton, unequipButton;

        public EquipmentControl(Control parent, int x, int y, int width, int height)
            : base(parent)
        {
            this.position = new Point(x, y);
            this.size = new Point(width, height);

            this.equipmentTitle = new Title(this, "-=Equipment=-", width / 2, 0, Title.TextAlignModes.Center);
            this.inventoryTitle = new Title(this, "-=Inventory=-", width / 2, 16, Title.TextAlignModes.Center);

            this.equipmentList = new ScrollingList(this, 0, 1, width, 12) { FillColor = new Color(20, 20, 20) };
            this.filteredList = new ScrollingList(this, 0, 17, width, 12) { FillColor = new Color(20, 20, 20) };
            this.descriptionBox = new TextBox(this, 0, 29, width, 15) { FillColor = new Color(20, 20, 20) };

            this.equipButton = new Button(this, "Equip", 0, 13, 14, 3);
            this.unequipButton = new Button(this, "Unequip", 14, 13, 14, 3);

            this.equipmentList.Selected += equipmentList_Selected;
            this.filteredList.Selected += filteredList_Selected;

            this.equipButton.Click += equipButton_Click;
            this.unequipButton.Click += unequipButton_Click;
        }

        public override void UpdateStep()
        {
            this.generateLists();

            base.UpdateStep();
        }

        private void filteredList_Selected(object sender, int index)
        {
            this.descriptionBox.Text = ((Item)this.filteredList.Items[index]).GetDescription();
        }
        private void equipmentList_Selected(object sender, int index)
        {
            EquipmentSlots slot = (EquipmentSlots)this.equipmentSlots.GetValue(index);

            if (Inventory.ItemEquipped(slot))
                this.descriptionBox.Text = Inventory.GetEquipment(slot).GetDescription();
            else if (slot == EquipmentSlots.MainHand && Inventory.ItemEquipped(EquipmentSlots.TwoHand))
                this.descriptionBox.Text = Inventory.TwoHand.GetDescription();
            else
                this.descriptionBox.Text = " ";
        }
        private void equipButton_Click(object sender, MouseButtons button)
        {
            if (this.filteredList.HasSelection)
            {
                Equipment item = (Equipment)this.filteredList.GetSelection();

                if (equipmentList.HasSelection)
                    Inventory.EquipItem(item, getSlotIndex(equipmentList.SelectedIndex));
                else
                    Inventory.EquipItem(item, item.EquipSlot);

                filteredList.ClearSelection();
                equipmentList.ClearSelection();

                this.descriptionBox.Text = " ";

                GameManager.Player.StatsPackage.CalculateStats();
                GameManager.Player.StatsPackage = Inventory.CalculateStats(GameManager.Player.StatsPackage);

                GameManager.Step();
            }
            this.descriptionBox.Text = string.Empty;
        }
        private void unequipButton_Click(object sender, MouseButtons button)
        {
            if (equipmentList.HasSelection)
            {
                this.descriptionBox.Text = " ";

                string equipmentTag = "";
                equipmentTag += equipmentList.GetSelection().ListText[0];
                equipmentTag += equipmentList.GetSelection().ListText[1];
                equipmentTag += equipmentList.GetSelection().ListText[2];
                equipmentTag += equipmentList.GetSelection().ListText[3];

                this.unequipItem(equipmentTag.Trim());

                filteredList.ClearSelection();
                equipmentList.ClearSelection();

                GameManager.Player.StatsPackage.CalculateStats();
                GameManager.Player.StatsPackage = Inventory.CalculateStats(GameManager.Player.StatsPackage);

                GameManager.Step();
            }
            this.descriptionBox.Text = string.Empty;
        }

        private void unequipItem(string tag)
        {
            if (tag == "HELM")
                Inventory.UnequipItem(EquipmentSlots.Head);
            else if (tag == "BACK")
                Inventory.UnequipItem(EquipmentSlots.Shoulder);
            else if (tag == "CHST")
                Inventory.UnequipItem(EquipmentSlots.Chest);
            else if (tag == "LEGS")
                Inventory.UnequipItem(EquipmentSlots.Legs);
            else if (tag == "BOTS")
                Inventory.UnequipItem(EquipmentSlots.Boots);
            else if (tag == "GLVS")
                Inventory.UnequipItem(EquipmentSlots.Gloves);
            else if (tag == "NECK")
                Inventory.UnequipItem(EquipmentSlots.Neck);
            else if (tag == "RING")
                Inventory.UnequipItem(EquipmentSlots.Ring);
            else if (tag == "RELC")
                Inventory.UnequipItem(EquipmentSlots.Relic);
            else if (tag == "MH")
            {
                if (Inventory.ItemEquipped(EquipmentSlots.TwoHand))
                    Inventory.UnequipItem(EquipmentSlots.TwoHand);
                else
                    Inventory.UnequipItem(EquipmentSlots.MainHand);
            }
            else if (tag == "OH")
                Inventory.UnequipItem(EquipmentSlots.OffHand);
        }
        private string getSlotName(EquipmentSlots slot)
        {
            if (slot == EquipmentSlots.Head)
                return "HELM - ";
            else if (slot == EquipmentSlots.Shoulder)
                return "BACK - ";
            else if (slot == EquipmentSlots.Chest)
                return "CHST - ";
            else if (slot == EquipmentSlots.Legs)
                return "LEGS - ";
            else if (slot == EquipmentSlots.Boots)
                return "BOTS - ";
            else if (slot == EquipmentSlots.Gloves)
                return "GLVS - ";

            else if (slot == EquipmentSlots.Neck)
                return "NECK - ";
            else if (slot == EquipmentSlots.Ring)
                return "RING - ";
            else if (slot == EquipmentSlots.Relic)
                return "RELC - ";

            else if (slot == EquipmentSlots.MainHand)
                return "  MH - ";
            else if (slot == EquipmentSlots.OffHand)
                return "  OH - ";

            return string.Empty;
        }
        private EquipmentSlots getSlotIndex(int index)
        {
            return (EquipmentSlots)equipmentSlots.GetValue(index);
        }
        private void generateLists()
        {
            //Equipment Slots
            this.equipmentList.Items.Clear();
            for (int i = 0; i < this.equipmentSlots.Length - 2; i++) //The last two values are OneHand and TwoHand
            {
                EquipmentSlots slot = (EquipmentSlots)this.equipmentSlots.GetValue(i);
                ListItem item = new ListItem();
                item.ListText = this.getSlotName(slot);

                if (Inventory.ItemEquipped(slot))
                {
                    Item equipment = Inventory.GetEquipment(slot);
                    item.ListText += equipment.Name;

                    item.TextColor = equipment.TextColor;
                }
                else
                {
                    item.ListText += "{[EMPTY]}";
                    item.TextColor = Color.White;
                }

                this.equipmentList.Items.Add(item);
            }

            //Specific Two Hand weapon check
            if (Inventory.ItemEquipped(EquipmentSlots.TwoHand))
            {
                equipmentList.Items[equipmentList.Items.Count - 2].ListText = "  MH - " + Inventory.TwoHand.Name;
                equipmentList.Items[equipmentList.Items.Count - 2].TextColor = Inventory.TwoHand.TextColor;

                equipmentList.Items[equipmentList.Items.Count - 1].ListText = "  OH - {[Two Handed]}";
                equipmentList.Items[equipmentList.Items.Count - 1].TextColor = new Color(100, 100, 100);
            }

            //Filtered Inventory List
            this.filteredList.Items.Clear();
            for (int i = 0; i < Inventory.PlayerInventory.Count; i++)
            {
                if (Inventory.PlayerInventory[i].ItemType == ItemTypes.Equipment ||
                    Inventory.PlayerInventory[i].ItemType == ItemTypes.Equipment)
                {
                    this.filteredList.Items.Add(Inventory.PlayerInventory[i]);
                }
            }
        }

        private Array equipmentSlots = Enum.GetValues(typeof(EquipmentSlots));
    }
    public class SpellbookControl : Control
    {
        private Title spellbookTitle;
        private ScrollingList spellList;
        private Button castSpellButton;
        private TextBox spellDescription;

        private Hotbar hotbar;

        public SpellbookControl(Control parent, int x, int y, int width, int height, Hotbar hotbar)
            : base(parent)
        {
            this.position = new Point(x, y);
            this.size = new Point(width, height);

            this.hotbar = hotbar;

            this.spellbookTitle = new Title(this, "-=Spell Boox=-", width / 2, 0);

            this.spellList = new ScrollingList(this, 0, 1, this.Size.X, 15) { FillColor = new Color(20, 20, 20) };
            this.castSpellButton = new Button(this, "Cast", 0, 16, this.Size.X, 3);
            this.spellDescription = new TextBox(this, 0, 19, this.Size.X, this.Size.Y - 19) { FillColor = new Color(20, 20, 20) };

            this.spellList.Selected += spellList_Selected;
            this.castSpellButton.Click += castSpellButton_Click;
        }

        public override void UpdateStep()
        {
            this.populateSpellbook();

            base.UpdateStep();
        }

        private void populateSpellbook()
        {
            this.spellList.SetList<Ability>(GameManager.Player.StatsPackage.AbilityList);
        }
        private void castSpellButton_Click(object sender, MouseButtons button)
        {
            if (this.spellList.HasSelection)
            {
                hotbar.CastAbility((Ability)this.spellList.GetSelection());
            }
        }
        private void spellList_Selected(object sender, int index)
        {
            if (this.spellList.HasSelection)
            {
                this.spellDescription.Text = ((Ability)this.spellList.GetSelection()).GetDescription();
            }
        }
    }
    public class CraftingControl : Control
    {
        public CraftingControl(Control parent)
            : base(parent)
        {

        }
    }
}
