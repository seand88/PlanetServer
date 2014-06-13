package com.rzr.login.game;

/**
 *
 * @author Mike
 */
public class PlayerCommand
{
    public static final String BASE_COMMAND = "player";
    
    public enum PlayerEnum { START, INFOPLAYER, INFOGROUP, MOVE, SHOOT };
    
    public static String getCommand(PlayerEnum command) { return BASE_COMMAND + "." + command.toString(); }
}
