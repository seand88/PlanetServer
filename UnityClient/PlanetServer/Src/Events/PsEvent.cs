using System.Collections.Generic;

using PS.Data;

namespace PS.Events
{
    /// <summary>
    /// Base event all PlanetServer events extend.
    /// </summary>
    public abstract class PsEvent
    {
        /// <summary>
        /// Type of message.
        /// </summary>
        public MessageType Type { get; protected set; }
        
        /// <summary>
        /// Initializes a new instance of the PsEvent class.
        /// </summary>
        /// <param name="type">Type of event.</param>
        public PsEvent(MessageType type)
        {
            Type = type;
        }

        /// <summary>
        /// Pack the data for this event.
        /// </summary>
        /// <param name="dict">Key-Value pairs of data for this event.</param>
        public abstract void Create(Dictionary<string, object> dict);
    }
}
