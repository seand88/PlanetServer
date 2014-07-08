/// <summary>
/// Player command map.
/// </summary>
public class PlayerCommand 
{
	public const string BASE_COMMAND = "player";

	/// <summary>
	/// Player subcommands.
	/// </summary>
	public enum PlayerEnum 
	{ 
		/// <summary>
		/// Player starting the game.
		/// </summary>
		Start, 
		/// <summary>
		/// Single player player.
		/// </summary>
		InfoPlayer, 
		/// <summary>
		/// Info for all player.
		/// </summary>
		InfoGroup, 
		/// <summary>
		/// Player moving.
		/// </summary>
		Move, 
		/// <summary>
		/// Player shooting.
		/// </summary>
		Shoot, 
		/// <summary>
		/// Player left.
		/// </summary>
		Leave 
	};

	/// <summary>
	/// Create the full command from a subcommand
	/// </summary>
	/// <returns>The command.</returns>
	/// <param name="command">PlayerEnum as the subcommand.</param>
	public static string GetCommand(PlayerEnum command) { return BASE_COMMAND + "." + command.ToString(); }
}
