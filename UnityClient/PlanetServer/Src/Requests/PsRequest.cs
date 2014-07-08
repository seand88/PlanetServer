using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Pathfinding.Serialization.JsonFx;

using PS.Data;

namespace PS.Requests
{
    /// <summary>
    /// Base request all PlanetServer requests extend.
    /// </summary>
    public class PsRequest
    {
        /// <summary>
        /// Request type constant.
        /// </summary>
        public static readonly string REQUEST_TYPE = "request_type";

        /// <summary>
        /// Type of message this request is.
        /// </summary>
        public RequestType Type { get; private set; }

        /// <summary>
        /// Internal PsObject for this request.
        /// </summary>
        protected PsObject _object;

        /// <summary>
        /// Initializes a new instance of the PsRequest class.
        /// </summary>
        /// <param name="type"></param>
        public PsRequest(RequestType type)
        {
            Type = type;
        }

        /// <summary>
        /// Initializes the internal PsObject.
        /// </summary>
        /// <param name="obj"></param>
        protected void Init(PsObject obj)
        {
            _object = obj;
            _object.SetInt(REQUEST_TYPE, (int)Type);
        }

        /// <summary>
        /// Creates byte array of the JSON representation of this message.
        /// </summary>
        /// <returns></returns>
        public byte[] GenerateMessage()
        {
            string str = JsonWriter.Serialize(_object.ToObject()) + Char.MinValue;

            return UTF8Encoding.UTF8.GetBytes(str);
        }
    }
}
