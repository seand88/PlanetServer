package planetserver.network;

import java.util.Collection;
import java.util.Map;

import net.sf.json.JSONArray;
import net.sf.json.JSONObject;

public class PsDataWrapper 
{
	private Object value;
	private int psType;
	
	@SuppressWarnings({ "unchecked", "rawtypes" })
	public PsDataWrapper(Object value, int type) {
		this.value = value;
                this.psType = type;
	}
	

	public Object getV() {
		return value;
	}

	public int getT() {
		return psType;
	}
	
	
	
}
