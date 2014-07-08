using UnityEngine;
using System.Collections.Generic;

using PS.Events;
using PS.Requests;

/// <summary>
/// Draws the GUI for the chat window as well as handling the server messages for chat.
/// </summary>
public class ChatController : MonoBehaviour 
{
	// used to find the chat controller in the sea of gameobjects
	public static string NAME = "ChatController";

	private static int SIZE = 20;
	private static int WIDTH = 650;
	private static int HEIGHT = 200;
	private static int ICON_WIDTH = 32;
	private static int ICON_HEIGHT = 32;

	private Server _server;

	private Rect _chatRect;
	private Rect _iconRect;

	private Texture2D _chatIcon;
	
	private List<ChatData> _messages;
	
	private Vector2 _index;
	private string _chat;
	private bool _showChat;
	private bool _showIcon;

	void Start()
	{
		// find the server so that we can interact with it
		_server = Utility.FindComponent<Server>(Server.NAME);
		// add a listener for any public messages received
		_server.PublicMessageEvent += OnPublicMesasge;

		// some helper rects to make it easier to draw the gui
		_chatRect = new Rect(Screen.width / 2 - WIDTH / 2, Screen.height - HEIGHT, WIDTH, HEIGHT);
		_iconRect = new Rect(Screen.width / 2 - ICON_WIDTH / 2, Screen.height - ICON_HEIGHT, ICON_WIDTH, ICON_HEIGHT);

		_chatIcon = Resources.Load<Texture2D>("Chat/chat");
		
		_messages = new List<ChatData>();
		
		_index = Vector2.zero;
		_showChat = false;
		_showIcon = false;
		_chat = "";
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			_showChat = !_showChat;
			_showIcon = false;
		}
	}
	
	void OnGUI()
	{
		if (_showChat)
			ShowChat();
		if (_showIcon)
			ShowIcon();
	}

	void OnDestroy()
	{
		// remove the listener so the controller can get garbage collected away
		_server.PublicMessageEvent -= OnPublicMesasge;
	}

	/// <summary>
	/// Update the chat log when a player joins or leaves the server.
	/// </summary>
	/// <param name="username">Username of the player.</param>
	/// <param name="action">Action of the player.</param>
	public void UserAction(string username, ChatAction action)
	{
		string message = "";

		if (action == ChatAction.Enter)
			message = "Entered chat";
		else if (action == ChatAction.Leave)
			message = "Left chat";

		_messages.Add(new ChatData(username, message));
	}

	// draw the chat window, inputbox, and all received messages
	private void ShowChat()
	{
		GUI.BeginGroup(_chatRect);
		
		GUI.Box(new Rect(0, 0, WIDTH, HEIGHT), "");
		
		int list_height = (_messages.Count * SIZE > 160) ? _messages.Count * SIZE : 160;

		// draw the list of all the recieved chat messages
		_index = GUI.BeginScrollView(new Rect(10, 5, WIDTH - 10, 160), _index, new Rect(0, 0, WIDTH - 40, list_height), false, true);
		for (int i = 0; i < _messages.Count; ++i)
		{
			ChatData message = _messages[i];
			
			GUI.Label(new Rect(0, i * SIZE, 580, 25), message.Username + ": " + message.Message);
		}
		GUI.EndScrollView();
		
		GUI.FocusControl("chat");
		GUI.SetNextControlName("chat");
		
		_chat = GUI.TextField(new Rect(0, HEIGHT - 30, WIDTH, 30), _chat, 75);
		
		GUI.EndGroup();
		
		if (Event.current.isKey && _chat.Length > 0 && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
		{
			// send whatever is in the inputbox as the message.  no filtering of the messages is being done  
			_server.SendRequest(new PublicMessageRequest(_chat));
			_chat = "";
		}
		
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab)
			_showChat = !_showChat;
	}

	// show a notification there is a new message
	private void ShowIcon()
	{
		if (Event.current.type == EventType.Repaint)
		{
			GUI.BeginGroup(_iconRect);

			GUI.Box(new Rect(0, 0, ICON_WIDTH, ICON_HEIGHT), "");
			GUI.DrawTexture(new Rect(2, 2, 28, 28), _chatIcon);

			GUI.EndGroup();
		}
	}

	// called when a new message is recieved from the server
	private void OnPublicMesasge(Dictionary<string, object> message)
	{
		_messages.Add(new ChatData((string)message["username"], (string)message["message"]));

		_index = new Vector2(0, _messages.Count * SIZE);
		
		if (!_showChat)
			_showIcon = true;
	}
}
