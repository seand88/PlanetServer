using System;
using System.Collections.Generic;

using PS.Data;

namespace PS.Events
{
    /// <summary>
    /// Response from connection attempt to server.
    /// </summary>
    public class ConnectionEvent : PsEvent
    {
        /// <summary>
        /// Success of connection to the server.
        /// </summary>
        public bool Success { get; private set; }
        /// <summary>
        /// Error from connecting to server (if any).
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ConnectionEvent class.
        /// </summary>
        public ConnectionEvent() : base(MessageType.ConnectionEvent)
        {        
        }

        /// <summary>
        /// Override to pack the data for this event.
        /// </summary>
        /// <param name="dict">Key-Value pairs of data for this event.</param>
        override public void Create(Dictionary<string, object> dict)
        {
            Success = (bool)dict["success"];
            if (dict.ContainsKey("error"))
                Error = (string)dict["error"];
        }
    }
}
