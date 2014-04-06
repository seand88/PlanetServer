using System.Collections.Generic;

namespace PlanetServer.Core
{
    public class PsEvent
    {
        public static readonly string CONNECTION = "connection";
		public static readonly string CONNECTION_LOST = "connectionLost";
        public static readonly string LOGIN = "login";
		public static readonly string EXTENSION_RESPONSE = "extensionResponse";

        public object Target { get; set; }
        public string Type { get; set; }
        public Dictionary<string, object> Data { get; set; }
        
        public PsEvent(string type, Dictionary<string, object> data)
        {
            Type = type;
            Data = data;
            if (Data == null)
                data = new Dictionary<string, object>();
        }

        public PsEvent(string type)
        {
            Type = type;
            Data = new Dictionary<string, object>();
        }

        public PsEvent Clone()
        {
            return new PsEvent(Type, Data);
        }

        public override string ToString()
        {
            return Type + " [ " + ((Target == null) ? "null" : Target.ToString()) + "]";
        }
    }
}
