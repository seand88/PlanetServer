package planetserver.network;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;

import net.sf.json.JSONArray;
import net.sf.json.JSONObject;

public class PsArray 
{
    public LinkedList<PsDataWrapper> content;

    public PsArray()
    {
        this.content = new LinkedList<PsDataWrapper>();
    }

    public int getInteger(int index)
    {
        return (Integer)this.content.get(index).getV();
    }

    public long getLong(int index)
    {
        return (Long)this.content.get(index).getV();
    }

    public String getString(int index)
    {
        return (String)this.content.get(index).getV();
    }

    public boolean getBoolean(int index)
    {
        int b = (Integer)this.content.get(index).getV();
        return (b == 1);
    }

    public float getFloat(int index)
    {
        return (Float)this.content.get(index).getV();
    }

    public double getDouble(int index)
    {
        return (Double)this.content.get(index).getV();
    }
    
    public PsObject getPsObject(int index)
    {
        return (PsObject)this.content.get(index).getV();
    }
    
    public PsArray getPsArray(int index)
    {
        return (PsArray)this.content.get(index).getV();
    }

    //number from as3 returns double in java
    public double getNumber(int index)
    {
        return (Double)this.content.get(index).getV();
    }

    public void addBoolean(boolean value)
    {
        this.content.add(new PsDataWrapper(value, PsType.TYPE_BOOLEAN));
    }

    public void addString(String value)
    {
        this.content.add(new PsDataWrapper(value, PsType.TYPE_STRING));
    }

    public void addInteger(int value)
    {
        this.content.add(new PsDataWrapper(value, PsType.TYPE_INTEGER));
    }

    public void addLong(Long value)
    {
        this.content.add(new PsDataWrapper(value, PsType.TYPE_LONG));
    }

    public void addFloat(float value)
    {
        this.content.add(new PsDataWrapper(value, PsType.TYPE_FLOAT));
    }

    public void addDouble(double value)
    {
        this.content.add(new PsDataWrapper(value, PsType.TYPE_DOUBLE));
    }

    public void addPsObject(PsObject value)
    {
        this.content.add(new PsDataWrapper(value, PsType.TYPE_PSOBJECT));
    }
    
    public void addPsArray(PsArray value)
    {
        this.content.add(new PsDataWrapper(value, PsType.TYPE_PSARRAY));
    }
    
    //add number as double
    public void addNumber(double value)
    {
        this.content.add(new PsDataWrapper(value, PsType.TYPE_NUMBER));
    }

    public JSONArray toJSONObject()
    {
        JSONArray jArray = new JSONArray();

        for (PsDataWrapper obj : this.content)
        {
            if (obj.getV() instanceof PsObject)
            {
                JSONObject psObject = new JSONObject();
                psObject.put("t", PsType.TYPE_PSOBJECT);
                psObject.put("v", ((PsObject) obj.getV()).toJSONObject());
                jArray.add(psObject);
            }
            else if (obj.getV() instanceof PsArray)
            {
                JSONObject psArray = new JSONObject();
                psArray.put("t", PsType.TYPE_PSARRAY);
                psArray.put("v", ((PsArray) obj.getV()).toJSONObject());
                jArray.add(psArray);
            }
            else
            {
                jArray.add(obj);
            }
        }
        return jArray;
    }

    /**
     * Takes in json object and applies properties to the ps object
     * @param jsonObject 
     */
    public void fromJsonObject(JSONArray jsonObject)
    {
        for (Object jsonValues : jsonObject)
        {
            JSONObject keyObj = (JSONObject) jsonValues;
            int type = keyObj.getInt(("t"));
            Object value = keyObj.get("v");

            switch (type)
            {
                case PsType.TYPE_BOOLEAN:
                    addBoolean((Boolean) value);
                    break;

                case PsType.TYPE_STRING:
                    addString((String) value);
                    break;

                case PsType.TYPE_DOUBLE:
                    addDouble((Double) value);
                    break;

                case PsType.TYPE_FLOAT:
                    addFloat((Float) value);
                    break;

                case PsType.TYPE_INTEGER:
                    addInteger((Integer) value);
                    break;

                case PsType.TYPE_LONG:
                    addLong((Long) value);
                    break;

                case PsType.TYPE_NUMBER:
                    addDouble((Double) value); //just convert number to decimal
                    break;

                case PsType.TYPE_PSARRAY:
                    PsArray newArray = new PsArray();
                    newArray.fromJsonObject((JSONArray) value);
                    addPsArray(newArray);
                    break;

                case PsType.TYPE_PSOBJECT:
                    PsObject newObject = new PsObject();
                    newObject.fromJsonObject((JSONObject) value);
                    addPsObject(newObject);
                    break;

                default:
                    throw new Error("Unsupported Type!");
            }
        }
    }

    ///////////to list functions
    public List<Object> toObjectList()
    {
        List<Object> list = new ArrayList<Object>();

        for (PsDataWrapper data : this.content)
        {
            Object obj = data.getV();

            if (obj instanceof PsObject)
            {
                list.add(((PsObject) obj).toMap());
            }
            else if (obj instanceof PsArray)
            {
                list.add(((PsArray) obj).toObjectList());
            }
            else
            {
                list.add(obj);
            }
        }
        return list;
    }
}
