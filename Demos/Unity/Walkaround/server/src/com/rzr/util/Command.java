package com.rzr.util;

import java.util.EnumSet;
import java.util.HashMap;
import java.util.Map;

public enum Command
{    
    Player("player");
    
    private static final Map<String, Command> lookup = new HashMap<String, Command>();

    static
    {
        for (Command e : EnumSet.allOf(Command.class))
            lookup.put(e.getCode(), e);
    }

    private String _code;
    
    private Command(String code) { _code = code;}

    public String getCode() { return _code; }

    public static Command get(int code) { return lookup.get(code); }    
}