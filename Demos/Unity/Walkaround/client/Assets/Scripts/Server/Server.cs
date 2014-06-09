using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PS.Core;
using PS.Data;
using PS.Events;
using PS.Requests;

public delegate void ServerEvent(Dictionary<string, object> message);

public class Server : MonoBehaviour
{
	public event ServerEvent ConnectionEvent;
	public event ServerEvent LoginEvent;
	public event ServerEvent ExtensionEvent;

	public static string NAME = "PSServer";

	PlanetServer _server;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	void FixedUpdate()
	{
		if (_server != null)
			_server.DispatchEvents();
	}

	void OnApplicationQuit()
	{
		if (_server != null)
		{
			_server.EventDispatcher.ExtensionEvent -= OnResponse;

			_server.Send(new LogoutRequest());
		}
	}

	public void Connect(string ip, int port)
	{
		_server = new PlanetServer();

		_server.EventDispatcher.ConnectionEvent += OnConnection;

		_server.Connect(ip, port);
	}

	public void Login(string username, string password)
	{
		_server.EventDispatcher.LoginEvent += OnLogin;

		LoginRequest login = new LoginRequest(username, password);

		_server.Send(login);
	}

	public void SendRequest(PsRequest request)
	{
		_server.Send(request);
	}

	private void OnConnection(ConnectionEvent e)
	{
		_server.EventDispatcher.ConnectionEvent -= OnConnection;
		
		if (ConnectionEvent != null)
			ConnectionEvent(new Dictionary<string, object>(){ { "success", e.Success }, { "error", e.Error } });
	}

	private void OnLogin(LoginEvent e)
	{
		_server.EventDispatcher.LoginEvent -= OnLogin;

		_server.EventDispatcher.ExtensionEvent += OnResponse;

		if (LoginEvent != null)
			LoginEvent(new Dictionary<string, object>() { { "success", e.Success }, { "message", e.Message }, { "data", e.Data } });
	}

	private void OnResponse(ExtensionEvent e)
	{
		if (ExtensionEvent != null)
			ExtensionEvent(new Dictionary<string, object>() { { "command", e.Command }, { "subcommand", e.SubCommand }, { "data", e.Data } });
	}
}
