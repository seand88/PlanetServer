package com.rzr.request;

import java.util.List;

import planetserver.handler.BasicClientRequestHandler;
import planetserver.network.PsObject;
import planetserver.session.UserSession;

import util.UserHelper;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

enum PlayerEnum { START, MOVE, SHOOT };

/**
 *
 * @author Mike
 */
public class PlayerRequest extends BasicClientRequestHandler
{
    private static final Logger logger = LoggerFactory.getLogger(PlayerRequest.class);
    
    @Override
    public void handleClientRequest(String command, UserSession sender, PsObject params)
    {
        String requestId = getSplitCommand(command);
        try
        {
            PlayerEnum action = PlayerEnum.valueOf(requestId.toUpperCase());
            
            logger.debug("GOT REQUEST : " + action);
            
            List<UserSession> users = UserHelper.getRecipientsList(roomManager.getRoom(sender.getCurrentRoom()));
            
            send(command, new PsObject(), users);
        }
        catch (Exception e)
        {
            logger.debug("Unknown request for " + requestId);
        }
    }
}
