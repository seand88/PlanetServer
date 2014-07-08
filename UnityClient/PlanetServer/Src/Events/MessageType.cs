using System;
using System.Collections.Generic;

namespace PS.Events
{
    /// <summary>
    /// Helper class to find paths to event classes for dispatching.
    /// </summary>
    public sealed class MessageType
    {
        /// <summary>
        /// ConnectionEvent.
        /// </summary>
        public static MessageType ConnectionEvent = new MessageType("connection_event", "PS.Events.ConnectionEvent");
        /// <summary>
        /// ConnectionLostEvent.
        /// </summary>
        public static MessageType ConnectionLostEvent = new MessageType("connection_lost_event", "PS.Events.ConnectionLostEvent");
        /// <summary>
        /// LoginEvent.
        /// </summary>
        public static MessageType LoginEvent = new MessageType("login_event", "PS.Events.LoginEvent");
        /// <summary>
        /// LogoutEvent.
        /// </summary>
        public static MessageType LogoutEvent = new MessageType("logout_event", "PS.Events.LogoutEvent");
        /// <summary>
        /// ExtensionEvent.
        /// </summary>
        public static MessageType ExtensionEvent = new MessageType("extension_event", "PS.Events.ExtensionEvent");
        /// <summary>
        /// PublicMessageEvent.
        /// </summary>
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

        /// <summary>
        /// Find the MessageType from the name.
        /// </summary>
        /// <param name="name">Name of MessageType.</param>
        /// <returns></returns>
        public static MessageType ValueOf(string name) { return VALUES[name]; }
        
        /// <summary>
        /// Lookup name of class.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Class package.
        /// </summary>
        public string Class { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MessageType class.
        /// </summary>
        /// <param name="name">Lookup name of class.</param>
        /// <param name="clazz">Class package.</param>
        public MessageType(string name, string clazz)
        {
            Name = name;
            Class = clazz;
        }
    }
}
