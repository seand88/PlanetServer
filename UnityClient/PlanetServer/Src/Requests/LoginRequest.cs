using PlanetServer.Data;

namespace PlanetServer.Requests
{
    public class LoginRequest : PsRequest
    {
        public static readonly string REQUEST_PLATFORM = "platform";
        public static readonly string REQUEST_USERID = "userid";
        public static readonly string REQUEST_PASSWORD = "authtoken";

        public LoginRequest(string username, string password) : base(RequestType.Login)
        {
            PsObject obj = new PsObject();

            obj.SetString(REQUEST_PLATFORM, "test");
            obj.SetString(REQUEST_USERID, username);
            obj.SetString(REQUEST_PASSWORD, password);

            Init(obj);
        }
    }
}
