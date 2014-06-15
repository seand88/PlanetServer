package planetserver.handler;

import planetserver.core.PsExtension;
import planetserver.handler.exceptions.PsException;
import planetserver.network.PsObject;
import planetserver.session.UserSession;

/**
 *
 * @author Mike
 */
public class BasicServerEventHandler
{
    protected PsExtension _parentExtension;
    
    /**
     * Handles verifying login
     * @param sender
     * @param params
     * @return 
     */
    public void handleServerEvent(UserSession sender, PsObject params) throws PsException
    {

    }
    
    public PsExtension getParentExtension()
    {
        return _parentExtension;
    }
    
    public void setParentExtension(PsExtension extension)
    {
        _parentExtension = extension;
    }
}
