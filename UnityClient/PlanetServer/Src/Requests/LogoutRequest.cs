using PS.Data;

namespace PS.Requests
{
    public class LogoutRequest : PsRequest
    {
        public LogoutRequest() : base(RequestType.Logout)
        {
            PsObject obj = new PsObject();

            Init(obj);
        }
    }
}
