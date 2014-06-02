using System;
using System.Collections.Generic;

using PS.Data;

namespace PS.Events
{
    public class LogoutEvent : PsEvent
    {
        public LogoutEvent() : base(MessageType.LogoutEvent)
        {

        }

        override public void Create(Dictionary<string, object> dict)
        {

        }
    }
}
