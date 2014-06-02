using System;
using System.Collections.Generic;

using PS.Data;

namespace PS.Events
{
    public class LoginEvent : PsEvent
    {
        public static readonly string LOGIN_SUCCESS = "login_success";
        public static readonly string LOGIN_MSG = "login_msg";
        public static readonly string LOGIN_DATA = "login_data";        
        
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public PsObject Data { get; private set; }

        public LoginEvent() : base(MessageType.LoginEvent)
        {

        }

        override public void Create(Dictionary<string, object> dict)
        {
            Dictionary<string, object> tmp = (Dictionary<string, object>)dict[LOGIN_SUCCESS];

            Success = Convert.ToBoolean(tmp["v"]);
            if (dict.ContainsKey(LOGIN_MSG))
            {
                tmp = (Dictionary<string, object>)dict[LOGIN_MSG];
                Message = Convert.ToString(tmp["v"]);
            }
            if (dict.ContainsKey(LOGIN_DATA))
            {
                tmp = (Dictionary<string, object>)dict[LOGIN_DATA];
                Data = PsObject.Create((Dictionary<string, object>)tmp["v"]);
            }
        }
    }
}
