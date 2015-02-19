using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Roguelike.Engine.Game.Entities;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Engine.Game
{
    public static class MessageCenter
    {
        private static List<Message> messageLog = new List<Message>();
        public static List<Message> MessageLog { get { return messageLog; } set { messageLog = value; } }

        public static void PostMessage(string shortMessage, string detailedMessage, Entity sender)
        {
            Message message = new Message(shortMessage, sender);
            message.DetailedMessage = detailedMessage;

            messageLog.Insert(0, message);
        }
        public static void PostMessage(string shortMessage)
        {
            messageLog.Insert(0, new Message(shortMessage));
        }
        public static void PostMessage(Message message)
        {
            messageLog.Insert(0, message);
        }

        public class Message : ListItem
        {
            private string shortMessage;
            private string detailedMessage;
            private Entity sender;

            public Message(string text, Entity sender)
            {
                this.shortMessage = text;
                this.sender = sender;

                if (this.sender.EntityType == Entity.EntityTypes.Enemy)
                    this.TextColor = Color.Red;
                else if (this.sender.EntityType == Entity.EntityTypes.Player)
                    this.TextColor = Color.LimeGreen;
                else
                    this.TextColor = Color.White;

                this.detailedMessage = text;
            }
            public Message(string text)
            {
                this.shortMessage = text;
                this.TextColor = Color.White;

                this.detailedMessage = text;
            }

            public override string ListText { get { return shortMessage; } set { shortMessage = value; } }
            public string ShortMessage { get { return shortMessage; } set { shortMessage = value; } }
            public string DetailedMessage { get { return this.detailedMessage; } set { this.detailedMessage = value; } }
            public Entity Sender { get { return this.sender; } set { this.sender = value; } }
        }
    }
}
