using System;
using System.Collections.Generic;

using PS.Data;

namespace PS.Events
{
    public class ExtensionEvent : PsEvent
    {
        public static readonly string REQUEST_COMMAND = "command";
        public static readonly string EXTENSION_DATA = "extension_data";

        public string Command { get; private set; }
        public string SubCommand { get; private set; }
        public PsObject Data { get; private set; }

        public ExtensionEvent() : base(MessageType.ExtensionEvent)
        {

        }

        override public void Create(Dictionary<string, object> dict)
        {
            Dictionary<string, object> tmp = (Dictionary<string, object>)dict[REQUEST_COMMAND];
            Command = Convert.ToString(tmp["v"]);

            string[] split = Command.Split('.');
            if (split.Length == 2)
                SubCommand = split[1];

            tmp = (Dictionary<string, object>)dict[EXTENSION_DATA];            
            Data = PsObject.Create((Dictionary<string, object>)tmp["v"]);
        }
    }
}
