﻿using System;
using Roguelike.Engine.UI;
using Roguelike.Engine.UI.Controls;
using Roguelike.Core;
using Roguelike.Core.Items;
using Roguelike.Core.Combat;

namespace Roguelike.Engine.UI.Interfaces
{
    public class DeadInterface : Interface
    {
        Title message, score;
        Button continueButton;

        public DeadInterface()
        {
            this.message = new Title(this, "You have died.", GraphicConsole.BufferWidth / 2, GraphicConsole.BufferHeight / 2, Title.TextAlignModes.Center);
            this.score = new Title(this, "Score", GraphicConsole.BufferWidth / 2, this.message.Position.Y + 1, Title.TextAlignModes.Center);

            continueButton = new Button(this, "Continue", this.message.Position.X - 5, this.message.Position.Y + 4);
            continueButton.Click += backButton_Pressed;
        }

        public override void OnCall()
        {
            GameManager.FakeScore += Inventory.Gold;
            this.score.Text = "Score: " + GameManager.FakeScore + ".  Sweet Rolls = " + GameManager.SweetRolls + ".  Gold = " + Inventory.Gold;
            GameManager.ResetGame();
            
            base.OnCall();
        }

        void backButton_Pressed(object sender, MouseButtons button)
        {
            GameManager.ChangeGameState(GameStates.MainMenu);
        }
    }
}
