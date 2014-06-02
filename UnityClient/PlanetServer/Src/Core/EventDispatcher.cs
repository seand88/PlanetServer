using System;
using System.Collections;
using System.ComponentModel;

using PS.Events;

namespace PS.Core
{
    public class EventDispatcher
    {
        private readonly EventHandlerList _handlers = new EventHandlerList();

        public event EventDelegate<ConnectionEvent> ConnectionEvent
        {
            add { AddEventHandler(MessageType.ConnectionEvent, value); }
            remove { RemoveEventHandler(MessageType.ConnectionEvent, value); }
        }

        public event EventDelegate<ConnectionLostEvent> ConnectionLostEvent
        {
            add { AddEventHandler(MessageType.ConnectionLostEvent, value); }
            remove { RemoveEventHandler(MessageType.ConnectionLostEvent, value); }
        }

        public event EventDelegate<LoginEvent> LoginEvent
        {
            add { AddEventHandler(MessageType.LoginEvent, value); }
            remove { RemoveEventHandler(MessageType.LoginEvent, value); }
        }

        public event EventDelegate<LogoutEvent> LogoutEvent
        {
            add { AddEventHandler(MessageType.LoginEvent, value); }
            remove { RemoveEventHandler(MessageType.LoginEvent, value); }
        }

        public event EventDelegate<ExtensionEvent> ExtensionEvent
        {
            add { AddEventHandler(MessageType.LoginEvent, value); }
            remove { RemoveEventHandler(MessageType.LoginEvent, value); }
        }

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
            _handlers.RemoveHandler(type.ToString(), handler);
        }               

        public void DispatchEvent(PsEvent e)
        {
            Delegate handler = _handlers[e.Type.Name];
            if (handler != null)
                handler.DynamicInvoke(new object[] { e });
        }
    }
}
