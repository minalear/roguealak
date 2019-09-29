using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using Roguelike.Engine.Console;
using Roguelike.Engine.UI.Controls;
using Roguelike.Core;
using Roguelike.Core.Combat;
using Roguelike.Core.Entities;
using Roguelike.Core.Items;

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
            manaBar = new BarTitle(this, GraphicConsole.Instance.BufferWidth - 23, 1, "MP", 20);
            manaBar.BarColor = Color4.DodgerBlue;
            manaBar.FillColor = Color4.DarkBlue;

            playerTitle = new Title(this, "[Name] of [Town] the [Title] - Level ##", GraphicConsole.Instance.BufferWidth / 2, 1, Title.TextAlignModes.Center);

            popupControl = new Popup(this);

            leftPanelButton = new Button(this, "▓", 1, 48, 1, 1);
            leftPanelButton.Click += leftPanelButton_Pressed;

            rightPanelButton = new Button(this, "▓", 123, 48, 1, 1);
            rightPanelButton.Click += rightPanelButton_Pressed;

            combatLogButton = new Button(this, "<. .>", GraphicConsole.Instance.BufferWidth / 2 - 2, GraphicConsole.Instance.BufferHeight - 2, 5, 1);
            combatLogButton.Click += combatLogButton_Pressed;

            hotbar = new Hotbar(this);
            leftTab = new LeftTab(this);
            rightTab = new RightTab(this, hotbar);
            bottomTab = new BottomTab(this, leftTab, rightTab);

            menuButton = new Button(this, "Menu", 3, GraphicConsole.Instance.BufferHeight - 2, 6, 1);
            mapButton = new Button(this, "Map*", 10, GraphicConsole.Instance.BufferHeight - 2, 6, 1) { KeyShortcut = Key.M };

            menuButton.Click += menuButton_Pressed;
            mapButton.Click += mapButton_Pressed;
        }

        #region Button Events
        void leftPanelButton_Pressed(object sender, MouseButtons button)
        {
            leftTab.IsVisible = !leftTab.IsVisible;
        }
        void rightPanelButton_Pressed(object sender, MouseButtons button)
        {
            rightTab.IsVisible = !rightTab.IsVisible;
        }
        void combatLogButton_Pressed(object sender, MouseButtons button)
        {
            if (button == MouseButtons.Left)
                bottomTab.IsVisible = !bottomTab.IsVisible;
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

            drawInterfaceBars();

            base.DrawStep();
        }

        public override void Update(GameTime gameTime)
        {
            GameManager.CurrentLevel.Update(gameTime);

            //Right Tab
            if (InputManager.KeyWasReleased(Key.I))
            {
                rightTab.IsVisible = true;
                rightTab.OpenMenu("Inventory");

                DrawStep();
            }
            else if (InputManager.KeyWasReleased(Key.E))
            {
                rightTab.IsVisible = true;
                rightTab.OpenMenu("Equipment");

                DrawStep();
            }
            else if (InputManager.KeyWasReleased(Key.B))
            {
                rightTab.IsVisible = true;
                rightTab.OpenMenu("Spellbook");

                DrawStep();
            }

            //Left Tab
            if (InputManager.KeyWasReleased(Key.C))
            {
                leftTab.IsVisible = true;
                leftTab.OpenMenu("Character");

                DrawStep();
            }
            else if (InputManager.KeyWasReleased(Key.K))
            {
                leftTab.IsVisible = true;
                leftTab.OpenMenu("Effects");

                DrawStep();
            }

            //Bottom Tab
            if (InputManager.KeyWasReleased(Key.Tab))
            {
                bottomTab.IsVisible = !bottomTab.IsVisible;
                DrawStep();
            }

            //Escape
            if (InputManager.KeyWasReleased(Key.Escape))
            {
                if (leftTab.IsVisible || rightTab.IsVisible || bottomTab.IsVisible)
                {
                    leftTab.IsVisible = false;
                    rightTab.IsVisible = false;
                    bottomTab.IsVisible = false;
                }
                else
                    menuButton_Pressed(this, MouseButtons.Left);

                GameManager.DrawStep();
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
            GraphicConsole.Instance.ClearColor();

            //Header Bar
            DrawingUtilities.DrawLine(1, 1, GraphicConsole.Instance.BufferWidth - 2, 1, ' ');
            DrawingUtilities.DrawLine(0, 0, GraphicConsole.Instance.BufferWidth, 0, '═');
            DrawingUtilities.DrawLine(0, 2, GraphicConsole.Instance.BufferWidth, 2, '═');

            //Left Bar
            DrawingUtilities.DrawLine(0, 1, 0, GraphicConsole.Instance.BufferHeight - 2, '│');

            //Right Bar
            DrawingUtilities.DrawLine(GraphicConsole.Instance.BufferWidth, 1, GraphicConsole.Instance.BufferWidth, GraphicConsole.Instance.BufferHeight - 2, '│');

            //Bottom Bar
            DrawingUtilities.DrawLine(1, GraphicConsole.Instance.BufferHeight - 2, GraphicConsole.Instance.BufferWidth - 2, GraphicConsole.Instance.BufferHeight - 2, ' ');
            DrawingUtilities.DrawLine(0, GraphicConsole.Instance.BufferHeight, GraphicConsole.Instance.BufferWidth, GraphicConsole.Instance.BufferHeight, '─');
            DrawingUtilities.DrawLine(0, GraphicConsole.Instance.BufferHeight - 3, GraphicConsole.Instance.BufferWidth, GraphicConsole.Instance.BufferHeight - 3, '─');

            //Bottom Left Corner
            GraphicConsole.Instance.Put('├', 0, GraphicConsole.Instance.BufferHeight - 3);
            GraphicConsole.Instance.Put('└', 0, GraphicConsole.Instance.BufferHeight);

            //Bottom Right Corner
            GraphicConsole.Instance.Put('┤', GraphicConsole.Instance.BufferWidth, GraphicConsole.Instance.BufferHeight - 3);
            GraphicConsole.Instance.Put('┘', GraphicConsole.Instance.BufferWidth, GraphicConsole.Instance.BufferHeight);

            //Top Left Corner
            GraphicConsole.Instance.Put('╒', 0, 0);
            GraphicConsole.Instance.Put('╞', 0, 2);

            //Top Right Corner
            GraphicConsole.Instance.Put('╕', GraphicConsole.Instance.BufferWidth, 0);
            GraphicConsole.Instance.Put('╡', GraphicConsole.Instance.BufferWidth, 2);
        }
    }
    public class BarTitle : Control
    {
        public BarTitle(Control parent, int x, int y, string textRoot, int width)
            : base(parent)
        {
            text = textRoot;
            formattedText = text + ": ##/##";

            position = new Point(x, y);
            size = new Point(width, 1);
        }

        public override void DrawStep()
        {
            GraphicConsole.Instance.SetColors(Color4.Transparent, fillColor);
            DrawingUtilities.DrawLine(Position.X, Position.Y, Position.X + Size.X, Position.Y, ' ');

            if (percent != 0)
            {
                int barLength = (int)(percent * Size.X);
                GraphicConsole.Instance.SetColors(Color4.Transparent, barColor);
                DrawingUtilities.DrawLine(Position.X, Position.Y, Position.X + barLength, Position.Y, ' ');
            }

            GraphicConsole.Instance.SetCursor(Position.X + (Size.X / 2 - formattedText.Length / 2), Position.Y);
            for (int i = 0; i < formattedText.Length; i++)
            {
                GraphicConsole.Instance.SetColors(textColor, GraphicConsole.Instance.GetColorsAtTile(GraphicConsole.Instance.CursorLeft, GraphicConsole.Instance.CursorTop).Item2);
                GraphicConsole.Instance.Write(formattedText[i]);
            }

            base.DrawStep();
        }

        public override void UpdateStep()
        {
            percent = CurrentValue / MaxValue;
            formattedText = text + ": " + currentValue + @"/" + maxValue;

            base.UpdateStep();
        }

        private double currentValue = 50;
        private double maxValue = 100;

        private string text;
        private string formattedText;
        private double percent;

        private Color4 barColor = Color4.Red;
        private Color4 fillColor = Color4.DarkRed;
        private Color4 textColor = Color4.Black;

        public double CurrentValue { get { return currentValue; } set { currentValue = value; } }
        public double MaxValue { get { return maxValue; } set { maxValue = value; } }
        public string Text { get { return text; } set { text = value; UpdateStep(); DrawStep(); } }
        public double Percent { get { return percent; } }
        public Color4 BarColor { get { return barColor; } set { barColor = value; } }
        public Color4 FillColor { get { return fillColor; } set { fillColor = value; } }
        public Color4 TextColor { get { return textColor; } set { textColor = value; } }
    }

    public class LeftTab : Control
    {
        private ToggleButton characterButton, currentEffects;
        private ToggleButtonGroup buttonGroup;

        private CharacterControl characterControl;
        private EffectsControl effectsControl;

        public LeftTab(Interface parent)
            : base(parent)
        {
            Position = new Point(1, 3);
            Size = new Point(28, 44);

            characterControl = new CharacterControl(this, 0, 0, Size.X, Size.Y);
            effectsControl = new EffectsControl(this, 0, 0, Size.X, Size.Y) { IsVisible = false };

            buttonGroup = new ToggleButtonGroup(this);
            characterButton = new ToggleButton(this, "⌂", Size.X + 1, Position.Y - 3, 1, 1);
            currentEffects = new ToggleButton(this, "§", Size.X + 1, Position.Y - 2, 1, 1);
            //currentEffects = new ToggleButton(this, "○", Size.X + 1, Position.Y - 1, 1, 1);
            characterButton.Enabled = true;

            buttonGroup.AddButton(characterButton);
            buttonGroup.AddButton(currentEffects);

            characterButton.Click += characterButton_Pressed;
            currentEffects.Click += currentEffects_Pressed;

            IsVisible = false;
        }

        public override void DrawStep()
        {
            clearArea();

            //Border
            DrawingUtilities.DrawLine(Size.X + 1, Position.Y, Size.X + 1, Size.Y + 2, '│');
            GraphicConsole.Instance.Put('╤', Size.X + 1, Position.Y - 1);
            GraphicConsole.Instance.Put('┴', Size.X + 1, Size.Y + 3);

            //Border Around Toggle Switches
            DrawingUtilities.DrawLine(Position.X + Size.X + 2, Position.Y, Position.X + Size.X + 2, Position.Y + 4, '│');
            GraphicConsole.Instance.Put('╤', Position.X + Size.X + 2, Position.Y - 1);
            GraphicConsole.Instance.Put('┘', Position.X + Size.X + 2, Position.Y + 4);
            GraphicConsole.Instance.Put('─', Position.X + Size.X + 1, Position.Y + 4);
            GraphicConsole.Instance.Put('├', Position.X + Size.X, Position.Y + 4);

            base.DrawStep();
        }
        public void OpenMenu(string menu)
        {
            if (menu == "Character")
            {
                characterButton.Enabled = true;
                currentEffects.Enabled = false;

                characterButton_Pressed(this);
            }
            else if (menu == "Effects")
            {
                characterButton.Enabled = false;
                currentEffects.Enabled = true;

                currentEffects_Pressed(this);
            }

            parent.UpdateStep();
            parent.DrawStep();
        }

        void characterButton_Pressed(object sender)
        {
            characterControl.IsVisible = true;
            effectsControl.IsVisible = false;
        }
        void currentEffects_Pressed(object sender)
        {
            characterControl.IsVisible = false;
            effectsControl.IsVisible = true;
        }
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
            Size = new Point(28, GraphicConsole.Instance.BufferHeight - 6);
            Position = new Point(GraphicConsole.Instance.BufferWidth - Size.X - 1, 3);

            inventoryButton = new ToggleButton(this, "⌂", -2, 0, 1, 1) { Enabled = true };
            equipmentButton = new ToggleButton(this, "♦", -2, 1, 1, 1) { Enabled = false };
            spellbookButton = new ToggleButton(this, "♠", -2, 2, 1, 1) { Enabled = false };

            buttonGroup = new ToggleButtonGroup(this);
            buttonGroup.AddButton(inventoryButton);
            buttonGroup.AddButton(equipmentButton);
            buttonGroup.AddButton(spellbookButton);

            inventoryButton.Click += inventoryButton_Click;
            equipmentButton.Click += equipmentButton_Click;
            spellbookButton.Click += spellbookButton_Click;

            inventoryControl = new InventoryControl(this, 0, 0, 28, 44, hotbar) { IsVisible = true };
            equipmentControl = new EquipmentControl(this, 0, 0, 28, 44) { IsVisible = false };
            spellbookControl = new SpellbookControl(this, 0, 0, 28, 44, hotbar) { IsVisible = false };

            IsVisible = false;
        }
        public override void DrawStep()
        {
            clearArea();

            //Left Border
            DrawingUtilities.DrawLine(Position.X - 1, Position.Y, Position.X - 1, Size.Y + 2, '│');
            GraphicConsole.Instance.Put('╤', Position.X - 1, Position.Y - 1);
            GraphicConsole.Instance.Put('┴', Position.X - 1, Size.Y + 3);

            //Border Around Toggle Switches
            DrawingUtilities.DrawLine(Position.X - 3, Position.Y, Position.X - 3, Position.Y + 4, '│');
            GraphicConsole.Instance.Put('╤', Position.X - 3, Position.Y - 1);
            GraphicConsole.Instance.Put('└', Position.X - 3, Position.Y + 4);
            GraphicConsole.Instance.Put('─', Position.X - 2, Position.Y + 4);
            GraphicConsole.Instance.Put('┤', Position.X - 1, Position.Y + 4);

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

            parent.UpdateStep();
            parent.DrawStep();
        }

        void inventoryButton_Click(object sender)
        {
            inventoryControl.IsVisible = true;
            equipmentControl.IsVisible = false;
            spellbookControl.IsVisible = false;
        }
        void equipmentButton_Click(object sender)
        {
            inventoryControl.IsVisible = false;
            equipmentControl.IsVisible = true;
            spellbookControl.IsVisible = false;
        }
        void spellbookButton_Click(object sender)
        {
            inventoryControl.IsVisible = false;
            equipmentControl.IsVisible = false;
            spellbookControl.IsVisible = true;
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
            Position = new Point(30, 40);

            messageList = new ScrollingList(this, 1, 0, 63, 7);
            messageInformation = new TextBox(this, 1, 0, 64, 7) { IsVisible = false };
            contextButton = new Button(this, "►", 0, 0, 1, 1);
            contextButton.Click += contextButton_Click;

            this.leftTab = leftTab;
            this.rightTab = rightTab;

            IsVisible = false;
        }

        public override void DrawStep()
        {
            drawBorders();

            base.DrawStep();
        }
        public override void UpdateStep()
        {
            messageList.SetList<MessageCenter.Message>(MessageCenter.MessageLog);

            base.UpdateStep();
        }

        void contextButton_Click(object sender, MouseButtons button)
        {
            detailedMode = !detailedMode;

            if (detailedMode)
            {
                messageInformation.IsVisible = true;
                messageList.IsVisible = false;

                if (messageList.HasSelection)
                    messageInformation.Text = ((MessageCenter.Message)messageList.GetSelection()).DetailedMessage;
                contextButton.Text = "◄";
            }
            else
            {
                messageInformation.IsVisible = false;
                messageList.IsVisible = true;

                contextButton.Text = "►";
            }
        }
        private void drawBorders()
        {
            int x, y;
            int panelWidth, panelHeight;

            x = 30;
            y = GraphicConsole.Instance.BufferHeight - 10;
            panelWidth = 94;
            panelHeight = 7;

            DrawingUtilities.DrawLine(30, y - 1, 94, y - 1, '─');
            DrawingUtilities.DrawRect(x, y, panelWidth - x + 1, panelHeight, ' ', true);

            if (leftTab.IsVisible)
                GraphicConsole.Instance.Put('├', x - 1, y - 1);
            else
            {
                DrawingUtilities.DrawLine(x - 1, y, x - 1, GraphicConsole.Instance.BufferHeight - 4, '│');
                GraphicConsole.Instance.Put('┌', x - 1, y - 1);
                GraphicConsole.Instance.Put('┴', x - 1, GraphicConsole.Instance.BufferHeight - 3);
            }

            if (rightTab.IsVisible)
                GraphicConsole.Instance.Put('┤', panelWidth + 1, y - 1);
            else
            {
                DrawingUtilities.DrawLine(panelWidth + 1, y, panelWidth + 1, GraphicConsole.Instance.BufferHeight - 4, '│');
                GraphicConsole.Instance.Put('┐', panelWidth + 1, y - 1);
                GraphicConsole.Instance.Put('┴', panelWidth + 1, GraphicConsole.Instance.BufferHeight - 3);
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
            button1 = new Button(this, "[ ABIL 1 ]", 70, 48, 10, 1) { KeyShortcut = Key.Number1 };
            button2 = new Button(this, "[ ABIL 2 ]", 83, 48, 10, 1) { KeyShortcut = Key.Number2 };
            button3 = new Button(this, "[ ABIL 3 ]", 96, 48, 10, 1) { KeyShortcut = Key.Number3 };
            button4 = new Button(this, "[ ABIL 4 ]", 109, 48, 10, 1) { KeyShortcut = Key.Number4 };

            buttonS1 = new Button(this, "[ ABIL 5 ]", 70, 48, 10, 1) { IsVisible = false, KeyShortcut = Key.Number1 };
            buttonS2 = new Button(this, "[ ABIL 6 ]", 83, 48, 10, 1) { IsVisible = false, KeyShortcut = Key.Number2 };
            buttonS3 = new Button(this, "[ ABIL 7 ]", 96, 48, 10, 1) { IsVisible = false, KeyShortcut = Key.Number3 };
            buttonS4 = new Button(this, "[ ABIL 8 ]", 109, 48, 10, 1) { IsVisible = false, KeyShortcut = Key.Number4 };

            button1.Click += button1_Pressed;
            button2.Click += button2_Pressed;
            button3.Click += button3_Pressed;
            button4.Click += button4_Pressed;

            buttonS1.Click += buttonS1_Pressed;
            buttonS2.Click += buttonS2_Pressed;
            buttonS3.Click += buttonS3_Pressed;
            buttonS4.Click += buttonS4_Pressed;

            GameCursor = new GameCursor(parent, GameManager.Viewport);
            GameCursor.AimingMode = GameCursor.AimingModes.Off;
            GameCursor.Selected += gameCursor_Selected;

            popupControl = new Popup(this);

            setupButtonNames();
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
            setupButtonNames();

            base.UpdateStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (InputManager.KeyWasPressed(Key.LShift))
                swapHotBar();

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
            castedAbility = ability;

            if (ability.Range != 0)
                GameCursor.Range = ability.Range;
            else
                GameCursor.Range = 100;

            if (castedAbility.TargetingType == TargetingTypes.EntityTarget)
            {
                GameCursor.Enable(GameCursor.AimingModes.Point, castedAbility.Range);
            }
            else if (castedAbility.TargetingType == TargetingTypes.GroundTarget)
            {
                GameCursor.Enable(GameCursor.AimingModes.Point, castedAbility.Range);
            }
            else if (castedAbility.TargetingType == TargetingTypes.Self)
            {
                CombatManager.PerformAbility(GameManager.Player.StatsPackage, GameManager.Player.StatsPackage, castedAbility);
                GameManager.Step();
            }
        }
        private void swapHotBar()
        {
            button1.IsVisible = !button1.IsVisible;
            button2.IsVisible = !button2.IsVisible;
            button3.IsVisible = !button3.IsVisible;
            button4.IsVisible = !button4.IsVisible;

            buttonS1.IsVisible = !buttonS1.IsVisible;
            buttonS2.IsVisible = !buttonS2.IsVisible;
            buttonS3.IsVisible = !buttonS3.IsVisible;
            buttonS4.IsVisible = !buttonS4.IsVisible;

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

        private Color4 foregroundColorCursor = Color4.Goldenrod;
        private Color4 backgroundColorCursor = Color4.Black;
        private Color4 foregroundColorFill = Color4.Crimson;
        private Color4 backgroundColorFill = Color4.Black;

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
            viewport = new Rectangle(
                viewport.X * GraphicConsole.CHAR_WIDTH,
                viewport.Y * GraphicConsole.CHAR_HEIGHT,
                viewport.Width * GraphicConsole.CHAR_WIDTH,
                viewport.Height * GraphicConsole.CHAR_HEIGHT);

            viewportCoords = viewport;
        }

        public override void DrawStep()
        {
            if (aimingMode != AimingModes.Off)
            {
                if (range != -1)
                {
                    GameManager.CurrentLevel.DrawCircle(MatrixLevels.Effect, new Circle() { X = GameManager.Player.X, Y = GameManager.Player.Y, Radius = range }, FILL_CURSOR, foregroundColorFill, backgroundColorFill, true);
                }

                if (aimingMode == AimingModes.Point)
                {
                    if (isInRange)
                    {
                        GraphicConsole.Instance.SetColors(foregroundColorCursor, backgroundColorCursor);
                        GraphicConsole.Instance.Put(CURSOR, point0.X, point0.Y);
                    }
                    else
                    {
                        GraphicConsole.Instance.SetColors(Color4.Red, backgroundColorCursor);
                        GraphicConsole.Instance.Put('X', point0.X, point0.Y);
                    }
                }
                else if (aimingMode == AimingModes.Line)
                {
                    /*GraphicConsole.Instance.SetColors(foregroundColorFill, backgroundColorFill);
                    DrawingUtilities.DrawLine(point0.X, point0.Y, point1.X, point1.Y, FILL_CURSOR);*/
                    GameManager.CurrentLevel.DrawLine(MatrixLevels.Effect, DrawingUtilities.GetWorldPositionFromScreen(point0), DrawingUtilities.GetWorldPositionFromScreen(point1), FILL_CURSOR);

                    GraphicConsole.Instance.SetColors(foregroundColorCursor, backgroundColorCursor);
                    GraphicConsole.Instance.Put(CURSOR, point0.X, point0.Y);
                }
                else if (aimingMode == AimingModes.DrawLine)
                {
                    if (point1 == new Point(-1, -1))
                    {
                        GraphicConsole.Instance.SetColors(foregroundColorCursor, backgroundColorCursor);
                        GraphicConsole.Instance.Put(CURSOR, point0.X, point0.Y);
                    }
                    else
                    {
                        GraphicConsole.Instance.SetColors(foregroundColorFill, backgroundColorFill);
                        DrawingUtilities.DrawLine(point0.X, point0.Y, point1.X, point1.Y, FILL_CURSOR);

                        GraphicConsole.Instance.SetColors(foregroundColorCursor, backgroundColorCursor);
                        GraphicConsole.Instance.Put(CURSOR, point0.X, point0.Y);
                        GraphicConsole.Instance.Put(CURSOR, point1.X, point1.Y);
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

                    if (viewport.Contains(mouse))
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
                                if (distance <= range)
                                    isInRange = true;
                                else
                                    isInRange = false;

                                point0 = mouse;
                                InterfaceManager.DrawStep();
                            }

                            if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
                            {
                                OnSelect(null);
                            }
                            #endregion
                        }
                        else if (aimingMode == AimingModes.Line)
                        {
                            #region LineAiming
                            if (mouse != point0)
                            {
                                Point1 = new Point(GameManager.Player.X, GameManager.Player.Y);
                                if (Vector2.Distance(new Vector2(point1.X, point1.Y), new Vector2(mouse.X, mouse.Y)) <= range)
                                {
                                    point0 = mouse;
                                }
                                InterfaceManager.DrawStep();
                            }

                            if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
                            {
                                OnSelect(null);
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
                                    OnSelect(null);
                            }
                            #endregion
                        }

                        if (InputManager.MouseButtonWasClicked(MouseButtons.Right))
                        {
                            aimingMode = AimingModes.Off;
                            GameManager.CurrentLevel.ClearLayer(MatrixLevels.Effect);

                            InterfaceManager.DrawStep();
                        }
                    }

                    delay = maxDelay;
                }

                delay += gameTime.ElapsedTime.Milliseconds;
            }
            else
                delay = 0.0;

            base.Update(gameTime);
        }

        public void OnSelect(EventArgs args)
        {
            if (Selected != null)
                Selected(AimingMode, null);

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

            aimingMode = mode;
            delay = 0.0;
        }

        public AimingModes AimingMode { get { return aimingMode; } set { aimingMode = value; } }
        public Point Point0 
        {
            get { return DrawingUtilities.GetWorldPositionFromScreen(point0); }
            set { point0 = DrawingUtilities.GetScreenPositionFromWorld(point0); }
        }
        public Point Point1 
        {
            get { return new        Point(point1.X + GameManager.CameraOffset.X - viewport.X, point1.Y + GameManager.CameraOffset.Y - viewport.Y); }
            set { point1 = new Point( (value.X - GameManager.CameraOffset.X) + viewport.X,  (value.Y - GameManager.CameraOffset.Y) + viewport.Y); }
        }
        public int Range { get { return range; } set { range = value; } }

        public enum AimingModes { Off, Point, Line, DrawLine, Path }
    }

    //Left Tab Controls
    public class CharacterControl : Control
    {
        //Character Info
        private Title characterName;
        private Title characterClass;
        private Title characterLevel;
        private BarTitle expBar;

        //Primary Stats
        private Title strTitle;
        private Title agiTitle;
        private Title dexTitle;

        private Title intTitle;
        private Title wilTitle;
        private Title wisTitle;

        private Title conTitle;
        private Title endTitle;
        private Title frtTitle;

        //Effective Stats
        private TextBox effectiveStats;

        public CharacterControl(Control parent, int x, int y, int width, int height)
            : base(parent)
        {
            position = new Point(x, y);
            size = new Point(width, height);

            //Character Info
            characterName = new Title(this, "Billy Bob Thorton", 0, 0, Title.TextAlignModes.Left);
            characterClass = new Title(this, "Singer/Movie Star", 0, 1, Title.TextAlignModes.Left);
            characterLevel = new Title(this, "Level: -7", 0, 2, Title.TextAlignModes.Left);

            expBar = new BarTitle(this, 0, 4, "EXP", size.X - 1);
            expBar.FillColor = new Color4(150, 140, 60, 255);
            expBar.BarColor = new Color4(255, 240, 100, 255);

            //Primary Stats
            strTitle = new Title(this, "STR: ##", 1, 6);
            agiTitle = new Title(this, "AGI: ##", 1, 7);
            dexTitle = new Title(this, "DEX: ##", 1, 8);

            intTitle = new Title(this, "INT: ##", 1, 10);
            wilTitle = new Title(this, "WIL: ##", 1, 11);
            wisTitle = new Title(this, "WIS: ##", 1, 12);

            conTitle = new Title(this, "CON: ##", 1, 14);
            endTitle = new Title(this, "END: ##", 1, 15);
            frtTitle = new Title(this, "FRT: ##", 1, 16);

            //Effective Stats
            effectiveStats = new TextBox(this, 0, 19, Size.X, Size.Y - 19);
        }

        public override void UpdateStep()
        {
            //Character Info
            characterName.Text = GameManager.Player.PlayerStats.Name;
            characterClass.Text = GameManager.Player.PlayerStats.Culture + " " + GameManager.Player.PlayerStats.Race + " - " + GameManager.Player.PlayerStats.Class;
            characterLevel.Text = "Level: 1";

            //Primary Stats
            strTitle.Text = "STR: " + GameManager.Player.PlayerStats.Strength;
            agiTitle.Text = "AGI: " + GameManager.Player.PlayerStats.Agility;
            dexTitle.Text = "DEX: " + GameManager.Player.PlayerStats.Dexterity;

            intTitle.Text = "INT: " + GameManager.Player.PlayerStats.Intelligence;
            wilTitle.Text = "WIL: " + GameManager.Player.PlayerStats.Willpower;
            wisTitle.Text = "WIS: " + GameManager.Player.PlayerStats.Wisdom;

            conTitle.Text = "CON: " + GameManager.Player.PlayerStats.Constitution;
            endTitle.Text = "END: " + GameManager.Player.PlayerStats.Endurance;
            frtTitle.Text = "FRT: " + GameManager.Player.PlayerStats.Fortitude;

            //Effective Stats
            effectiveStats.Text = GameManager.Player.StatsPackage.GetInformation();

            base.UpdateStep();
        }
    }
    public class EffectsControl : Control
    {
        private Title effectsTitle;
        private ScrollingList effectsList;
        private TextBox effectDescription;

        public EffectsControl(Control parent, int x, int y, int width, int height)
            : base(parent)
        {
            position = new Point(x, y);
            size = new Point(width, height);

            effectsTitle = new Title(this, "-=Current Effects=-", size.X / 2, 0, Title.TextAlignModes.Center);
            effectsList = new ScrollingList(this, 0, 2, size.X, 24) { FillColor = new Color4(20, 20, 20, 255) };
            effectDescription = new TextBox(this, 0, 27, size.X, 17) { FillColor = new Color4(20, 20, 20, 255) };

            effectsList.Selected += effectsList_Selected;
            effectsList.Deselected += effectsList_Deselected;
        }

        public override void UpdateStep()
        {
            effectsList.SetList(GameManager.Player.PlayerStats.AppliedEffects);

            base.UpdateStep();
        }

        void effectsList_Selected(object sender, int index)
        {
            effectDescription.Text = ((Effect)(effectsList.Items[index])).EffectDescription;
        }
        void effectsList_Deselected(object sender)
        {
            effectDescription.Text = " ";
        }
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
            position = new Point(x, y);
            size = new Point(width, height);
            this.hotbar = hotbar;

            inventoryTitle = new Title(this, "-=Inventory=-", width / 2, 0, Title.TextAlignModes.Center);
            groundTitle = new Title(this, "-=Ground Items=-", width / 2, 19, Title.TextAlignModes.Center);
            goldTracker = new Title(this, "Gold: ###", 0, 1, Title.TextAlignModes.Left) { TextColor = Color4.Goldenrod, FillColor = new Color4(20, 20, 20, 255) };

            inventoryList = new ScrollingList(this, 0, 2, Size.X, 14) { FillColor = new Color4(20, 20, 20, 255) };
            groundList = new ScrollingList(this, 0, 20, Size.X, 8) { FillColor = new Color4(20, 20, 20, 255) };

            useButton = new Button(this, "Use", 0, 16, 14, 3);
            dropButton = new Button(this, "Drop", 14, 16, 14, 3);
            pickupButton = new Button(this, "Pickup", 0, 28, 28, 3);

            descriptionBox = new TextBox(this, 0, 31, Size.X, 13) { FillColor = new Color4(20, 20, 20, 255) };

            inventoryList.Selected += inventoryList_Selected;
            groundList.Selected += groundList_Selected;

            useButton.Click += useButton_Click;
            dropButton.Click += dropButton_Click;
            pickupButton.Click += pickupButton_Click;
        }
        public override void UpdateStep()
        {
            populateInventoryList();
            populateGroundList();

            goldTracker.Text = "Gold: " + Inventory.Gold.ToString();

            base.UpdateStep();
        }
        public override void DrawStep()
        {
            GraphicConsole.Instance.SetColors(Color4.White, new Color4(20, 20, 20, 255));
            DrawingUtilities.DrawLine(Position.X, Position.Y + 1, Position.X + Size.X - 1, Position.Y + 1, ' ');
            GraphicConsole.Instance.ClearColor();

            base.DrawStep();
        }

        private void populateInventoryList()
        {
            int index = inventoryList.SelectedIndex;
            inventoryList.SetList(Inventory.PlayerInventory);

            if (inventoryList.Items.Count > index)
                inventoryList.SetSelection(index);
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
            groundList.SetList<Item>(groundItems);
        }

        private void inventoryList_Selected(object sender, int index)
        {
            if (inventoryList.HasSelection)
            {
                descriptionBox.Text = ((Item)inventoryList.GetSelection()).GetDescription();
            }
        }
        private void groundList_Selected(object sender, int index)
        {
            if (groundList.HasSelection)
                descriptionBox.Text = ((Item)groundList.GetSelection()).GetDescription();
        }

        private void useButton_Click(object sender, MouseButtons button)
        {
            if (inventoryList.HasSelection)
            {
                Item item = (Item)inventoryList.GetSelection();

                if (item.ItemType == ItemTypes.Scroll)
                {
                    hotbar.CastAbility(((Scroll)item).ScrollAbility);
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
            descriptionBox.Text = string.Empty;
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
            descriptionBox.Text = string.Empty;
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
            descriptionBox.Text = string.Empty;
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
            position = new Point(x, y);
            size = new Point(width, height);

            equipmentTitle = new Title(this, "-=Equipment=-", width / 2, 0, Title.TextAlignModes.Center);
            inventoryTitle = new Title(this, "-=Inventory=-", width / 2, 16, Title.TextAlignModes.Center);

            equipmentList = new ScrollingList(this, 0, 1, width, 12) { FillColor = new Color4(20, 20, 20, 255) };
            filteredList = new ScrollingList(this, 0, 17, width, 12) { FillColor = new Color4(20, 20, 20, 255) };
            descriptionBox = new TextBox(this, 0, 29, width, 15) { FillColor = new Color4(20, 20, 20, 255) };

            equipButton = new Button(this, "Equip", 0, 13, 14, 3);
            unequipButton = new Button(this, "Unequip", 14, 13, 14, 3);

            equipmentList.Selected += equipmentList_Selected;
            filteredList.Selected += filteredList_Selected;

            equipButton.Click += equipButton_Click;
            unequipButton.Click += unequipButton_Click;
        }

        public override void UpdateStep()
        {
            generateLists();

            base.UpdateStep();
        }

        private void filteredList_Selected(object sender, int index)
        {
            descriptionBox.Text = ((Item)filteredList.Items[index]).GetDescription();
        }
        private void equipmentList_Selected(object sender, int index)
        {
            EquipmentSlots slot = (EquipmentSlots)equipmentSlots.GetValue(index);

            if (Inventory.ItemEquipped(slot))
                descriptionBox.Text = Inventory.GetEquipment(slot).GetDescription();
            else if (slot == EquipmentSlots.MainHand && Inventory.ItemEquipped(EquipmentSlots.TwoHand))
                descriptionBox.Text = Inventory.TwoHand.GetDescription();
            else
                descriptionBox.Text = " ";
        }
        private void equipButton_Click(object sender, MouseButtons button)
        {
            if (filteredList.HasSelection)
            {
                Equipment item = (Equipment)filteredList.GetSelection();

                if (equipmentList.HasSelection)
                    Inventory.EquipItem(item, getSlotIndex(equipmentList.SelectedIndex));
                else
                    Inventory.EquipItem(item, item.EquipSlot);

                filteredList.ClearSelection();
                equipmentList.ClearSelection();

                descriptionBox.Text = " ";

                GameManager.Player.StatsPackage.CalculateStats();
                GameManager.Player.StatsPackage = Inventory.CalculateStats(GameManager.Player.StatsPackage);

                GameManager.Step();
            }
            descriptionBox.Text = string.Empty;
        }
        private void unequipButton_Click(object sender, MouseButtons button)
        {
            if (equipmentList.HasSelection)
            {
                descriptionBox.Text = " ";

                string equipmentTag = "";
                equipmentTag += equipmentList.GetSelection().ListText[0];
                equipmentTag += equipmentList.GetSelection().ListText[1];
                equipmentTag += equipmentList.GetSelection().ListText[2];
                equipmentTag += equipmentList.GetSelection().ListText[3];

                unequipItem(equipmentTag.Trim());

                filteredList.ClearSelection();
                equipmentList.ClearSelection();

                GameManager.Player.StatsPackage.CalculateStats();
                GameManager.Player.StatsPackage = Inventory.CalculateStats(GameManager.Player.StatsPackage);

                GameManager.Step();
            }
            descriptionBox.Text = string.Empty;
        }

        private void unequipItem(string tag)
        {
            if (tag == "HELM")
                Inventory.UnequipItem(EquipmentSlots.Head);
            else if (tag == "NECK")
                Inventory.UnequipItem(EquipmentSlots.Neck);
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
            else if (slot == EquipmentSlots.Neck)
                return "NECK - ";
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
            equipmentList.Items.Clear();
            for (int i = 0; i < equipmentSlots.Length - 2; i++) //The last two values are OneHand and TwoHand
            {
                EquipmentSlots slot = (EquipmentSlots)equipmentSlots.GetValue(i);
                ListItem item = new ListItem();
                item.ListText = getSlotName(slot);

                if (Inventory.ItemEquipped(slot))
                {
                    Item equipment = Inventory.GetEquipment(slot);
                    item.ListText += equipment.Name;

                    item.TextColor = equipment.TextColor;
                }
                else
                {
                    item.ListText += "{[EMPTY]}";
                    item.TextColor = Color4.White;
                }

                equipmentList.Items.Add(item);
            }

            //Specific Two Hand weapon check
            if (Inventory.ItemEquipped(EquipmentSlots.TwoHand))
            {
                equipmentList.Items[equipmentList.Items.Count - 2].ListText = "  MH - " + Inventory.TwoHand.Name;
                equipmentList.Items[equipmentList.Items.Count - 2].TextColor = Inventory.TwoHand.TextColor;

                equipmentList.Items[equipmentList.Items.Count - 1].ListText = "  OH - {[Two Handed]}";
                equipmentList.Items[equipmentList.Items.Count - 1].TextColor = new Color4(100, 100, 100, 255);
            }

            //Filtered Inventory List
            filteredList.Items.Clear();
            for (int i = 0; i < Inventory.PlayerInventory.Count; i++)
            {
                if (Inventory.PlayerInventory[i].ItemType == ItemTypes.Equipment ||
                    Inventory.PlayerInventory[i].ItemType == ItemTypes.Equipment)
                {
                    filteredList.Items.Add(Inventory.PlayerInventory[i]);
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
            position = new Point(x, y);
            size = new Point(width, height);

            this.hotbar = hotbar;

            spellbookTitle = new Title(this, "-=Spell Boox=-", width / 2, 0);

            spellList = new ScrollingList(this, 0, 1, Size.X, 15) { FillColor = new Color4(20, 20, 20, 255) };
            castSpellButton = new Button(this, "Cast", 0, 16, Size.X, 3);
            spellDescription = new TextBox(this, 0, 19, Size.X, Size.Y - 19) { FillColor = new Color4(20, 20, 20, 255) };

            spellList.Selected += spellList_Selected;
            castSpellButton.Click += castSpellButton_Click;
        }

        public override void UpdateStep()
        {
            populateSpellbook();

            base.UpdateStep();
        }

        private void populateSpellbook()
        {
            spellList.SetList<Ability>(GameManager.Player.StatsPackage.AbilityList);
        }
        private void castSpellButton_Click(object sender, MouseButtons button)
        {
            if (spellList.HasSelection)
            {
                hotbar.CastAbility((Ability)spellList.GetSelection());
            }
        }
        private void spellList_Selected(object sender, int index)
        {
            if (spellList.HasSelection)
            {
                spellDescription.Text = ((Ability)spellList.GetSelection()).GetDescription();
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
