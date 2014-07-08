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
    /// <summary>
    /// PlanetServer is a multithreaded TCP based socket server.  The server is based around .NET 2.0 for the widest possible compatiblity.
    /// Designed with Unity in mind, it will work with any C# based client. 
    /// </summary>
    public class PlanetServer
    {
        /// <summary>
        /// The host machine the server will try and connect to.
        /// </summary>
        public string Host { get; private set; }
        /// <summary>
        /// The IP on the host machine.
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// Flag to determine if messages will be dispatached as soon as they are recieved.
        /// Or queued up to be dispatched as a group.
        /// </summary>
        public bool QueueMessages { get; set; }

        private Socket _socket;

        private EventDispatcher _dispatcher;
        
        private String response = String.Empty;

        private object _lock = new object();
        private Queue<PsEvent> _eventQueue;

        /// <summary>
        /// Creates an instance of the PlanetServer class.
        /// By default, message queuing is turned on.
        /// </summary>
        public PlanetServer()
        {
            Setup(true);            
        }

        /// <summary>
        /// Creates an instance of the PlanetServer class.
        /// </summary>
        /// <param name="queueMessages">Flag to determine if messages will be dispatached as soon as they are recieved.  Or queued up to be dispatched as a group.</param>
        public PlanetServer(bool queueMessages)
        {
            Setup(queueMessages);
        }

        /// <summary>
        /// Create resources for operation
        /// </summary>
        /// <param name="queueMessages">Flag to determine if messages will be dispatached as soon as they are recieved.  Or queued up to be dispatched as a group.</param>
        private void Setup(bool queueMessages)
        {
            QueueMessages = queueMessages;

            _eventQueue = new Queue<PsEvent>();

            _dispatcher = new EventDispatcher();
        }

        /// <summary>
        /// Attempt a connection to the server.
        /// </summary>
        /// <param name="host">The host machine the server will try and connect to.</param>
        /// <param name="port">The IP on the host machine.</param>
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

        /// <summary>
        /// Callback for a connection attempt.
        /// Will dispatch a connection event with the success of the operation.
        /// </summary>
        /// <param name="ar">Status of the current operation.</param>
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

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        public void Disconnect()
        {
            _socket.BeginDisconnect(false, DisconnectCallback, null);
        }

        /// <summary>
        /// Callback for a disconnect.
        /// </summary>
        /// <param name="ar">Status of the current operation.</param>
        private void DisconnectCallback(IAsyncResult ar)
        {
            SendMessage(MessageHelper.CreateMessage(MessageType.ConnectionLostEvent.Name));
        }

        /// <summary>
        /// Start to receive a message from the server.
        /// </summary>
        /// <param name="client">Socket connection to the server.</param>
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

        /// <summary>
        /// Callback for recieving a message from the server.  Maybe be called more then once per message if large enough.  This will dispatch the appropriate PsEvent when the
        /// entire message wahs been receivied.
        /// </summary>
        /// <param name="ar">Status of the current operation.</param>
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

        /// <summary>
        /// Send a PsRequest to the server.
        /// </summary>
        /// <param name="request"></param>
        public void Send(PsRequest request)
        {
            if (_socket.Connected)
            {
                byte[] data = request.GenerateMessage();
                
                _socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), _socket);
            }
        }

        /// <summary>
        /// Callback for sending message to server.
        /// </summary>
        /// <param name="ar">Status of the current operation.</param>
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

        /// <summary>
        /// Will either send the message or queue it for later delivery.
        /// </summary>
        /// <param name="message"></param>
        private void SendMessage(PsEvent message)
        {
            if (QueueMessages)
                _eventQueue.Enqueue(message);
            else
                _dispatcher.DispatchEvent(message);
        }

        /// <summary>
        /// Dispatches any messages enqueqed since the last dispatch.
        /// </summary>
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

        /// <summary>
        /// Endpoint for listening for messages.
        /// </summary>
        public EventDispatcher EventDispatcher { get { return _dispatcher; } }
    }
}
