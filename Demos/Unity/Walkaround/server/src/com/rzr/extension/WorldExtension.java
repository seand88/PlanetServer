package com.rzr.extension;

import planetserver.core.PsExtension;
import planetserver.util.PsEvents;

import com.rzr.handler.LoginHandler;
import com.rzr.handler.LogoutHandler;
import com.rzr.login.game.Game;
import com.rzr.login.game.PlayerCommand;
import com.rzr.request.PlayerRequest;
import com.rzr.service.ServiceManager;

public class WorldExtension extends PsExtension
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
  
        removeEventHandler(PsEvents.LOGIN);
        removeEventHandler(PsEvents.LOGOUT);
        
        removeRequestHandler(PlayerCommand.BASE_COMMAND);
    }
 
    private void startServices()
    {
        ServiceManager.getInstance().initialize(this);
    }
 
    public void registerHandlers()
    {
        addEventHandler(PsEvents.LOGIN, LoginHandler.class);
        addEventHandler(PsEvents.LOGOUT, LogoutHandler.class);
        
        addRequestHandler(PlayerCommand.BASE_COMMAND, PlayerRequest.class);
    }
    
    public Game getGame() { return _game; }
}