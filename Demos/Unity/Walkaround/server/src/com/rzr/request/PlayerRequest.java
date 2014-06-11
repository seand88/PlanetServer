package com.rzr.request;

import planetserver.handler.BasicClientRequestHandler;
import planetserver.network.PsObject;
import planetserver.session.UserSession;

import com.rzr.login.game.Game;
import com.rzr.login.game.PlayerCommand.PlayerEnum;
import util.RoomHelper;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

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
            
            Game game = RoomHelper.getGame(this);
            
            switch (action)
            {
                case START:
                    game.start(command, sender, params);
                    break;
            }
        }
        catch (IllegalArgumentException e)
        {
            logger.debug("Unknown request for " + requestId);
        }
    }
}
