package planetserver.core;

import java.util.Map;
import java.util.Properties;
import java.util.HashMap;

import planetserver.PSApi;
import planetserver.network.PsObject;
import planetserver.room.RoomManager;
import planetserver.session.UserSession;
import planetserver.handler.BasicClientRequestHandler;
import planetserver.handler.BasicServerEvent;
import planetserver.handler.exceptions.PsException;
import planetserver.requests.RequestType;
import planetserver.util.PSConstants;
import planetserver.util.PSEvents;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 *
 * @author Sean
 */
public class PSExtension
{
    private static final Logger logger = LoggerFactory.getLogger(PSExtension.class);
    private Map<String, Class<?>> eventHandlers;
    private Map<String, Class<?>> requestHandlers;
    private RoomManager roomManager;
    private PSApi psApi;
    private Properties properties;

    public PSExtension()
    {
        eventHandlers = new HashMap<String, Class<?>>();
        requestHandlers = new HashMap<String, Class<?>>();
        //TODO: setup the api class!
        //lets create a threadpool here so we can forard any requests onto the thread pool!
    }

    protected void addEventHandler(String eventId, Class<?> theClass)
    {
        eventHandlers.put(eventId, theClass);
    }
    
    protected void removeEventHandler(String eventId)
    {
        eventHandlers.remove(eventId);
    }
    
    protected void addRequestHandler(String requestId, Class<?> theClass)
    {
        requestHandlers.put(requestId, theClass);
    }

    protected void removeRequestHandler(String requestId)
    {
        requestHandlers.remove(requestId);
    }

    public void handleEventRequest(RequestType request, UserSession user, PsObject params)
    {
        Class<?> eventClass = eventHandlers.get(request.getName());
        
        if (eventClass != null)
        {        
            try
            {
                Object event = eventClass.newInstance();  // InstantiationException
                BasicServerEvent serverEvent = (BasicServerEvent)event;
                
                switch (request)
                {
                    case LOGIN:
                        login(serverEvent, user, params);
                        break;
                }
                
            }
            catch (Exception e)
            {
                e.printStackTrace();
            }
        }
    }
    
    private void login(BasicServerEvent event, UserSession user, PsObject params)
    {
        try
        {
            PsObject loginData = new PsObject();
            params.setPsObject(PSConstants.LOGIN_DATA, loginData);
            
            event.handleServerEvent(user, params);
      
            user.setAuthenticated(true);
            
            PsObject ret = new PsObject();
            ret.setString(PSConstants.REQUEST_TYPE, PSEvents.LOGIN);
            ret.setBoolean(PSConstants.LOGIN_SUCCESS, true);
            ret.setPsObject(PSConstants.LOGIN_DATA, params);
            
            user.getChannelWriter().send(ret);
            
            if (roomManager.roomExists("world"))
            {
                roomManager.getRoom("world").addUserToRoom(user);
            }
            else
            {
                roomManager.createRoom("world", user);
            }
        }
        catch (PsException e)
        {
            PsObject ret = new PsObject();
            ret.setString(PSConstants.REQUEST_TYPE, PSEvents.LOGIN);
            ret.setBoolean(PSConstants.LOGIN_SUCCESS, false);            
            ret.setString(PSConstants.LOGIN_MSG, e.getMessage());
            ret.setPsObject(PSConstants.LOGIN_DATA, e.getPsObject());
            
            user.getChannelWriter().send(ret);
        }
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
        if (user.isAuthenticated())
        {
            try
            {
                Class<?> handlerClass = requestHandlers.get(baseCommand);
                Object handler = handlerClass.newInstance();  // InstantiationException
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
        // make sure there is always at least an empty login handler
        addEventHandler(PSEvents.LOGIN, BaseLoginHandler.class); 
    }

    public void setRoomManager(RoomManager roomManager)
    {
        this.roomManager = roomManager;
    }

    public void trace(String msg)
    {
        logger.debug(msg);
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
