public class ChatData
{
	public string Username { get; private set; }
	public string Message { get; private set; }
	
	public ChatData(string username, string message)
	{
		Username = username;
		Message = message;
	}
}