package com.rzr.util;

public enum Command
{    
    Character("character");
    
    private final String code;
    
    private Command(String code)
    {
        this.code = code;
    }

    public String getCode()
    {
        return code;
    }    
}