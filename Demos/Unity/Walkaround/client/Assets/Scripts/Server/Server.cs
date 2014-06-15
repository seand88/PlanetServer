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
	public event ServerEvent ConnectionLostEvent;
	public event ServerEvent LoginEvent;
	public event ServerEvent ExtensionEvent;
	public event ServerEvent PublicMessageEvent;

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
			_server.EventDispatcher.ConnectionLostEvent -= OnConnectionLost;
			_server.EventDispatcher.ExtensionEvent -= OnResponse;
			_server.EventDispatcher.PublicMessageEvent -= OnPublicMessage;

			_server.Disconnect();
			_server = null;
		}
	}

	public void Connect(string ip, int port)
	{
		_server = new PlanetServer();

		_server.EventDispatcher.ConnectionEvent += OnConnection;
		_server.EventDispatcher.ConnectionLostEvent += OnConnectionLost;
		_server.EventDispatcher.PublicMessageEvent += OnPublicMessage;

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

	private void OnConnectionLost(ConnectionLostEvent e)
	{
		if (ConnectionLostEvent != null)
			ConnectionLostEvent(new Dictionary<string, object>());
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

	private void OnPublicMessage(PublicMessageEvent e)
	{
		if (PublicMessageEvent != null)
			PublicMessageEvent(new Dictionary<string, object>() { { "username", e.User }, { "message", e.Message } });
	}
}
