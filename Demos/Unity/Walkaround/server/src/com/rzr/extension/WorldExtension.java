package com.rzr.extension;

import planetserver.core.PSExtension;
import planetserver.util.PSEvents;

import com.rzr.handler.LoginHandler;
import com.rzr.handler.LogoutHandler;
import com.rzr.login.game.Game;
import com.rzr.request.PlayerRequest;
import com.rzr.service.ServiceManager;

import com.rzr.util.Command;

public class WorldExtension extends PSExtension
{
    private Game _game;
    
    @Override
    public void init()
    {
       //lets start up all the data manager classes and load in any info that we need to serve to clients
       startServices();        
       // Register the login event
       registerHandlers();
       
       _game = new Game(this);
    }
 
    @Override
    public void destroy()
    {
       super.destroy();
    
        removeEventHandler(PSEvents.LOGIN);
        removeEventHandler(PSEvents.LOGOUT);
        
        removeRequestHandler(Command.Player.getCode());
    }
 
    private void startServices()
    {
        ServiceManager.getInstance().initialize(this);
    }
 
    public void registerHandlers()
    {
        addEventHandler(PSEvents.LOGIN, LoginHandler.class);
        addEventHandler(PSEvents.LOGOUT, LogoutHandler.class);
        
        addRequestHandler(Command.Player.getCode(), PlayerRequest.class);
    }
    
    public Game getGame() { return _game; }
}