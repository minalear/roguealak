using System;
using OpenTK.Input;
using OpenTK.Graphics;
using Roguelike.Engine.Console;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Engine.UI.Interfaces
{
    public class AboutInterface : Interface
    {
        Title mainTitle;

        Title title01, name01;
        Title title02, name02;
        Title title03, name03;

        Button backButton;

        public AboutInterface()
        {
            backButton = new Button(this, "X", GraphicConsole.Instance.BufferWidth - 1, 0, 1, 1) { KeyShortcut = Key.Escape };
            backButton.Click += backButton_Click;

            mainTitle = new Title(this, "Roguealak", GraphicConsole.Instance.BufferWidth / 2, 2, Title.TextAlignModes.Center);
            mainTitle.TextColor = Color4.Red;

            title01 = new Title(this, "Programmer", GraphicConsole.Instance.BufferWidth / 2, 6, Title.TextAlignModes.Center);
            title01.TextColor = Color4.Gray;
            name01 = new Title(this, "Trevor \"Minalear\" Fisher", GraphicConsole.Instance.BufferWidth / 2, 7, Title.TextAlignModes.Center);

            title02 = new Title(this, "Art & Design", GraphicConsole.Instance.BufferWidth / 2, 9, Title.TextAlignModes.Center);
            title02.TextColor = Color4.Gray;
            name02 = new Title(this, "Max \"Flinnan\" Forgothowtospell", GraphicConsole.Instance.BufferWidth / 2, 10, Title.TextAlignModes.Center);

            title03 = new Title(this, "Loser", GraphicConsole.Instance.BufferWidth / 2, 12, Title.TextAlignModes.Center);
            title03.TextColor = Color4.Gray;
            name03 = new Title(this, "Asahp", GraphicConsole.Instance.BufferWidth / 2, 13, Title.TextAlignModes.Center);
        }

        void backButton_Click(object sender, MouseButtons button)
        {
            GameManager.ChangeGameState(GameStates.MainMenu);
        }

        public override void OnCall()
        {
            base.OnCall();
        }
    }
}
