/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package planetserver.util;

public enum PSEvents
{
    Login("login"),
    Logout("logout"),
    ;
    private final String code;
    
    private PSEvents(String code)
    {
        this.code = code;
    }

    public String getCode()
    {
        return code;
    }
    
}


