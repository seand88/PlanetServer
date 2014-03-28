/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package planetserver.core;

import java.util.Properties;
import java.util.concurrent.ConcurrentHashMap;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import planetserver.PSApi;
import planetserver.handler.BasicClientRequestHandler;
import planetserver.network.PsObject;
import planetserver.room.RoomManager;
import planetserver.session.UserSession;
import planetserver.util.PSConstants;
import planetserver.util.PSEvents;

/**
 *
 * @author Sean
 */
public class PSExtension
{
   private static final Logger logger = LoggerFactory.getLogger( PSExtension.class );
     
   private ConcurrentHashMap<String, Class<?>> requestHandlers;
   private RoomManager roomManager;
   private PSApi psApi;
   private Properties properties;
   
   public PSExtension()
   {
        requestHandlers = new ConcurrentHashMap<String,Class<?>>();
        //TODO: setup the api class!
        //lets create a threadpool here so we can forard any requests onto the thread pool!
   }
   
   protected void addRequestHandler(String requestId, Class<?> theClass)
   {
       requestHandlers.put(requestId, theClass);
   }

   protected void removeRequestHandler(String requestId)
   {
        requestHandlers.remove(requestId);
   }
   
    public void handlePolicyRequest(UserSession user) 
    {
       String NEWLINE = "\r\n";  
       String policyString =   "<?xml version=\"1.0\"?>" + NEWLINE +
            "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">" + NEWLINE +
            "" + NEWLINE +
            "<!-- Policy file for xmlsocket://socks.example.com -->" + NEWLINE +
            "<cross-domain-policy> " + NEWLINE +
            "" + NEWLINE +
            "   <!-- This is a master socket policy file -->" + NEWLINE +
            "   <!-- No other socket policies on the host will be permitted -->" + NEWLINE +
            "   <site-control permitted-cross-domain-policies=\"master-only\"/>" + NEWLINE +
            "" + NEWLINE +
            "   <!-- Instead of setting to-ports=\"*\", administrator's can use ranges and commas -->" + NEWLINE +
            "   <allow-access-from domain=\"*\" to-ports=\"*\" />" + NEWLINE +
            "" + NEWLINE +
            "</cross-domain-policy>" + NEWLINE;   
          
       
         //handles sending back the policy request!
         user.getChannelWriter().sendPolicy(policyString);
    }
   
   
   /**
    * Directs the request to the appropriate registered handler
    * 
    * This method needs to be thread safe since it may be accessed by multiple threads
    * @param requestId
    * @param sender
    * @param params 
    */
   public void handleClientRequest(UserSession user, PsObject params) 
   {
      String command = params.getString(PSConstants.COMMAND);
      String baseCommand = getBaseCommand(command);
       //first check if the command is registered!!!
      if (requestHandlers.containsKey(baseCommand) == false)
      {
         logger.warn("USER SENT UNREGISTERED COMMAND :" + baseCommand);
         return;
      }

      //only allow the command through if its a login command, or the user is already authenticated!
      if (command.equalsIgnoreCase(PSEvents.Login.getCode()) || user.isAuthenticated())
      {
            try
            {
                Class<?> handlerClass = requestHandlers.get(baseCommand);
                Object handler  = handlerClass.newInstance();  // InstantiationException
                BasicClientRequestHandler bcrHandler = (BasicClientRequestHandler)handler;
                bcrHandler.setRoomManager(this.roomManager);
                bcrHandler.setProperties(this.properties);
                bcrHandler.handleClientRequest(command, user, params);
            } 
            catch (Exception e)
            {
                  e.printStackTrace();
            }          
      }
      else
      {
        logger.warn("USER WAS NOT AUTHENTICATED TO SEND THIS COMMAND!");
      }
      
   }   
   
   private String getBaseCommand(String command)
   {
        //split it on the period
        String[] commandParts = command.split("\\.");
        if (commandParts.length > 1)
        {
            return commandParts[0];
        }
        else 
        {
            return command;
        }
   }
       
   
   /**
    * Called when server is shut down
    */
   public void destroy()
   {
    
   }   
   
   /**
    * Called when server is initialized
    */
    public void init()
    {
       
    }
   

    public void setRoomManager(RoomManager roomManager)
    {
        this.roomManager = roomManager;
    }   
    
    public void trace(String msg)
    {
        logger.debug(msg);
    }

    public Properties getProperties() {
        return properties;
    }

    public void setProperties(Properties properties) {
        this.properties = properties;
    }
  
   

}
