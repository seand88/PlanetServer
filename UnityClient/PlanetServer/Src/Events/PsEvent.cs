using System.Collections.Generic;

using PS.Data;

namespace PS.Events
{
    public abstract class PsEvent
    {
        public object Target { get; set; }
        public MessageType Type { get; protected set; }
        
        public PsEvent(MessageType type)
        {
            Type = type;
        }

        public abstract void Create(Dictionary<string, object> dict);

        public override string ToString()
        {
            return Type + " [ " + ((Target == null) ? "null" : Target.ToString()) + "]";
        }
    }
}
