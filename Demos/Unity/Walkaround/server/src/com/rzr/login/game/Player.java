package com.rzr.login.game;

import planetserver.session.UserSession;

/**
 *
 * @author Mike
 */
public class Player
{
    private UserSession _session;
    private int _type;
    private int _positionX;
    private int _positionY;
    
    public Player(UserSession session)
    {
        _session = session;
    }
    
    public UserSession getUserSession() { return _session; }
    
    public int getType() { return _type; }
    public void setType(int value) { _type = value; }
    
    public int getPositionX() { return _positionX; }
    public void setPositionX(int value) { _positionX = value; }
    
    public int getPositionY() { return _positionY; }
    public void setPositionY(int value) { _positionY = value; }
}
