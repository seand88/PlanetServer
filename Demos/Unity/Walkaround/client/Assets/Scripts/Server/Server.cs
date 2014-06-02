using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PS.Core;
using PS.Data;
using PS.Events;
using PS.Requests;

public delegate void ServerEvent(Dictionary<string, object> data);

public class Server : MonoBehaviour
{
	public event ServerEvent ConnectionEvent;
	public event ServerEvent LoginEvent;

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
			//_server.EventDispatcher.RemoveListener(PsEvent.EXTENSION_RESPONSE, OnResponse);
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

	private void OnConnection(ConnectionEvent e)
	{
		_server.EventDispatcher.ConnectionEvent -= OnConnection;
		
		if (ConnectionEvent != null)
			ConnectionEvent(new Dictionary<string, object>(){ { "success", e.Success }, { "error", e.Error } });
	}

	private void OnLogin(LoginEvent e)
	{
		_server.EventDispatcher.LoginEvent -= OnLogin;

		if (LoginEvent != null)
			LoginEvent(new Dictionary<string, object>() { { "success", e.Success }, { "message", e.Message }, { "data", e.Data } });
	}

	/*private void OnResponse(PsEvent e)
	{
		PsObject psobj = (PsObject)e.Data["psobj"];
	}*/
}
