using System;
using System.Collections.Generic;
using System.Runtime.Remoting;

using PS.Events;

namespace PS.Core
{
    sealed class MessageHelper
    {
        public static PsEvent CreateMessage(string name)
        {
            MessageType message = MessageType.ValueOf(name);
            ObjectHandle obj = Activator.CreateInstance(null, message.Class);

            PsEvent e = (PsEvent)obj.Unwrap();
            e.Create(null);

            return e;
        }

        public static PsEvent CreateMessage(string name, Dictionary<string, object> dict)
        {
            MessageType message = MessageType.ValueOf(name);
            ObjectHandle obj = Activator.CreateInstance(null, message.Class);

            PsEvent e = (PsEvent)obj.Unwrap();
            e.Create(dict);

            return e; 
        }
    }
}
