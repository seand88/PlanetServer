
public class PrefabUtility 
{
	private static string CHARACTERS_PATH = "Prefabs/Characters/";
	private static string TILE_PATH = "Prefabs/Tiles/";
	private static string SHOTS_PATH = "Prefabs/Shots/";

	public static string GetCharactersPath(string filename)
	{
		return CHARACTERS_PATH + filename;
	}

	public static string GetTilePath(string filename)
	{
		return TILE_PATH + filename;
	}

	public static string GetShotsPath(string filename)
	{
		return SHOTS_PATH + filename;
	}
}
