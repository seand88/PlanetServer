package planetserver.core;

import planetserver.handler.BasicServerEvent;
import planetserver.handler.exceptions.PsException;
import planetserver.network.PsObject;
import planetserver.session.UserSession;

/**
 *
 * @author Mike
 */
class BaseLoginHandler extends BasicServerEvent
{
    @Override
    public void handleServerEvent(UserSession sender, PsObject params) throws PsException
    {
        
    }
}
