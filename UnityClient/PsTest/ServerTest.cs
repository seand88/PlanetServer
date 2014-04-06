using System;
using System.Collections.Generic;
using System.Text;

using PlanetServer.Core;
using PlanetServer.Data;
using PlanetServer.Requests;

public class ServerTest
{
    private Server _server;

    public ServerTest()
    {
        _server = new Server();

         _server.EventDispatcher.AddListener(PsEvent.CONNECTION, OnConnect);
         _server.EventDispatcher.AddListener(PsEvent.LOGIN, OnLogin);
         _server.EventDispatcher.AddListener(PsEvent.EXTENSION_RESPONSE, OnResponse);

        //"23.226.232.160"
        _server.Connect("127.0.0.1", 8000);
    }

    private void OnConnect(PsEvent e)
    {
        PsArray arr = new PsArray();
        arr.AddInt(1);
        arr.AddInt(3);

        PsObject obj = new PsObject();
        obj.SetString("authtoken", "");
        obj.SetString("username", "");
        obj.SetString("platform", "test");
        obj.SetString("userid", "test");
        obj.SetPsArray("list", arr);

        arr.AddInt(4);
        PsObject o = new PsObject();
        o.SetInt("a", 1);
        o.SetPsArray("list1", arr);
        obj.SetPsObject("obj", o);
        
        ExtensionRequest request = new ExtensionRequest("login", obj);

        _server.Send(request);
    }

    private void OnLogin(PsEvent e)
    {
        PsObject psobj = (PsObject)e.Data["psobj"];

        Console.WriteLine("command " + psobj.GetString("command"));
        Console.WriteLine("arr " + psobj.GetPsArray("test").ToString());
    }

    private void OnResponse(PsEvent e)
    {
        PsObject psobj = (PsObject)e.Data["psobj"];
    }
}

