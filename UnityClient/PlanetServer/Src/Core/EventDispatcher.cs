using System;
using System.Collections;

namespace PlanetServer.Core
{
    public class EventDispatcher
    {
        private object _target;

        private Hashtable _dispatchers;

        public EventDispatcher(object target)
        {
            _target = target;

            _dispatchers = new Hashtable();
        }

        public void AddListener(string type, EventListenerDelegate listener)
        {
            EventListenerDelegate delegates;

            if (!_dispatchers.ContainsKey(type))
                _dispatchers.Add(type, null);

            delegates = (EventListenerDelegate)_dispatchers[type];
     
            delegates += listener;
            
            _dispatchers[type] = delegates;
        }

        public void RemoveListener(string type, EventListenerDelegate listener)
        {
            EventListenerDelegate delegates;

            if (_dispatchers.ContainsKey(type))
            {
                delegates = (EventListenerDelegate)_dispatchers[type];

                delegates -= listener;

                _dispatchers[type] = delegates;
            }
        }

        public void RemoveListeners()
        {
            _dispatchers.Clear();
        }

        public void DispatchEvent(PsEvent e)
        {
            EventListenerDelegate delegates = (EventListenerDelegate)_dispatchers[e.Type];
            if (delegates != null)
            {
                e.Target = _target;
                try
                {
                    delegates(e);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error dispatching event " + e.Type + ": " + ex.Message + " " + ex.StackTrace, ex);
                }
            }
        }
    }
}
