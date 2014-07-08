using System;
using System.Collections.Generic;

using PS.Data;

namespace PS.Events
{
    /// <summary>
    /// Message sent from another player to all players on the server.
    /// </summary>
    public class PublicMessageEvent : PsEvent
    {
        private static readonly string PM_USER = "pm_user";
        private static readonly string PM_MSG = "pm_msg";
        private static readonly string PM_DATA = "pm_data";
        
        /// <summary>
        /// Username of player that sent the message.
        /// </summary>
        public string User { get; private set; }
        /// <summary>
        /// Message the player sent.
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Data that was sent with the message (if any).
        /// </summary>
        public PsObject Data { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the PublicMessageEvent class.
        /// </summary>
        public PublicMessageEvent() : base(MessageType.PublicMessageEvent)
        {
            
        }

        /// <summary>
        /// Override to pack the data for this event.
        /// </summary>
        /// <param name="dict">Key-Value pairs of data for this event.</param>
        override public void Create(Dictionary<string, object> dict)
        {        
            Dictionary<string, object> tmp = (Dictionary<string, object>)dict[PM_USER];
            User = Convert.ToString(tmp["v"]);

            tmp = (Dictionary<string, object>)dict[PM_MSG];
            Message = Convert.ToString(tmp["v"]);

            if (dict.ContainsKey(PM_DATA))
            {
                tmp = (Dictionary<string, object>)dict[PM_DATA];
                Data = PsObject.Create((Dictionary<string, object>)tmp["v"]);
            }
        }
    }
}
