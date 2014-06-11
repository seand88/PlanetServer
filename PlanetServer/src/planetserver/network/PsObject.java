package planetserver.network;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

import net.sf.json.JSONArray;
import net.sf.json.JSONObject;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class PsObject
{
    private static final Logger logger = LoggerFactory.getLogger(PsObject.class);
    
    private HashMap<String, PsDataWrapper> _content;
    public int version;

    public PsObject()
    {
        _content = new HashMap<String, PsDataWrapper>();
    }

    public boolean hasKey(String key)
    {
        return _content.containsKey(key);
    }

    public int getInteger(String key)
    {
        return (Integer)_content.get(key).getV();
    }

    public long getLong(String key)
    {
        return (Long)_content.get(key).getV();
    }

    public String getString(String key)
    {
        return (String)_content.get(key).getV();
    }

    public boolean getBoolean(String key)
    {
        Boolean b = (Boolean)_content.get(key).getV();
        return b;
    }

    public float getFloat(String key)
    {
        return (Float)_content.get(key).getV();
    }

    public double getDouble(String key)
    {
        return (Double)_content.get(key).getV();
    }

    public PsArray getPsArray(String key)
    {
        return (PsArray)_content.get(key).getV();
    }

    public PsObject getPsObject(String key)
    {
        return (PsObject)_content.get(key).getV();
    }
    
    //number from as3, just returns a double
    public double getNumber(String key)
    {
        return (Double)_content.get(key).getV();
    }
    
    public List<Boolean> getBoolArray(String name)
    {
        List<Boolean> list = new ArrayList<Boolean>();
        for (PsDataWrapper data : getPsArray(name).content)
        {
            Object value = data.getV();
            list.add((Boolean)value);
        }
        return list;
    }
    
    public List<String> getStringArray(String name)
    {
        List<String> list = new ArrayList<String>();
        for (PsDataWrapper data : getPsArray(name).content)
        {
            Object value = data.getV();
            list.add((String)value);
        }
        return list;
    }

    public List<Integer> getIntegerArray(String name)
    {
        List<Integer> list = new ArrayList<Integer>();
        for (PsDataWrapper data : getPsArray(name).content)
        {
            Object value = data.getV();
            list.add((Integer)value);
        }
        return list;
    }

    public List<Long> getLongArray(String name)
    {
        List<Long> list = new ArrayList<Long>();
        for (PsDataWrapper data : getPsArray(name).content)
        {
            Object value = data.getV();
            list.add((Long)value);
        }
        return list;
    }

    public List<Float> getFloatArray(String name)
    {
        List<Float> list = new ArrayList<Float>();
        for (PsDataWrapper data : getPsArray(name).content)
        {
            Object value = data.getV();
            list.add((Float)value);
        }
        return list;
    }

    public List<Double> getDoubleArray(String name)
    {
        List<Double> list = new ArrayList<Double>();
        for (PsDataWrapper data : getPsArray(name).content)
        {
            Object value = data.getV();
            list.add((Double)value);
        }
        return list;
    }

    public List<PsObject> getPsObjectArray(String name)
    {
        List<PsObject> list = new ArrayList<PsObject>();
        for (PsDataWrapper data : getPsArray(name).content)
        {
            Object value = data.getV();
            list.add((PsObject)value);
        }
        return list;
    }
    
    public List<Double> getNumberArray(String name)
    {
        List<Double> list = new ArrayList<Double>();
        for (PsDataWrapper data : getPsArray(name).content)
        {
            Object value = data.getV();
            list.add((Double)value);
        }
        return list;
    }

    public void setBoolean(String key, boolean value)
    {
        _content.put(key, new PsDataWrapper(value, PsType.TYPE_BOOLEAN));
    }
    
    public void setString(String key, String value)
    {
        _content.put(key, new PsDataWrapper(value, PsType.TYPE_STRING));
    }
    
    public void setInteger(String key, int value)
    {
        _content.put(key, new PsDataWrapper(value, PsType.TYPE_INTEGER));
    }

    public void setLong(String key, Long value)
    {
        _content.put(key, new PsDataWrapper(value, PsType.TYPE_LONG));
    }
    
    public void setFloat(String key, float value)
    {
        _content.put(key, new PsDataWrapper(value, PsType.TYPE_FLOAT));
    }

    public void setDouble(String key, double value)
    {
        _content.put(key, new PsDataWrapper(value, PsType.TYPE_DOUBLE));
    }

    public void setPsObject(String key, PsObject value)
    {
        _content.put(key, new PsDataWrapper(value, PsType.TYPE_PSOBJECT));
    }
    
    public void setPsArray(String key, PsArray value)
    {
        _content.put(key, new PsDataWrapper(value, PsType.TYPE_PSARRAY));
    }

    public void setNumber(String key, double value)
    {
        _content.put(key, new PsDataWrapper(value, PsType.TYPE_NUMBER));
    }

    public void setBoolArray(String string, List<Boolean> clctn)
    {
        PsArray psArray = new PsArray();
        for (Boolean element : clctn)
        {
            psArray.addBoolean(element);
        }
        setPsArray(string, psArray);
    }
    
    public void setStringArray(String string, List<String> clctn)
    {
        PsArray psArray = new PsArray();
        for (String element : clctn)
        {
            psArray.addString(element);
        }
        setPsArray(string, psArray);
    }

    public void setIntegerArray(String string, List<Integer> clctn)
    {
        PsArray psArray = new PsArray();
        for (Integer element : clctn)
        {
            psArray.addInteger(element);
        }
        setPsArray(string, psArray);
    }

    public void setLongArray(String string, List<Long> clctn)
    {
        PsArray psArray = new PsArray();
        for (Long element : clctn)
        {
            psArray.addLong(element);
        }
        setPsArray(string, psArray);
    }

    public void setFloatArray(String string, List<Float> clctn)
    {
        PsArray psArray = new PsArray();
        for (Float element : clctn)
        {
            psArray.addFloat(element);
        }
        setPsArray(string, psArray);
    }

    public void setDoubleArray(String string, List<Double> clctn)
    {
        PsArray psArray = new PsArray();
        for (Double element : clctn)
        {
            psArray.addDouble(element);
        }
        setPsArray(string, psArray);
    }
 
    public void setPsObjectArray(String string, List<PsObject> clctn)
    {
        PsArray psArray = new PsArray();
        for (PsObject element : clctn)
        {
            psArray.addPsObject(element);
        }
        setPsArray(string, psArray);
    }

    public String toJsonString()
    {
        return toJSONObject().toString();
    }

    /**
     * Takes in json object and applies properties to the ps object
     * @param jsonObject 
     */
    public void fromJsonObject(JSONObject jsonObject)
    {
        Iterator<?> keys = jsonObject.keys();
        while (keys.hasNext())
        {
            String key = (String) keys.next();
            JSONObject keyObj = (JSONObject) jsonObject.get(key);

            String typeID = "t";
            String valueID = "v";

            int type = keyObj.getInt((typeID));
            Object value = keyObj.get(valueID);

            switch (type)
            {
                case PsType.TYPE_BOOLEAN:
                    setBoolean(key, (Boolean) value);
                    break;

                case PsType.TYPE_STRING:
                    setString(key, (String) value);
                    break;

                case PsType.TYPE_DOUBLE:
                    setDouble(key, Double.valueOf(keyObj.getDouble(valueID)));
                    break;

                case PsType.TYPE_FLOAT:
                    setFloat(key, (Float) value);
                    break;

                case PsType.TYPE_INTEGER:
                    setInteger(key, (Integer) value);
                    break;

                case PsType.TYPE_LONG:
                    setLong(key, Long.valueOf(keyObj.getLong(valueID)));
                    break;

                case PsType.TYPE_NUMBER:
                    setNumber(key, Double.valueOf(keyObj.getDouble(valueID)));
                    break;

                case PsType.TYPE_PSARRAY:
                    PsArray newArray = new PsArray();
                    newArray.fromJsonObject((JSONArray) value);
                    setPsArray(key, newArray);
                    break;

                case PsType.TYPE_PSOBJECT:
                    PsObject newObject = new PsObject();
                    newObject.fromJsonObject((JSONObject) value);
                    setPsObject(key, newObject);
                    break;

                default:
                    throw new Error("Unsupported Type!");

            }
        }
    }

    public JSONObject toJSONObject()
    {
        JSONObject jObject = new JSONObject();

        for (String key : _content.keySet())
        {
            PsDataWrapper obj = (PsDataWrapper) (_content.get(key));

            if (obj.getV() instanceof PsObject)
            {
                JSONObject psObject = new JSONObject();
                psObject.put(PsType.TYPE_FLAG, PsType.TYPE_PSOBJECT);
                psObject.put(PsType.VALUE_FLAG, ((PsObject) obj.getV()).toJSONObject());
                jObject.put(key, psObject);
            }
            else if (obj.getV() instanceof PsArray)
            {
                JSONObject psArray = new JSONObject();
                psArray.put(PsType.TYPE_FLAG, PsType.TYPE_PSARRAY);
                psArray.put(PsType.VALUE_FLAG, ((PsArray) obj.getV()).toJSONObject());
                jObject.put(key, psArray);
            }
            else
            {
                jObject.put(key, obj);
            }
        }
        return jObject;
    }

    public Map<String, Object> toMap()
    {
        Map<String, Object> map = new HashMap<String, Object>();

        for (String key : _content.keySet())
        {
            Object obj = _content.get(key).getV();

            if (obj instanceof PsObject)
            {
                map.put(key, ((PsObject) obj).toMap());
            }
            else if (obj instanceof PsArray)
            {
                map.put(key, ((PsArray) obj).toObjectList());
            }
            else
            {
                map.put(key, obj);
            }
        }

        return map;
    }
}
