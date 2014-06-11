using System;
using System.Collections.Generic;

namespace PS.Data
{
    public class PsArray
    {
        public List<PsDataWrapper> _content;

        public static PsArray Create(object[] obj)
        {
            PsArray psa = new PsArray();

            foreach (object o in obj)
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)o;

                int type = (int)dict["t"];

                switch (type)
                {
                    case (int)Constants.PsType.Boolean:
                        psa.AddBoolean(Convert.ToBoolean(dict["v"]));
                        break;

                    case (int)Constants.PsType.String:
                        psa.AddString(Convert.ToString(dict["v"]));
                        break;

                    case (int)Constants.PsType.Integer:
                        psa.AddInt(Convert.ToInt32(dict["v"]));
                        break;

                    case (int)Constants.PsType.Long:
                        psa.AddLong(Convert.ToInt64(dict["v"]));
                        break;

                    case (int)Constants.PsType.Float:
                        psa.AddFloat(Convert.ToSingle(dict["v"]));
                        break;

                    case (int)Constants.PsType.PSObject:
                        psa.AddPsObject(PsObject.Create((Dictionary<string, object>)dict["v"]));
                        break;

                    case (int)Constants.PsType.PSArray:
                        psa.AddPsArray(PsArray.Create((object[])dict["v"]));
                        break;

                    case (int)Constants.PsType.Number:
                        psa.AddNumber(Convert.ToSingle(dict["v"]));
                        break;
                }
            }

            return psa;
        }

        public PsArray()
        {
            _content = new List<PsDataWrapper>();
        }

        public bool GetBoolean(int key)
        {
            return (bool)_content[key].v;
        }

        public string GetString(int key)
        {
            return (string)_content[key].v;
        }

        public int GetInt(int key)
        {
            return (int)_content[key].v;
        }

        public long GetLong(int key)
        {
            return (long)_content[key].v;
        }

        public float GetFloat(int key)
        {
            return (float)_content[key].v;
        }

        public double GetDouble(int key)
        {
            return (double)_content[key].v;
        }

        public PsObject GetPsObject(int key)
        {
            return (PsObject)_content[key].v;
        }

        public PsArray GetPsArray(int key)
        {
            return (PsArray)_content[key].v;
        }

        public double GetNumber(int key)
        {
            return (double)_content[key].v;
        }

        public void AddBoolean(bool value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Boolean, value));
        }

        public void AddString(string value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.String, value));
        }

        public void AddInt(int value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Integer, value));
        }

        public void AddLong(long value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Long, value));
        }

        public void AddFloat(float value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Float, value));
        }

        public void AddDouble(double value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Double, value));
        }
        
        public void AddPsObject(PsObject value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.PSObject, value));
        }

        public void AddPsArray(PsArray value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.PSArray, value));
        }

        public void AddNumber(double value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Number, value));
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
