using PS.Data;

namespace PS.Requests
{
    /// <summary>
    /// Send a logout request to the server.
    /// </summary>
    public class LogoutRequest : PsRequest
    {
        /// <summary>
        /// Initializes a new instance of the LogoutRequest class.
        /// </summary>
        public LogoutRequest() : base(RequestType.Logout)
        {
            PsObject obj = new PsObject();

            Init(obj);
        }
    }
}
