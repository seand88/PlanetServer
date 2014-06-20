using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginController : MonoBehaviour
{
	private static int LOGIN_WIDTH = 400;
	private static int LOGIN_HEIGHT = 205;
	private static int SELECT_WIDTH = 450;
	private static int SELECT_HEIGHT = 210;
	private static int STATUS_WIDTH = 432;
	private static int STATUS_HEIGHT = 120;
	private static int FONT_SIZE = 24;

	private static int NUM_CHARS = 4;
	private static int CHAR_SPACING = 40;
	private static int CHAR_WIDTH = Constants.TILE_WIDTH * 2;
	private static int CHAR_HEIGHT = Constants.TILE_HEIGHT * 2;

	public string IP;
	public int Port;

	private Server _server;

	private string _username;
	private string _password;

	private string _status;

	private Rect _loginRect;
	private Rect _selectRect;
	private Rect _statusRect;
	private Rect _charRect;

	private bool _showLogin;
	private bool _showSelect;
	private bool _showStatus;

	private Texture2D _glow;
	private Texture2D[] _charTextures;

	void Start()
	{
		_server = Utility.FindComponent<Server>(Server.NAME);

		_server.ConnectionEvent += OnConnectionEvent;
		_server.ConnectionLostEvent += OnConnectionLost;
		_server.Connect(IP, Port);

		_showLogin = false;
		_showStatus = true;

		_username = "";
		_password = "";
		_status = "Connecting to: " + IP + " on port: " + Port + "...";

		_loginRect = new Rect(Screen.width / 2 - LOGIN_WIDTH / 2, Screen.height / 2 - LOGIN_HEIGHT / 2, LOGIN_WIDTH, LOGIN_HEIGHT);
		_selectRect = new Rect(Screen.width / 2 - SELECT_WIDTH / 2, Screen.height / 2 - SELECT_HEIGHT / 2, SELECT_WIDTH, SELECT_HEIGHT);
		_statusRect = new Rect(Screen.width / 2 - STATUS_WIDTH / 2, Screen.height / 2 + STATUS_HEIGHT, STATUS_WIDTH, STATUS_HEIGHT);
		_charRect = new Rect(Screen.width / 2 - (NUM_CHARS * CHAR_WIDTH + (NUM_CHARS - 1) * CHAR_SPACING) + 10, SELECT_HEIGHT / 2 - CHAR_HEIGHT / 2 - 20, (NUM_CHARS * CHAR_WIDTH + (NUM_CHARS - 1) * CHAR_SPACING), CHAR_HEIGHT);

		_glow = Resources.Load<Texture2D>("GUI/glow");

		_charTextures = new Texture2D[NUM_CHARS];
		for (int i = 0; i < NUM_CHARS; ++i)
			_charTextures[i] = Resources.Load<Texture2D>("GUI/char" + i + "_select");
	}

	void OnGUI()
	{
		if (_showLogin)
			ShowLogin();
		if (_showSelect)
			ShowCharacterSelect();
		if (_showStatus)
			ShowStatus();
	}

	void OnDestroy()
	{
		_server.ConnectionLostEvent -= OnConnectionLost;
	}

	private void OnConnectionEvent(Dictionary<string, object> message)
	{
		_server.ConnectionEvent -= OnConnectionEvent;

		bool success = (bool)message["success"];
		if (success)
		{
			_status += "\nConnected to server";
			_showLogin = true;
		}
		else
			_status += "\nError connecting to server: " + message["error"];
	}

	private void OnConnectionLost(Dictionary<string, object> message)
	{
		_showLogin = false;

		_status += "\nConnection lost";
	}

	private void OnLoginEvent(Dictionary<string, object> message)
	{
		_server.LoginEvent -= OnLoginEvent;

		bool success = (bool)message["success"];
		if (success)
		{
			_showLogin = false;
			_status += "\nLogged in...Starting game";

			_showSelect = true;
		}
		else
		{
			_status += "\nError logging in: " + message["message"];
		
			_username = "";
			_password = "";
		}
	}

	private void ShowLogin()
	{
		GUI.BeginGroup(_loginRect);

		int x = (LOGIN_WIDTH - 296) / 2;

		GUI.Box(new Rect(0, 0, LOGIN_WIDTH, LOGIN_HEIGHT), "");
		GUI.Label (new Rect(x, 5, 70, 37), "Username");
		_username = GUI.TextField(new Rect(x, 30, 296, FONT_SIZE), _username, FONT_SIZE);
		GUI.Label (new Rect(x, 70, 70, 37), "Password");
		_password = GUI.PasswordField(new Rect(x, 95, 296, FONT_SIZE), _password, "*"[0], FONT_SIZE);
		
		if (GUI.Button(new Rect((LOGIN_WIDTH - 120) / 2, 140, 120, 48), "Login") && _username.Length > 0 && _password.Length > 0)
			Login();

		GUI.EndGroup();
	}

	private void ShowCharacterSelect()
	{
		GUI.BeginGroup(_selectRect);

		GUI.Box(new Rect(0, 0, SELECT_WIDTH, SELECT_HEIGHT), "Select Character");

		float width = Constants.TILE_WIDTH * 1.75f;
		float height = Constants.TILE_HEIGHT * 1.75f;

		GUI.BeginGroup(_charRect);

		if (Event.current.type == EventType.MouseUp)
		{	
			for (int i = 0; i < NUM_CHARS; ++i)
			{
				Rect rect = new Rect(i * (CHAR_WIDTH + CHAR_SPACING), 0, CHAR_WIDTH, CHAR_HEIGHT);
				if (rect.Contains(Event.current.mousePosition))
				{
					PlayerPrefs.SetInt(Constants.PLAYER_TYPE, i);
					PlayerPrefs.Save();
				}
			}
		}

		int type = PlayerPrefs.GetInt(Constants.PLAYER_TYPE, 0);

		for (int i = 0; i < NUM_CHARS; ++i)
		{
			if (i == type)
				GUI.DrawTexture(new Rect(i * (CHAR_WIDTH + CHAR_SPACING), 0, CHAR_WIDTH, CHAR_HEIGHT), _glow);
			GUI.DrawTexture(new Rect(i * (CHAR_WIDTH + CHAR_SPACING), 0, CHAR_WIDTH, CHAR_HEIGHT), _charTextures[i]);
		}

		GUI.EndGroup();

		if (GUI.Button(new Rect((SELECT_WIDTH - 120) / 2, 140, 120, 48), "Login"))
			StartGame();

		GUI.EndGroup();
	}

	private void ShowStatus()
	{
		GUI.BeginGroup(_statusRect);

		GUI.Box(new Rect(0, 0, STATUS_WIDTH, STATUS_HEIGHT), "");
		GUI.Label(new Rect(8, 8, 424, 104), _status);

		GUI.EndGroup();
	}

	private void Login()
	{
		_server.LoginEvent += OnLoginEvent;

		_server.Login(_username, _password);
	}

	private void StartGame()
	{
		_status += "\nEntering world...";

		Application.LoadLevel("game");
	}
}
