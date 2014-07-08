using PS.Data;

namespace PS.Requests
{
    /// <summary>
    /// Send a login request to the server.
    /// </summary>
    public class LoginRequest : PsRequest
    {
        private static readonly string REQUEST_USERNAME = "username";
        private static readonly string REQUEST_PASSWORD = "password";

        /// <summary>
        /// Initializes a new instance of the LoginRequest class.
        /// </summary>
        /// <param name="username">Username to login with.</param>
        /// <param name="password">Password for the username.</param>
        public LoginRequest(string username, string password) : base(RequestType.Login)
        {
            PsObject obj = new PsObject();

            obj.SetString(REQUEST_USERNAME, username);
            obj.SetString(REQUEST_PASSWORD, password);

            Init(obj);
        }
    }
}
