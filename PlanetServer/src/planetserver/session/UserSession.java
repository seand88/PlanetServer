package planetserver.session;

import java.util.concurrent.atomic.AtomicInteger;
import org.jboss.netty.channel.Channel;

import planetserver.channel.ChannelWriter;
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
    private ChannelWriter _channelWriter;
    private boolean _authenticated;
    private String _currentRoom;
    
    public UserSession(Channel channel, ChannelWriter channelWriter) 
    {
        _id = counter.getAndIncrement();
        _channel = channel;
        _channelWriter = channelWriter;
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

    public ChannelWriter getChannelWriter()
    {
        return _channelWriter;
    }

    public void setChannelWriter(ChannelWriter channelWriter)
    {
        _channelWriter = channelWriter;
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
