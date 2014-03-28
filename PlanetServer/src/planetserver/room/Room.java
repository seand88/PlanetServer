/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package planetserver.room;

import java.util.Collection;
import java.util.concurrent.ConcurrentHashMap;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import planetserver.network.PsObject;
import planetserver.session.UserSession;
import planetserver.user.UserInfo;

/**
 *
 * @author Sean
 */
public class Room
{
     private static final Logger logger = LoggerFactory.getLogger( Room.class );
    
    private String name;
    private ConcurrentHashMap<Integer, UserSession> roomUsers;
    
    protected Room(String name, UserSession user)
    {
        this.name = name;
        roomUsers = new ConcurrentHashMap<Integer, UserSession>();
        
        addUserToRoom(user);
    }
    
    public Collection<UserSession> getPeopleInRoom() 
    {
       return this.roomUsers.values();
    }
    
    public void addUserToRoom(UserSession user)
    {
        if (roomUsers.containsKey(user.getId())) return;
        
        logger.debug("ADDING USER TO ROOM: " + name + " WITH ID OF :" + user.getId()); 
        user.setCurrentRoom(name);
        roomUsers.put(user.getId(), user);
    }
    
    public void removeUserFromRoom(UserSession user)
    {
         //logger.debug("ATTEMPTING TO REMOVE USER FROM ROOM WITH ID OF: " + user.getId());
         if (roomUsers.containsKey(user.getId()))
         {
            logger.debug("USER REMOVED FROM ROOM: " + name + " WITH ID OF :" + user.getId()); 
            user.setCurrentRoom("");
            roomUsers.remove(user.getId());
            logger.debug("ROOM SIZE IS NOW : " + roomUsers.size());
         }
    }
    
    public void sendMessageToRoom(PsObject msg)
    {
        //loop through all the user sessions and send the information!
        for (UserSession session : roomUsers.values())
        {
            session.getChannelWriter().send(msg);
        }
    }
    
}    