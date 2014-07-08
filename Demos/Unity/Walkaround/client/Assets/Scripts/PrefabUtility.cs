/// <summary>
/// Utility functions for prefabs.
/// </summary>
public class PrefabUtility 
{
	private static string CHARACTERS_PATH = "Prefabs/Characters/";
	private static string TILE_PATH = "Prefabs/Tiles/";
	private static string SHOTS_PATH = "Prefabs/Shots/";

	/// <summary>
	/// Gets the full path for a character.
	/// </summary>
	/// <returns>Path to prefab.</returns>
	/// <param name="filename">Base filename.</param>
	public static string GetCharactersPath(string filename)
	{
		return CHARACTERS_PATH + filename;
	}

	/// <summary>
	/// Gets the full path for a tile.
	/// </summary>
	/// <returns>Path to prefab.</returns>
	/// <param name="filename">Base filename.</param>
	public static string GetTilePath(string filename)
	{
		return TILE_PATH + filename;
	}

	/// <summary>
	/// Gets the full path for a shot.
	/// </summary>
	/// <returns>Path to prefab.</returns>
	/// <param name="filename">Base filename.</param>
	public static string GetShotsPath(string filename)
	{
		return SHOTS_PATH + filename;
	}
}
