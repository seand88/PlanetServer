using UnityEngine;
using System.Collections.Generic;

using PS.Events;
using PS.Requests;

public class ChatController : MonoBehaviour 
{
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
		_server = Utility.FindComponent<Server>(Server.NAME);
		_server.PublicMessageEvent += OnPublicMesasge;

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
		_server.PublicMessageEvent -= OnPublicMesasge;
	}

	public void UserAction(string username, ChatAction action)
	{
		string message = "";

		if (action == ChatAction.Enter)
			message = "Entered chat";
		else if (action == ChatAction.Leave)
			message = "Left chat";

		_messages.Add(new ChatData(username, message));
	}

	private void ShowChat()
	{
		GUI.BeginGroup(_chatRect);
		
		GUI.Box(new Rect(0, 0, WIDTH, HEIGHT), "");
		
		int list_height = (_messages.Count * SIZE > 160) ? _messages.Count * SIZE : 160;
		
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
			_server.SendRequest(new PublicMessageRequest(_chat));
			_chat = "";
		}
		
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab)
			_showChat = !_showChat;
	}
	
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

	private void OnPublicMesasge(Dictionary<string, object> message)
	{
		_messages.Add(new ChatData((string)message["username"], (string)message["message"]));

		_index = new Vector2(0, _messages.Count * SIZE);
		
		if (!_showChat)
			_showIcon = true;
	}
}
