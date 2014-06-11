package com.rzr.util;

public enum Field
{
    PlayerObj("pl01"),
    PlayerName("pl02"),
    PlayerType("pl03"),
    PlayerPosition("pl04");
    
    private final String code;
    
    private Field(String code)
    {
        this.code = code;
    }

    public String getCode()
    {
        return code;
    }
}