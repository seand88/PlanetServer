﻿using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace PlanetServer.Data
{
    public class PsArray
    {
       // public object Data { get { return _content; } }

        public List<PsDataWrapper> _content;

        public static PsArray Create(JArray array)
        {
            PsArray psa = new PsArray();

            foreach (JToken t in array.Children())
            {
                int type = Int32.Parse(t.Value<string>("t"));

                switch (type)
                {
                    case (int)Constants.PsType.Boolean:
                        psa.AddBoolean(t.Value<bool>("v"));
                        break;

                    case (int)Constants.PsType.String:
                        psa.AddString(t.Value<string>("v"));
                        break;

                    case (int)Constants.PsType.Integer:
                        psa.AddInt(t.Value<int>("v"));
                        break;

                    case (int)Constants.PsType.Long:
                        psa.AddLong(t.Value<long>("v"));
                        break;

                    case (int)Constants.PsType.Float:
                        psa.AddFloat(t.Value<float>("v"));
                        break;

                    case (int)Constants.PsType.PSObject:
                        psa.AddPsObject(PsObject.Create(t.Value<string>("v")));
                        break;

                    case (int)Constants.PsType.PSArray:
                        psa.AddPsArray(PsArray.Create(t.Value<JArray>("v")));
                        break;

                    case (int)Constants.PsType.Number:
                        psa.AddNumber(t.Value<float>("v"));
                        break;
                }
            }

            return psa;
        }

        public PsArray()
        {
            _content = new List<PsDataWrapper>();
        }

        public void AddInt(int value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Integer, value));
        }

        public void AddNumber(float value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Number, value));
        }

        public void AddString(string value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.String, value));
        }

        public void AddBoolean(bool value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Boolean, value));
        }

        public void AddFloat(float value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Float, value));
        }

        public void AddLong(long value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Long, value));
        }

        public void AddPsObject(PsObject value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.PSObject, value));
        }

        public void AddPsArray(PsArray value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.PSArray, value));
        }

        public int GetInt(int key)
        {
            return (int)_content[key].v;
        }

        public float GetNumber(int key)
        {
            return (float)_content[key].v;
        }

        public string GetString(int key)
        {
            return (string)_content[key].v;
        }

        public bool GetBoolean(int key)
        {
            return (bool)_content[key].v;
        }

        public float GetFloat(int key)
        {
            return (float)_content[key].v;
        }

        public float GetLong(int key)
        {
            return (long)_content[key].v;
        }

        public PsObject GetPsObject(int key)
        {
            return (PsObject)_content[key].v;
        }

        public PsArray GetPsArray(int key)
        {
            return (PsArray)_content[key].v;
        }

        public int Count { get { return _content.Count; } }

        public List<object> ToArrayObject()
        {
            List<object> ret = new List<object>();

            for (int i = 0; i < _content.Count; ++i)
            {
                PsDataWrapper data = _content[i];

                if (data.v is PsObject)
                {
                    ret.Add((data.v as PsObject).ToObject());
                }
                else if (data.v is PsArray)
                {
                    ret.Add((data.v as PsArray).ToArrayObject());
                }
                else
                    ret.Add(data);
            }

            return ret;
        }

        public override string ToString()
        {
            string str = "Array [";
            for (int i = 0; i < Count; ++i)
            {
                str += _content[i].v;
                if (i < Count - 1)
                    str += ",";
            }
            str += "]";

            return str;
        }
    }
}