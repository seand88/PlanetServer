using System;

using PlanetServer.Data;

namespace PlanetServer.Requests
{
    public class ExtensionRequest : PsRequest
    {
        public static readonly string REQUEST_COMMAND = "command";

        public string Command { get; private set; }

        public ExtensionRequest() : base(RequestType.Extension)
        {

        }

        public ExtensionRequest(string command, PsObject obj) : base(RequestType.Extension)
		{
			Command = command;

            obj.SetString(REQUEST_COMMAND, Command);

            Init(obj);
		}
    }
}
