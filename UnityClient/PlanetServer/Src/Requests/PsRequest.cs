using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Pathfinding.Serialization.JsonFx;

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
            string str = JsonWriter.Serialize(_object.ToObject()) + Char.MinValue;

            return UTF8Encoding.UTF8.GetBytes(str);

            //Console.WriteLine("AAAAA " + s);

            /*using (StringWriter sw = new StringWriter(new StringBuilder()))
            {
                using (Pathfinding.Serialization.JsonFx.JsonWriter j = new Pathfinding.Serialization.JsonFx.JsonWriter(sw))
                {
                    j.Write(_object.ToObject());
                    // Console.WriteLine(j.ToString());
                }
            }*/

            /*JsonSerializer serializer = new JsonSerializer();

            using (StringWriter sw = new StringWriter(new StringBuilder()))
            {

                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, _object.ToObject());

                    string str = sw.ToString() + Char.MinValue;

                    Console.WriteLine("BBBBB " + str);

                    return UTF8Encoding.UTF8.GetBytes(str);
                }
            }*/
        }
    }
}
