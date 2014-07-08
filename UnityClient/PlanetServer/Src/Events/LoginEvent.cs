using System;
using System.Collections.Generic;

using PS.Data;

namespace PS.Events
{
    /// <summary>
    /// Response from logging into the server.
    /// </summary>
    public class LoginEvent : PsEvent
    {
        private static readonly string LOGIN_SUCCESS = "login_success";
        private static readonly string LOGIN_MSG = "login_msg";
        private static readonly string LOGIN_DATA = "login_data";        
        
        /// <summary>
        /// Success of the login.
        /// </summary>
        public bool Success { get; private set; }
        /// <summary>
        /// Message from the login. (if any).
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Data from the login (if any).
        /// </summary>
        public PsObject Data { get; private set; }

        /// <summary>
        /// Initializes a new instance of the LoginEvent class.
        /// </summary>
        public LoginEvent() : base(MessageType.LoginEvent)
        {

        }

        /// <summary>
        /// Override to pack the data for this event.
        /// </summary>
        /// <param name="dict">Key-Value pairs of data for this event.</param>
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
