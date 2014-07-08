using System;

using PS.Data;

namespace PS.Requests
{
    /// <summary>
    /// Send a chat message to everybody currently connected to the server.
    /// </summary>
    public class PublicMessageRequest : PsRequest
    {
        private static readonly string PM_MSG = "pm_msg";
        private static readonly string PM_DATA = "pm_data";

        /// <summary>
        /// Message to be sent to the server.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the PublicMessageRequest class.
        /// </summary>
        /// <param name="message">Message to be sent to the server.</param>
        public PublicMessageRequest(string message) : base(RequestType.PublicMessage)
        {
            Message = message;

            PsObject psobj = new PsObject();
            psobj.SetString(PM_MSG, Message);

            Init(psobj);
        }

        /// <summary>
        /// Initializes a new instance of the PublicMessageRequest class.
        /// </summary>
        /// <param name="message">Message to be sent to the server.</param>
        /// <param name="obj">Data to be sent to the server with this message.</param>
        public PublicMessageRequest(string message, PsObject obj) : base(RequestType.PublicMessage)
        {
            Message = message;

            PsObject psobj = new PsObject();
            psobj.SetString(PM_MSG, Message);
            psobj.SetPsObject(PM_DATA, obj);

            Init(psobj);
        }
    }
}
