using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PlanetServer.Data
{
    public class PsObject
    {
        private Dictionary<string, PsDataWrapper> _content;

        public static PsObject Create(string json)
        {
            JObject o = JObject.Parse(json);

            return Create(o);
        }

        public static PsObject Create(JObject jobj)
        {
            PsObject psobj = new PsObject();

            foreach (JProperty property in jobj.Properties())
            {
                string key = property.Name;

                foreach (JToken t in property.Children())
                {
                    int type = Int32.Parse(t.Value<string>("t"));

                    switch (type)
                    {
                        case (int)Constants.PsType.Boolean:
                            psobj.SetBoolean(key, t.Value<bool>("v"));
                            break;

                        case (int)Constants.PsType.String:
                            psobj.SetString(key, t.Value<string>("v"));
                            break;

                        case (int)Constants.PsType.Integer:
                            psobj.SetInt(key, t.Value<int>("v"));
                            break;

                        case (int)Constants.PsType.Long:
                            psobj.SetLong(key, t.Value<long>("v"));
                            break;

                        case (int)Constants.PsType.Float:
                            psobj.SetFloat(key, t.Value<float>("v"));
                            break;

                        case (int)Constants.PsType.PSObject:
                            psobj.SetPsObject(key, PsObject.Create(t.Value<JObject>("v")));
                            break;

                        case (int)Constants.PsType.PSArray:
                            psobj.SetPsArray(key, PsArray.Create(t.Value<JArray>("v")));
                            break;

                        case (int)Constants.PsType.Number:
                            psobj.SetNumber(key, t.Value<float>("v"));
                            break;
                    }
                }
            }

            return psobj;
        }

        public PsObject()
        {
            _content = new Dictionary<string, PsDataWrapper>();
        }

        internal PsObject(Dictionary<string, PsDataWrapper> content)
        {
            _content = content;
        }

        public bool HasKey(string key)
        {
            return _content.ContainsKey(key);
        }

        public void SetInt(string key, int value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.Integer, value);
        }

        public void SetNumber(string key, float value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.Number, value);
        }

        public void SetString(string key, string value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.String, value);
        }

        public void SetBoolean(string key, bool value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.Boolean, value);
        }

        public void SetFloat(string key, float value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.Float, value);
        }

        public void SetLong(string key, long value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.Long, value);
        }

        public void SetPsObject(string key, PsObject value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.PSObject, value);
        }

        public void SetPsArray(string key, PsArray value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.PSArray, value);
        }

        public int GetInt(string key)
        {
            return (int)_content[key].v;
        }

        public float GetNumber(string key)
        {
            return (float)_content[key].v;
        }

        public string GetString(string key)
        {
            return (string)_content[key].v;
        }

        public bool GetBoolean(string key)
        {
            return (bool)_content[key].v;
        }

        public float GetFloat(string key)
        {
            return (float)_content[key].v;
        }

        public float GetLong(string key)
        {
            return (long)_content[key].v;
        }

        public PsObject GetPsObject(string key)
        {
            return (PsObject)_content[key].v;
        }

        public PsArray GetPsArray(string key)
        {
            return (PsArray)_content[key].v;
        }

        public object ToObject()
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();

            string[] keys = new string[_content.Keys.Count];
            _content.Keys.CopyTo(keys, 0);

            for (int i = 0; i < keys.Length; ++i)
            {
                string key = keys[i];
                PsDataWrapper data = _content[key];

                if (data.v is PsObject)
                {
                    Hashtable hash = new Hashtable(2);
                    hash.Add(Constants.TYPE_FLAG, Constants.PsType.PSObject);
                    hash.Add(Constants.VALUE_FLAG, (data.v as PsObject).ToObject());

                    ret[key] = hash;
                }
                else if (data.v is PsArray)
                {
                    Hashtable hash = new Hashtable(2);
                    hash.Add(Constants.TYPE_FLAG, Constants.PsType.PSArray);
                    hash.Add(Constants.VALUE_FLAG, (data.v as PsArray).ToArrayObject());

                    ret[key] = hash;
                }
                else
                    ret[key] = data;
            }

            return ret;
        }
    }
}