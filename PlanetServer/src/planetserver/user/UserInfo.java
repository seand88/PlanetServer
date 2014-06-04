package planetserver.user;

/**
 *
 * @author Sean
 */
public class UserInfo 
{
    private String _userid;
    private String _authToken;

    public UserInfo()
    {
        _userid = "";
        _authToken = "";
    }

    public String getUserid()
    {
        return _userid;
    }

    public void setUserid(String userid)
    {
        _userid = userid;
    }

    public String getAuthToken()
    {
        return _authToken;
    }

    public void setAuthToken(String authToken)
    {
        _authToken = authToken;
    }
}
