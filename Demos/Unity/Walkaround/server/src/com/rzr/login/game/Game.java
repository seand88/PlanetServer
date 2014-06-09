package com.rzr.login.game;

import java.util.List;
import java.util.concurrent.ConcurrentHashMap;

import planetserver.session.UserSession;
import planetserver.network.PsObject;

import com.rzr.extension.WorldExtension;
import com.rzr.util.Field;
import util.UserHelper;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 *
 * @author Mike
 */
public class Game
{
    private static final Logger logger = LoggerFactory.getLogger(Game.class);
    
    private ConcurrentHashMap<Integer, Player> _users;
    
    private WorldExtension _extension;
    
    public Game(WorldExtension extension)
    {
        _extension = extension;
        
        _users = new ConcurrentHashMap<Integer, Player>();
    }
    
    public Player userAdded(UserSession session)
    {
        if (_users.containsKey(session.getId()))
            return null;
  
        Player player = new Player(session);
        _users.put(session.getId(), player);
        
        return player;
    }
    
    public void userLeft(UserSession session)
    {
        _users.remove(session.getId());
    }
    
    public void start(String command, UserSession session, PsObject params)
    {
        int type = params.getInteger(Field.PlayerType.getCode());
        List<Integer> position = params.getIntegerArray(Field.PlayerPosition.getCode());
        
        Player player = userAdded(session);
        player.setType(type);
        player.setPositionX(position.get(0));
        player.setPositionY(position.get(1));
               
        List<UserSession> users = UserHelper.getRecipientsList(_extension.getRoomManager().getRoom(session.getCurrentRoom()));
        
        PsObject psobj = new PsObject();
        psobj.setString(Field.PlayerName.getCode(), session.getUserInfo().getUserid());
        psobj.setInteger(Field.PlayerType.getCode(), type);
        psobj.setIntegerArray(Field.PlayerPosition.getCode(), position);
        
        _extension.send(command, psobj, users);
    }
}
