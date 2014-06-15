package com.rzr.handler;

import planetserver.network.PsObject;
import planetserver.session.UserSession;
import planetserver.handler.BasicServerEventHandler;
import planetserver.handler.exceptions.PsException;
import planetserver.util.PsConstants;

public class LoginHandler extends BasicServerEventHandler
{
    @Override
    public void handleServerEvent(UserSession sender, PsObject params) throws PsException
    {
        String username = params.getString(PsConstants.USER_NAME);
        String password = params.getString(PsConstants.PASSWORD);

        if (username.equals("bad_user"))
            throw new PsException("bad users not allowed here!");
    }
}