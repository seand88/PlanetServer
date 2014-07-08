using System;
using System.Net.Sockets;
using System.Text;

namespace PS.Core
{
    /// <summary>
    /// State of current connection.
    /// </summary>
    public class StateObject
    {
        public const int BufferSize = 1024;
        
        public Socket workSocket = null;

        public byte[] buffer = new byte[BufferSize];
 
        public StringBuilder sb = new StringBuilder();

        public int dataRecieved = 0;
        public int dataSize = 0;
    }
}
