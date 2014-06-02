package planetserver.requests;

import java.util.EnumSet;
import java.util.HashMap;
import java.util.Map;
import planetserver.util.PSEvents;

/**
 *
 * @author Mike
 */
public enum RequestType
{
    NONE(0, PSEvents.NONE),
    LOGIN(1, PSEvents.LOGIN),
    LOGOUT(2, PSEvents.LOGOUT),
    EXTENSION(3, PSEvents.EXTENSION);

    private static final Map<Integer, RequestType> lookup = new HashMap<Integer, RequestType>();

    static
    {
        for (RequestType e : EnumSet.allOf(RequestType.class))
            lookup.put(e.getCode(), e);
    }

    private int _code;
    private String _name;
    
    private RequestType(int code, String name) { _code = code; _name = name;}

    public int getCode() { return _code; }
    
    public String getName() { return _name; }

    public static RequestType get(int code) { return lookup.get(code); }
}
