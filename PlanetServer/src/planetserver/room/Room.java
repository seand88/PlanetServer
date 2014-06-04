package planetserver.room;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;

import planetserver.network.PsObject;
import planetserver.session.UserSession;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 *
 * @author Sean
 */
public class Room
{
    private static final Logger logger = LoggerFactory.getLogger(Room.class);
    
    private String _name;
    private ConcurrentHashMap<Integer, UserSession> _roomUsers;
    
    protected Room(String name, UserSession user)
    {
        _name = name;
        _roomUsers = new ConcurrentHashMap<Integer, UserSession>();
        
        addUserToRoom(user);
    }
    
    public List<UserSession> getPeopleInRoom() 
    {
       return new ArrayList(_roomUsers.values());
    }
    
    public void addUserToRoom(UserSession user)
    {
        if (_roomUsers.containsKey(user.getId())) return;
        
        logger.debug("ADDING USER TO ROOM: " + _name + " WITH ID OF : " + user.getId()); 
        user.setCurrentRoom(_name);
        _roomUsers.put(user.getId(), user);
    }
    
    public void removeUserFromRoom(UserSession user)
    {
         //logger.debug("ATTEMPTING TO REMOVE USER FROM ROOM WITH ID OF: " + user.getId());
         if (_roomUsers.containsKey(user.getId()))
         {
            logger.debug("USER REMOVED FROM ROOM: " + _name + " WITH ID OF : " + user.getId()); 
            user.setCurrentRoom("");
            _roomUsers.remove(user.getId());
            logger.debug("ROOM SIZE IS NOW : " + _roomUsers.size());
         }
    }
    
    public void sendMessageToRoom(PsObject msg)
    {
        //loop through all the user sessions and send the information!
        for (UserSession session : _roomUsers.values())
        {
            session.getChannelWriter().send(msg);
        }
    }
}    