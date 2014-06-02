package com.rzr.service;

public class BaseService
{
    private ServiceManager _serviceManager;

    public BaseService(ServiceManager sm)
    {       
        _serviceManager = sm;
    }

    public ServiceManager getServiceManager() { return _serviceManager; }    
}
