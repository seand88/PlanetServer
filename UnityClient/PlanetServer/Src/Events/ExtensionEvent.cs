using System;
using System.Collections.Generic;

using PS.Data;

namespace PS.Events
{
    public class ExtensionEvent : PsEvent
    {
        public static readonly string EXTENSION_DATA = "extension_data";

        public PsObject Data { get; private set; }

        public ExtensionEvent() : base(MessageType.ExtensionEvent)
        {

        }

        override public void Create(Dictionary<string, object> dict)
        {
            Dictionary<string, object> tmp = (Dictionary<string, object>)dict[EXTENSION_DATA];
            Data = PsObject.Create((Dictionary<string, object>)tmp["v"]);
        }
    }
}
