package com.rzr.extension;

import planetserver.core.PSExtension;
import planetserver.util.PSEvents;

import com.rzr.handler.LoginHandler;
import com.rzr.request.PlayerRequest;
import com.rzr.service.ServiceManager;

import com.rzr.util.Command;

public class WorldExtension extends PSExtension
{
    @Override
    public void init()
    {
       //lets start up all the data manager classes and load in any info that we need to serve to clients
       startServices();        
       // Register the login event
       registerHandlers();
    }
 
    @Override
    public void destroy()
    {
       super.destroy();
    
        removeEventHandler(PSEvents.LOGIN);    
        
        removeRequestHandler(Command.Player.getCode());
    }
 
    private void startServices()
    {
        ServiceManager.getInstance().initialize(this);
    }
 
    public void registerHandlers()
    {
        addEventHandler(PSEvents.LOGIN, LoginHandler.class);    
        
        addRequestHandler(Command.Player.getCode(), PlayerRequest.class);
    }
}