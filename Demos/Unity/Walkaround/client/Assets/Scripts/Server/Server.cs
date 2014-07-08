using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PS.Core;
using PS.Data;
using PS.Events;
using PS.Requests;

// all our messages to the outside world will use the same parameters
public delegate void ServerEvent(Dictionary<string, object> message);

/// <summary>
/// The main interaction point the client and the server.  This is responsible for all communcation to and from the server. 
/// </summary>
public class Server : MonoBehaviour
{
	// simple delegates for the outside world to add handlers to receive various server messages
	public event ServerEvent ConnectionEvent;
	public event ServerEvent ConnectionLostEvent;
	public event ServerEvent LoginEvent;
	public event ServerEvent ExtensionEvent;
	public event ServerEvent PublicMessageEvent;

	// used to find the server in the sea of gameobjects
	public static string NAME = "PSServer";

	PlanetServer _server;

	void Awake()
	{
		// make sure this doesn't get removed when loading new levels
		DontDestroyOnLoad(gameObject);
	}

	void FixedUpdate()
	{
		// since unity isn't thread safe we have to wait until we are back in the unity thread to handle any of the server events
		if (_server != null)
			_server.DispatchEvents();
	}

	void OnApplicationQuit()
	{
		if (_server != null)
		{
			// remove event handlers
			_server.EventDispatcher.ConnectionLostEvent -= OnConnectionLost;
			_server.EventDispatcher.ExtensionEvent -= OnResponse;
			_server.EventDispatcher.PublicMessageEvent -= OnPublicMessage;

			// tell the server to disconnect this user so it isn't hanging around
			_server.Disconnect();
			_server = null;
		}
	}

	/// <summary>
	/// Connect the server with the specified IP and port.
	/// </summary>
	/// <param name="ip">IP of host server.</param>
	/// <param name="port">Port of host server.</param>
	public void Connect(string ip, int port)
	{
		_server = new PlanetServer();

		// add handlers for basic actions
		_server.EventDispatcher.ConnectionEvent += OnConnection;
		_server.EventDispatcher.ConnectionLostEvent += OnConnectionLost;
		_server.EventDispatcher.PublicMessageEvent += OnPublicMessage;

		_server.Connect(ip, port);
	}

	/// <summary>
	/// Login in to the server with the specified username and password.
	/// </summary>
	/// <param name="username">Username to login with.</param>
	/// <param name="password">Password for username.</param>
	public void Login(string username, string password)
	{
		_server.EventDispatcher.LoginEvent += OnLogin;

		LoginRequest login = new LoginRequest(username, password);

		_server.Send(login);
	}

	/// <summary>
	/// Send a request to the server.
	/// </summary>
	/// <param name="request">The request to send.</param>
	public void SendRequest(PsRequest request)
	{
		_server.Send(request);
	}

	// got a response for trying to connect to the server
	private void OnConnection(ConnectionEvent e)
	{
		// connected or not don't need to have this anymore.  since these are delegates need to be careful to remove them when not needed
		_server.EventDispatcher.ConnectionEvent -= OnConnection;

		// just pass the info through
		if (ConnectionEvent != null)
			ConnectionEvent(new Dictionary<string, object>(){ { "success", e.Success }, { "error", e.Error } });
	}

	// either the player disconnected from the server or quit the game
	private void OnConnectionLost(ConnectionLostEvent e)
	{
		// just pass the info through
		if (ConnectionLostEvent != null)
			ConnectionLostEvent(new Dictionary<string, object>());
	}

	// got a response for trying to log in
	private void OnLogin(LoginEvent e)
	{
		_server.EventDispatcher.LoginEvent -= OnLogin;

		// if able to login start listening for regular responses
		if (e.Success)
			_server.EventDispatcher.ExtensionEvent += OnResponse;

		// just pass the info through
		if (LoginEvent != null)
			LoginEvent(new Dictionary<string, object>() { { "success", e.Success }, { "message", e.Message }, { "data", e.Data } });
	}

	// got a response back from a request sent to the server
	private void OnResponse(ExtensionEvent e)
	{
		// just pass the info through
		if (ExtensionEvent != null)
			ExtensionEvent(new Dictionary<string, object>() { { "command", e.Command }, { "subcommand", e.SubCommand }, { "data", e.Data } });
	}

	// got a message from another player
	private void OnPublicMessage(PublicMessageEvent e)
	{
		// just pass the info through
		if (PublicMessageEvent != null)
			PublicMessageEvent(new Dictionary<string, object>() { { "username", e.User }, { "message", e.Message } });
	}
}
