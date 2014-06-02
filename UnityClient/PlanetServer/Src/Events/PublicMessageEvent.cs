using System;
using System.Collections.Generic;

using PS.Data;

namespace PS.Events
{
    public class PublicMessageEvent : PsEvent
    {
        public static readonly string PM_USER = "pm_user";
        public static readonly string PM_MSG = "pm_msg";
        
        public string User { get; private set; }
        public string Message { get; private set; }

        public PublicMessageEvent() : base(MessageType.PublicMessageEvent)
        {

        }

        override public void Create(Dictionary<string, object> dict)
        {
            Dictionary<string, object> tmp = (Dictionary<string, object>)dict[PM_USER];
            User = Convert.ToString(tmp["v"]);

            tmp = (Dictionary<string, object>)dict[PM_MSG];
            Message = Convert.ToString(tmp["v"]);           
        }
    }
}
