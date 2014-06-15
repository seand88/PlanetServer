package planetserver.room;

import java.util.Collection;
import java.util.concurrent.ConcurrentHashMap;

import planetserver.network.PsObject;
import planetserver.session.UserSession;
import planetserver.util.PsConstants;
import planetserver.util.PsEvents;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 *
 * @author Sean
 */
public class RoomManager 
{
    private static final Logger logger = LoggerFactory.getLogger(RoomManager.class);
    
    private ConcurrentHashMap<String, Room> _rooms;

    public RoomManager()
    {
        _rooms = new ConcurrentHashMap<String, Room>();  
    }

    public Room getRoom(String name) 
    {	
       return _rooms.get(name);
    }    

    public void createRoom(String name, UserSession user)
    {
        Room room = new Room(name, user);
        
        if (!_rooms.contains(name)) 
        {
           _rooms.put(name, room);	
        }      
     } 

    public void deleteRoom(String name, UserSession user) 
    {	
        Room room = _rooms.get(name);
        if (room != null)
        {
           _rooms.remove(name);
        }		
    } 

    public boolean roomExists(String name)
    {
        return _rooms.containsKey(name);		
    }

    public Collection<Room> getRooms() 
    {
        return _rooms.values();
    }        
    
    public void sendMessageToCurrentRoom(UserSession user, PsObject pso)
    {
        if (user.getCurrentRoom().length() > 1)
        {
            PsObject ret = new PsObject();
            ret.setString(PsConstants.REQUEST_TYPE, PsEvents.PUBLICMESSAGE);
            ret.setString(PsConstants.PM_USER, user.getUserInfo().getUserid());
            ret.setString(PsConstants.PM_MSG, pso.getString(PsConstants.PM_MSG));

            if (pso.hasKey(PsConstants.PM_DATA))
                ret.setPsObject(PsConstants.PM_DATA, pso.getPsObject(PsConstants.PM_DATA));
            
            getRoom(user.getCurrentRoom()).sendMessageToRoom(ret);
        }
    }        
}
