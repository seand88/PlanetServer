package planetserver.core;

import java.util.List;
import java.util.Map;
import java.util.Properties;
import java.util.HashMap;

import planetserver.PSApi;
import planetserver.network.PsObject;
import planetserver.room.RoomManager;
import planetserver.session.UserSession;
import planetserver.handler.BasicClientRequestHandler;
import planetserver.handler.BasicServerEventHandler;
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
    
    private Map<String, Class<?>> _eventHandlers;
    private Map<String, Class<?>> _requestHandlers;
    private RoomManager _roomManager;
    private PSApi _psApi;
    private Properties _properties;

    public PSExtension()
    {
        _eventHandlers = new HashMap<String, Class<?>>();
        _requestHandlers = new HashMap<String, Class<?>>();
        //TODO: setup the api class!
        //lets create a threadpool here so we can forard any requests onto the thread pool!
    }

    protected void addEventHandler(String eventId, Class<?> theClass)
    {
        _eventHandlers.put(eventId, theClass);
    }
    
    protected void removeEventHandler(String eventId)
    {
        _eventHandlers.remove(eventId);
    }
    
    protected void addRequestHandler(String requestId, Class<?> theClass)
    {
        _requestHandlers.put(requestId, theClass);
    }

    protected void removeRequestHandler(String requestId)
    {
        _requestHandlers.remove(requestId);
    }

    public void handleEventRequest(RequestType request, UserSession user, PsObject params)
    {
        Class<?> eventClass = _eventHandlers.get(request.getName());
        
        if (eventClass != null)
        {        
            try
            {
                Object event = eventClass.newInstance();  // InstantiationException
                BasicServerEventHandler serverEvent = (BasicServerEventHandler)event;
                
                switch (request)
                {
                    case LOGIN:
                        login(serverEvent, user, params);
                        break;
                        
                    case LOGOUT:
                        logout(user);
                        break;
                }
                
            }
            catch (Exception e)
            {
                e.printStackTrace();
            }
        }
    }
    
    private void login(BasicServerEventHandler event, UserSession user, PsObject params)
    {
        try
        {
            PsObject loginData = new PsObject();
            params.setPsObject(PSConstants.LOGIN_DATA, loginData);
            
            event.setParentExtension(this);
            event.handleServerEvent(user, params);
      
            user.setAuthenticated(true);
            user.getUserInfo().setUserid(params.getString(PSConstants.USER_NAME));
            
            PsObject ret = new PsObject();
            ret.setString(PSConstants.REQUEST_TYPE, PSEvents.LOGIN);
            ret.setBoolean(PSConstants.LOGIN_SUCCESS, true);
            ret.setPsObject(PSConstants.LOGIN_DATA, params);
            
            user.send(ret);
            
            if (_roomManager.roomExists("world"))
            {
                _roomManager.getRoom("world").addUserToRoom(user);
            }
            else
            {
                _roomManager.createRoom("world", user);
            }
        }
        catch (PsException e)
        {
            PsObject ret = new PsObject();
            ret.setString(PSConstants.REQUEST_TYPE, PSEvents.LOGIN);
            ret.setBoolean(PSConstants.LOGIN_SUCCESS, false);            
            ret.setString(PSConstants.LOGIN_MSG, e.getMessage());
            ret.setPsObject(PSConstants.LOGIN_DATA, e.getPsObject());
            
            user.send(ret);
        }
    }
    
    public void logout(UserSession user)
    {
        user.setAuthenticated(false);

        //remove the user from any rooms they are in!
        if (user.getCurrentRoom().length() > 1)
        {
            logger.debug("REMOVING USER FROM ROOM WITH ID: " + user.getId());
            _roomManager.getRoom(user.getCurrentRoom()).removeUserFromRoom(user);
        }
        
        // if there is a logout handler registered then process it
        Class<?> eventClass = _eventHandlers.get(RequestType.LOGOUT.getName());
        
        if (eventClass != null)
        {        
            try
            {
                Object event = eventClass.newInstance();  // InstantiationException
                BasicServerEventHandler serverEvent = (BasicServerEventHandler)event;
                serverEvent.setParentExtension(this);
                serverEvent.handleServerEvent(user, null);
            }
            catch (Exception e)
            {
                e.printStackTrace();
            }
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
        if (_requestHandlers.containsKey(baseCommand) == false)
        {
            logger.warn("USER SENT UNREGISTERED COMMAND :" + baseCommand);
            return;
        }

        //only allow the command through if its a login command, or the user is already authenticated!
        if (user.isAuthenticated())
        {
            try
            {
                Class<?> handlerClass = _requestHandlers.get(baseCommand);
                Object handler = handlerClass.newInstance();  // InstantiationException
                BasicClientRequestHandler bcrHandler = (BasicClientRequestHandler)handler;
                bcrHandler.setParentExtension(this);
                bcrHandler.setRoomManager(_roomManager);
                bcrHandler.setProperties(_properties);
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

    public void send(String cmdName, PsObject params, UserSession recipient)
    {
        PsObject psobj = new PsObject();
        psobj.setString(PSConstants.REQUEST_TYPE, PSEvents.EXTENSION);
        psobj.setString(PSConstants.COMMAND, cmdName);
        psobj.setPsObject(PSConstants.EXTENSION_DATA, params);
        
        recipient.send(psobj);
    }
    
    public void send(String cmdName, PsObject params, List<UserSession> recipientList)
    {
        PsObject psobj = new PsObject();
        psobj.setString(PSConstants.REQUEST_TYPE, PSEvents.EXTENSION);
        psobj.setString(PSConstants.COMMAND, cmdName);
        psobj.setPsObject(PSConstants.EXTENSION_DATA, params);
       
        for (UserSession recipient : recipientList)
            recipient.send(psobj);
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

    public RoomManager getRoomManager()
    {
        return _roomManager;
    }
    
    public void setRoomManager(RoomManager roomManager)
    {
        _roomManager = roomManager;
    }

    public Properties getProperties()
    {
        return _properties;
    }

    public void setProperties(Properties properties)
    {
        _properties = properties;
    }
}
