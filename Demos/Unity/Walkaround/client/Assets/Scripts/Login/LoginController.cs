using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginController : MonoBehaviour
{
	public string IP;
	public int Port;

	private Server _server;

	private string _username;
	private string _password;

	private string _status;

	private bool _showLogin;
	private bool _showStatus;

	void Start()
	{
		_server = Utility.GetServer();

		_server.ConnectionEvent += OnConnectionEvent;
		_server.Connect(IP, Port);

		_showLogin = false;
		_showStatus = true;

		_username = "";
		_password = "";
		_status = "Connecting to: " + IP + " on port: " + Port + "...";
	}

	void OnGUI()
	{
		if (_showLogin)
			ShowLogin();
		if (_showStatus)
			ShowStatus();
	}

	private void OnConnectionEvent(Dictionary<string, object> data)
	{
		_server.ConnectionEvent -= OnConnectionEvent;

		bool success = (bool)data["success"];
		if (success)
		{
			_status += "\nConnected to server";
			_showLogin = true;
		}
		else
			_status += "\nError connecting to server: " + data["error"];
	}

	private void OnLoginEvent(Dictionary<string, object> data)
	{
		_server.LoginEvent -= OnLoginEvent;

		bool success = (bool)data["success"];
		if (success)
		{
			_showLogin = false;
			_status += "\nLogged in...Starting game";

			Application.LoadLevel("game");
		}
		else
		{
			_status += "\nError logging in: " + data["message"];
		
			_username = "";
			_password = "";
		}
	}

	private void ShowLogin()
	{
		int half_width = Screen.width / 2;
		int half_height = Screen.height / 2;

		GUI.Box(new Rect(half_width - 200, half_height - 170, 400, 205), "");
		GUI.Label (new Rect(half_width - 192, half_height - 130, 70, 37), "Username");
		_username = GUI.TextField(new Rect(half_width - 120, half_height - 137, 296, 37), _username, 24);
		GUI.Label (new Rect(half_width - 192, half_height - 80, 70, 37), "Password");
		_password = GUI.PasswordField(new Rect(half_width - 120, half_height - 87, 296, 37), _password, "*"[0], 24);
		
		if(GUI.Button(new Rect(half_width - 61, half_height - 24, 122, 48), "Login") && _username.Length > 0 && _password.Length > 0)
			Login();
	}

	private void ShowStatus()
	{
		int half_width = Screen.width / 2;
		int half_height = Screen.height / 2;

		GUI.Box(new Rect(half_width - 160, half_height + 150, 320, 120), "");
		GUI.Label(new Rect(half_width - 152, half_height + 158, 304, 104), _status);
	}

	private void Login()
	{
		_server.LoginEvent += OnLoginEvent;

		_server.Login(_username, _password);
	}
}
