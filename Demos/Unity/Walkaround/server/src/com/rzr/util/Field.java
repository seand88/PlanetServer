package com.rzr.util;

public enum Field
{
    //auth
    UserNotCreated("usernotcreated"),
    
    // login
    UserName("username"),
    Password("password");
    
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