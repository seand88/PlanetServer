using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;

using Pathfinding.Serialization.JsonFx;

namespace PlanetServer.Data
{
    public class PsObject
    {
        private Dictionary<string, PsDataWrapper> _content;

        public static PsObject Create(string json)
        {
            object obj = JsonReader.Deserialize(json);

            if (obj is IDictionary)
            {
                return Create((Dictionary<string, object>)obj);
            }
            else
            {
                throw new Exception("JSON not in proper format");
            }
        }

        public static PsObject Create(Dictionary<string, object> dict)
        {
            PsObject psobj = new PsObject();

            foreach (KeyValuePair<string, object> item in dict)
            {
                string key = item.Key;
                Dictionary<string, object> value = (Dictionary<string, object>)item.Value;

                int type = (int)value["t"];

                switch (type)
                {
                    case (int)Constants.PsType.Boolean:
                        psobj.SetBoolean(key, Convert.ToBoolean(value["v"]));
                        break;

                    case (int)Constants.PsType.String:
                        psobj.SetString(key, Convert.ToString(value["v"]));
                        break;

                    case (int)Constants.PsType.Integer:
                        psobj.SetInt(key, Convert.ToInt32(value["v"]));
                        break;

                    case (int)Constants.PsType.Long:
                        psobj.SetLong(key, Convert.ToInt64(value["v"]));
                        break;

                    case (int)Constants.PsType.Float:
                        psobj.SetFloat(key, Convert.ToSingle(value["v"]));
                        break;

                    case (int)Constants.PsType.PSObject:
                        psobj.SetPsObject(key, PsObject.Create((Dictionary<string, object>)value["v"]));
                        break;

                    case (int)Constants.PsType.PSArray:
                        psobj.SetPsArray(key, PsArray.Create((object[])value["v"]));
                        break;

                    case (int)Constants.PsType.Number:
                        psobj.SetFloat(key, Convert.ToSingle(value["v"]));
                        break;
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
                    hash.Add(Constants.TYPE_FLAG, (int)Constants.PsType.PSObject);
                    hash.Add(Constants.VALUE_FLAG, (data.v as PsObject).ToObject());

                    ret[key] = hash;
                }
                else if (data.v is PsArray)
                {
                    Hashtable hash = new Hashtable(2);
                    hash.Add(Constants.TYPE_FLAG, (int)Constants.PsType.PSArray);
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