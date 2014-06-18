using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

using Pathfinding.Serialization.JsonFx;

using PS.Data;
using PS.Events;
using PS.Requests;

namespace PS.Core
{
    public class PlanetServer
    {
        public string Host { get; private set; }
        public int Port { get; private set; }
        public bool QueueMessages { get; set; }

        private Socket _socket;

        private EventDispatcher _dispatcher;
        
        private String response = String.Empty;

        byte[] buffer = new byte[256];

        private object _lock = new object();
        private Queue<PsEvent> _eventQueue;

        public PlanetServer()
        {
            Setup(true);            
        }

        public PlanetServer(bool queueMessages)
        {
            Setup(queueMessages);
        }

        private void Setup(bool queueMessages)
        {
            QueueMessages = queueMessages;

            _eventQueue = new Queue<PsEvent>();

            _dispatcher = new EventDispatcher();
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

                SendMessage(MessageHelper.CreateMessage(MessageType.ConnectionEvent.Name, dict));
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                client.EndConnect(ar);

                Console.WriteLine("Socket connected to " + client.RemoteEndPoint.ToString());

                StateObject state = new StateObject();
                state.workSocket = _socket;

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["success"] = true;

                SendMessage(MessageHelper.CreateMessage(MessageType.ConnectionEvent.Name, dict));
            }
            catch (Exception e)
            {
                string error = "Failed to lookup host " + Host + " on port " + Port + ". Reason: " + e.Message;

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["success"] = false;
                dict["error"] = error;

                SendMessage(MessageHelper.CreateMessage(MessageType.ConnectionEvent.Name, dict));
            }
        }

        public void Disconnect()
        {
            _socket.BeginDisconnect(false, DisconnectCallback, null);
        }

        private void DisconnectCallback(IAsyncResult ar)
        {
            SendMessage(MessageHelper.CreateMessage(MessageType.ConnectionLostEvent.Name));
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

                if (!client.Connected || bytesRead == 0)
                {
                    SendMessage(MessageHelper.CreateMessage(MessageType.ConnectionLostEvent.Name));

                    return;
                }

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                    if (state.buffer[bytesRead - 1] == 0)
                    {
                        object obj = JsonReader.Deserialize(state.sb.ToString());

                        if (obj is IDictionary)
                        {
                            Dictionary<string, object> dict = (Dictionary<string, object>)obj;
                            Dictionary<string, object> value = (Dictionary<string, object>)dict[PsRequest.REQUEST_TYPE];
                            string request = Convert.ToString(value["v"]) + "_event";
                           
                            SendMessage(MessageHelper.CreateMessage(request, dict));
                        }

                        state.sb.Length = 0;
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
            // server is trying to send a response the client doesn't know about
            catch (KeyNotFoundException e)
            {
                
            }
            catch (Exception e)
            {
                SendMessage(MessageHelper.CreateMessage(MessageType.ConnectionLostEvent.Name));
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

        private void SendMessage(PsEvent message)
        {
            if (QueueMessages)
                _eventQueue.Enqueue(message);
            else
                _dispatcher.DispatchEvent(message);
        }

        public void DispatchEvents()
        {
            PsEvent[] events;

            lock (_lock)
            {
                events = _eventQueue.ToArray();
                _eventQueue.Clear();
            }

            for (int i = 0; i < events.Length; ++i)
                _dispatcher.DispatchEvent(events[i]);
        }

        public EventDispatcher EventDispatcher { get { return _dispatcher; } }
    }
}
