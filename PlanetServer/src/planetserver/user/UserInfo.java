package planetserver.user;

/**
 *
 * @author Sean
 */
public class UserInfo 
{
    private String userid;
    private String authToken;
    private String platform;
    
    public UserInfo()
    {
        this.userid = "";
        this.authToken = "";
        this.platform = "";
    }   
    
    public String getUserid() {
        return userid;
    }

    public void setUserid(String userid) {
        this.userid = userid;
    }

    public String getAuthToken() {
        return authToken;
    }

    public void setAuthToken(String authToken) {
        this.authToken = authToken;
    }

    public String getPlatform() {
        return platform;
    }

    public void setPlatform(String platform) {
        this.platform = platform;
    }    
    

    
}
