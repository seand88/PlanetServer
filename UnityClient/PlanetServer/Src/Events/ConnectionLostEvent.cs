using System;
using System.Collections.Generic;

namespace PS.Events
{
    /// <summary>
    /// Connection was lost to the server.
    /// </summary>
    public class ConnectionLostEvent : PsEvent
    {
        /// <summary>
        /// Initializes a new instance of the ConnectionLostEvent class.
        /// </summary>
        public ConnectionLostEvent() : base(MessageType.ConnectionLostEvent)
        {        
        }

        /// <summary>
        /// Override to pack the data for this event.
        /// </summary>
        /// <param name="dict">Key-Value pairs of data for this event.</param>
        override public void Create(Dictionary<string, object> dict)
        {
        }
    }
}
