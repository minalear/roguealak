using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using Roguelike.Core.Entities;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Core
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
                shortMessage = text;
                this.sender = sender;

                if (sender.EntityType == Entity.EntityTypes.Enemy)
                    TextColor = Color4.Red;
                else if (sender.EntityType == Entity.EntityTypes.Player)
                    TextColor = Color4.LimeGreen;
                else
                    TextColor = Color4.White;

                detailedMessage = text;
            }
            public Message(string text)
            {
                shortMessage = text;
                TextColor = Color4.White;

                detailedMessage = text;
            }

            public override string ListText { get { return shortMessage; } set { shortMessage = value; } }
            public string ShortMessage { get { return shortMessage; } set { shortMessage = value; } }
            public string DetailedMessage { get { return detailedMessage; } set { detailedMessage = value; } }
            public Entity Sender { get { return sender; } set { sender = value; } }
        }
    }
}
