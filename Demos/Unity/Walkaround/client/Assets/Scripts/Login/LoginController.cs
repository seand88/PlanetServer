using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles connecting to the server, login, and character selection.
/// </summary>
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
		// find the server so that we can interact with it
		_server = Utility.FindComponent<Server>(Server.NAME);

		// listen for some events we care about
		_server.ConnectionEvent += OnConnectionEvent;
		_server.ConnectionLostEvent += OnConnectionLost;

		_showLogin = false;
		_showStatus = true;

		_username = "";
		_password = "";
		_status = "Connecting to: " + IP + " on port: " + Port + "...";

		// some helper rects to make it easier to draw the gui
		_loginRect = new Rect(Screen.width / 2 - LOGIN_WIDTH / 2, Screen.height / 2 - LOGIN_HEIGHT / 2, LOGIN_WIDTH, LOGIN_HEIGHT);
		_selectRect = new Rect(Screen.width / 2 - SELECT_WIDTH / 2, Screen.height / 2 - SELECT_HEIGHT / 2, SELECT_WIDTH, SELECT_HEIGHT);
		_statusRect = new Rect(Screen.width / 2 - STATUS_WIDTH / 2, Screen.height / 2 + STATUS_HEIGHT, STATUS_WIDTH, STATUS_HEIGHT);
		_charRect = new Rect(Screen.width / 2 - (NUM_CHARS * CHAR_WIDTH + (NUM_CHARS - 1) * CHAR_SPACING) + 10, SELECT_HEIGHT / 2 - CHAR_HEIGHT / 2 - 20, (NUM_CHARS * CHAR_WIDTH + (NUM_CHARS - 1) * CHAR_SPACING), CHAR_HEIGHT);

		// load the resources for character select
		_glow = Resources.Load<Texture2D>("GUI/glow");

		_charTextures = new Texture2D[NUM_CHARS];
		for (int i = 0; i < NUM_CHARS; ++i)
			_charTextures[i] = Resources.Load<Texture2D>("GUI/char" + i + "_select");

		// try and connect to the server
		_server.Connect(IP, Port);
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
		// remove the listener so the controller can get garbage collected away
		_server.ConnectionLostEvent -= OnConnectionLost;
	}

	// let the player know what happened when they tried to connect
	private void OnConnectionEvent(Dictionary<string, object> message)
	{
		_server.ConnectionEvent -= OnConnectionEvent;

		bool success = (bool)message["success"];
		if (success)
		{
			// let the player be able to enter their username/password
			_status += "\nConnected to server";
			_showLogin = true;
		}
		// let them know what they couldn't connect
		else
			_status += "\nError connecting to server: " + message["error"];
	}

	// not much to do here other then tell them.  trying to reconnect in a few seconds would be a nice thing to do
	private void OnConnectionLost(Dictionary<string, object> message)
	{
		_showLogin = false;

		_status += "\nConnection lost";
	}

	// let the player know what happened when logging in
	private void OnLoginEvent(Dictionary<string, object> message)
	{
		_server.LoginEvent -= OnLoginEvent;

		bool success = (bool)message["success"];
		if (success)
		{
			// let the player be able to select their character
			_showLogin = false;
			_status += "\nLogged in...Starting game";

			_showSelect = true;
		}
		else
		{
			// let them know why they couldn't log in
			_status += "\nError logging in: " + message["message"];
		
			_username = "";
			_password = "";
		}
	}

	private void ShowLogin()
	{
		GUI.BeginGroup(_loginRect);

		int x = (LOGIN_WIDTH - 296) / 2;

		// draw a box with an input field for the username and password
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

		GUI.BeginGroup(_charRect);

		if (Event.current.type == EventType.MouseUp)
		{	
			for (int i = 0; i < NUM_CHARS; ++i)
			{
				Rect rect = new Rect(i * (CHAR_WIDTH + CHAR_SPACING), 0, CHAR_WIDTH, CHAR_HEIGHT);
				if (rect.Contains(Event.current.mousePosition))
				{
					// save the character they selected so it can be grabbed when the game starts
					PlayerPrefs.SetInt(Constants.PLAYER_TYPE, i);
					PlayerPrefs.Save();
				}
			}
		}

		int type = PlayerPrefs.GetInt(Constants.PLAYER_TYPE, 0);

		// show a highlight glow for the currently selected character
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

		// show a box a textfield with all the accumlated status messages
		GUI.Box(new Rect(0, 0, STATUS_WIDTH, STATUS_HEIGHT), "");
		GUI.Label(new Rect(8, 8, 424, 104), _status);

		GUI.EndGroup();
	}

	// try and log in with the username and password given
	private void Login()
	{
		_server.LoginEvent += OnLoginEvent;

		_server.Login(_username, _password);
	}

	// finally start the game
	private void StartGame()
	{
		_status += "\nEntering world...";

		Application.LoadLevel("game");
	}
}
