using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;

using Pathfinding.Serialization.JsonFx;

namespace PS.Data
{
    /// <summary>
    /// PsObject is the basis of sending data to and from the server.  It can hold an arbitrary number of key-value pairs of primitive data types, 
    /// Lists of primitive data types, PsArrays, and other PsObjects.
    /// </summary>
    public class PsObject
    {
        private Dictionary<string, PsDataWrapper> _content;

        /// <summary>
        /// Create a PsObject from a JSON string.
        /// </summary>
        /// <param name="json">JSON to create PsObject.</param>
        /// <returns>PsObject containing values from JSON.</returns>
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

        /// <summary>
        /// Create a PsObject from Dictionary of string, object pairs.
        /// </summary>
        /// <param name="dict">Key-value pairs of data.</param>
        /// <returns>PsObject containing values from Dictionary.</returns>
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

        /// <summary>
        /// Initializes a new instance of the PsObject class.
        /// </summary>
        public PsObject()
        {
            _content = new Dictionary<string, PsDataWrapper>();
        }

        internal PsObject(Dictionary<string, PsDataWrapper> content)
        {
            _content = content;
        }

        /// <summary>
        /// Get if PsObject contains a key.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>true if the key exisits, false if it doesn't.</returns>
        public bool HasKey(string key)
        {
            return _content.ContainsKey(key);
        }

        /// <summary>
        /// Look for a int value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>Int value of key.</returns>
        public int GetInt(string key)
        {
            return (int)_content[key].v;
        }

        /// <summary>
        /// Look for a (Flash)number value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>Number value of key.</returns>
        public float GetNumber(string key)
        {
            return (float)_content[key].v;
        }

        /// <summary>
        /// Look for a string value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>String value of key.</returns>
        public string GetString(string key)
        {
            return (string)_content[key].v;
        }

        /// <summary>
        /// Look for a boolean value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>Boolean value of key.</returns>
        public bool GetBoolean(string key)
        {
            return (bool)_content[key].v;
        }

        /// <summary>
        /// Look for a float value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>Float value of key.</returns>
        public float GetFloat(string key)
        {
            return (float)_content[key].v;
        }

        /// <summary>
        /// Look for a long value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>Long value of key.</returns>
        public float GetLong(string key)
        {
            return (long)_content[key].v;
        }

        /// <summary>
        /// Look for a PsObject value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>PsObject value of key.</returns>
        public PsObject GetPsObject(string key)
        {
            return (PsObject)_content[key].v;
        }

        /// <summary>
        /// Look for a PsArray value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>PsArray value of key.</returns>
        public PsArray GetPsArray(string key)
        {
            return (PsArray)_content[key].v;
        }

        /// <summary>
        /// Look for a boolean array value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>Boolean array value of key.</returns>
        public List<bool> GetBoolArray(string key)
        {            
            PsArray psa = GetPsArray(key);

            List<bool> list = new List<bool>();
            for (int i = 0; i < psa.Count; ++i)
                list.Add(psa.GetBoolean(i));
   
            return list;
        }

        /// <summary>
        /// Look for a string array value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>String array value of key.</returns>
        public List<string> GetStringArray(string key)
        {
            PsArray psa = GetPsArray(key);

            List<string> list = new List<string>();
            for (int i = 0; i < psa.Count; ++i)
                list.Add(psa.GetString(i));

            return list;
        }

        /// <summary>
        /// Look for a int array value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>Int array value of key.</returns>
        public List<int> GetIntArray(string key)
        {
            PsArray psa = GetPsArray(key);

            List<int> list = new List<int>();
            for (int i = 0; i < psa.Count; ++i)
                list.Add(psa.GetInt(i));

            return list;
        }

        /// <summary>
        /// Look for a long array value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>Long array value of key.</returns>
        public List<long> GetLongArray(string key)
        {
            PsArray psa = GetPsArray(key);

            List<long> list = new List<long>();
            for (int i = 0; i < psa.Count; ++i)
                list.Add(psa.GetLong(i));

            return list;
        }

        /// <summary>
        /// Look for a float array value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>float array value of key.</returns>
        public List<float> GetFloatArray(string key)
        {
            PsArray psa = GetPsArray(key);

            List<float> list = new List<float>();
            for (int i = 0; i < psa.Count; ++i)
                list.Add(psa.GetFloat(i));

            return list;
        }

