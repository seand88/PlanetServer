public class PlayerCommand 
{
	public const string BASE_COMMAND = "Player";

	public enum PlayerEnum { Start, InfoPlayer, InfoGroup, Move, Shoot };

	public static string GetCommand(PlayerEnum command) { return BASE_COMMAND + "." + command.ToString(); }
}
