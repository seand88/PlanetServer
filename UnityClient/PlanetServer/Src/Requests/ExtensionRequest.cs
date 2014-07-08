using System;

using PS.Data;

namespace PS.Requests
{
    /// <summary>
    /// Send a extension request to the server.
    /// </summary>
    public class ExtensionRequest : PsRequest
    {
        private static readonly string REQUEST_COMMAND = "command";

        /// <summary>
        /// Command for this request.
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ExtensionRequest class.
        /// </summary>
        public ExtensionRequest() : base(RequestType.Extension)
        {

        }

        /// <summary>
        /// Initializes a new instance of the ExtensionRequest class.
        /// </summary>
        /// <param name="command">Command for this request.  A subcommand may also be specified using the format "command.subcommand".</param>
        /// <param name="obj">Data for the request.</param>
        public ExtensionRequest(string command, PsObject obj) : base(RequestType.Extension)
		{
			Command = command;

            obj.SetString(REQUEST_COMMAND, Command);

            Init(obj);
		}
    }
}
