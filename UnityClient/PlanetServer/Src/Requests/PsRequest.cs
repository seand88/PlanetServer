using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using PlanetServer.Data;

namespace PlanetServer.Requests
{
    public class PsRequest
    {
        public RequestType Type { get; private set; }

        protected PsObject _object;

        public PsRequest(RequestType type)
        {
            Type = type;
        }

        protected void Init(PsObject obj)
        {
            _object = obj;
        }

        public byte[] GenerateMessage()
        {
            JsonSerializer serializer = new JsonSerializer();

            using (StringWriter sw = new StringWriter(new StringBuilder()))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, _object.ToObject());

                string str = sw.ToString() + Char.MinValue;

                return UTF8Encoding.UTF8.GetBytes(str);
            }
        }
    }
}
