package planetserver.handler;

import java.util.Properties;

import planetserver.network.PsObject;
import planetserver.room.RoomManager;
import planetserver.session.UserSession;
import planetserver.util.PSConstants;
import planetserver.util.PSEvents;

public class BasicClientRequestHandler 
{
    protected RoomManager roomManager;
    protected Properties properties;

    /**
     * Handles the incoming client request to this specific handler
     * @param command
     * @param sender
     * @param params 
     */
    public void handleClientRequest(String command, UserSession sender, PsObject params)
    {
    }

    protected void send(String cmdName, PsObject params, UserSession recipient)
    {
        PsObject psobj = new PsObject();
        psobj.setString(PSConstants.REQUEST_TYPE, PSEvents.EXTENSION);
        psobj.setPsObject(PSConstants.EXTENSION_DATA, params);
        recipient.getChannelWriter().send(psobj);
    }

    protected String getSplitCommand(String command)
    {
        //split it on the period
        String[] commandParts = command.split("\\.");
        if (commandParts.length > 1)
        {
            return commandParts[1];
        }
        else
        {
            return command;
        }
    }

    public void setRoomManager(RoomManager roomManager)
    {
        this.roomManager = roomManager;
    }

    public Properties getProperties()
    {
        return properties;
    }

    public void setProperties(Properties properties)
    {
        this.properties = properties;
    }        
 }