        /// <summary>
        /// Look for a double array value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>Double array value of key.</returns>
        public List<double> GetDoubleArray(string key)
        {
            PsArray psa = GetPsArray(key);

            List<double> list = new List<double>();
            for (int i = 0; i < psa.Count; ++i)
                list.Add(psa.GetDouble(i));

            return list;
        }

        /// <summary>
        /// Look for a PsObject array value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>PsObject array value of key.</returns>
        public List<PsObject> GetPsObjectArray(string key)
        {
            PsArray psa = GetPsArray(key);

            List<PsObject> list = new List<PsObject>();
            for (int i = 0; i < psa.Count; ++i)
                list.Add(psa.GetPsObject(i));

            return list;
        }

        /// <summary>
        /// Look for a (Flash)number array value in the PsObject.
        /// </summary>
        /// <param name="key">Key to look for.</param>
        /// <returns>Number array value of key.</returns>
        public List<double> GetNumberArray(string key)
        {
            PsArray psa = GetPsArray(key);

            List<double> list = new List<double>();
            for (int i = 0; i < psa.Count; ++i)
                list.Add(psa.GetNumber(i));

            return list;
        }

        /// <summary>
        /// Set a int in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetInt(string key, int value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.Integer, value);
        }

        /// <summary>
        /// Set a number in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetNumber(string key, float value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.Number, value);
        }

        /// <summary>
        /// Set a string in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetString(string key, string value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.String, value);
        }

        /// <summary>
        /// Set a boolean in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetBoolean(string key, bool value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.Boolean, value);
        }

        /// <summary>
        /// Set a float in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetFloat(string key, float value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.Float, value);
        }

        /// <summary>
        /// Set a long in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetLong(string key, long value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.Long, value);
        }

        /// <summary>
        /// Set a PsObject in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetPsObject(string key, PsObject value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.PSObject, value);
        }

        /// <summary>
        /// Set a PsArray in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetPsArray(string key, PsArray value)
        {
            _content[key] = new PsDataWrapper((int)Constants.PsType.PSArray, value);
        }

        /// <summary>
        /// Set a boolean array in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetBooleanArray(string key, List<bool> value)
        {
            PsArray array = new PsArray();

            for (int i = 0; i < value.Count; ++i)
            {
                array.AddBoolean(value[i]);
            }

            SetPsArray(key, array);
        }

        /// <summary>
        /// Set a string array in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetStringArray(string key, List<string> value)
        {
            PsArray array = new PsArray();

            for (int i = 0; i < value.Count; ++i)
            {
                array.AddString(value[i]);
            }

            SetPsArray(key, array);
        }

        /// <summary>
        /// Set a int array in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetIntArray(string key, List<int> value)
        {
            PsArray array = new PsArray();

            for (int i = 0; i < value.Count; ++i)
            {
                array.AddInt(value[i]);
            }

            SetPsArray(key, array);
        }

        /// <summary>
        /// Set a long array in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetLongArray(string key, List<long> value)
        {
            PsArray array = new PsArray();

            for (int i = 0; i < value.Count; ++i)
            {
                array.AddLong(value[i]);
            }

            SetPsArray(key, array);
        }

        /// <summary>
        /// Set a float array in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetFloatArray(string key, List<float> value)
        {
            PsArray array = new PsArray();

            for (int i = 0; i < value.Count; ++i)
            {
                array.AddFloat(value[i]);
            }

            SetPsArray(key, array);
        }

        /// <summary>
        /// Set a double array in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetDoubleArray(string key, List<double> value)
        {
            PsArray array = new PsArray();

            for (int i = 0; i < value.Count; ++i)
            {
                array.AddDouble(value[i]);
            }

            SetPsArray(key, array);
        }

        /// <summary>
        /// Set a PsObject array in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetPsObjectArray(string key, List<PsObject> value)
        {
            PsArray array = new PsArray();
            
            for (int i = 0; i < value.Count; ++i)
            {
                array.AddPsObject(value[i]);
            }

            SetPsArray(key, array);
        }

        /// <summary>
        /// Set a (Flash)number array in the PsObject.
        /// </summary>
        /// <param name="key">Key for value.</param>
        /// <param name="value">Value to add.</param>
        public void SetNumberArray(string key, List<double> value)
        {
            PsArray array = new PsArray();

            for (int i = 0; i < value.Count; ++i)
            {
                array.AddNumber(value[i]);
            }

            SetPsArray(key, array);
        }

        /// <summary>
        /// Create a raw object for JSON serialization.
        /// </summary>
        /// <returns>Object from PsObject.</returns>
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