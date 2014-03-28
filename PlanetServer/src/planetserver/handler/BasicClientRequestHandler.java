package planetserver.handler;

import java.util.Properties;
import planetserver.network.PsObject;
import planetserver.room.RoomManager;
import planetserver.session.UserSession;
import planetserver.user.UserInfo;
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
            params.setString(PSConstants.COMMAND, cmdName);
            recipient.getChannelWriter().send(params);
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
        

        public void setRoomManager(RoomManager roomManager) {
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