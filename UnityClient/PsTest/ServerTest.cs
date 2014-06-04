using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using PS.Core;
using PS.Data;
using PS.Events;
using PS.Requests;

public class ServerTest
{
    private PlanetServer _server;

    public ServerTest()
    {
        _server = new PlanetServer();

        _server.EventDispatcher.ConnectionEvent += OnConnection;
        _server.EventDispatcher.ConnectionLostEvent += OnConnectionLost;
        _server.EventDispatcher.LoginEvent += OnLogin;
        _server.EventDispatcher.ExtensionEvent += OnResponse;

        _server.Connect("127.0.0.1", 8000);

        ServerThread worker = new ServerThread(_server);
        Thread thread = new Thread(worker.Update);

        while (!thread.IsAlive)
        {
            worker.Update();
      
            Thread.Sleep(1);
        }
    }

    private void OnConnection(ConnectionEvent e)
    {
        _server.EventDispatcher.ConnectionEvent -= OnConnection;

        LoginRequest request = new LoginRequest("user", "pass");

        _server.Send(request);
    }

    private void OnConnectionLost(ConnectionLostEvent e)
    {
        Console.WriteLine("connection lost");
    }

    private void OnLogin(LoginEvent e)
    {
        _server.EventDispatcher.LoginEvent -= OnLogin;

        Console.WriteLine(e.Success + " msg " + e.Message + " obj " + e.Data.ToString());

        ExtensionRequest req = new ExtensionRequest("player.move", new PsObject());
        _server.Send(req);
    }

    private void OnResponse(ExtensionEvent e)
    {
        Console.WriteLine(e.Command);
    }
}

class ServerThread
{
    PlanetServer _server;

    public ServerThread(PlanetServer server)
    {
        _server = server;
    }

    public void Update()
    {
        _server.DispatchEvents();
    }
}

