using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

using PlanetServer.Data;
using PlanetServer.Requests;

namespace PlanetServer.Core
{
    public class Server
    {
        public string Host { get; private set; }
        public int Port { get; private set; }

        private Socket _socket;

        private EventDispatcher _dispatcher;

        private String response = String.Empty;

        byte[] buffer = new byte[256];

        public Server()
        {
            _dispatcher = new EventDispatcher(this);
        }

        public void DoEvents()
        {
          
        }

        public void Connect(string host, int port)
        {
            try
            {
                Host = host;
                Port = port;

                IPHostEntry entry = Dns.GetHostEntry(Host);
                IPAddress address = entry.AddressList[0];
                IPEndPoint ep = new IPEndPoint(address, Port);

                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);       
                _socket.BeginConnect(ep, new AsyncCallback(ConnectCallback), _socket);
            }
            catch (Exception e)
            {
                string error = "Failed to lookup host " + Host + " on port " + Port + ". Reason: " + e.Message;

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["success"] = false;
                dict["error"] = error;
                _dispatcher.DispatchEvent(new PsEvent(PsEvent.CONNECTION, dict));
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());

                StateObject state = new StateObject();
                state.workSocket = _socket;

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["success"] = true;
                _dispatcher.DispatchEvent(new PsEvent(PsEvent.CONNECTION, dict));
            }
            catch (Exception e)
            {
                string error = "Failed to lookup host " + Host + " on port " + Port + ". Reason: " + e.Message;

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["success"] = false;
                dict["error"] = error;
                _dispatcher.DispatchEvent(new PsEvent(PsEvent.CONNECTION, dict));
            }
        }

        public void Disconnect()
        {
            _socket.BeginDisconnect(false, DisconnectCallback, null);
        }

        private void DisconnectCallback(IAsyncResult ar)
        {
            _dispatcher.DispatchEvent(new PsEvent(PsEvent.CONNECTION_LOST));
        }

        private void Receive(Socket client)
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = client;

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                    if (state.buffer[bytesRead - 1] == 0)
                    {
                        PsObject psobj = PsObject.Create(state.sb.ToString());

                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        dict["psobj"] = psobj;

                        if (psobj.GetString("command") == "login")
                            _dispatcher.DispatchEvent(new PsEvent(PsEvent.LOGIN, dict));
                        else
                            _dispatcher.DispatchEvent(new PsEvent(PsEvent.EXTENSION_RESPONSE, dict));                    
                    }
                }
                else
                {
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Send(PsRequest request)
        {
            if (_socket.Connected)
            {
                byte[] data = request.GenerateMessage();
                
                _socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), _socket);
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesSent = client.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public EventDispatcher EventDispatcher { get { return _dispatcher; } }
    }
}
