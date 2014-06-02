package com.rzr.extension;

import planetserver.core.PSExtension;
import planetserver.util.PSEvents;

import com.rzr.handler.LoginHandler;
import com.rzr.service.ServiceManager;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class WorldExtension extends PSExtension
{
    private static final Logger logger = LoggerFactory.getLogger(WorldExtension.class);
    
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
    }
 
    private void startServices()
    {
        ServiceManager.getInstance().initialize(this);
    }
 
    public void registerHandlers()
    {
        addEventHandler(PSEvents.LOGIN, LoginHandler.class);       
    }
}