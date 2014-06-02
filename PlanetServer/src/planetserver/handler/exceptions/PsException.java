package planetserver.handler.exceptions;

import planetserver.network.PsObject;

/**
 *
 * @author Mike
 */
public class PsException extends Exception
{
    private PsObject _obj;
    
    public PsException()
    {
        super("");
        
        _obj = new PsObject();
    }
    
    public PsException(String message)
    {
        super(message);
        
        _obj = new PsObject();
    }
    
    public PsException(String message, PsObject obj)
    {
        super(message);
        
        _obj = obj;
    }
    
    public PsObject getPsObject() { return _obj; }
}
