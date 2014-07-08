using System;
using System.Collections.Generic;

using PS.Data;

namespace PS.Events
{
    /// <summary>
    /// Response from sending an ExtensionRequest to the server.
    /// </summary>
    public class ExtensionEvent : PsEvent
    {
        private static readonly string REQUEST_COMMAND = "command";
        private static readonly string EXTENSION_DATA = "extension_data";

        /// <summary>
        /// Main command of the received message.
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// Optional routing command of the received message.
        /// </summary>
        public string SubCommand { get; private set; }
        /// <summary>
        /// Data associated with the request message.
        /// </summary>
        public PsObject Data { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ExtensionEvent class.
        /// </summary>
        public ExtensionEvent() : base(MessageType.ExtensionEvent)
        {

        }

        /// <summary>
        /// Override to pack the data for this event.
        /// </summary>
        /// <param name="dict">Key-Value pairs of data for this event.</param>
        override public void Create(Dictionary<string, object> dict)
        {
            Dictionary<string, object> tmp = (Dictionary<string, object>)dict[REQUEST_COMMAND];
            string str = Convert.ToString(tmp["v"]);
            
            string[] split = str.Split('.');
            Command = split[0];
            if (split.Length == 2)
                SubCommand = split[1];

            tmp = (Dictionary<string, object>)dict[EXTENSION_DATA];            
            Data = PsObject.Create((Dictionary<string, object>)tmp["v"]);
        }
    }
}
