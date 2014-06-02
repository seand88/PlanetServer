package com.rzr.service;

import planetserver.core.PSExtension;

/**
 * Holds references to all the services
 */
public class ServiceManager
{    
    private PSExtension _parentExtension;

    private static ServiceManager _instance;
    
    private ServiceManager()
    {
    }
    
    public static synchronized ServiceManager getInstance()
    {
        if (_instance == null)
            _instance = new ServiceManager();
        
        return _instance;
    }
    
    @Override
    protected Object clone() throws CloneNotSupportedException
    {
        throw new CloneNotSupportedException("Clone is not allowed.");
    }
    
    /**
     * Called to initialize this class before its used!!
     * @param ext 
     */
    public void initialize(PSExtension ext)
    {
        _parentExtension = ext;
        registerServices(this);
    }
    
    public void registerServices( ServiceManager sm)
    {

    } 
    
    public PSExtension getParentExtension() { return _parentExtension; }
    public void setParentExtension(PSExtension value) { _parentExtension = value; } 
}
