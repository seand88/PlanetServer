using PS.Data;

namespace PS.Requests
{
    public class LoginRequest : PsRequest
    {
        public static readonly string REQUEST_USERNAME = "username";
        public static readonly string REQUEST_PASSWORD = "password";

        public LoginRequest(string username, string password) : base(RequestType.Login)
        {
            PsObject obj = new PsObject();

            obj.SetString(REQUEST_USERNAME, username);
            obj.SetString(REQUEST_PASSWORD, password);

            Init(obj);
        }
    }
}
