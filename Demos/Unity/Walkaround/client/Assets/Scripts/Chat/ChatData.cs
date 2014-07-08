/// <summary>
/// Hold data for chat messages
/// </summary>
public class ChatData
{
	/// <summary>
	/// Username for the message.
	/// </summary>
	/// <value>The username.</value>
	public string Username { get; private set; }
	/// <summary>
	/// The message.
	/// </summary>
	/// <value>The message.</value>
	public string Message { get; private set; }

	/// <summary>
	/// Initializes a new instance of the ChatData class.
	/// </summary>
	/// <param name="username">Username for the message.</param>
	/// <param name="message">The message.</param>
	public ChatData(string username, string message)
	{
		Username = username;
		Message = message;
	}
}