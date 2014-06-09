package planetserver.core;

import planetserver.handler.BasicServerEventHandler;
import planetserver.handler.exceptions.PsException;
import planetserver.network.PsObject;
import planetserver.session.UserSession;

/**
 *
 * @author Mike
 */
class BaseLoginHandler extends BasicServerEventHandler
{
    @Override
    public void handleServerEvent(UserSession sender, PsObject params) throws PsException
    {
        
    }
}
