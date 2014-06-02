package com.rzr.login;

import planetserver.core.LoginContext;
import planetserver.core.PSLoginHandler;
import planetserver.session.UserSession;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 *
 * @author Mike
 */
public class LoginHandler extends PSLoginHandler
{
    private static final Logger logger = LoggerFactory.getLogger(LoginHandler.class);
    
    @Override
    public boolean handleLogin(UserSession sender, LoginContext context)
    {
        logger.debug("fuck");
        
        return true;
    }
}
