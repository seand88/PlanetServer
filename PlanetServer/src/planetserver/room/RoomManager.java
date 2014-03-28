/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package planetserver.room;

import java.util.Collection;
import java.util.HashMap;
import java.util.concurrent.ConcurrentHashMap;
import planetserver.network.PsObject;
import planetserver.session.UserSession;
import planetserver.user.UserInfo;

/**
 *
 * @author Sean
 */
public class RoomManager 
{
    private ConcurrentHashMap<String, Room> rooms;

    public RoomManager()
    {
        rooms = new ConcurrentHashMap<String, Room>();  
    }

    public Room getRoom(String name) 
    {	
       return this.rooms.get(name);
    }    

    public void createRoom(String name, UserSession user)
    {
        Room room = new Room(name, user);
        
        if (!this.rooms.contains(name)) 
        {
           this.rooms.put(name, room);	
        }
      
     } 

    public void deleteRoom(String name, UserSession user) 
    {	
        Room room = this.rooms.get(name);
        if (room != null )
        {
           this.rooms.remove(name);
        }		
    } 

    public boolean roomExists(String name)
    {
        return this.rooms.containsKey(name);		
    }

    public Collection<Room> getRooms() 
    {
        return this.rooms.values();
    }        
    
    public void sendMessageToCurrentRoom(UserSession user, PsObject pso)
    {
        if (user.getCurrentRoom().length() > 1)
        {
            this.getRoom(user.getCurrentRoom()).sendMessageToRoom(pso);
        }
    }
        
}
