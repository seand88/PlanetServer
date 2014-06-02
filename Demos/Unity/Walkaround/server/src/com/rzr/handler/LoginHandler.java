package com.rzr.handler;

import planetserver.network.PsObject;
import planetserver.session.UserSession;
import planetserver.handler.BasicServerEvent;
import planetserver.handler.exceptions.PsException;
import planetserver.util.PSConstants;

public class LoginHandler extends BasicServerEvent
{
    @Override
    public void handleServerEvent(UserSession sender, PsObject params) throws PsException
    {
        String username = params.getString(PSConstants.USER_NAME);
        String password = params.getString(PSConstants.PASSWORD);

        if (username.equals("bad_user"))
            throw new PsException("bad users not allowed here!");
    }
}