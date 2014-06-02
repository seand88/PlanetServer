using System;
using System.Collections.Generic;

namespace PS.Events
{
    public sealed class MessageType
    {
        public static MessageType ConnectionEvent = new MessageType("connection_event", "PS.Events.ConnectionEvent");
        public static MessageType ConnectionLostEvent = new MessageType("connection_lost_event", "PS.Events.ConnectionLostEvent");
        public static MessageType LoginEvent = new MessageType("login_event", "PS.Events.LoginEvent");
        public static MessageType LogoutEvent = new MessageType("logout_event", "PS.Events.LogoutEvent");
        public static MessageType ExtensionEvent = new MessageType("extension_event", "PS.Events.ExtensionEvent");
        public static MessageType PublicMessageEvent = new MessageType("publicmessage_event", "PS.Events.PublicMessageEvent");

        private static Dictionary<string, MessageType> VALUES = new Dictionary<string, MessageType>()
        {
            { ConnectionEvent.Name, ConnectionEvent },
            { ConnectionLostEvent.Name, ConnectionLostEvent },
            { LoginEvent.Name, LoginEvent },
            { LogoutEvent.Name, LogoutEvent },
            { ExtensionEvent.Name, ExtensionEvent },
            { PublicMessageEvent.Name, PublicMessageEvent }
        };

        public static MessageType ValueOf(string name) { return VALUES[name]; }
        
        public string Name { get; private set; }
        public string Class { get; private set; }

        public MessageType(string name, string clazz)
        {
            Name = name;
            Class = clazz;
        }
    }
}
