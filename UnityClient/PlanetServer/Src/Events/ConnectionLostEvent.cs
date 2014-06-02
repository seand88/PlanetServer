using System;
using System.Collections.Generic;

namespace PS.Events
{
    public class ConnectionLostEvent : PsEvent
    {
        public ConnectionLostEvent() : base(MessageType.ConnectionLostEvent)
        {        
        }

        override public void Create(Dictionary<string, object> dict)
        {
        }
    }
}
