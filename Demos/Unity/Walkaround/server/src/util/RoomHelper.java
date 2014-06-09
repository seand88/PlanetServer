package util;

import planetserver.handler.BasicClientRequestHandler;
import planetserver.handler.BasicServerEventHandler;

import com.rzr.extension.WorldExtension;
import com.rzr.login.game.Game;

/**
 *
 * @author Mike
 */
public class RoomHelper
{
    public static Game getGame(BasicClientRequestHandler handler)
    {
        WorldExtension ext = (WorldExtension)handler.getParentExtension();
        return ext.getGame();
    }

    public static Game getGame(BasicServerEventHandler handler)
    {
        WorldExtension ext = (WorldExtension)handler.getParentExtension();
        return ext.getGame();
    }
}
