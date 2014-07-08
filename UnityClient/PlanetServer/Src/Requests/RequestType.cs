namespace PS.Requests
{
    /// <summary>
    /// Types of requests that can be sent.
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// No request.
        /// </summary>
        None = 0,
        /// <summary>
        /// Login request.
        /// </summary>
        Login,
        /// <summary>
        /// Logout request.
        /// </summary>
        Logout,
        /// <summary>
        /// Extension request.
        /// </summary>
        Extension,
        /// <summary>
        /// Public message request.
        /// </summary>
        PublicMessage
    }
}
