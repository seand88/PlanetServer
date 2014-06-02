package planetserver.util;

public enum Field
{
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