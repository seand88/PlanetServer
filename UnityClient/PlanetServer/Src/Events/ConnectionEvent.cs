using System;
using System.Collections.Generic;

using PS.Data;

namespace PS.Events
{
    public class ConnectionEvent : PsEvent
    {
        public bool Success { get; private set; }
        public string Error { get; private set; }

        public ConnectionEvent() : base(MessageType.ConnectionEvent)
        {        
        }

        override public void Create(Dictionary<string, object> dict)
        {
            Success = (bool)dict["success"];
            if (dict.ContainsKey("error"))
                Error = (string)dict["error"];
        }
    }
}
