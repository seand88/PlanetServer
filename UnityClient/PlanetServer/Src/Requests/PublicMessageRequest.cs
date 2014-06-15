using System;

using PS.Data;

namespace PS.Requests
{
    public class PublicMessageRequest : PsRequest
    {
        public static readonly string PM_MSG = "pm_msg";
        public static readonly string PM_DATA = "pm_data";

        public string Message { get; private set; }

        public PublicMessageRequest(string message) : base(RequestType.PublicMessage)
        {
            Message = message;

            PsObject psobj = new PsObject();
            psobj.SetString(PM_MSG, Message);

            Init(psobj);
        }

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
