using System;
using System.Collections.Generic;

namespace PS.Data
{
    /// <summary>
    /// Holds an array of data.
    /// </summary>
    public class PsArray
    {
        /// <summary>
        /// Data for the array.
        /// </summary>
        public List<PsDataWrapper> _content;

        /// <summary>
        /// Create an PsArray from an array of objects.
        /// </summary>
        /// <param name="obj">Array of objects.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Initializes a new instance of the PsArray class.
        /// </summary>
        public PsArray()
        {
            _content = new List<PsDataWrapper>();
        }

        /// <summary>
        /// Look for a boolean value in the array.
        /// </summary>
        /// <param name="key">Key to lookup.</param>
        /// <returns>Boolean value of the key.</returns>
        public bool GetBoolean(int key)
        {
            return (bool)_content[key].v;
        }

        /// <summary>
        /// Look for a string value in the array.
        /// </summary>
        /// <param name="key">Key to lookup.</param>
        /// <returns>String value of the key.</returns>
        public string GetString(int key)
        {
            return (string)_content[key].v;
        }

        /// <summary>
        /// Look for a int value in the array.
        /// </summary>
        /// <param name="key">Key to lookup.</param>
        /// <returns>Int value of the key.</returns>
        public int GetInt(int key)
        {
            return (int)_content[key].v;
        }

        /// <summary>
        /// Look for a long value in the array.
        /// </summary>
        /// <param name="key">Key to lookup.</param>
        /// <returns>Long value of the key.</returns>
        public long GetLong(int key)
        {
            return (long)_content[key].v;
        }

        /// <summary>
        /// Look for a float value in the array.
        /// </summary>
        /// <param name="key">Key to lookup.</param>
        /// <returns>Float value of the key.</returns>
        public float GetFloat(int key)
        {
            return (float)_content[key].v;
        }

        /// <summary>
        /// Look for a double value in the array.
        /// </summary>
        /// <param name="key">Key to lookup.</param>
        /// <returns>Double value of the key.</returns>
        public double GetDouble(int key)
        {
            return (double)_content[key].v;
        }

        /// <summary>
        /// Look for a PsObject value in the array.
        /// </summary>
        /// <param name="key">Key to lookup.</param>
        /// <returns>PsObject value of the key.</returns>
        public PsObject GetPsObject(int key)
        {
            return (PsObject)_content[key].v;
        }

        /// <summary>
        /// Look for a PsArray value in the array.
        /// </summary>
        /// <param name="key">Key to lookup.</param>
        /// <returns>PsArray value of the key.</returns>
        public PsArray GetPsArray(int key)
        {
            return (PsArray)_content[key].v;
        }

        /// <summary>
        /// Look for a (Flash)number value in the array.
        /// </summary>
        /// <param name="key">Key to lookup.</param>
        /// <returns>Number value of the key.</returns>
        public double GetNumber(int key)
        {
            return (double)_content[key].v;
        }

        /// <summary>
        /// Add a boolean to the array.
        /// </summary>
        /// <param name="value">Boolean to add.</param>
        public void AddBoolean(bool value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Boolean, value));
        }

        /// <summary>
        /// Add a string to the array.
        /// </summary>
        /// <param name="value">String to add.</param>
        public void AddString(string value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.String, value));
        }

        /// <summary>
        /// Add a int to the array.
        /// </summary>
        /// <param name="value">Int to add.</param>
        public void AddInt(int value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Integer, value));
        }

        /// <summary>
        /// Add a long to the array.
        /// </summary>
        /// <param name="value">Long to add.</param>
        public void AddLong(long value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Long, value));
        }

        /// <summary>
        /// Add a float to the array.
        /// </summary>
        /// <param name="value">Float to add.</param>
        public void AddFloat(float value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Float, value));
        }

        /// <summary>
        /// Add a double to the array.
        /// </summary>
        /// <param name="value">Double to add.</param>
        public void AddDouble(double value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Double, value));
        }

        /// <summary>
        /// Add a PsObject to the array.
        /// </summary>
        /// <param name="value">PsObject to add.</param>
        public void AddPsObject(PsObject value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.PSObject, value));
        }

        /// <summary>
        /// Add a PsArray to the array.
        /// </summary>
        /// <param name="value">PsArray to add.</param>
        public void AddPsArray(PsArray value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.PSArray, value));
        }

        /// <summary>
        /// Add a (Flash)number to the array.
        /// </summary>
        /// <param name="value">Number to add.</param>
        public void AddNumber(double value)
        {
            _content.Add(new PsDataWrapper((int)Constants.PsType.Number, value));
        }

        /// <summary>
        /// Number of items in the array.
        /// </summary>
        public int Count { get { return _content.Count; } }

        /// <summary>
        /// Creates a List of objects from the array.
        /// </summary>
        /// <returns>List of objects.</returns>
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

        /// <summary>
        /// Creates a string from the data.
        /// </summary>
        /// <returns>String</returns>
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
