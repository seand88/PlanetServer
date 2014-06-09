package planetserver.handler;

import planetserver.core.PSExtension;
import planetserver.handler.exceptions.PsException;
import planetserver.network.PsObject;
import planetserver.session.UserSession;

/**
 *
 * @author Mike
 */
public class BasicServerEventHandler
{
    protected PSExtension _parentExtension;
    
    /**
     * Handles verifying login
     * @param sender
     * @param params
     * @return 
     */
    public void handleServerEvent(UserSession sender, PsObject params) throws PsException
    {

    }
    
    public PSExtension getParentExtension()
    {
        return _parentExtension;
    }
    
    public void setParentExtension(PSExtension extension)
    {
        _parentExtension = extension;
    }
}
