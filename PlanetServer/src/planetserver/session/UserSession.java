package planetserver.session;

import java.util.concurrent.atomic.AtomicInteger;
import org.jboss.netty.channel.Channel;
import planetserver.network.PsObject;
import planetserver.user.UserInfo;

/**
 *
 * @author SeanD
 */
public class UserSession 
{
    private static AtomicInteger counter = new AtomicInteger();
    
    private int _id; //unique id to the server, right now using the channel id
    private Channel _channel;
    private UserInfo _userInfo;
    private boolean _authenticated;
    private String _currentRoom;
    
    public UserSession(Channel channel) 
    {
        _id = counter.getAndIncrement();
        _channel = channel;
        _userInfo = new UserInfo();
        _currentRoom = "";
        _authenticated = false;
    }   
    
    public Channel getChannel()
    {
        return _channel;
    }

    public void setChannel(Channel channel)
    {
        _channel = channel;
    }

    public UserInfo getUserInfo()
    {
        return _userInfo;
    }

    public void setUserInfo(UserInfo user)
    {
        _userInfo = user;
    }

    public void send(PsObject params)
    {	
        String message = params.toJsonString();
        _channel.write(message + '\0');			
    }

    public void sendPolicy(String policyString)
    {
        _channel.write(policyString + '\0');	
    }

    public int getId()
    {
        return _id;
    }

    public String getCurrentRoom()
    {
        return _currentRoom;
    }

    public void setCurrentRoom(String currentRoom)
    {
        _currentRoom = currentRoom;
    }

    public boolean isAuthenticated()
    {
        return _authenticated;
    }

    public void setAuthenticated(boolean authenticated)
    {
        _authenticated = authenticated;
    }    
}
