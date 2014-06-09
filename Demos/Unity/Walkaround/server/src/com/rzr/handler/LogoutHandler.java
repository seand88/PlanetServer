package com.rzr.handler;

import planetserver.network.PsObject;
import planetserver.session.UserSession;
import planetserver.handler.BasicServerEventHandler;
import planetserver.handler.exceptions.PsException;

import com.rzr.login.game.Game;
import util.RoomHelper;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 *
 * @author Mike
 */
public class LogoutHandler extends BasicServerEventHandler
{
    private static final Logger logger = LoggerFactory.getLogger(LogoutHandler.class);
    
    @Override
    public void handleServerEvent(UserSession sender, PsObject params) throws PsException
    {
        Game game = RoomHelper.getGame(this);
        game.userLeft(sender);
    }
}
