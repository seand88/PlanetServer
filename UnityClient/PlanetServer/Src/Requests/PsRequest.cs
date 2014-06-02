using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Pathfinding.Serialization.JsonFx;

using PS.Data;

namespace PS.Requests
{
    public class PsRequest
    {
        public static readonly string REQUEST_TYPE = "request_type";

        public RequestType Type { get; private set; }

        protected PsObject _object;

        public PsRequest(RequestType type)
        {
            Type = type;
        }

        protected void Init(PsObject obj)
        {
            _object = obj;
            _object.SetInt(REQUEST_TYPE, (int)Type);
        }

        public byte[] GenerateMessage()
        {
            string str = JsonWriter.Serialize(_object.ToObject()) + Char.MinValue;

            return UTF8Encoding.UTF8.GetBytes(str);
        }
    }
}
