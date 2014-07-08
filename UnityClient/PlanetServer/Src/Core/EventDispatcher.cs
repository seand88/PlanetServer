using System;
using System.Collections;
using System.ComponentModel;

using PS.Events;

namespace PS.Core
{
    /// <summary>
    /// Holds the list of event handlers.
    /// </summary>
    public class EventDispatcher
    {
        private readonly EventHandlerList _handlers = new EventHandlerList();

        /// <summary>
        /// ConnectionEvent handler.
        /// </summary>
        public event EventDelegate<ConnectionEvent> ConnectionEvent
        {
            add { AddEventHandler(MessageType.ConnectionEvent, value); }
            remove { RemoveEventHandler(MessageType.ConnectionEvent, value); }
        }

        /// <summary>
        /// ConnectionLostEvent handler.
        /// </summary>
        public event EventDelegate<ConnectionLostEvent> ConnectionLostEvent
        {
            add { AddEventHandler(MessageType.ConnectionLostEvent, value); }
            remove { RemoveEventHandler(MessageType.ConnectionLostEvent, value); }
        }

        /// <summary>
        /// LoginEvent handler.
        /// </summary>
        public event EventDelegate<LoginEvent> LoginEvent
        {
            add { AddEventHandler(MessageType.LoginEvent, value); }
            remove { RemoveEventHandler(MessageType.LoginEvent, value); }
        }

        /// <summary>
        /// LogoutEvent handler.
        /// </summary>
        public event EventDelegate<LogoutEvent> LogoutEvent
        {
            add { AddEventHandler(MessageType.LoginEvent, value); }
            remove { RemoveEventHandler(MessageType.LoginEvent, value); }
        }

        /// <summary>
        /// ExtensionEvent handler.
        /// </summary>
        public event EventDelegate<ExtensionEvent> ExtensionEvent
        {
            add { AddEventHandler(MessageType.ExtensionEvent, value); }
            remove { RemoveEventHandler(MessageType.ExtensionEvent, value); }
        }

        /// <summary>
        /// PublicMessageEvent handler.
        /// </summary>
        public event EventDelegate<PublicMessageEvent> PublicMessageEvent
        {
            add { AddEventHandler(MessageType.PublicMessageEvent, value); }
            remove { RemoveEventHandler(MessageType.PublicMessageEvent, value); }
        }

        private void AddEventHandler(MessageType type, Delegate handler)
        {
            _handlers.AddHandler(type.Name, handler);
        }

        private void RemoveEventHandler(MessageType type, Delegate handler)
        {
            _handlers.RemoveHandler(type.Name, handler);
        }               

        /// <summary>
        /// Dispatches the message to the appropiate handler for the given message.
        /// </summary>
        /// <param name="e"></param>
        public void DispatchEvent(PsEvent e)
        {
            Delegate handler = _handlers[e.Type.Name];
            if (handler != null)
                handler.DynamicInvoke(new object[] { e });
        }
    }
}
