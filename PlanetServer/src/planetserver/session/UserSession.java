/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package planetserver.session;

import org.jboss.netty.channel.Channel;
import planetserver.channel.ChannelWriter;
import planetserver.user.UserInfo;

/**
 *
 * @author SeanD
 */
public class UserSession 
{
    private int id; //unique id to the server, right now using the channel id
    private Channel channel;
    private UserInfo userInfo;
    private ChannelWriter channelWriter;
    private boolean authenticated;
    private String currentRoom;
    
    public UserSession(Channel channel, ChannelWriter channelWriter) 
    {
        this.id = channel.getId();
        this.channel = channel;
        this.channelWriter = channelWriter;
        this.userInfo = new UserInfo();
        this.currentRoom = "";
        this.authenticated = false;
    }   
    
    public String getPlatform() //just a wrapper for easier access
    {
        return this.userInfo.getPlatform();
    }
            
    
    public Channel getChannel()
    {
        return channel;
    }

    public void setChannel(Channel channel)
    {
        this.channel = channel;
    }

    public UserInfo getUserInfo() {
        return userInfo;
    }

    public void setUserInfo(UserInfo user) {
        this.userInfo = user;
    }    
    
    
    public ChannelWriter getChannelWriter() {
        return channelWriter;
    }

    public void setChannelWriter(ChannelWriter channelWriter) {
        this.channelWriter = channelWriter;
    }        
    
    public int getId() {
        return id;
    }
    
    public String getCurrentRoom() {
        return this.currentRoom;
    }
    
    public void setCurrentRoom(String currentRoom) {
        this.currentRoom = currentRoom;
    }    
    
    public boolean isAuthenticated() {
        return authenticated;
    }

    public void setAuthenticated(boolean authenticated) {
        this.authenticated = authenticated;
    }
    
}
