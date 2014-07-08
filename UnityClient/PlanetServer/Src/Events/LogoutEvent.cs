using System;
using System.Collections.Generic;

using PS.Data;

namespace PS.Events
{
    /// <summary>
    /// Response from logging out of the server.
    /// </summary>
    public class LogoutEvent : PsEvent
    {
        /// <summary>
        /// Initializes a new instance of LogoutEvent.
        /// </summary>
        public LogoutEvent() : base(MessageType.LogoutEvent)
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
